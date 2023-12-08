using System;
namespace testApi.Entities
{
	public class Customer
	{
        public int CustomerId { get; set; }

        public string? Name { get; set; }

        public double Debts { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }


    }
}

