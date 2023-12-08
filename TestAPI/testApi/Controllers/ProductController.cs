using System;
using Microsoft.AspNetCore.Mvc;
using testApi.Interfaces;
using testApi.Models;
using testApi.Entities;
using AuthorizeAttribute = testApi.Models.AuthorizeAttribute;
using testApi.Data;

namespace testApi.Controllers
{
	[Route("api/product")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}


		[HttpGet("getProduct")]
		public async Task<IActionResult> GetProduct(int id)
		{
			var product = await _productService.GetProduct(id);

			if (product == null)
			{
				return NotFound("Ürün Bulunamadı");
			}

			var result = new ProductModel
			{
				Name = product.Name,
				Price = product.Price,
				CategoryId = product.CategoryId,
				//CategoryName =product.Category.Name
			};



			return Ok(result);
		}




		[HttpGet("getProducts")]
		public async Task<IActionResult> GetProducts()
		{
			var products = await _productService.GetProducts();

			var result = new List<ProductModel>();

			foreach (var product in products)
			{
				var productModel = new ProductModel
				{
					Name = product.Name,
					Price = product.Price,
					//CategoryId=product.CategoryId,
					CategoryName = product.Category.Name
				};

				result.Add(productModel);
			}

			return Ok(result);
		}



		[HttpPost("CreateProduct")]
		public async Task<IActionResult> CreateProduct(ProductModel model)
		{
			Product product = new Product()
			{
				Name = model.Name,
				Price = model.Price,
				CategoryId = model.CategoryId,
			};

			await _productService.CreateAsync(product);

			//var result = await _productService.GetProduct(product.ProductId);
			return Ok("Ürün veritabanına eklendi");
		}




		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, UpdateModel model)
		{


			var entityToUpdate = await _productService.GetProduct(id);

			if (entityToUpdate == null)
			{
				return NotFound("Ürün bulunamadı");
			}

			var entity = new Product
			{
				Name = model.Name,
				Price = model.Price,
				CategoryId = model.CategoryId,
			};

			await _productService.UpdateAsync(entityToUpdate, entity);

			return NoContent();


		}



		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _productService.GetProduct(id);
			if (product == null)
			{
				return NotFound("Ürün Bulunamadı");
			}

			await _productService.Delete(product);
			return NoContent();
		}


	}
	
}
