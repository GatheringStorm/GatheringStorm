using System;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Auth;
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
        public async Task<IActionResult> StartNewGame([FromBody] DtoNewGameInfo newGameInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await this.gamesService.StartNewGame(newGameInfo, cancellationToken).ConfigureAwait(false);
            return utility.GetActionResult(result);
        }

        [HttpPost("Join")]
        public async Task<IActionResult> JoinGame([FromBody] DtoJoinGameInfo joinGameInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await this.gamesService.JoinGame(joinGameInfo, cancellationToken).ConfigureAwait(false);
            return utility.GetActionResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.gamesService.GetGamesAsync().ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }

        [HttpGet("{gameId}/Board")]
        public async Task<IActionResult> GetBoard(Guid gameId)
        {
            var result = await this.gamesService.GetBoardAsync(gameId).ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }

        [HttpPost("{gameId}/EndTurn")]
        public async Task<IActionResult> EndTurn(Guid gameId)
        {
            var result = await this.gamesService.EndTurnAsync(gameId).ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }

        [HttpPost("{gameId}/PlayCard")]
        public async Task<IActionResult> PlayCard(Guid gameId, [FromBody] DtoPlayCardMove move)
        {
            var result = await this.gamesService.PlayCardAsync(gameId, move).ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }

        [HttpPost("{gameId}/Attack")]
        public async Task<IActionResult> PlayCard(Guid gameId, [FromBody] DtoAttackMove move)
        {
            var result = await this.gamesService.AttackAsync(gameId, move).ConfigureAwait(false);
            return this.utility.GetActionResult(result);
        }
    }
}
