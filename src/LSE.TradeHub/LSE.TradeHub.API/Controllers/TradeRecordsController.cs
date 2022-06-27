using AutoMapper;
using LSE.TradeHub.API.Models.Request;
using LSE.TradeHub.Core.Interfaces;
using LSE.TradeHub.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace LSE.TradeHub.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TradeRecordsController : ControllerBase {
    private readonly ITradeRecordService tradeRecordService;
    private readonly ISystemClock clock;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public TradeRecordsController(
        ITradeRecordService tradeRecordService, 
        ISystemClock clock, 
        IMapper mapper, 
        ILogger logger) {
        this.tradeRecordService = tradeRecordService;
        this.clock = clock;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpPost]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> AddTradeRecord([FromBody] SaleRecord record) {
        if (record == null || !ModelState.IsValid) {
            return BadRequest("Unable to deserialise request");
        }

        try {
            var model = mapper.Map<TradeRecord>(record);
            model.Timestamp = clock.UtcNow;
            await tradeRecordService.Create(model);
            return Ok();
        } catch (DbUpdateException ex) {
            logger.Log(LogLevel.Information, ex.Message, record);
            return BadRequest();
        } catch (Exception e) {
            logger.Log(LogLevel.Error, e.Message, e);
            return StatusCode(500);
        }
    }
}
