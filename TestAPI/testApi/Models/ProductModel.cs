using System;
namespace testApi.Models
{
	public class ProductModel
	{
        public string Name { get; set; }

        public int Price { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }



    }
}

