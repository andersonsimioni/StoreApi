using StoreApi.DataLayer.DataRepository.Models;

namespace StoreApi.DataLayer.DataExtensions;

public static class OrderItemExtensions{

    public static void CreateOrderItems(this Context context, params OrderItem[] orderItems){
        if(orderItems != null && orderItems.Any())
        {
            context.OrderItems.AddRange(orderItems.ToArray());
            context.SaveChanges();
        }
    }

    

}