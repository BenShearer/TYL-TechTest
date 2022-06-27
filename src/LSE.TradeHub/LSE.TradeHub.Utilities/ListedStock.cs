using System.Text.Json.Serialization;

namespace LSE.TradeHub.Utilities;

public partial class FTSE100Reader
{
    internal class ListedStock {
        [JsonPropertyName("symbol")] public string Symbol { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
    }
}
