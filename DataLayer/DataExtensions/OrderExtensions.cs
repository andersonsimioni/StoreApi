using System.Data.Entity;
using System.Data.Entity.Core;
using StoreApi.DataLayer.DataRepository.Models;

namespace StoreApi.DataLayer.DataExtensions;

public static class OrderExtensions
{
    public static IEnumerable<Order> GetOrders(this Context context){
        return context.Orders.Include(o=>o.OrderItems).ToArray();
    }

    public static IEnumerable<Order> GetOrdersByCustomerId(this Context context, uint customerId){
        if(customerId == 0) return Array.Empty<Order>();

        return context.Orders.Include(o=>o.OrderItems).Where(c=>c.CustomerId==customerId).ToArray();
    }

    public static Order? GetOrderById(this Context context, uint id){
        if(id == 0) return null;
        return context.Orders.Include(o=>o.CustomerId).FirstOrDefault(c=>c.Id==id);
    }

    public static Order? GetOrderByNumber(this Context context, ulong number){
        return context.Orders.FirstOrDefault(c=>c.Number==number);
    }

    /// <summary>
    /// Find orders with exact register date
    /// </summary>
    /// <param name="context">EF Database connection object</param>
    /// <param name="maxRegisterDate">Exact register date order can have</param>
    /// <returns>Orders with register date equals registerDate</returns>
    public static IEnumerable<Order> GetOrderByRegisterDate(this Context context, DateTime registerDate){
        if(registerDate == null) throw new NullReferenceException("Register date is null!");
        return context.Orders.Where(c=>c.RegisterDate==registerDate).ToArray();
    }

    /// <summary>
    /// Find orders with register date higher than minRegisterDate
    /// </summary>
    /// <param name="context">EF Database connection object</param>
    /// <param name="maxRegisterDate">Minimum register date order can have</param>
    /// <returns>Orders with register date higher than minRegisterDate</returns>
    public static IEnumerable<Order> GetOrderByRegisterDateFrom(this Context context, DateTime minRegisterDate){
        if(minRegisterDate == null) throw new NullReferenceException("Register date is null!");
        return context.Orders.Where(c=>c.RegisterDate >= minRegisterDate).ToArray();
    }

    /// <summary>
    /// Find orders with register date lower than maxRegisterDate
    /// </summary>
    /// <param name="context">EF Database connection object</param>
    /// <param name="maxRegisterDate">Maximum register date order can have</param>
    /// <returns>Orders with register date lower than maxRegisterDate</returns>
    public static IEnumerable<Order> GetOrderByRegisterDateTo(this Context context, DateTime maxRegisterDate){
        if(maxRegisterDate == null) throw new NullReferenceException("Register date is null!");
        return context.Orders.Where(c=>c.RegisterDate <= maxRegisterDate).ToArray();
    }

    // <summary>
    /// Save new Order on DB without any verification
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="Order">Object to save on database</param>
    /// <returns></returns>
    public static Exception? CreateOrder(this Context context, Order order){
        try
        {
            order.Id = 0;
            context.Orders.Add(order);
            context.SaveChanges();

            return null;
        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }

    /// <summary>
    /// Find the Order by ID and the update if exist
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="order">Object to save on database</param>
    /// <returns></returns>
    public static Exception? UpdateOrder(this Context context, Order order){
        try
        {
            if(order == null) throw new NullReferenceException("Order object NULL!");
            if(order.Id == 0) return new InvalidDataException("Invalid Order PRIMARY KEY ID");

            var idCache = order.Id;
            var OrderCache = context.GetOrderById(order.Id);
            if(OrderCache == null)
                throw new ObjectNotFoundException("Order not found!");

            OrderCache = order;
            OrderCache.Id = idCache;
            context.SaveChanges();

            return null;
        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }

    /// <summary>
    /// Find the Order by ID and delete from database
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="orderId">PRIMARY KEY of the Order that will be deleted</param>
    /// <returns>If the routine success, will return null, if not, will return an exception</returns>
    public static Exception? DeleteOrderById(this Context context, uint orderId){
        try
        {
            var Order = context.GetOrderById(orderId);
            if(Order == null) return new ObjectNotFoundException($"Order {orderId} not found!");

            context.Orders.Remove(Order);
            context.SaveChanges();

            return null;
        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }
}