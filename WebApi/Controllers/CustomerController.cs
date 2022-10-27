using System.Data.Entity.Core;
using AutoMapper;
using DelegateDecompiler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.DataLayer.DataExtensions;
using StoreApi.DataLayer.DataRepository.Models;
using StoreApi.Shared.Services.ExceptionSecurityChecker;
using StoreApi.Shared.Structs.ApiDTOs;
using StoreApi.Shared.Structs.DataLayerAbstractions;

namespace StoreApi.WebApi.Controllers;

public class CustomerController : Controller
{
    private readonly Context _storeApiContext;
    private readonly IMapper _autoMapper;
    private readonly ILogger<CustomerController> _logger;
    private readonly IExceptionSecurityChecker _exceptionSecurity;

    public CustomerController(
        Context storeApiContext,
        IMapper autoMapper, 
        ILogger<CustomerController> logger,
        IExceptionSecurityChecker exceptionSecurity)
    {
        this._logger = logger;
        this._autoMapper = autoMapper;
        this._storeApiContext = storeApiContext;
        this._exceptionSecurity = exceptionSecurity;
    }

    [HttpGet("GetCustomerById")]
    public IActionResult GetCustomerById([FromQuery] uint customerId)
    {
        try
        {
            var customer = _storeApiContext.GetCustomerById(customerId);
            if (customer == null) return NotFound($"Customer {customerId} not found in our database, sorry.");
            var mapperd = _autoMapper.Map<CustomerDTO>(customer);
            return Ok(customer);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on GET customer by ID");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpGet("ListCustomers")]
    public IActionResult ListCustomers()
    {
        try
        {
            var customers = _storeApiContext.ListCustomers();
            var mapped = customers.Select(c=>_autoMapper.Map<CustomerDTO>(c));
            return Ok(mapped);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on GET customers");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpPost("CreateCustomer")]
    public IActionResult CreateCustomer([FromBody] CreateCustomerDTO customer)
    {
        try
        {
            var customerModel = _autoMapper.Map<Customer>(customer);
            var result = _storeApiContext.CreateCustomer(customerModel);
            if((result != null) && _exceptionSecurity.IsSecure(result))
                return BadRequest();

            return Ok($"Customer {customerModel.Id} created with success!");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on POST customer");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpPut("UpdateCustomer")]
    public IActionResult UpdateCustomer([FromBody] CustomerDTO customer)
    {
        try
        {
            var customerModel = _autoMapper.Map<Customer>(customer);
            var result = _storeApiContext.UpdateCustomer(customerModel);
            if((result != null) && _exceptionSecurity.IsSecure(result))
                return BadRequest(result.Message);

            return Ok("Customer updated with success!");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on PUT customer");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpDelete("DeleteCustomerById")]
    public IActionResult DeleteCustomerById([FromQuery] uint customerId)
    {
        try
        {
            var result = _storeApiContext.DeleteCustomerById(customerId);

            if(result != null)
                if(result is ObjectNotFoundException)
                    return NotFound($"Customer {customerId} not found in our database, sorry.");
                else
                    throw result;

            return Ok($"Customer {customerId} removed with success!");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on DELETE Customer by ID");
            return BadRequest("Internal Server Error");
        }
    }


}