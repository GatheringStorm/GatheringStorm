using System;
using System.Collections.Generic;
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
        Task<AppResult<DtoGame>> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken));
        Task<AppResult<List<DtoGame>>> GetGames(CancellationToken cancellationToken = default(CancellationToken));
        Task<AppResult<DtoBoard>> GetBoard(Guid gameId, CancellationToken cancellationToken = default(CancellationToken));
        Task<AppResult> EndTurn(Guid gameId, CancellationToken cancellationToken = default(CancellationToken));
        Task<AppResult> PlayCard(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken));
        Task<AppResult> Attack(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken));
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

        public Task<AppResult<DtoGame>> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine(this.loginManager.LoggedInUser.Mail);

            return Task.FromResult(new AppResult<DtoGame>(new DtoGame
            {
                Id = Guid.NewGuid(),
                CurrentPlayer = "opponent@gmail.com",
                BeginDate = DateTime.Now,
                Player = new DtoPlayer
                {
                    Mail = "you@gmail.com",
                    Class = new DtoClass
                    {
                        Id = ClassTypes.Swift,
                        Name = "Schnell"
                    }
                },
                Opponent = new DtoPlayer
                {
                    Mail = "opponent@gmail.com",
                    Class = null
                }
            }));
        }

        public Task<AppResult<List<DtoGame>>> GetGames(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new AppResult<List<DtoGame>>(new List<DtoGame>
            {
                new DtoGame
                {
                    Id = Guid.NewGuid(),
                    CurrentPlayer = "opponent@gmail.com",
                    BeginDate = DateTime.Now,
                    Player = new DtoPlayer
                    {
                        Mail = "you@gmail.com",
                        Class = new DtoClass
                        {
                            Id = ClassTypes.Swift,
                            Name = "Schnell"
                        }
                    },
                    Opponent = new DtoPlayer
                    {
                        Mail = "opponent@gmail.com",
                        Class = new DtoClass
                        {
                            Id = ClassTypes.Tank,
                            Name = "Langsam"
                        }
                    }
                }
            }));
        }

        public Task<AppResult<DtoBoard>> GetBoard(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new AppResult<DtoBoard>(new DtoBoard
            {
                Id = Guid.NewGuid(),
                CurrentPlayer = "you@gmail.com",
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

        public Task<AppResult> EndTurn(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new AppResult(AppActionResultType.RuleError));
        }

        public Task<AppResult> PlayCard(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new AppResult(AppActionResultType.Success));
        }

        public Task<AppResult> Attack(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new AppResult(AppActionResultType.Success));
        }
    }
}
