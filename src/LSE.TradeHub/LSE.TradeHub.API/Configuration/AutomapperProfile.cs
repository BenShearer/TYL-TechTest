using AutoMapper;
using LSE.TradeHub.API.Models.Request;
using LSE.TradeHub.API.Models.Response;
using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.API.Configuration {
    public class AutomapperProfile : Profile {
        public AutomapperProfile() {
            CreateMap<KeyValuePair<string, decimal>, StockValue>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(x => x.Key))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(x => x.Value));

            CreateMap<SaleRecord, TradeRecord>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(x => x.TotalSaleValue / x.Quantity))
                .ForMember(dest => dest.StockId, opt => opt.MapFrom(x => x.StockSymbol));
        }
    }
}
