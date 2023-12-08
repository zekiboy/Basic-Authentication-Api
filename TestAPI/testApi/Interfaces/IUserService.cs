using System;
using testApi.Entities;

namespace testApi.Interfaces
{
	public interface IUserService
	{

        Task<User> Login(string username, string password);

        Task<List<User>> GetUsers();

        Task<User> GetUserById(int id);
    }
}

