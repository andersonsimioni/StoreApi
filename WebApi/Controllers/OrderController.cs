using System.Data.Entity.Core;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreApi.DataLayer.DataExtensions;
using StoreApi.DataLayer.DataRepository.Models;
using StoreApi.Shared.Services.ExceptionSecurityChecker;
using StoreApi.Shared.Structs.DataLayerAbstractions;

namespace StoreApi.WebApi.Controllers;

public class OrderController : Controller{
    private readonly Context _storeApiContext;
    private readonly IMapper _autoMapper;
    private readonly ILogger<OrderController> _logger;
    private readonly IExceptionSecurityChecker _exceptionSecurity;

    public OrderController(
        Context storeApiContext,
        IMapper autoMapper, 
        ILogger<OrderController> logger,
        IExceptionSecurityChecker exceptionSecurity)
    {
        this._logger = logger;
        this._autoMapper = autoMapper;
        this._storeApiContext = storeApiContext;
        this._exceptionSecurity = exceptionSecurity;
    }

    [HttpGet("GetOrderById")]
    public IActionResult GetOrderById([FromQuery] uint OrderId){
        try
        {
            var order = _storeApiContext.GetOrderById(OrderId);
            if(order == null) return NotFound($"Order {OrderId} not found!");
            var mapped = _autoMapper.Map<OrderDTO>(order);

            return Ok(mapped);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on GET Order by ID");
            return BadRequest("Internal Server Error");
        }
    }

    /// <summary>
    /// Find on database for items
    /// </summary>
    /// <param name="customerId">Optional parameter for filter by order id</param>
    /// <returns>if orderId is not null, will return by customerId filtered, if yes, will return all registers</returns>
    [HttpGet("ListOrders")]  
    public IActionResult ListOrders([FromQuery] uint? customerId){
        try
        {
            var items = customerId != null ? _storeApiContext.GetOrdersByCustomerId(customerId.Value) : _storeApiContext.GetOrders();
            var mapped = items.Select(i=>_autoMapper.Map<OrderDTO>(i));
            return Ok(mapped);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on GET item by ID");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpPost("CreateOrder")]
    public IActionResult CreateOrder([FromBody] CreateOrderDTO order){
        try
        {
            if(order.Items == null || !order.Items.Any() || order.Items.Sum(i=>i.Amount)==0) 
                return BadRequest("Order without items!");

            var items = _storeApiContext.GetItems(order.Items.Select(i=>i.ItemId).ToArray());
            if(!items.Any()) return NotFound("Items not found!");

            var customer = _storeApiContext.GetCustomerById(order.CustomerId);
            if(customer == null) 
                return BadRequest("Customer invalid!");

            var orderItems = items.Select(i=>{
                var ret = _autoMapper.Map<OrderItem>(i);
                ret.Amount = (uint)order.Items.Where(i2=>i2.ItemId == i.Id).Sum(i2=>i2.Amount);
                ret.UnitPrice = i.UnitPrice;
                ret.Name = i.Name;
                return ret;
            }).ToArray();

            var orderModel = _autoMapper.Map<Order>(order);
            orderModel.OrderItems = orderItems;
            var result = _storeApiContext.CreateOrder(orderModel);

            if(result != null)
                if(_exceptionSecurity.IsSecure(result))
                    return BadRequest(result.Message);
                else 
                    throw result;

            return Ok($"Success on create order {orderModel.Id}");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on POST Order");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpDelete("DeleteOrderById")]
    public IActionResult DeleteOrderById([FromQuery] uint OrderId)
    {
        try
        {
            var result = _storeApiContext.DeleteOrderById(OrderId);

            if(result != null)
                if(result is ObjectNotFoundException)
                    return NotFound($"Order {OrderId} not found in our database, sorry.");
                else
                    throw result;

            return Ok($"Order {OrderId} removed with success!");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on DELETE Order by ID");
            return BadRequest("Internal Server Error");
        }
    }

}