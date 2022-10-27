using System.Text.Json.Serialization;

namespace StoreApi.Shared.Structs.DataLayerAbstractions;

public class OrderDTO
{
    [JsonPropertyName("CustomerId")]
    public uint CustomerId {get;set;}

    [JsonPropertyName("OrderNumber")]
    public ulong OrderNumber {get;set;}

    [JsonPropertyName("RegisterDate")]
    public DateTime RegisterDate {get;set;}
}
