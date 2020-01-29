using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Models.ViewModels
{
    public class CategoryViewModel
    {
        private readonly ApplicationDBContext _db;
        public CategoryViewModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public IEnumerable<Category> GetAll()
        {
            var res = _db.Category.ToList();
            return res;
        }

        public Category Get(int id)
        {
            var res = _db.Category.FirstOrDefault(x => x.Id == id);
            return res;
        }

        public DbResponse Delete(int id)
        {
            try
            {
                var category = _db.Category.FirstOrDefault(x => x.Id == id);
                if (category != null)
                {
                    _db.Entry(category).State = EntityState.Deleted;
                    _db.SaveChanges();
                    return new DbResponse()
                    {
                        Result = true,
                        ExceptionMessage = "Category Deleted"
                    };
                }
                else
                {
                    return new DbResponse()
                    {
                        Result = false,
                        ExceptionMessage = "Category does not Exists"
                    };
                }
            }
            catch (Exception ex)
            {
                return new DbResponse()
                {
                    Result = false,
                    ExceptionMessage = ex.Message.ToString()
                };
            }
        }

        public DbResponse Insert(Category category)
        {
            try
            {
                if (_db.Category.FirstOrDefault(x => x.Name == category.Name) == null)
                {
                    _db.Category.Add(category);
                    _db.SaveChanges();
                    return new DbResponse()
                    {
                        Result = true,
                        ExceptionMessage = "Category Added"
                    };
                }
                else
                {
                    return new DbResponse()
                    {
                        Result = false,
                        ExceptionMessage = "Category Already Exists"
                    };
                }
            }
            catch (Exception ex)
            {
                return new DbResponse()
                {
                    Result = false,
                    ExceptionMessage = ex.Message.ToString()
                };
            }

        }

        public DbResponse Update(Category category)
        {
            try
            {
                var categoryObj = _db.Category.FirstOrDefault(x => x.Id == category.Id);
                if (categoryObj != null)
                {
                    categoryObj = category;
                    _db.SaveChanges();
                    return new DbResponse()
                    {
                        Result = true,
                        ExceptionMessage = "Category Updated"
                    };
                }
                else
                {
                    return new DbResponse()
                    {
                        Result = false,
                        ExceptionMessage = "Category does not Exists"
                    };
                }
            }
            catch (Exception ex)
            {
                return new DbResponse()
                {
                    Result = false,
                    ExceptionMessage = ex.Message.ToString()
                };
            }

        }
    }
}
