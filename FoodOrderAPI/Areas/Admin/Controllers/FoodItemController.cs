using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
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
            return Ok(new FoodItemViewModel(_db, _hostEnvironment).GetAll());
        }

        [HttpGet("Get/{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            return Ok(new FoodItemViewModel(_db, _hostEnvironment).Get(id));
        }

        // GET: api/FoodItem/5
        [HttpGet("GetCategoryItems/{id}", Name = "GetCategoryItems")]
        public IActionResult GetCategoryItems(int id)
        {
            return Ok(new FoodItemViewModel(_db, _hostEnvironment).GetCategoryItems(id));
        }

        // POST: api/FoodItem
        [HttpPost]
        [Authorize(Roles = SD.Admin)]
        public IActionResult Post([FromForm] FoodItemUpsert foodItem)
        {
            if (ModelState.IsValid)
            {
                return CreatedAtAction("PostFoodItem",new FoodItemViewModel(_db, _hostEnvironment).Insert(foodItem));
            }
            return ValidationProblem();
        }

        // PUT: api/FoodItem/5
        [Authorize(Roles = SD.Admin)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] FoodItemUpsert foodItem)
        {
            if (ModelState.IsValid)
            {
                return Ok(new FoodItemViewModel(_db, _hostEnvironment).Update(id, foodItem));
            }
            return ValidationProblem();
        }

        // DELETE: api/FoodItem/5
        [HttpDelete("{id}")]
        [Authorize(Roles = SD.Admin)]
        public IActionResult Delete(int id)
        {
            return Ok(new FoodItemViewModel(_db, _hostEnvironment).Delete(id));
        }
    }
}
