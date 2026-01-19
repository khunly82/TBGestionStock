using GestionStock.API.Data;
using GestionStock.Domain.Entities;
using GestionStock.Utils.Security;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace GestionStock.API.Services
{
    public class UserService(StockContext db)
    {
        public User Login(string username, string password)
        {
            // SELECT * FROM Users WHERE Username = "admin"
            // User? us2 = db.Users.SingleOrDefault(u => u.Username == username);
            // SELECT * FROM Users WHERE Username LIKE "admin"
            User us = db.Users.SingleOrDefault(u => EF.Functions.Like(u.Username , username)) ?? throw new AuthenticationException();

            if(!PasswordUtils.CheckPassword(password, us.EncodedPassword))
            {
                throw new AuthenticationException();
            }
            return us;

            //db.Users.First();
            //db.Users.FirstOrDefault();
            //db.Users.Single();
            //db.Users.SingleOrDefault();
            //db.Users.Last();
            //db.Users.LastOrDefault();

        }
    }
}
