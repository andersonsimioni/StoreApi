using System.Text.Json.Serialization;

namespace StoreApi.Shared.Structs.ApiDTOs;

public class CreateCustomerDTO
{
    [JsonPropertyName("Name")]
    public string Name {get;set;}
    
    [JsonPropertyName("Email")]
    public string Email {get;set;}
}
