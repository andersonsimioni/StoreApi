using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace StoreApi.DataLayer.DataRepository.Models;

[Table("order")]
public class Order
{
    [Column("id")]
    public uint Id { get; set; }

    [Column("number")]
    public ulong Number { get; set; }

    [Column("register_date")]
    public DateTime RegisterDate { get; set; }


    [Column("customer_id")]
    [ForeignKey("customer_id")]
    public uint CustomerId {get;set;}
    public Customer? Customer {get;set;}


    public ICollection<OrderItem>? OrderItems {get;set;}
}