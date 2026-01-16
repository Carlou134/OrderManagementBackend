using Microsoft.AspNetCore.Mvc;
using OrderManagementBackend.Application.Dtos.Requests.Product;
using OrderManagementBackend.Application.Interfaces;

namespace OrderManagementBackend.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetProducts()
        {
            return Ok(await _productService.GetProducts());
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.CreateProduct(request);

            return (result) ? NoContent() : NotFound();
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto request)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ProductId");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.UpdateProduct(id, request);

            return (result) ? NoContent() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ProductId");
            }

            var result = await _productService.DeleteProduct(id);

            return (result) ? NoContent() : NotFound();
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ProductId");
            }

            var result = await _productService.GetProduct(id);

            if (result == null)
            {
                return NotFound("The product don't exists");
            }

            return Ok(result);
        }
    }
}
