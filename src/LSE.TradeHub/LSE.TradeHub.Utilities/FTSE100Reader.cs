using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using LSE.TradeHub.Core.Models;

namespace LSE.TradeHub.Utilities;

public class FTSE100Reader : IStockLoader {
    private const string RESOURCE_NAME = "LSE.TradeHub.Utilities.Data.FTSE100-list.json";

    public Stock[] GetStockList() {
        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME)) {
            using (var reader = new StreamReader(stream)) {
                var fileContent = reader.ReadToEnd();
                if (fileContent != null) {
                    return JsonSerializer.Deserialize<ListedStock[]>(fileContent).Select(s => new Stock { Id = s.Symbol, Name = s.Name }).ToArray();
                }

                return null;
            }
        }
    }

    class ListedStock {
        [JsonPropertyName("symbol")] public string Symbol { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
    }
}
