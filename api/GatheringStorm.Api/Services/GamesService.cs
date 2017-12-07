using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GatheringStorm.Api.Models.Dto;

namespace GatheringStorm.Api.Services
{
    public interface IGamesService
    {
        Task<List<DtoGame>> GetGames();
    }

    public class GamesService : IGamesService
    {
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
    }
}