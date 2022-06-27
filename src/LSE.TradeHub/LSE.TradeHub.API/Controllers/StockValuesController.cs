using AutoMapper;
using LSE.TradeHub.API.Models.Response;
using LSE.TradeHub.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LSE.TradeHub.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StockValuesController : ControllerBase {
    private readonly ITradeRecordService tradeRecordService;
    private readonly IMapper mapper;
    private readonly ILogger logger;

    public StockValuesController(
        ITradeRecordService tradeRecordService, 
        IMapper mapper, 
        ILogger logger) {
        this.tradeRecordService = tradeRecordService;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(StockValue[]))]
    public async Task<IActionResult> GetAllStockValues() {
        try {
            var stockValues = tradeRecordService.GetAllStockMeanValues();

            var result = mapper.Map<StockValue[]>(stockValues.ToList());
            return Ok(result);
        } catch (Exception ex) {
            logger.Log(LogLevel.Error, "Error retreiving stock values", ex);
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("{symbol}")]
    [ProducesResponseType(200, Type = typeof(StockValue))]
    public async Task<IActionResult> GetStockValuesBySymbol([FromRoute] string symbol) {
        if (string.IsNullOrEmpty(symbol)) {
            return BadRequest();
        }

        try {
            var stockValue = tradeRecordService.GetMeanValueBySymbol(symbol);

            if (stockValue == null) {
                return NotFound();
            }

            var result = mapper.Map<StockValue>(stockValue);
            return Ok(result);
        } catch (Exception ex) {
            logger.Log(LogLevel.Error, $"Error retreiving stock value for {symbol}", ex);
            return StatusCode(500);
        }
    }
}
