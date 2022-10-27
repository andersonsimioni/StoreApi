using System.Text.Json.Serialization;
using StoreApi.Shared.Structs.ApiDTOs;

namespace StoreApi.Shared.Structs.DataLayerAbstractions;

public class CreateOrderDTO
{
    public class OrderItemDTO{

        [JsonPropertyName("ItemId")]
        public uint ItemId {get;set;}
        
        [JsonPropertyName("Amount")]
        public uint Amount {get;set;}
    }

    [JsonPropertyName("CustomerId")]
    public uint CustomerId {get;set;}

    [JsonPropertyName("Number")]
    public ulong Number {get;set;}

    [JsonPropertyName("RegisterDate")]
    public DateTime RegisterDate {get;set;}
    
    [JsonPropertyName("Items")]
    public ICollection<OrderItemDTO>? Items {get;set;}
}
