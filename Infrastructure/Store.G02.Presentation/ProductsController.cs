using Microsoft.AspNetCore.Mvc;
using Store.G02.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet] //GET api/Products
        public async Task<IActionResult> GetAllProducts(int? brandId,int? typeId,string? sort,string? search,int? pageIndex = 1,int? pageSize = 4)
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync(brandId,typeId,sort,search,pageIndex,pageSize);
            if (products is null) return BadRequest(); // 400
            return Ok(products); // 200

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null || id <= 0) return BadRequest(); // 400
            var products = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);
            if (products is null) return NotFound(); // 404
            return Ok(products); // 200

        }

        [HttpGet("brands")] // GET: baseUrl/api/products/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest(); // 400
            return Ok(result); // 200
        }

        [HttpGet("types")] // GET: baseUrl/api/products/types
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest(); // 400
            return Ok(result); // 200
        }



    }
}
