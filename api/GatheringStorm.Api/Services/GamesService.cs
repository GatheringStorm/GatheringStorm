using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Auth;
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
        Task<AppResult<List<DtoGame>>> GetGamesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<AppResult<DtoBoard>> GetBoardAsync(Guid gameId, CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> EndTurnAsync(Guid gameId, CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> PlayCardAsync(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> AttackAsync(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class GamesService : IGamesService
    {
        private readonly ILoginManager loginManager;
        public readonly AppDbContext dbContext;

        public GamesService(ILoginManager loginManager, AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.loginManager = loginManager;
        }

        public async Task<VoidAppResult> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine(this.loginManager.LoggedInUser.Mail);

            var findOpponentResult = await this.dbContext.Users.FindEntity(newGameInfo.OpponentMail, cancellationToken);
            if (findOpponentResult.Result != AppActionResultType.Success)
            {
                return findOpponentResult.GetVoidAppResult();
            }

            var choices = newGameInfo.ClassIds.Select((classId, index) => new ClassChoice
            {
                ClassId = classId,
                Priority = index + 1
            }).ToList();
            var newGame = new Game
            {
                BeginDate = DateTime.Now,
                Status = GameStatusIds.OpponentInvited,
                UserParticipations = new List<UserParticipation>
                {
                    new UserParticipation
                    {
                        User = this.loginManager.LoggedInUser,
                        ClassChoices = choices
                    },
                    new UserParticipation
                    {
                        User = findOpponentResult.SuccessReturnValue
                    }
                }
            };

            await this.dbContext.Games.AddAsync(newGame, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        public async Task<AppResult<List<DtoGame>>> GetGamesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var loggedInUserMail = this.loginManager.LoggedInUser.Mail;
            List<Game> games = await this.dbContext.Games.Where(_ => _.UserParticipations.Any(x => x.Mail == loggedInUserMail)).ToListAsync();
            List<DtoGame> dtoGames = new List<DtoGame>();

            foreach (Game game in games)
            {
                var opponentMail = game.UserParticipations.Where(_ => _.Mail != loggedInUserMail).Select(_ => _.Mail).ToString();
                var loggedInUserParticipation = game.UserParticipations.Single(_ => _.Mail == loggedInUserMail && _.GameId == game.Id);
                var opponentParticipation = game.UserParticipations.Single(_ => _.Mail == opponentMail && _.GameId == game.Id);

                var newDtoGame = new DtoGame
                {
                    Id = game.Id,
                    CurrentTurnPlayer = (await this.GetCurrentTurnPlayer(game.Id, cancellationToken)).SuccessReturnValue.Mail,
                    BeginDate = game.BeginDate,
                    Player = new DtoPlayer
                    {
                        Mail = loggedInUserMail,
                        Class = new DtoClass
                        {
                            Id = loggedInUserParticipation.ClassId,
                            Name = "TODO"
                        }
                    },
                    Opponent = new DtoPlayer
                    {
                        Mail = opponentMail,
                        Class = new DtoClass
                        {
                            Id = opponentParticipation.ClassId,
                            Name = "TODO"
                        }
                    }
                };
                dtoGames.Add(newDtoGame);
            }

            return AppResult<List<DtoGame>>.Success(dtoGames);
        }

        public async Task<AppResult<DtoBoard>> GetBoardAsync(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = (await this.dbContext.Games.FindEntity(gameId, cancellationToken)).SuccessReturnValue;

            return AppResult<DtoBoard>.Success(new DtoBoard
            {
                Id = Guid.NewGuid(),
                CurrentTurnPlayer = "x",    // How do I get the current player? Get move -> the one that didn't do the last move?
                PlayerHealth = 20,
                OpponentHealth = 19,
                PlayerHandCards = new List<DtoCard>
                {
                    new DtoCard
                    {
                        Id = Guid.NewGuid(),
                        Name = "Drahlget",
                        Title = "The monk",
                        Cost = 2,
                        Attack = 3,
                        Health = 2,
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
                OpponentHandCardsCount = 4,
                PlayerBoardCards = new List<DtoCard>
                {
                    new DtoCard
                    {
                        Id = Guid.NewGuid(),
                        Name = "Drahlget",
                        Title = "The monk",
                        Cost = 5,
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
                OpponentBoardCards = new List<DtoCard>
                {
                    new DtoCard
                    {
                        Id = Guid.NewGuid(),
                        Name = "Drahlget",
                        Title = "The monk",
                        Cost = 2,
                        Attack = 3,
                        Health = 4,
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
                }
            });
        }

        public async Task<VoidAppResult> EndTurnAsync(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = (await this.dbContext.Games.FindEntity(gameId, cancellationToken)).SuccessReturnValue;
            var move = new Move
            {
                Date = DateTime.Now,
                Type = MoveTypes.EndTurn,
                Game = game,
                SourceEntity = game.Entities.Single(_ => _.User.Mail == this.loginManager.LoggedInUser.Mail 
                    && _ is Player)
            };

            await this.dbContext.Moves.AddAsync(move, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        public async Task<VoidAppResult> PlayCardAsync(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = (await this.dbContext.Games.FindEntity(gameId, cancellationToken)).SuccessReturnValue;

            // TODO

            return VoidAppResult.Success();
        }

        public async Task<VoidAppResult> AttackAsync(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            var game = (await this.dbContext.Games.FindEntity(gameId, cancellationToken)).SuccessReturnValue;

            var attacker = (await this.dbContext.GameCards.FindEntity(move.AttackerId, cancellationToken)).SuccessReturnValue;
            var target = (await this.dbContext.GameCards.FindEntity(move.TargetId, cancellationToken)).SuccessReturnValue;

            var dbMove = new Move
            {
                Date = DateTime.Now,
                Game = game,
                Type = MoveTypes.Attack,
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
                attacker.CardLocation = null; // TODO: CardLocations string enum
            }

            if (target.Health <= 0)
            {
                target.CardLocation = null; // TODO: CardLocations string enum
            }

            await this.dbContext.Moves.AddAsync(dbMove, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            return VoidAppResult.Success();
        }

        private async Task<AppResult<User>> GetCurrentTurnPlayer(Guid gameId, CancellationToken cancellationToken)
        {
            var lastEndTurn = await this.dbContext.Moves
                .Where(_ => _.Type == MoveTypes.EndTurn && _.Game.Id == gameId)
                .OrderByDescending(_ => _.Date)
                .SingleAsync();

            var game = (await this.dbContext.Games.FindEntity(gameId, cancellationToken)).SuccessReturnValue;
            var currentUser = game.UserParticipations.Single(_ => _.Mail != lastEndTurn.SourceEntity.User.Mail).User;

            return AppResult<User>.Success(currentUser);
        }
    }
}
