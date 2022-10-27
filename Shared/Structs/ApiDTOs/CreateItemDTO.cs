using System.Text.Json.Serialization;

namespace StoreApi.Shared.Structs.DataLayerAbstractions;

public class CreateItemDTO
{
    [JsonPropertyName("Name")]
    public string Name {get;set;}

    [JsonPropertyName("UnitPrice")]
    public double UnitPrice {get;set;}
}
