using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Auth;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.Dto;
using GatheringStorm.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GatheringStorm.Api.Controllers
{
    [Route("api/[controller]")]
    [ValidateGoogleLogin]
    public class GamesController : Controller
    {
        private readonly IGamesService gamesService;
        private readonly IControllerUtility utility;

        public GamesController(IGamesService gamesService, IControllerUtility utility)
        {
            this.gamesService = gamesService;
            this.utility = utility;
        }

        [HttpPost("New")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorActionResultContent), 400)]
        public async Task<IActionResult> StartNewGame([FromBody] DtoNewGameInfo newGameInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await this.gamesService.StartNewGame(newGameInfo, cancellationToken).ConfigureAwait(false);
            return utility.GetActionResult(result);
        }

        [HttpPost("Join")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorActionResultContent), 400)]
        public async Task<IActionResult> JoinGame([FromBody] DtoJoinGameInfo joinGameInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await this.gamesService.JoinGame(joinGameInfo, cancellationToken).ConfigureAwait(false);
            return utility.GetActionResult(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DtoGame>), 200)]
        [ProducesResponseType(typeof(ErrorActionResultContent), 400)]
        public async Task<IActionResult> Get()
        {
            var result = await this.gamesService.GetGames().ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }

        [HttpGet("{gameId}/Board")]
        [ProducesResponseType(typeof(DtoBoard), 200)]
        [ProducesResponseType(typeof(ErrorActionResultContent), 400)]
        public async Task<IActionResult> GetBoard(Guid gameId)
        {
            var result = await this.gamesService.GetBoard(gameId).ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }

        [HttpPost("{gameId}/EndTurn")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorActionResultContent), 400)]
        public async Task<IActionResult> EndTurn(Guid gameId)
        {
            var result = await this.gamesService.EndTurn(gameId).ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }

        [HttpPost("{gameId}/PlayCard")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorActionResultContent), 400)]
        public async Task<IActionResult> PlayCard(Guid gameId, [FromBody] DtoPlayCardMove move)
        {
            var result = await this.gamesService.PlayCard(gameId, move).ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }

        [HttpPost("{gameId}/Attack")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorActionResultContent), 400)]
        public async Task<IActionResult> Attack(Guid gameId, [FromBody] DtoAttackMove move)
        {
            var result = await this.gamesService.Attack(gameId, move).ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }
    }
}
