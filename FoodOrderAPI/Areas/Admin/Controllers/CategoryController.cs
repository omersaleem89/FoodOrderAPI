using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodOrderAPI.Models;
using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        public CategoryController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
           return new JsonResult(new CategoryViewModel(_db).GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(new CategoryViewModel(_db).Get(id));
        }

        [HttpPost]
        public IActionResult Insert([FromForm] Category category)
        {
            return new JsonResult(new CategoryViewModel(_db).Insert(category));
        }
        [HttpPut]
        public IActionResult Update([FromForm] Category category)
        {
            return new JsonResult(new CategoryViewModel(_db).Update(category));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return new JsonResult(new CategoryViewModel(_db).Delete(id));
        }
    }
}