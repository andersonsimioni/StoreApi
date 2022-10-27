using System.Data.Entity.Core;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StoreApi.DataLayer.DataExtensions;
using StoreApi.DataLayer.DataRepository.Models;
using StoreApi.Shared.Services.ExceptionSecurityChecker;
using StoreApi.Shared.Structs.DataLayerAbstractions;

namespace StoreApi.WebApi.Controllers;

public class ItemController : Controller{
    private readonly Context _storeApiContext;
    private readonly IMapper _autoMapper;
    private readonly ILogger<ItemController> _logger;
    private readonly IExceptionSecurityChecker _exceptionSecurity;

    public ItemController(
        Context storeApiContext,
        IMapper autoMapper, 
        ILogger<ItemController> logger,
        IExceptionSecurityChecker exceptionSecurity)
    {
        this._logger = logger;
        this._autoMapper = autoMapper;
        this._storeApiContext = storeApiContext;
        this._exceptionSecurity = exceptionSecurity;
    }

    [HttpGet("GetItemById")]
    public IActionResult GetItemById([FromQuery] uint itemId){
        try
        {
            var item = _storeApiContext.GetItemById(itemId);
            if(item == null) return NotFound($"Item {itemId} not found!");

            return Ok(item);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on GET item by ID");
            return BadRequest("Internal Server Error");
        }
    }

    /// <summary>
    /// Find on database for items
    /// </summary>
    /// <returns>All list of available items</returns>
    [HttpGet("ListItems")]  
    public IActionResult ListItems(){
        try
        {
            var items = _storeApiContext.GetItems();
            return Ok(items);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on GET items");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpPost("CreateItem")]
    public IActionResult CreateItem([FromBody] CreateItemDTO item){
        try
        {
            var itemModel = _autoMapper.Map<Item>(item);
            var result = _storeApiContext.CreateItem(itemModel);

            if(result != null)
                if(_exceptionSecurity.IsSecure(result))
                    return BadRequest(result.Message);
                else 
                    throw result;

            return Ok($"Success to create item {itemModel.Id}");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on POST item");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpPut("UpdateItem")]
    public IActionResult UpdateItem([FromBody] UpdateItemDTO item){
        try
        {
            var itemModel = _autoMapper.Map<Item>(item);
            var result = _storeApiContext.UpdateItem(itemModel);

            if(result != null)
                if(_exceptionSecurity.IsSecure(result))
                    return BadRequest(result.Message);
                else 
                    throw result;

            return Ok(item);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on PUT item");
            return BadRequest("Internal Server Error");
        }
    }

    [HttpDelete("DeleteItemById")]
    public IActionResult DeleteItemById([FromQuery] uint itemId)
    {
        try
        {
            var result = _storeApiContext.DeleteItemById(itemId);

            if(result != null)
                if(result is ObjectNotFoundException)
                    return NotFound($"Item {itemId} not found in our database, sorry.");
                else
                    throw result;

            return Ok($"Item {itemId} removed with success!");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(new EventId(), ex, "Server exception on DELETE item by ID");
            return BadRequest("Internal Server Error");
        }
    }
}