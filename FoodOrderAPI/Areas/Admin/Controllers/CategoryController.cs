using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = SD.Admin)]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CategoryController(ApplicationDBContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
           return new JsonResult(new CategoryViewModel(_db,_hostEnvironment).GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(new CategoryViewModel(_db, _hostEnvironment).Get(id));
        }

        [HttpPost]
        public IActionResult Insert([FromForm] CategoryUpsert categoryUpsert)
        {
            return new JsonResult(new CategoryViewModel(_db, _hostEnvironment).Insert(categoryUpsert));
        }
        
        [HttpPut]
        public IActionResult Update([FromForm] CategoryUpsert categoryUpsert)
        {
            return new JsonResult(new CategoryViewModel(_db, _hostEnvironment).Update(categoryUpsert));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return new JsonResult(new CategoryViewModel(_db, _hostEnvironment).Delete(id));
        }
    }
}