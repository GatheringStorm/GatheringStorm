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

namespace GatheringStorm.Api.Services
{
    public interface IGamesService
    {
        Task<VoidAppResult> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken));
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

        public GamesService(ILoginManager loginManager, AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.loginManager = loginManager;
        }

        public async Task<VoidAppResult> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine(this.loginManager.LoggedInUser.Mail);

            var findOpponentResult = await this.loginManager.FindUser(newGameInfo.OpponentMail, cancellationToken);
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

        public Task<AppResult<List<DtoGame>>> GetGames(CancellationToken cancellationToken = default(CancellationToken))
        {
            var currentPlayerMail = this.loginManager.LoggedInUser.Mail;
            List<Game> games = this.dbContext.Games.Where(_ => _.UserParticipations.Any(x => x.Mail == currentPlayerMail)).ToList();
            List<DtoGame> dtoGames = new List<DtoGame>();

            foreach (Game game in games)
            {
                var opponentMail = game.UserParticipations.Where(_ => _.Mail != currentPlayerMail).Select(_ => _.Mail).ToString();

                var newDtoGame = new DtoGame
                {
                    Id = Guid.NewGuid(),
                    CurrentPlayer = "x",    // How do I get the current player? Get move -> the one that didn't do the last move?
                    BeginDate = game.BeginDate,
                    Player = new DtoPlayer
                    {
                        Mail = currentPlayerMail,
                        Class = new DtoClass
                        {
                            Id = "1", // obsolete?
                            Name = game.UserParticipations.Where(_ => _.Mail == currentPlayerMail).Select(_ => _.ClassId).ToString()
                        }
                    },
                    Opponent = new DtoPlayer
                    {
                        Mail = opponentMail,
                        Class = new DtoClass
                        {
                            Id = "2",   // obsolete?
                            Name = game.UserParticipations.Where(_ => _.Mail == opponentMail).Select(_ => _.ClassId).ToString()
                        }
                    }
                };
                dtoGames.Add(newDtoGame);
            }

            return Task.FromResult(AppResult<List<DtoGame>>.Success(dtoGames));
        }

        public Task<AppResult<DtoBoard>> GetBoard(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Game game = this.dbContext.Games.Find(gameId);

            return Task.FromResult(AppResult<DtoBoard>.Success(new DtoBoard
            {
                Id = Guid.NewGuid(),
                CurrentPlayer = "x",    // How do I get the current player? Get move -> the one that didn't do the last move?
                PlayerHealth = 20,
                OpponentHealth = 19,
                PlayerHandCards = new List<DtoCard>
                {
                    new DtoCard
                    {
                        Id = Guid.NewGuid(),
                        Name = "Drahlget",
                        Title = "The monk",
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
            }));
        }

        public Task<VoidAppResult> EndTurn(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(VoidAppResult.Success());
        }

        public Task<VoidAppResult> PlayCard(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(VoidAppResult.Success());
        }

        public Task<VoidAppResult> Attack(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(VoidAppResult.Success());
        }
    }
}
