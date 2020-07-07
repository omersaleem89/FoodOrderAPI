using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        public OrderController(ApplicationDBContext db)
        {
            _db = db;
        }
        // GET: api/Order
        [HttpGet]
        [Authorize(Roles = SD.Admin)]
        public IActionResult GetAll()
        {
            return new JsonResult(new OrderViewModel(_db).GetAll());
        }

        // GET: api/Order/5

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(new OrderViewModel(_db).Get(id));
        }

        [HttpGet("GetUserOrder/{userid}", Name = "GetUserOrder")]
        public IActionResult GetUserOrder(int userId)
        {
            return new JsonResult(new OrderViewModel(_db).GetUserOrder(userId));
        }
        [HttpGet("GetOrderDetails/{id}", Name = "GetOrderDetails")]
        public IActionResult GetOrderDetails(int id)
        {
            return new JsonResult(new OrderViewModel(_db).GetOrderDetails(id));
        }

        // POST: api/Order
        [HttpPost]
        public IActionResult Post([FromBody] OrderUpsert orderUpsert)
        {
            return new JsonResult(new OrderViewModel(_db).Insert(orderUpsert));
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        [Authorize(Roles = SD.Admin)]
        public IActionResult Put(int id,[FromForm] OrderUpsert orderUpsert)
        {
            return new JsonResult(new OrderViewModel(_db).Update(id,orderUpsert));
        }


    }
}
