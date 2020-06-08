using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FoodOrderAPI.Areas.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private IConfiguration _config;
        public LoginController(ApplicationDBContext db, IConfiguration config )
        {
            _db = db;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginUser(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                IActionResult response = new JsonResult("") ;
                var user = new LoginViewModel(_db).AuthenticateUser(login);

                if (user != null)
                {
                    var tokenString = new JWTHandler(_config).GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenString });
                }

                return response;
            }
            else
            {
                return NotFound();
            }
        }
    }
}