using System;
using Microsoft.EntityFrameworkCore;
using testApi.Data;
using testApi.Entities;
using testApi.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace testApi.Service
{
    public class ProductService : IProductService
    {
        public async Task<Product> GetProduct(int id)
        {
            using(var context = new ApplicationDbContext())
            {
                //var product  = await context.Products.Include(x => x.Category)
                //    .FirstOrDefaultAsync(x => x.ProductId == id);
                var product = await context.Products
                    .FirstOrDefaultAsync(x => x.ProductId == id);
                return product;
            }
        }


        public async Task<List<Product>> GetProducts()
        {
            using(var context = new ApplicationDbContext())
            {
                var products = await context.Products.Include(x=>x.Category)
                 .ToListAsync();

                return products;
            }
        }


        public async Task<Product> CreateAsync(Product product)
        {
            using(var context = new ApplicationDbContext())
            {

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();

                return product;
            }
        }

        public async Task UpdateAsync(Product entityToUpdate, Product entity)
        {
            using(var context = new ApplicationDbContext())
            {

                entityToUpdate.Name = entity.Name;
                entityToUpdate.Price = entity.Price;
                entityToUpdate.CategoryId = entity.CategoryId;

                //context.Entry(entityToUpdate).State() = EntityState.Modified;
                context.Products.Update(entityToUpdate);

                await context.SaveChangesAsync();


             
            }
        }

        public async Task Delete(Product product)
        {
            using(var context = new ApplicationDbContext())
            {
                context.Products.Remove(product);

                await context.SaveChangesAsync();
            }
        }

    }
}
