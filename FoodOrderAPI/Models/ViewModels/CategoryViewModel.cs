using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderAPI.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Models.ViewModels
{
    public class CategoryViewModel
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CategoryViewModel(ApplicationDBContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
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

        public DbResponse Insert(string name,IFormFile imageFile, IFormFile imageFileThumb)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            try
            {
                if (_db.Category.FirstOrDefault(x => x.Name == name) == null)
                {
                    var ext1 = Path.GetExtension(imageFile.FileName);
                    var ext2 = Path.GetExtension(imageFileThumb.FileName);
                    if ((imageFile == null 
                        || imageFileThumb == null) 
                        || (imageFile.Length == 0 
                        || imageFileThumb.Length==0)
                        || !permittedExtensions.Contains(ext1)
                        || !permittedExtensions.Contains(ext2))
                        return new DbResponse()
                        {
                            Result = false,
                            ExceptionMessage = "Select Image"
                        };
                    _db.Category.Add(new Category() { 
                        Name=name,
                        Image= ImageHelper.UploadImageFile("wwwroot/images", imageFile),
                        ImageThumb = ImageHelper.UploadImageFile("wwwroot/images/thumb", imageFileThumb)
                    });
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

        public DbResponse Update(CategoryUpsert categoryUpsert)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            try
            {
                var category = _db.Category.FirstOrDefault(x => x.Id == categoryUpsert.id);
                if (category != null)
                {
                    if(!String.IsNullOrEmpty(categoryUpsert.name))
                        category.Name = categoryUpsert.name;
                    if (!(categoryUpsert.imageFile == null
                           || categoryUpsert.imageFileThumb == null))
                    {
                        var ext1 = Path.GetExtension(categoryUpsert.imageFile.FileName);
                        var ext2 = Path.GetExtension(categoryUpsert.imageFileThumb.FileName);
                        if (permittedExtensions.Contains(ext1)
                           && permittedExtensions.Contains(ext2))
                        {
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
                            || !(ImageHelper.DeleteImage(_hostEnvironment, @"images\thumb", category.ImageThumb.Replace("/images/thumb/", "")))) {
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

    public class CategoryUpsert {
        public int id { get; set; }
        public string name { get; set; }
        public IFormFile imageFile { get; set; }
        public IFormFile imageFileThumb { get; set; }
    }
}
