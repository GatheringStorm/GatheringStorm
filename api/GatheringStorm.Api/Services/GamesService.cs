using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Auth;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.Dto;

namespace GatheringStorm.Api.Services
{
    public interface IGamesService
    {
        Task<AppActionResult<DtoGame>> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<DtoGame>> GetGames(CancellationToken cancellationToken = default(CancellationToken));
        Task<DtoBoard> GetBoard(Guid gameId, CancellationToken cancellationToken = default(CancellationToken));
        Task EndTurn(Guid gameId, CancellationToken cancellationToken = default(CancellationToken));
        Task PlayCard(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken));
        Task Attack(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class GamesService : IGamesService
    {
        private readonly ILoginManager loginManager;

        public GamesService(ILoginManager loginManager)
        {
            this.loginManager = loginManager;
        }

        public Task<AppActionResult<DtoGame>> StartNewGame(DtoNewGameInfo newGameInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine(this.loginManager.LoggedInUser.Mail);

            return Task.FromResult(new AppActionResult<DtoGame>(new DtoGame
            {
                Id = Guid.NewGuid(),
                CurrentPlayer = "opponent@gmail.com",
                BeginDate = DateTime.Now,
                Player = new DtoPlayer
                {
                    Mail = "you@gmail.com",
                    Class = new DtoClass
                    {
                        Id = 1,
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

        public Task<List<DtoGame>> GetGames(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new List<DtoGame>
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
                            Id = 1,
                            Name = "Schnell"
                        }
                    },
                    Opponent = new DtoPlayer
                    {
                        Mail = "opponent@gmail.com",
                        Class = new DtoClass
                        {
                            Id = 3,
                            Name = "Langsam"
                        }
                    }
                }
            });
        }

        public Task<DtoBoard> GetBoard(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new DtoBoard
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
            });
        }

        public Task EndTurn(Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public Task PlayCard(Guid gameId, DtoPlayCardMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public Task Attack(Guid gameId, DtoAttackMove move, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}
