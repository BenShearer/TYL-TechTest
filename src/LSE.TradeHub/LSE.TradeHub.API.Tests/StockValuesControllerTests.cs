using AutoMapper;

using LSE.TradeHub.API.Configuration;
using LSE.TradeHub.API.Controllers;
using LSE.TradeHub.API.Models.Response;
using LSE.TradeHub.Core.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

using Shouldly;

namespace LSE.TradeHub.API.Tests;

public class StockValuesControllerTests {
    private readonly Mock<ITradeRecordService> tradeRecordServiceMock;
    private readonly Mock<ILogger<StockValuesController>> loggerMock;
    private readonly IMapper mapper;

    private const string TEST_SYMBOL_1 = "TEST_1";
    private const string TEST_SYMBOL_2 = "TEST_2";
    private List<KeyValuePair<string, decimal>> TestValues =>
        new() {
            new KeyValuePair<string, decimal>(TEST_SYMBOL_1, (decimal) 101.1),
            new KeyValuePair<string, decimal>(TEST_SYMBOL_2, (decimal) 102.2)
        };

    public StockValuesControllerTests() {
        tradeRecordServiceMock = new Mock<ITradeRecordService>();
        loggerMock = new Mock<ILogger<StockValuesController>>();

        mapper = new MapperConfiguration(c =>
            c.AddProfile<AutomapperProfile>()).CreateMapper();
    }

    [Fact]
    public async Task GetAllStockValues_Maps_Values_Correctly() {
        tradeRecordServiceMock
            .Setup(svc => svc.GetAllStockMeanValues())
            .Returns(TestValues.ToDictionary(x => x.Key, y => y.Value));

        var controller = ConstructController();
        var result = await controller.GetAllStockValues();

        result.ShouldBeOfType<OkObjectResult>();

        var objectResult = result as OkObjectResult;

        objectResult.Value.ShouldBeOfType<StockValue[]>();

        var values = objectResult.Value as StockValue[];

        values.Length.ShouldBe(TestValues.Count);

        foreach (var testValue in TestValues) {
            values.Any(x =>
                    x.Symbol == testValue.Key && x.Value == testValue.Value)
                .ShouldBeTrue();
        }
    }

    [Fact]
    public async Task GetStockValuesBySymbol_CallsServiceCorrectly() {
        tradeRecordServiceMock
            .Setup(svc => svc.GetMeanValueBySymbol(TEST_SYMBOL_1))
            .Returns(TestValues[0])
            .Verifiable();

        var controller = ConstructController();
        var result = await controller.GetStockValuesBySymbol(TEST_SYMBOL_1);

        result.ShouldBeOfType<OkObjectResult>();

        var objectResult = result as OkObjectResult;

        objectResult.Value.ShouldBeOfType<StockValue>();

        var value = objectResult.Value as StockValue;
        value.Symbol.ShouldBe(TEST_SYMBOL_1);
        value.Value.ShouldBe((decimal) 101.1);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetStockValuesBySymbol_With_BadSymbol_Returns_BadRequest(string symbol) {
        var controller = ConstructController();
        var result = await controller.GetStockValuesBySymbol(symbol);

        result.ShouldBeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task GetStockValuesBySymbol_With_NonExistentSymbol_Returns_NotFOund() {
        tradeRecordServiceMock
            .Setup(svc => svc.GetMeanValueBySymbol(TEST_SYMBOL_1))
            .Returns<KeyValuePair<string, decimal> ?>(null);
        var controller = ConstructController();
        var result = await controller.GetStockValuesBySymbol(TEST_SYMBOL_1);

        result.ShouldBeOfType<NotFoundResult>();
    }

    private StockValuesController ConstructController() {
        return new StockValuesController(
            tradeRecordServiceMock.Object,
            mapper,
            loggerMock.Object
        );
    }
}