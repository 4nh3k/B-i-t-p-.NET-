using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace ProductManagement.Repositories
{
    public class ProductRepository
    {
        public List<ProductDTO> RetrieveAllProducts(Expression<Func<Product, bool>> condition = null)
        {
            using (var dbContext = new ProductEntities())
            {
                // Initial query on Product table
                IQueryable<Product> productQuery = dbContext.Products;

                // Apply condition if provided
                if (condition != null)
                {
                    Debug.Print("Predicate được áp dụng trong truy vấn");
                    productQuery = productQuery.Where(condition);
                }

                // Project query result into ProductDTO and return as a list
                var productList = productQuery
                    .Select(product => new ProductDTO
                    {
                        ProdID = product.ProdID ?? "N/A",
                        ProdName = product.ProdName ?? "N/A",
                        Quantity = product.Quantity ?? 0,
                        Price = product.Price ?? 0,
                        Origin = product.Origin ?? "N/A",
                        ExpireDate = product.ExpireDate ?? DateTime.Now
                    })
                    .ToList();

                return productList;
            }
        }

        public ProductDTO GetProductWithMaxPrice()
        {
            using (var dbContext = new ProductEntities())
            {
                // Select the product with the highest price
                var productWithMaxPrice = dbContext.Products
                    .Select(product => new ProductDTO
                    {
                        ProdID = product.ProdID ?? "N/A",
                        ProdName = product.ProdName ?? "N/A",
                        Quantity = product.Quantity ?? 0,
                        Price = product.Price ?? 0,
                        Origin = product.Origin ?? "N/A",
                        ExpireDate = product.ExpireDate ?? DateTime.Now
                    })
                    .OrderByDescending(p => p.Price)
                    .FirstOrDefault();

                return productWithMaxPrice;
            }
        }

        public void SaveOrUpdateProduct(ProductDTO productDTO)
        {
            using (var dbContext = new ProductEntities())
            {
                // Check if the product already exists in the database
                var existingProduct = dbContext.Products
                    .FirstOrDefault(p => p.ProdID == productDTO.ProdID);

                if (existingProduct != null)
                {
                    // Update the existing product
                    UpdateProduct(existingProduct, productDTO);
                }
                else
                {
                    // Add a new product if it doesn't exist
                    var newProduct = new Product();
                    InitializeProductValues(newProduct);
                    UpdateProduct(newProduct, productDTO);
                    dbContext.Products.Add(newProduct);
                }

                // Save changes to the database
                dbContext.SaveChanges();
            }
        }

        private void InitializeProductValues(Product product)
        {
            product.ProdID = "";
            product.ProdName = "";
            product.Price = 0;
            product.Quantity = 0;
            product.Origin = "";
            product.ExpireDate = null;
        }

        private void UpdateProduct(Product product, ProductDTO productDTO)
        {
            product.ProdID = productDTO.ProdID; // Always update ProdID
            product.ProdName = productDTO.ProdName;
            product.Price = productDTO.Price;
            product.Quantity = productDTO.Quantity;
            product.Origin = productDTO.Origin;
            product.ExpireDate = productDTO.ExpireDate;
        }

        public void RemoveProduct(Func<Product, bool> condition)
        {
            using (var dbContext = new ProductEntities())
            {
                // Find products that match the condition
                var productsToRemove = dbContext.Products.Where(condition).ToList();

                // Remove each product
                productsToRemove.ForEach(product => dbContext.Products.Remove(product));

                // Commit the changes to the database
                dbContext.SaveChanges();
            }
        }

        public void RemoveAllProducts()
        {
            using (var dbContext = new ProductEntities())
            {
                // Execute raw SQL to delete all products
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Product");
            }
        }
    }
}
