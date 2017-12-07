using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace GatheringStorm.Api.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        public readonly AppDbContext dbContext;

        public GamesController(AppDbContext dbContext)
        {
            // TODO: USE GAMES SERVICE
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            throw new NotImplementedException();
        }
    }
}
