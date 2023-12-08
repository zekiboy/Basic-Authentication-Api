using System;
using testApi.Models;
using testApi.Interfaces;
using testApi.Data;
using testApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace testApi.Service
{
	public class UserService : IUserService
	{



        public async Task<User> Login(string username, string password)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefaultAsync
                    (x => x.Username == username && x.Password == password);

                return await user;
            }
        }


       public async Task<List<User>> GetUsers()
        {
            using(var context = new ApplicationDbContext())
            {
                var users = await context.Users.ToListAsync(); 


                return users;
            }
        }


        public async Task<User> GetUserById (int id)
        {
            using(var context = new ApplicationDbContext())
            {
                var user =  await context.Users
                    .FirstOrDefaultAsync(x => x.UserId == id);


                return user;
            }
        }


    }
}

