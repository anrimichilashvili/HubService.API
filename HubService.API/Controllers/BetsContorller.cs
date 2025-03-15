
using HubService.Application.Dtos;
using HubService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace HubService.API.Controllers
{
    [Authorize]
    public class BetsContorller : ODataBaseController
    {
        private readonly IBetService _betService;

        public BetsContorller(IBetService betService)
        {
            _betService = betService;
        }


        [EnableQuery(MaxExpansionDepth = 5)]
        [HttpGet("odata/Bets")]
        public async Task<IActionResult> GetAllBets()
        {
            return Ok(DataContext.Bets.AsNoTracking().AsQueryable());
        }

        [HttpPost("odata/Bets/Create")]
        public async Task<IActionResult> CreateBet([FromBody] CreateBetDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _betService.CreateBetAsync(request.Amount);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);

        }
    }
}
