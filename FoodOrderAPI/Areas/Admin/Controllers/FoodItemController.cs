using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {

        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FoodItemController(ApplicationDBContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/Item
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new FoodItemViewModel(_db, _hostEnvironment).GetAll());
        }

        // GET: api/FoodItem/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            return new JsonResult(new FoodItemViewModel(_db, _hostEnvironment).Get(id));
        }

        // POST: api/FoodItem
        [HttpPost]
        public IActionResult Post([FromForm] FoodItemUpsert foodItem)
        {
            return new JsonResult(new FoodItemViewModel(_db, _hostEnvironment).Insert(foodItem));
        }

        // PUT: api/FoodItem/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] FoodItemUpsert foodItem)
        {
            if (ModelState.IsValid)
            {
                return new JsonResult(new FoodItemViewModel(_db, _hostEnvironment).Update(id, foodItem));
            }
            else {
                return new JsonResult(new DbResponse()
                {
                    Result = false,
                    ExceptionMessage = "All fields required"
                });
            }
        }

        // DELETE: api/FoodItem/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
