using FoodOrderAPI.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderAPI.Models.ViewModels
{
    public class FoodItemViewModel
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FoodItemViewModel(ApplicationDBContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }

        public IEnumerable<FoodItem> GetAll()
        {
            var res = _db.FoodItem.ToList();
            return res;
        }

        public FoodItem Get(int id)
        {
            var res = _db.FoodItem.FirstOrDefault(x => x.Id == id);
            return res;
        }

        public IEnumerable<FoodItem> GetCategoryItems(int id)
        {
            var res = _db.FoodItem.Where(x => x.CategoryId == id);
            return res;
        }

        public DbResponse Insert(FoodItemUpsert foodItem)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            try
            {
                if (_db.FoodItem.FirstOrDefault(x => x.Name == foodItem.Name) == null)
                {
                    var ext1 = Path.GetExtension(foodItem.Image.FileName);
                    var ext2 = Path.GetExtension(foodItem.ImageThumb.FileName);
                    if ((foodItem.Image == null
                        || foodItem.ImageThumb == null)
                        || (foodItem.Image.Length == 0
                        || foodItem.ImageThumb.Length == 0)
                        || !permittedExtensions.Contains(ext1)
                        || !permittedExtensions.Contains(ext2))
                        return new DbResponse()
                        {
                            Result = false,
                            ExceptionMessage = "Select Image"
                        };
                    _db.FoodItem.Add(new FoodItem()
                    {
                        Name = foodItem.Name,
                        Image = ImageHelper.UploadImageFile("wwwroot/images", foodItem.Image),
                        ImageThumb = ImageHelper.UploadImageFile("wwwroot/images/thumb", foodItem.ImageThumb),
                        Price = foodItem.Price,
                        CategoryId = foodItem.CategoryId,
                        Description = foodItem.Description,
                        Quantity = foodItem.Quantity,
                        IsDeleted = false,
                        IsEnabled =true
                    });
                    _db.SaveChanges();
                    return new DbResponse()
                    {
                        Result = true,
                        ExceptionMessage = "FoodItem Added"
                    };
                }
                else
                {
                    return new DbResponse()
                    {
                        Result = false,
                        ExceptionMessage = "FoodItem Already Exists"
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

        public DbResponse Update(int id, FoodItemUpsert foodItemUpsert)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            try
            {
                var foodItem = _db.FoodItem.FirstOrDefault(x => x.Id == id);
                if (foodItem != null)
                {
                    foodItem.Name = foodItemUpsert.Name;
                    foodItem.Price = foodItemUpsert.Price;
                    foodItem.Description = foodItemUpsert.Description;
                    foodItem.Quantity = foodItemUpsert.Quantity;
                    foodItem.IsDeleted = foodItemUpsert.IsDeleted;
                    foodItem.IsEnabled = foodItemUpsert.IsEnabled;

                    if (!(foodItemUpsert.Image == null
                           || foodItemUpsert.ImageThumb == null))
                    {
                        var ext1 = Path.GetExtension(foodItemUpsert.Image.FileName);
                        var ext2 = Path.GetExtension(foodItemUpsert.ImageThumb.FileName);
                        if (permittedExtensions.Contains(ext1)
                           && permittedExtensions.Contains(ext2))
                        {
                            if (!(ImageHelper.DeleteImage(_hostEnvironment, @"images", foodItem.Image.Replace("/images/", "")))
                            || !(ImageHelper.DeleteImage(_hostEnvironment, @"images\thumb", foodItem.ImageThumb.Replace("/images/thumb/", ""))))
                            {
                                return new DbResponse()
                                {
                                    Result = false,
                                    ExceptionMessage = "File does not Exists"
                                };
                            }
                            foodItem.Image = ImageHelper.UploadImageFile("wwwroot/images", foodItemUpsert.Image);
                            foodItem.ImageThumb = ImageHelper.UploadImageFile("wwwroot/images/thumb", foodItemUpsert.ImageThumb);
                        }

                    }

                    _db.SaveChanges();
                    return new DbResponse()
                    {
                        Result = true,
                        ExceptionMessage = "FoodItem Updated"
                    };

                }
                else
                {
                    return new DbResponse()
                    {
                        Result = false,
                        ExceptionMessage = "FoodItem does not Exists"
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

        public DbResponse Delete(int id)
        {
            try
            {
                var foodItem = _db.FoodItem.FirstOrDefault(x => x.Id == id);
                if (foodItem != null)
                {
                    if (foodItem.Image != null || foodItem.Image != "" ||
                        foodItem.ImageThumb != null || foodItem.ImageThumb != ""
                        )
                    {
                        if (!(ImageHelper.DeleteImage(_hostEnvironment, @"images", foodItem.Image.Replace("/images/", "")))
                            || !(ImageHelper.DeleteImage(_hostEnvironment, @"images\thumb", foodItem.ImageThumb.Replace("/images/thumb/", ""))))
                        {
                            return new DbResponse()
                            {
                                Result = false,
                                ExceptionMessage = "File does not Exists"
                            };
                        }

                    }
                    _db.Entry(foodItem).State = EntityState.Deleted;
                    _db.SaveChanges();
                    return new DbResponse()
                    {
                        Result = true,
                        ExceptionMessage = "FoodItem Deleted"
                    };
                }
                else
                {
                    return new DbResponse()
                    {
                        Result = false,
                        ExceptionMessage = "FoodItem does not Exists"
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

    public class FoodItemUpsert
    {
        [Required]
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile ImageThumb { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than {1}")]
        public int Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than {1}")]
        public int Quantity { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }

    }
}
