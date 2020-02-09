using FoodOrderAPI.Models.ViewModels;
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
                return Ok(new CartViewModel(_session,_db).AddToCart(orderDetail));
            }
            return NotFound(false);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateQuantity(int id, [FromForm] int quantity)
        {
            if(new CartViewModel(_session, _db).UpdateQuantity(id, quantity))
                return Ok();
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (new CartViewModel(_session, _db).Delete(id))
                return Ok();
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAllCartItems()
        {
            return Ok(new CartViewModel(_session,_db).GetAllCartItems());
        }
    }
}