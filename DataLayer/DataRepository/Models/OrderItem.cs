using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace StoreApi.DataLayer.DataRepository.Models;

[Table("order_item")]
public class OrderItem{
    
    [Column("id")]
    public uint Id {get;set;}

    [Column("name")]
    public string Name {get;set;}

    [Column("amount")]
    public uint Amount {get;set;}

    [Column("unit_price")]
    public double UnitPrice {get;set;}


    [Column("order_id")]
    [ForeignKey("order_id")]
    public uint OrderId {get;set;}
    public Order? Order {get;set;}
}