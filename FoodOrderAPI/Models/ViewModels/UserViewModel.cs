using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FoodOrderAPI.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Models.ViewModels
{
    public class UserViewModel
    {
        private readonly ApplicationDBContext _db;
        public UserViewModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAll()
        {
            var res = _db.User.ToList();
            return res;
        }

        public User Get(int id)
        {
            var res = _db.User.FirstOrDefault(x => x.Id == id);
            return res;
        }
    }
}
