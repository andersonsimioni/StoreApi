using System.Text.Json.Serialization;

namespace StoreApi.Shared.Structs.DataLayerAbstractions;

public class ItemDTO
{
    [JsonPropertyName("Id")]
    public uint Id {get;set;}

    [JsonPropertyName("Name")]
    public string Name {get;set;}

    [JsonPropertyName("UnitPrice")]
    public double UnitPrice {get;set;}
}
