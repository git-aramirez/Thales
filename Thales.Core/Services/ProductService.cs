using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thales.Core.IServices;
using Thales.Domain.Models;

namespace Thales.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration=configuration;
        }

        public async Task<IEnumerable<Product>> All()
        {
            string apiUrl = _configuration["ApiUrl"];
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    IEnumerable<Product> result = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseContent);
                    _logger.LogInformation("The products were successfully deserialized!");

                    return result;
                }

                _logger.LogError("Something went wrong when the products were attempted to obtain!");
                throw new Exception("Something went wrong when the product was attempted to obtain!");
            }
        }

        public async Task<Product> ById(int productId)
        {
            string apiUrl = $"{_configuration["ApiUrl"]}/{productId}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Product>(responseContent);
                    _logger.LogInformation("The product was successfully deserialized!");

                    return result;
                }

                _logger.LogError("Something went wrong when the product was attempted to obtain!");

                if (response.StatusCode==System.Net.HttpStatusCode.BadRequest)
                {
                    throw new IndexOutOfRangeException("please enter a correct id");
                }
                throw new Exception("Something went wrong when the product was attempted to obtain!");
            }
        }

        public IEnumerable<Product> CalculateTaxProducts(IEnumerable<Product> products)
        {
            products.ToList().ForEach(p => p.Tax = p.Price * 0.19m);

            return products;
        }

        public Product CalculateTaxProduct(Product product)
        {
            product.Tax = product.Price * 0.19m;

            return product;
        }
    }
}
