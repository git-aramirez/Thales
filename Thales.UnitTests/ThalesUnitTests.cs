using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thales.Core.IServices;
using Thales.Domain.Models;

namespace Thales.UnitTests
{
    [TestClass]
    public class ThalesUnitTests
    {
        private static WebApplicationFactory<Program>? _factory = null;
        private static IServiceScopeFactory? _scopeFactory = null;

        [ClassInitialize]
        public static void Inicialize(TestContext contex)
        {
            _factory = new CustomApplicationFactory();
            _scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        }

        [TestMethod]
        public async Task TestGetAllProducts()
        {
            //Arrange
            using var scope = _scopeFactory?.CreateScope();
            var productService = scope?.ServiceProvider?.GetService<IProductService>();
            //Act
            IEnumerable<Product> products = await productService.All();
            //Assert
            Assert.IsNotNull(products);
        }

        [TestMethod]
        public async Task TestGetValidProductId()
        {
            //Arrange
            using var scope = _scopeFactory?.CreateScope();
            var productService = scope?.ServiceProvider?.GetService<IProductService>();
            //Act
            Product result = await productService.ById(47);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(73, result.Price);
            Assert.AreEqual("Radiant Citrus Eau de Parfum", result.Title);
            Assert.AreEqual("radiant-citrus-eau-de-parfum", result.Slug);
            Assert.AreEqual("2025-03-02T09:30:22.000Z", result.CreationAt);
            Assert.AreEqual("2025-03-02T09:30:22.000Z", result.UpdatedAt);
        }

        [TestMethod]
        public async Task TestGetInvalidId()
        {
            //Arrange
            using var scope = _scopeFactory?.CreateScope();
            var productService = scope?.ServiceProvider?.GetService<IProductService>();
            //Act
            try
            {
                Product result = await productService.ById(-1);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual("Something went wrong when the product was attempted to obtain!", ex.Message);
            }
        }
    }
}