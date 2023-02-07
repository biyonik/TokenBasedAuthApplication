using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenBasedAuthApplication.Core.DTOs;

namespace TokenBasedAuthApplication.API.Controllers;

[Authorize]
public class ProductsController: BaseApiController
{
    private readonly IGenericService<Product, ProductDto> _productService;

    public ProductsController(IGenericService<Product, ProductDto> productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _productService.GetAllAsync(default);
        return await HandleResponse(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _productService.GetByIdAsync(id, default);
        return await HandleResponse(response);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ProductDto productDto)
    {
        var response = await _productService.AddAsync(productDto, default);
        return await HandleResponse(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update(ProductDto productDto, Guid id)
    {
        var response = await _productService.UpdateAsync(productDto, id,default);
        return await HandleResponse(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _productService.DeleteAsync(id,default);
        return await HandleResponse(response);
    }
}