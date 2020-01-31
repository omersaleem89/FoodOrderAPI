using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FoodOrderAPI.Models.ViewModels
{
    public class LoginViewModel
    {
        private readonly ApplicationDBContext _db;
        public LoginViewModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public User AuthenticateUser(LoginVM login)
        {
            User user = _db.User.FirstOrDefault(x => x.Email == login.Email 
                                                && x.Password == login.Password);
            return user;
        }
        
    }

    public class LoginVM {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
