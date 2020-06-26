using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = SD.Admin)]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        public UserController(ApplicationDBContext db)
        {
            _db = db;
        }
        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new UserViewModel(_db).GetAll());
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            return new JsonResult(new UserViewModel(_db).Get(id));
        }

    }
}
