using System;
using testApi.Entities;

namespace testApi.Interfaces
{
	public interface IProductService
	{

		Task<List<Product>> GetProducts();

		Task<Product> GetProduct(int id);

		Task<Product> CreateAsync(Product product);

		Task UpdateAsync(Product entityToUpdate, Product entity);

		Task Delete(Product entity);
	}
}

