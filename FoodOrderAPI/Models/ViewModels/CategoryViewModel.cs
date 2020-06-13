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

        public DbResponse Insert(CategoryUpsert categoryUpsert)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            try
            {
                if (_db.Category.FirstOrDefault(x => x.Name == categoryUpsert.Name) == null)
                {
                    var ext1 = Path.GetExtension(categoryUpsert.ImageFile.FileName).ToLower();
                    var ext2 = Path.GetExtension(categoryUpsert.ImageFileThumb.FileName).ToLower();
                    if ((categoryUpsert.ImageFile == null 
                        || categoryUpsert.ImageFileThumb == null) 
                        || (categoryUpsert.ImageFile.Length == 0 
                        || categoryUpsert.ImageFileThumb.Length==0)
                        || !permittedExtensions.Contains(ext1)
                        || !permittedExtensions.Contains(ext2))
                        return new DbResponse()
                        {
                            Result = false,
                            ExceptionMessage = "Select Image"
                        };
                    _db.Category.Add(new Category() { 
                        Name= categoryUpsert.Name,
                        Image= ImageHelper.UploadImageFile("wwwroot/images", categoryUpsert.ImageFile),
                        ImageThumb = ImageHelper.UploadImageFile("wwwroot/images/thumb", categoryUpsert.ImageFileThumb)
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
                var category = _db.Category.FirstOrDefault(x => x.Id == categoryUpsert.Id);
                if (category != null)
                {
                    if(!string.IsNullOrEmpty(categoryUpsert.Name))
                        category.Name = categoryUpsert.Name;
                    if (!(categoryUpsert.ImageFile == null
                           || categoryUpsert.ImageFileThumb == null))
                    {
                        var ext1 = Path.GetExtension(categoryUpsert.ImageFile.FileName);
                        var ext2 = Path.GetExtension(categoryUpsert.ImageFileThumb.FileName);
                        if (permittedExtensions.Contains(ext1)
                           && permittedExtensions.Contains(ext2))
                        {
                            ImageHelper.DeleteImage(_hostEnvironment, @"images", category.Image.Replace("/images/", ""));
                            ImageHelper.DeleteImage(_hostEnvironment, @"images\thumb", category.ImageThumb.Replace("/images/thumb/", ""));
                            category.Image = ImageHelper. UploadImageFile("wwwroot/images", categoryUpsert.ImageFile);
                            category.ImageThumb = ImageHelper.UploadImageFile("wwwroot/images/thumb", categoryUpsert.ImageFileThumb);
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
                        ImageHelper.DeleteImage(_hostEnvironment, @"images", category.Image.Replace("/images/", ""));
                        ImageHelper.DeleteImage(_hostEnvironment, @"images\thumb", category.ImageThumb.Replace("/images/thumb/", ""));
                            
                        

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
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile ImageFile { get; set; }
        public IFormFile ImageFileThumb { get; set; }
    }
}
