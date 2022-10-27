using System.Text.Json.Serialization;

namespace StoreApi.Shared.Structs.DataLayerAbstractions;

public class OrderItemDTO
{
    [JsonPropertyName("OrderId")]
    public uint OrderId {get;set;}

    [JsonPropertyName("Name")]
    public string? Name {get;set;}

    [JsonPropertyName("Amount")]
    public double Amount {get;set;}

    [JsonPropertyName("UnitPrice")]
    public double UnitPrice {get;set;}
}
