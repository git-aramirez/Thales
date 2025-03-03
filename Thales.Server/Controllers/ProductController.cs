using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Thales.Core.IServices;
using Thales.Core.Services;
using Thales.Domain.Models;

namespace Thales.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /*
         * <summary>
         * Gets a list of all products.
         * </summary>
         * <returns>A list of object Product.</returns>
         * <response code="200">Return the list of products.</response>
         * <response code="500">If there is an internal server error.</response>
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> All()
        {
            try
            {
                _logger.LogInformation("the products are tried to be obtained");
                IEnumerable<Product> products = await _productService.All();
                IEnumerable<Product> productsWithTaxt = _productService.CalculateTaxProducts(products);
                _logger.LogInformation("the products are successfully obtained");

                return Ok(productsWithTaxt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /*
       * <summary>
       * Gets a product.
       * </summary>
       * <returns>A object Product.</returns>
       * <response code="200">Return a product.</response>
       * <response code="400">If the id from the product is not valid.</response>
       * <response code="500">If there is an internal server error.</response>
       */

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> ById(int productId)
        {
            try
            {
                if(productId < 0)
                {
                    _logger.LogError("The id entered is incorrect!");
                    return BadRequest("please enter a correct id");
                }

                _logger.LogInformation("the product is tried to be obtained");
                Product product = await _productService.ById(productId);
                Product productWithTaxt = _productService.CalculateTaxProduct(product);
                _logger.LogInformation("the product is successfully obtained");

                return Ok(product);
            }
            catch (IndexOutOfRangeException ex)
            {
                _logger.LogError("The id entered is incorrect!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
