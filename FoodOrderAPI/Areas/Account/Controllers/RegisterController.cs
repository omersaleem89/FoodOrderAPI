﻿using FoodOrderAPI.Models;
using FoodOrderAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderAPI.Areas.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        public RegisterController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult RegisterUser(User user) 
        {
            if (ModelState.IsValid)
            {
                return Ok(new RegisterViewModel(_db).RegisterUser(user));
            }
            else 
            {
                return ValidationProblem();
            }
        }
    }
}