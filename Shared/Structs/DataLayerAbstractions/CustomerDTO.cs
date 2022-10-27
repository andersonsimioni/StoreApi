using System.Text.Json.Serialization;

namespace StoreApi.Shared.Structs.DataLayerAbstractions;

public class CustomerDTO
{
    [JsonPropertyName("Id")]
    public uint Id {get;set;}

    [JsonPropertyName("Name")]
    public string Name {get;set;}
    
    [JsonPropertyName("Email")]
    public string Email {get;set;}
}
