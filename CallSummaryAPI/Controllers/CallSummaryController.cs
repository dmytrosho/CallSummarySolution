using CallSummaryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CallSummaryAPI.Controllers
{
    [Route("demo")]
    [ApiController]
    public class CallSummaryController : ControllerBase
    {
        private readonly CallSummaryService _callSummaryService;
        
        public CallSummaryController(CallSummaryService callSummaryService)
        {
            _callSummaryService = callSummaryService;
        }

        [HttpGet("call-summary")]
        public async Task<IActionResult> GetCallSummary([FromQuery] DateTime date)
        {
            var summary = await _callSummaryService.GetCallSummary(date);
            return Ok(summary);
        }
    }
}
