using FoodOrderAPI.Models;
using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        public RegisterController(ApplicationDBContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterUser([FromForm]User user) {
            if (ModelState.IsValid)
            {
                return new JsonResult(new RegisterViewModel(_db).RegisterUser(user));
            }
            else 
            {
                return new JsonResult(new DbResponse()
                {
                    Result = false,
                    ExceptionMessage = "Registration Failed",
                    DataResult = null
                });
            }
        }
    }
}