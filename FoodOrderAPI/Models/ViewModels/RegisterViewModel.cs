using System;
using System.Linq;

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
                var check= _db.User.FirstOrDefault(x => x.Email == user.Email);
                if (check == null)
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
