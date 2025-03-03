using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thales.Domain.Models;

namespace Thales.Core.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> All();
        Task<Product> ById(int productId);
        IEnumerable<Product> CalculateTaxProducts(IEnumerable<Product> products);
        Product CalculateTaxProduct(Product product);
    }
}
