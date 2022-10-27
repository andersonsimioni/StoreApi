using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace StoreApi.DataLayer.DataRepository.Models;

[Table("customer")]
public class Customer
{
    [Column("id")]
    public uint Id { get; set; }

    [NotNull]
    [MaxLength(50)]
    [Column("name")]
    public string Name { get; set; }

    [NotNull]
    [MaxLength(50)]
    [Column("email")]
    public string Email { get; set; }


    public ICollection<Order>? Orders {get;set;}
}