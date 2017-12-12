using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GatheringStorm.Api.Models.Dto;

namespace GatheringStorm.Api.Services
{
    public interface IGamesService
    {
        Task<DtoGame> StartNewGame(DtoNewGameInfo newGameInfo);
        Task<List<DtoGame>> GetGames();
        Task<DtoBoard> GetBoard(Guid gameId);
        Task EndTurn(Guid gameId);
        Task PlayCard(Guid gameId, DtoPlayCardMove move);
        Task Attack(Guid gameId, DtoAttackMove move);
    }

    public class GamesService : IGamesService
    {
        public Task<DtoGame> StartNewGame(DtoNewGameInfo newGameInfo)
        {
            return Task.FromResult(new DtoGame
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
            });
        }

        public Task<List<DtoGame>> GetGames()
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

        public Task<DtoBoard> GetBoard(Guid gameId)
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

        public Task EndTurn(Guid gameId)
        {
            return Task.CompletedTask;
        }

        public Task PlayCard(Guid gameId, DtoPlayCardMove move)
        {
            return Task.CompletedTask;
        }

        public Task Attack(Guid gameId, DtoAttackMove move)
        {
            return Task.CompletedTask;
        }
    }
}
