﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models.Dto;
using GatheringStorm.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GatheringStorm.Api.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        public readonly IGamesService gamesService;

        public GamesController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        [HttpPost("New")]
        public async Task<IActionResult> StartNewGame([FromBody] DtoNewGameInfo newGameInfo)
        {
            return new OkObjectResult(await this.gamesService.StartNewGame(newGameInfo).ConfigureAwait(false));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(await this.gamesService.GetGames().ConfigureAwait(false));
        }

        [HttpGet("{gameId}/Board")]
        public async Task<IActionResult> GetBoard(Guid gameId)
        {
            return new OkObjectResult(await this.gamesService.GetBoard(gameId).ConfigureAwait(false));
        }

        [HttpPost("{gameId}/EndTurn")]
        public async Task<IActionResult> EndTurn(Guid gameId)
        {
            await this.gamesService.EndTurn(gameId).ConfigureAwait(false);
            return new OkResult();
        }

        [HttpPost("{gameId}/PlayCard")]
        public async Task<IActionResult> PlayCard(Guid gameId, [FromBody] DtoPlayCardMove move)
        {
            await this.gamesService.PlayCard(gameId, move).ConfigureAwait(false);
            return new OkResult();
        }

        [HttpPost("{gameId}/Attack")]
        public async Task<IActionResult> PlayCard(Guid gameId, [FromBody] DtoAttackMove move)
        {
            await this.gamesService.Attack(gameId, move).ConfigureAwait(false);
            return new OkResult();
        }
    }
}
