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
        public async Task<ActionResult> GetProducts([FromQuery] ProductQuery query)
        {
            return Ok(await _productService.GetProducts(query));
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductDto request)
        {
            var result = await _productService.CreateProduct(request);

            return (result) ? NoContent() : NotFound();
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto request)
        {
            if (id <= 0)
            {
                return Problem(detail: "Invalid ProductId", statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await _productService.UpdateProduct(id, request);

            return (result) ? NoContent() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return Problem(detail: "Invalid ProductId", statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await _productService.DeleteProduct(id);

            return (result) ? NoContent() : NotFound();
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            if (id <= 0)
            {
                return Problem(detail: "Invalid ProductId", statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await _productService.GetProduct(id);

            if (result == null)
            {
                return Problem(detail: "The product doesn't exist", statusCode: StatusCodes.Status404NotFound);
            }

            return Ok(result);
        }
    }
}
