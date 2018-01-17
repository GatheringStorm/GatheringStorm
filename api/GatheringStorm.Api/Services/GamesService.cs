using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Auth;
using GatheringStorm.Api.Controllers;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace GatheringStorm.Api.Services
{
    public interface IGamesService
    {
        Task<VoidAppResult> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> JoinGame(DtoJoinGameInfo joinGameInfo, CancellationToken cancellationToken);
        Task<AppResult<List<DtoGame>>> GetGames(CancellationToken cancellationToken = default(CancellationToken));
        Task<AppResult<DtoBoard>> GetBoard(Guid gameId, CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> EndTurn(Guid gameId, CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> PlayCard(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> Attack(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class GamesService : IGamesService
    {
        private readonly ILoginManager loginManager;
        public readonly AppDbContext dbContext;
        private readonly IEffectsService effectsService;
        private readonly ICardInitializerService cardInitializerService;

        public GamesService(ILoginManager loginManager, AppDbContext dbContext, IEffectsService effectsService, ICardInitializerService cardInitializerService)
        {
            this.dbContext = dbContext;
            this.effectsService = effectsService;
            this.cardInitializerService = cardInitializerService;
            this.loginManager = loginManager;
        }

        public async Task<VoidAppResult> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            var opponent = await this.dbContext.Users.SingleOrDefaultAsync(_ => _.Mail == newGameInfo.OpponentMail.ToLower());
            if (opponent == null)
            {
                var createOpponentResult = await this.loginManager.CreateUser(newGameInfo.OpponentMail, cancellationToken);
                if (createOpponentResult.IsErrorResult)
                {
                    return createOpponentResult.GetVoidAppResult();
                }
                opponent = createOpponentResult.SuccessReturnValue;
            }

            var choices = ClassChoice.ChoicesFromClassTypes(newGameInfo.ClassTypes);
            var newGame = new Game
            {
                BeginDate = DateTime.Now,
                Status = GameStatus.InvitePending,
                UserParticipations = new List<UserParticipation>
                {
                    new UserParticipation
                    {
                        User = this.loginManager.LoggedInUser,
                        ClassChoices = choices
                    },
                    new UserParticipation
                    {
                        User = opponent
                    }
                },
                Entities = new List<Entity>
                {
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Health = 20,
                        User = this.loginManager.LoggedInUser
                    },
                    
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Health = 20,
                        User = opponent
                    }
                }
            };

            await this.cardInitializerService.InitializeGame(newGame);

            await this.dbContext.Games.AddAsync(newGame, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        public async Task<VoidAppResult> JoinGame(DtoJoinGameInfo joinGameInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = await this.dbContext.Games.IncludeUserParticipations().IncludeEntities().SingleOrDefaultAsync(_ => _.Id == joinGameInfo.GameId);
            if (game == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData);
            }

            game.Status = GameStatus.InProgress;
            var playerParticipation = game.UserParticipations.Single(_ => _.Mail == this.loginManager.LoggedInUser.Mail);
            var opponentParticipation = game.UserParticipations.Single(_ => _.Mail != this.loginManager.LoggedInUser.Mail);
            playerParticipation.ClassChoices = ClassChoice.ChoicesFromClassTypes(joinGameInfo.ClassTypes);

            var classTypesCount = Enum.GetValues(typeof(ClassType)).Length;
            // Select first choice that was not identical
            for (var i = 1; i <= classTypesCount; i++)
            {
                playerParticipation.ClassType = playerParticipation.ClassChoices.Single(_ => _.Priority == i).ClassType;
                opponentParticipation.ClassType = opponentParticipation.ClassChoices.Single(_ => _.Priority == i).ClassType;

                if (opponentParticipation.ClassType != playerParticipation.ClassType)
                {
                    break;
                }
            }
            // If all choices were identical, select random class for both
            if (playerParticipation.ClassType == opponentParticipation.ClassType)
            {
                var random = new Random();
                do
                {
                    playerParticipation.ClassType = (ClassType)random.Next(classTypesCount);
                    opponentParticipation.ClassType = (ClassType)random.Next(classTypesCount);
                }
                while (playerParticipation.ClassType == opponentParticipation.ClassType);
            }

            // Add class bonuses
            switch(playerParticipation.ClassType)
            {
                case ClassType.Medium:
                    var drawResult = await this.DrawCard(game, playerParticipation.Mail, 1);
                    if (drawResult.IsErrorResult)
                    {
                        return drawResult;
                    }
                    break;
                case ClassType.Slow:
                    var stormlingResult = await this.cardInitializerService.GenerateStormling(playerParticipation.User);
                    if (stormlingResult.IsErrorResult)
                    {
                        return stormlingResult.GetVoidAppResult();
                    }
                    game.Entities.Add(stormlingResult.SuccessReturnValue);
                    break;
            }

            var currentTurnPlayerResult = await this.GetCurrentTurnPlayer(game, cancellationToken);
            if (currentTurnPlayerResult.IsErrorResult)
            {
                return currentTurnPlayerResult.GetVoidAppResult();
            }
            var drawCardResult = await this.DrawCard(game, currentTurnPlayerResult.SuccessReturnValue.Mail, 2);
            if (drawCardResult.IsErrorResult)
            {
                return drawCardResult;
            }

            await dbContext.SaveChangesAsync();

            return VoidAppResult.Success();
        }

        public async Task<AppResult<List<DtoGame>>> GetGames(CancellationToken cancellationToken = default(CancellationToken))
        {            
            var loggedInUserMail = this.loginManager.LoggedInUser.Mail;
            List<Game> games = await this.dbContext.Games.IncludeUserParticipations().Where(_ => _.UserParticipations.Any(x => x.Mail == loggedInUserMail))
                .Include(_ => _.UserParticipations)
                    .ThenInclude(_ => _.User)
                .Include(_ => _.UserParticipations)
                    .ThenInclude(_ => _.ClassChoices)
                .ToListAsync();
            List<DtoGame> dtoGames = new List<DtoGame>();

            foreach (Game game in games)
            {
                var currentTurnPlayerResult = await this.GetCurrentTurnPlayer(game, cancellationToken);
                if (currentTurnPlayerResult.IsErrorResult)
                {
                    return currentTurnPlayerResult.GetErrorAppResult<List<DtoGame>>();
                }
                var currentTurnPlayerMail = currentTurnPlayerResult.SuccessReturnValue?.Mail;
                var opponentMail = game.UserParticipations.Single(_ => _.Mail != loggedInUserMail).User.Mail;

                var newDtoGame = new DtoGame
                {
                    Id = game.Id,
                    CurrentTurnPlayer = currentTurnPlayerMail,
                    BeginDate = game.BeginDate,
                    Status = MapGameStatus(game, currentTurnPlayerMail),
                    OpponentMail = opponentMail
                };

                dtoGames.Add(newDtoGame);
            }

            return AppResult<List<DtoGame>>.Success(dtoGames);
        }
        
        public DtoGameStatus MapGameStatus(Game game, string currentPlayerMail)
        {
            switch (game.Status)
            {
                case GameStatus.Finished:
                    if (currentPlayerMail == this.loginManager.LoggedInUser.Mail) {
                        return DtoGameStatus.Won;
                    }
                    else
                    {
                        return DtoGameStatus.Lost;
                    }
                case GameStatus.InvitePending:
                    var invitedUserMail = game.UserParticipations.Single(_ => _.ClassChoices.Count == 0).Mail;
                    if (invitedUserMail == this.loginManager.LoggedInUser.Mail)
                    {
                        return DtoGameStatus.Invited;
                    }
                    return DtoGameStatus.InvitePending;
                default:    //GameStatus.InProgress is default
                    if (currentPlayerMail == this.loginManager.LoggedInUser.Mail)
                    {
                        return DtoGameStatus.YourTurn;
                    }
                    return DtoGameStatus.OpponentTurn;
            }
        }

        public async Task<AppResult<DtoBoard>> GetBoard(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = await this.dbContext.Games.IncludeUserParticipations().IncludeEntities().SingleOrDefaultAsync(_ => _.Id == gameId, cancellationToken);
            if (game == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData).GetErrorAppResult<DtoBoard>();
            }
            if (game.UserParticipations.All(_ => _.Mail != this.loginManager.LoggedInUser.Mail)) 
            {
                return AppResult<DtoBoard>.Error(AppActionResultType.UserError, "This user is not part of this game.");
            }

            var opponentParticipation = game.UserParticipations.Single(_ => _.Mail != this.loginManager.LoggedInUser.Mail);
            var opponent = opponentParticipation.User;
            var opponentEntity = game.Entities.Select(_ => _ as Player).Single(_ => _ != null && _.User.Mail == opponent.Mail);

            var loggedInParticipation = game.UserParticipations.Single(_ => _.Mail == this.loginManager.LoggedInUser.Mail);
            var loggedInEntity = game.Entities.Select(_ => _ as Player).Single(_ => _ != null && _.User.Mail == this.loginManager.LoggedInUser.Mail);

            var currentTurnPlayerResult = await this.GetCurrentTurnPlayer(game, cancellationToken);
            if (currentTurnPlayerResult.IsErrorResult)
            {
                return AppResult<DtoBoard>.Error(AppActionResultType.ServerError, "There was an error while loading game info.");
            }
            var gameCards = await this.dbContext.GameCards.IncludeAll().Where(_ => _.Game.Id == gameId).ToListAsync();

            var loggedInHandCardsResult = await this.GetDtoCardsFromGameCards(gameCards
                .Where(_ => _.User.Mail == this.loginManager.LoggedInUser.Mail && _.CardLocation == CardLocation.Hand)
                .ToList());
            if (loggedInHandCardsResult.IsErrorResult)
            {
                return loggedInHandCardsResult.GetErrorAppResult<DtoBoard>();
            }

            var loggedInBoardCardsResult = await this.GetDtoCardsFromGameCards(gameCards
                .Where(_ => _.User.Mail == this.loginManager.LoggedInUser.Mail && _.CardLocation == CardLocation.Board)
                .ToList());
            if (loggedInBoardCardsResult.IsErrorResult)
            {
                return loggedInBoardCardsResult.GetErrorAppResult<DtoBoard>();
            }

            var opponentBoardCardsResult = await this.GetDtoCardsFromGameCards(gameCards
                .Where(_ => _.User.Mail == opponent.Mail && _.CardLocation == CardLocation.Board)
                .ToList());
            if (opponentBoardCardsResult.IsErrorResult)
            {
                return opponentBoardCardsResult.GetErrorAppResult<DtoBoard>();
            }

            return AppResult<DtoBoard>.Success(new DtoBoard
            {
                Id = game.Id,
                CurrentTurnPlayer = currentTurnPlayerResult.SuccessReturnValue.Mail,
                PlayerHandCards = loggedInHandCardsResult.SuccessReturnValue,
                OpponentHandCardsCount = gameCards.Where(_ => _.User.Mail == opponent.Mail && _.CardLocation == CardLocation.Hand).Count(),
                Player = new DtoBoardPlayer
                {
                    Health = loggedInEntity.Health,
                    Mail = this.loginManager.LoggedInUser.Mail,
                    BoardCards = loggedInBoardCardsResult.SuccessReturnValue,
                    ClassType = loggedInParticipation.ClassType
                },
                Opponent = new DtoBoardPlayer
                {
                    Health = opponentEntity.Health,
                    Mail = opponent.Mail,
                    BoardCards = opponentBoardCardsResult.SuccessReturnValue,
                    ClassType = opponentParticipation.ClassType
                }
            });
        }

        public async Task<VoidAppResult> EndTurn(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {            
            var game = await this.dbContext.Games.IncludeEntities().IncludeUserParticipations().SingleOrDefaultAsync(_ => _.Id == gameId, cancellationToken);
            if (game == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData);
            }

            var move = new Move
            {
                Date = DateTime.Now,
                Type = MoveType.EndTurn,
                Game = game,
                SourceEntity = game.Entities.Single(_ => _.User.Mail == this.loginManager.LoggedInUser.Mail
                    && _ is Player)
            };

            await this.dbContext.Moves.AddAsync(move, cancellationToken);

            var drawCardResult = await this.DrawCard(game, game.UserParticipations.SingleOrDefault(_ => _.Mail != this.loginManager.LoggedInUser.Mail).Mail, 2);
            if (drawCardResult.IsErrorResult)
            {
                return drawCardResult;
            }

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        public async Task<VoidAppResult> PlayCard(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = await this.dbContext.Games.IncludeUserParticipations().IncludeEntities().SingleOrDefaultAsync(_ => _.Id == gameId, cancellationToken);
            if (game == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData);
            }
            var currentTurnPlayerResult = await this.GetCurrentTurnPlayer(game, cancellationToken);
            if (currentTurnPlayerResult.IsErrorResult)
            {
                return currentTurnPlayerResult.GetVoidAppResult();
            }
            var currentTurnPlayer = currentTurnPlayerResult.SuccessReturnValue;

            var playedGameCard = await this.dbContext.GameCards.IncludeCards().SingleOrDefaultAsync(_ => _.Id == move.CardId);
            if (playedGameCard == null || playedGameCard.CardLocation != CardLocation.Hand)
            {
                return VoidAppResult.Error(AppActionResultType.RuleError, "Selected card cannot be played.");
            }

            if (currentTurnPlayer.Mail != this.loginManager.LoggedInUser.Mail)
            {
                return VoidAppResult.Error(ErrorPreset.NotYourTurn);
            }

            if (game.UserParticipations.All(_ => _.Mail != this.loginManager.LoggedInUser.Mail))
            {
                return VoidAppResult.Error(ErrorPreset.NotAParticipant);
            }

            playedGameCard.CardLocation = CardLocation.Board;

            // Discard cards
            if (playedGameCard.Card.Cost != move.DiscardedCardIds.Count)
            {
                return VoidAppResult.Error(AppActionResultType.RuleError, $"You must discard exactly {playedGameCard.Card.Cost} cards.");
            }
            foreach (var entityId in move.DiscardedCardIds)
            {
                var gameCard = game.Entities.SingleOrDefault(_ => _.Id == entityId) as GameCard;
                if (gameCard == null || gameCard.CardLocation != CardLocation.Hand)
                {
                    return VoidAppResult.Error(AppActionResultType.RuleError, "The selected card cannot be discarded.");
                }
                gameCard.CardLocation = CardLocation.OutOfPlay;
            }

            // Execute effects
            foreach(var effect in move.EffectTargets)
            {
                var executeEffectResult = await this.effectsService.ExecuteEffect(effect, game, currentTurnPlayer, cancellationToken);
                if (executeEffectResult.IsErrorResult)
                {
                    return executeEffectResult;
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        public async Task<VoidAppResult> Attack(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = await this.dbContext.Games.SingleOrDefaultAsync(_ => _.Id == gameId, cancellationToken);
            var attacker = await this.dbContext.GameCards.IncludeCards().SingleOrDefaultAsync(_ => _.Id == move.AttackerId);
            var target = await this.dbContext.GameCards.IncludeCards().SingleOrDefaultAsync(_ => _.Id == move.TargetId);
            
            if (game == null || attacker == null || target == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData);
            }

            var dbMove = new Move
            {
                Date = DateTime.Now,
                Game = game,
                Type = MoveType.Attack,
                SourceEntity = attacker,
                TargetEntities = new List<MoveTargetEntity>
                {
                    new MoveTargetEntity
                    {
                        Entity = target
                    }
                }
            };

            target.Health -= attacker.Card.Attack + attacker.StatModifiersCount;
            attacker.Health -= target.Card.Attack + target.StatModifiersCount;

            if (attacker.Health <= 0)
            {
                attacker.CardLocation = CardLocation.OutOfPlay;
            }

            if (target.Health <= 0)
            {
                target.CardLocation = CardLocation.OutOfPlay;
            }

            await this.dbContext.Moves.AddAsync(dbMove, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        private Task<VoidAppResult> DrawCard(Game game, string userMail, int cardsCount)
        {
            for(var i = 0; i < cardsCount; i++)
            {
                var cards = game.Entities.Select(_ => _ as GameCard)
                                        .Where(_ => _ != null && _.User.Mail == userMail && _.CardLocation == CardLocation.Cellar)
                                        .ToList();

                if (cards.Count == 0)
                {
                    return Task.FromResult(VoidAppResult.Success());
                }

                var drawnCardIndex = new Random().Next(cards.Count());
                cards[drawnCardIndex].CardLocation = CardLocation.Hand;
            }
            return Task.FromResult(VoidAppResult.Success());
        }

        private async Task<AppResult<List<DtoCard>>> GetDtoCardsFromGameCards(List<GameCard> gameCards)
        {
            var dtoCards = new List<DtoCard>();

            foreach(var gameCard in gameCards)
            {
                var effects = new List<DtoEffect>();
                foreach(var cardEffect in gameCard.Card.Effects)
                {
                    var effectResult = await this.effectsService.GetDtoEffect(cardEffect);
                    if (effectResult.IsErrorResult)
                    {
                        return effectResult.GetErrorAppResult<List<DtoCard>>();
                    }
                    effects.Add(effectResult.SuccessReturnValue);
                }

                dtoCards.Add(new DtoCard
                {
                    Id = gameCard.Id,
                    Name = gameCard.Card.Character.Name,
                    Title = gameCard.Card.Title.Name,
                    Cost = gameCard.Card.Cost,
                    Attack = gameCard.Card.Attack,
                    CanAttack = true, // TODO
                    Health = gameCard.Health,
                    StatsModifiersCount = gameCard.StatModifiersCount,
                    Effects = effects
                });
            }

            return AppResult<List<DtoCard>>.Success(dtoCards);
        }

        private async Task<AppResult<User>> GetCurrentTurnPlayer(Game game, CancellationToken cancellationToken)
        {
            if (game.UserParticipations.Any(_ => _.ClassType == null))
            {
                return AppResult<User>.Success(null);
            }

            var lastEndTurn = await this.dbContext.Moves
                .Include(_ => _.Game)
                .Include(_ => _.SourceEntity)
                    .ThenInclude(_ => _.User)
                .Where(_ => _.Type == MoveType.EndTurn && _.Game.Id == game.Id)
                .OrderByDescending(_ => _.Date)
                .FirstOrDefaultAsync();

            if (lastEndTurn == null)
            {
                return AppResult<User>.Success(game.UserParticipations.OrderBy(_ => _.ClassType).First().User);
            }

            return AppResult<User>.Success(game.UserParticipations.Single(_ => _.Mail != lastEndTurn.SourceEntity.User.Mail).User);
        }
    }
}
