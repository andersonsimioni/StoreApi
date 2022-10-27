using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StoreApi.DataLayer.DataRepository.Models;

[Table("item")]
public class Item
{
    [Column("id")]
    public uint Id { get; set; }

    [MaxLength(50)]
    [Column("name")]
    public string Name { get; set; }

    [Column("unit_price")]
    public double UnitPrice { get; set; }


    public ICollection<OrderItem>? OrderItems {get;set;}
}