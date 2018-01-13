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
            var findOpponentResult = await this.dbContext.Users.FindEntity(newGameInfo.OpponentMail, cancellationToken);
            if (findOpponentResult.Result != AppActionResultType.Success)
            {
                findOpponentResult = await this.loginManager.CreateUser(newGameInfo.OpponentMail, cancellationToken);
            }
            if (findOpponentResult.Result != AppActionResultType.Success)
            {
                return findOpponentResult.GetVoidAppResult();
            }
            var opponent = findOpponentResult.SuccessReturnValue;

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
            var gameResult = await this.dbContext.Games.FindEntity(joinGameInfo.GameId);
            if (gameResult.Result != AppActionResultType.Success)
            {
                return gameResult.GetVoidAppResult();
            }
            var game = gameResult.SuccessReturnValue;

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

                if (opponentParticipation != playerParticipation)
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

            await dbContext.SaveChangesAsync();

            return VoidAppResult.Success();
        }

        public async Task<AppResult<List<DtoGame>>> GetGames(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loggedInUserMail = this.loginManager.LoggedInUser.Mail;
            List<Game> games = await this.dbContext.Games.Where(_ => _.UserParticipations.Any(x => x.Mail == loggedInUserMail)).ToListAsync();
            List<DtoGame> dtoGames = new List<DtoGame>();

            foreach (Game game in games)
            {
                var currentPlayerMail = (await this.GetCurrentTurnPlayer(game.Id, cancellationToken)).SuccessReturnValue.Mail;
                var opponentMail = game.UserParticipations.Single(_ => _.Mail != loggedInUserMail).User.Mail;

                var newDtoGame = new DtoGame
                {
                    Id = game.Id,
                    CurrentTurnPlayer = currentPlayerMail,
                    BeginDate = game.BeginDate,
                    Status = MapGameStatus(game),//DtoGameStatus.Lost, // TODO: Mapping in separate function
                    OpponentMail = opponentMail
                };

                dtoGames.Add(newDtoGame);
            }

            return AppResult<List<DtoGame>>.Success(dtoGames);
        }
        
        public DtoGameStatus MapGameStatus(Game game)
        {
            switch (game.Status)
            {
                case GameStatus.Finished:
                    return DtoGameStatus.Lost; //|| DtoGameStatus.Won;
                case GameStatus.InvitePending:
                    return DtoGameStatus.InvitePending; //|| DtoGameStatus.Invited;
                default:    //GameStatus.InProgress is default
                    return DtoGameStatus.YourTurn; //|| DtoGameStatus.OpponentTurn;
            }
        }

        public async Task<AppResult<DtoBoard>> GetBoard(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var gameResult = await this.dbContext.Games.FindEntity(gameId, cancellationToken);
            if (gameResult.Result != AppActionResultType.Success)
            {
                return AppResult<DtoBoard>.Error(AppActionResultType.ServerError, "There was an error while loading game info.");
            }
            var game = gameResult.SuccessReturnValue;
            if (game.UserParticipations.All(_ => _.Mail != this.loginManager.LoggedInUser.Mail)) 
            {
                return AppResult<DtoBoard>.Error(AppActionResultType.UserError, "This user is not part of this game.");
            }

            var opponent = game.UserParticipations.Single(_ => _.Mail != this.loginManager.LoggedInUser.Mail).User;

            var currentTurnPlayerResult = await this.GetCurrentTurnPlayer(game.Id, cancellationToken);
            if (currentTurnPlayerResult.Result != AppActionResultType.Success)
            {
                return AppResult<DtoBoard>.Error(AppActionResultType.ServerError, "There was an error while loading game info.");
            }
            var gameCards = game.Entities.Where(_ => _ is GameCard)
                .Select(_ => _ as GameCard);

            return AppResult<DtoBoard>.Success(new DtoBoard
            {
                Id = game.Id,
                CurrentTurnPlayer = currentTurnPlayerResult.SuccessReturnValue.Mail,
                PlayerHandCards = gameCards.Where(_ => _.User.Mail == this.loginManager.LoggedInUser.Mail).Select(_ => new DtoCard
                {
                    
                }).ToList(),
                OpponentHandCardsCount = 4,
                Player = new DtoBoardPlayer
                {
                    Health = 20,
                    Mail = "you@gmail.com",
                    BoardCards = new List<DtoCard>
                    {
                        new DtoCard
                        {
                            Id = Guid.NewGuid(),
                            Name = "Drahlget",
                            Title = "The monk",
                            Cost = 5,
                            CanAttack = false,
                            Attack = 3,
                            Health = 1,
                            StatsModifiersCount = 1,
                            Effects = new List<DtoEffect>
                            {
                                new DtoEffect
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "Buff cards",
                                    Description = "Give all cards with name 'Uni' +1/+1",
                                    TargetsCount = 0
                                }
                            }
                        }
                    },
                    ClassType = ClassType.Medium
                },
                Opponent = new DtoBoardPlayer
                {
                    Health = 1,
                    Mail = "xXTHE_EVILXx@gmail.com",
                    BoardCards = new List<DtoCard>
                    {
                        new DtoCard
                        {
                            Id = Guid.NewGuid(),
                            Name = "Drahlget",
                            Title = "The monk",
                            Cost = 5,
                            CanAttack = true,
                            Attack = 3,
                            Health = 1,
                            StatsModifiersCount = 1,
                            Effects = new List<DtoEffect>
                            {
                                new DtoEffect
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "Buff cards",
                                    Description = "Give all cards with name 'Uni' +1/+1",
                                    TargetsCount = 0
                                }
                            }
                        }
                    },
                    ClassType = ClassType.Quick
                }
            });
        }

        public async Task<VoidAppResult> EndTurn(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {            
            var game = (await this.dbContext.Games.FindEntity(gameId, cancellationToken)).SuccessReturnValue; //todo check if its own turn
            var move = new Move
            {
                Date = DateTime.Now,
                Type = MoveType.EndTurn,
                Game = game,
                SourceEntity = game.Entities.Single(_ => _.User.Mail == this.loginManager.LoggedInUser.Mail
                    && _ is Player)
            };

            await this.dbContext.Moves.AddAsync(move, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        public async Task<VoidAppResult> PlayCard(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            var gameResult = await this.dbContext.Games.FindEntity(gameId, cancellationToken);
            var currentTurnPlayerResult = await this.GetCurrentTurnPlayer(gameId, cancellationToken);
            if (gameResult.Result != AppActionResultType.Success)
            {
                return gameResult.GetVoidAppResult();
            }
            if (currentTurnPlayerResult.Result != AppActionResultType.Success)
            {
                return currentTurnPlayerResult.GetVoidAppResult();
            }
            var game = gameResult.SuccessReturnValue;
            var currentTurnPlayer = currentTurnPlayerResult.SuccessReturnValue;

            var playedGameCard = game.Entities.SingleOrDefault(_ => _.Id == move.CardId) as GameCard;
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
                if (executeEffectResult.Result != AppActionResultType.Success)
                {
                    return executeEffectResult;
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        public async Task<VoidAppResult> Attack(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = (await this.dbContext.Games.FindEntity(gameId, cancellationToken)).SuccessReturnValue;

            var attacker = (await this.dbContext.GameCards.FindEntity(move.AttackerId, cancellationToken)).SuccessReturnValue;
            var target = (await this.dbContext.GameCards.FindEntity(move.TargetId, cancellationToken)).SuccessReturnValue;

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

        private async Task<AppResult<User>> GetCurrentTurnPlayer(Guid gameId, CancellationToken cancellationToken)
        {
            var lastEndTurn = await this.dbContext.Moves
                .Where(_ => _.Type == MoveType.EndTurn && _.Game.Id == gameId)
                .OrderByDescending(_ => _.Date)
                .SingleOrDefaultAsync();

            var gameUserParticipations = this.dbContext.UserParitcipations.Where(_ => _.GameId == gameId);

            if (lastEndTurn == null)
            {
                return AppResult<User>.Success(gameUserParticipations.OrderBy(_ => _.ClassType).First().User);
            }

            return AppResult<User>.Success(gameUserParticipations.Single(_ => _.Mail != lastEndTurn.SourceEntity.User.Mail).User);
        }
    }
}
