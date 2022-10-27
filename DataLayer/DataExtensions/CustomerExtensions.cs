using System.Data.Entity.Core;
using StoreApi.DataLayer.DataRepository.Models;

namespace StoreApi.DataLayer.DataExtensions;

public static class CustomerExtensions
{
    public static IEnumerable<Customer> ListCustomers(this Context context){
        return context.Customers.ToArray();
    }

    public static Customer? GetCustomerById(this Context context, uint id){
        if(id == 0) return null;
        return context.Customers.FirstOrDefault(c=>c.Id==id);
    }

    public static Customer? GetCustomerByEmail(this Context context, string email, bool sensitiveCase = false){
        if(string.IsNullOrEmpty(email)) throw new NullReferenceException("Email is null or empty!");
        return context.Customers.FirstOrDefault(c=>sensitiveCase ? (c.Email==email) : (c.Email.ToLower()==email.ToLower()));
    }

    public static IEnumerable<Customer> GetCustomerByName(this Context context, string name, bool sensitiveCase = false){
        if(string.IsNullOrEmpty(name)) throw new NullReferenceException("Name is null or empty!");
        return context.Customers.Where(c=>sensitiveCase ? (c.Name==name) : (c.Name.ToLower()==name.ToLower())).ToArray();
    }

    /// <summary>
    /// Find the Customer by ID and delete from database
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="customerId">PRIMARY KEY of the Customer that will be deleted</param>
    /// <returns>If the routine success, will return null, if not, will return an exception</returns>
    public static Exception? DeleteCustomerById(this Context context, uint customerId){
        try
        {
            var customer = context.GetCustomerById(customerId);
            if(customer == null) return new ObjectNotFoundException("Customer not found!");

            context.Customers.Remove(customer);
            context.SaveChanges();

            return null;
        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }

    /// <summary>
    /// Check if there are any Customer with same email,
    /// if not, create a new Customer
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="Customer">Customer object to be added to the database</param>
    /// <returns>If the routine success, will return null, if not, will return an exception</returns>
    public static Exception? CreateCustomer(this Context context, Customer customer){
        try
        {
            if(customer == null) 
                throw new NullReferenceException("Customer object NULL!");
            if(context.GetCustomerByEmail(customer.Email) != null)
                return new DuplicateWaitObjectException("Email already in use");

            customer.Id = 0;
            context.Customers.Add(customer);
            context.SaveChanges();

            return null;
        }   
        catch (System.Exception ex)
        {
            return ex;
        }
    }

    /// <summary>
    /// Find the Customer by ID and then update it if exist
    /// </summary>
    /// <param name="context">Database connection context object</param>
    /// <param name="Customer">Customer object to be update to the database</param>
    /// <returns>If the routine success, will return null, if not, will return an exception</returns>
    public static Exception? UpdateCustomer(this Context context, Customer customer){
        try
        {
            if(customer == null) throw new NullReferenceException("Customer object NULL!");
            if(customer.Id == 0) return new InvalidDataException("Invalid Customer PRIMARY KEY ID");

            var idCache = customer.Id;
            var customerCache = context.GetCustomerById(customer.Id);
            if(customerCache == null)
                throw new ObjectNotFoundException("Customer not found!");

            customerCache = customer;
            customerCache.Id = idCache;
            context.SaveChanges();

            return null;
        }
        catch (System.Exception ex)
        {
            return ex;
        }
    }
}