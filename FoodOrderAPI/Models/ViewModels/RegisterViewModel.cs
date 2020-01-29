using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderAPI.Models.ViewModels
{
    public class RegisterViewModel
    {
        private readonly ApplicationDBContext _db;
        public RegisterViewModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public DbResponse RegisterUser(User user)
        {
            try
            {
                var temp= _db.User.FirstOrDefault(x => x.Email == user.Email);
                if (temp == null)
                {
                    _db.User.Add(user);
                    _db.SaveChanges();

                    return new DbResponse()
                    {
                        Result = true,
                        ExceptionMessage = "Registration Successful",
                        DataResult = null
                    };
                }
                else
                {
                    return new DbResponse()
                    {
                        Result = false,
                        ExceptionMessage = "Email Already Exists",
                        DataResult = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new DbResponse()
                {
                    Result = false,
                    ExceptionMessage = ex.Message.ToString(),
                    DataResult = null
                };
            }

        }
        
    }
}
