using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using StoreApi.Shared.Settings;

namespace StoreApi.DataLayer.DataRepository.Models;

public class Context : DbContext
{
    public DbSet<Item> Items {get;set;}
    public DbSet<Order> Orders {get;set;}
    public DbSet<Customer> Customers {get;set;}
    public DbSet<OrderItem> OrderItems {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        var configuration = Shared.Settings.ConfigurationProvider.GetConfiguration();
        var connectionString = configuration.GetValue<string>("ConnectionStrings:StoreApi");
        optionsBuilder.UseSqlServer(connectionString);
    }

    public Context(){}
    public Context(DbContextOptions options) : base(options){}
}