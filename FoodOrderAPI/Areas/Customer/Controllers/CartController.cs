using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderAPI.Helper;
using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly ISession _session;
        public CartController(ApplicationDBContext db)
        {
            _db = db;
            _session = HttpContext.Session;
        }

        [HttpPost]
        public IActionResult AddToCart([FromForm] OrderDetailVM orderDetail)
        {
            if (ModelState.IsValid)
            {
                return new JsonResult(new CartViewModel(_session,_db).AddToCart(orderDetail));
            }
            else {
                return new JsonResult(false);
            }
        }
        [HttpGet]
        public IActionResult GetAllCartItems()
        {
            return new JsonResult(new CartViewModel(_session,_db).GetAllCartItems());
        }
    }
}