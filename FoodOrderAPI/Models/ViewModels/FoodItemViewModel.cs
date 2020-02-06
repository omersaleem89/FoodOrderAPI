using FoodOrderAPI.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                    if (!String.IsNullOrEmpty(foodItemUpsert.Name))
                        foodItemUpsert.Name = foodItemUpsert.Name;
                    if (!(categoryUpsert.imageFile == null
                           || categoryUpsert.imageFileThumb == null))
                    {
                        var ext1 = Path.GetExtension(categoryUpsert.imageFile.FileName);
                        var ext2 = Path.GetExtension(categoryUpsert.imageFileThumb.FileName);
                        if (permittedExtensions.Contains(ext1)
                           && permittedExtensions.Contains(ext2))
                        {
                            if (!(ImageHelper.DeleteImage(_hostEnvironment, @"images", category.Image.Replace("/images/", "")))
                            || !(ImageHelper.DeleteImage(_hostEnvironment, @"images\thumb", category.ImageThumb.Replace("/images/thumb/", ""))))
                            {
                                return new DbResponse()
                                {
                                    Result = false,
                                    ExceptionMessage = "File does not Exists"
                                };
                            }
                            category.Image = ImageHelper.UploadImageFile("wwwroot/images", categoryUpsert.imageFile);
                            category.ImageThumb = ImageHelper.UploadImageFile("wwwroot/images/thumb", categoryUpsert.imageFileThumb);
                        }

                    }

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

        public DbResponse Delete(int id)
        {
            try
            {
                var category = _db.Category.FirstOrDefault(x => x.Id == id);
                if (category != null)
                {
                    if (category.Image != null || category.Image != "" ||
                        category.ImageThumb != null || category.ImageThumb != ""
                        )
                    {
                        if (!(ImageHelper.DeleteImage(_hostEnvironment, @"images", category.Image.Replace("/images/", "")))
                            || !(ImageHelper.DeleteImage(_hostEnvironment, @"images\thumb", category.ImageThumb.Replace("/images/thumb/", ""))))
                        {
                            return new DbResponse()
                            {
                                Result = false,
                                ExceptionMessage = "File does not Exists"
                            };
                        }

                    }
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
    }

    public class FoodItemUpsert
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile ImageThumb { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }

    }
}
