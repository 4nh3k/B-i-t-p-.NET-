using ProductManagement.Models;
using ProductManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProductManagement.Services
{
    public class ProductService
    {
        private ProductRepository _productRepository = new ProductRepository();

        public List<ProductDTO> GetAllProducts(Expression<Func<Product, bool>> predicate = null)
        {
            // Retrieve all products with optional filter predicate
            return _productRepository.RetrieveAllProducts(predicate);
        }

        public void SaveProduct(ProductDTO productDTO)
        {
            // Save or update a product
            _productRepository.SaveOrUpdateProduct(productDTO);
        }

        public void DeleteProduct(Func<Product, bool> predicate)
        {
            // Delete product(s) based on condition
            _productRepository.RemoveProduct(predicate);
        }

        public ProductDTO GetMaxPriceProduct()
        {
            // Get the product with the highest price
            return _productRepository.GetProductWithMaxPrice();
        }

        public void DeleteAllProducts()
        {
            // Remove all products from the database
            _productRepository.RemoveAllProducts();
        }
    }
}
