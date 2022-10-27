using System.Data.Entity.Core;
using StoreApi.DataLayer.DataRepository.Models;

namespace StoreApi.DataLayer.DataExtensions;

public static class ItemExtensions
{
    /// <summary>
    /// Find items by id or return all items
    /// </summary>
    /// <param name="context">EF Connection object</param>
    /// <param name="ids">Optional param to filter by id list</param>
    /// <returns>Items list, if set idList will filter by contains values</returns>
    public static IEnumerable<Item> GetItems(this Context context, params uint[] idList){
        if(idList.Any())
            return context.Items.Where(i=>idList.Contains(i.Id)).ToArray();
        else
            return context.Items.ToArray();
    }

    public static Item? GetItemById(this Context context, uint id){
        if(id == 0) return null;

        return context.Items.FirstOrDefault(c=>c.Id==id);
    }

    public static Item? GetItemByUnitPrice(this Context context, double unitPrice){
        return context.Items.FirstOrDefault(c=>c.UnitPrice==unitPrice);
    }

    public static IEnumerable<Item> GetItemByName(this Context context, string name){
        return context.Items.Where(c=>c.Name==name).ToArray();
    }

    /// <summary>
    /// Save new item on DB without any verification
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="item">Object to save on database</param>
    /// <returns></returns>
    public static Exception? CreateItem(this Context context, Item item){
        try
        {
            item.Id = 0;
            context.Items.Add(item);
            context.SaveChanges();

            return null;
        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }

    /// <summary>
    /// Find the item by ID and the update if exist
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="item">Object to save on database</param>
    /// <returns></returns>
    public static Exception? UpdateItem(this Context context, Item item){
        try
        {
            if(item == null) throw new NullReferenceException("item object NULL!");
            if(item.Id == 0) return new InvalidDataException("Invalid item PRIMARY KEY ID");

            var idCache = item.Id;
            var itemCache = context.GetItemById(item.Id);
            if(itemCache == null)
                throw new ObjectNotFoundException("item not found!");

            itemCache = item;
            itemCache.Id = idCache;
            context.SaveChanges();

            return null;
        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }

    /// <summary>
    /// Find the Item by ID and delete from database
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="itemId">PRIMARY KEY of the item that will be deleted</param>
    /// <returns>If the routine success, will return null, if not, will return an exception</returns>
    public static Exception? DeleteItemById(this Context context, uint itemId){
        try
        {
            var item = context.GetItemById(itemId);
            if(item == null) return new ObjectNotFoundException($"Item {itemId} not found!");

            context.Items.Remove(item);
            context.SaveChanges();

            return null;
        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }
}