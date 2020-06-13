using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using FoodOrderAPI.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Models.ViewModels
{
    public class OrderViewModel
    {
        private readonly ApplicationDBContext _db;
        public OrderViewModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public List<dynamic> GetAll()
        {
            var res = _db.Order.ToList();
            List<dynamic> list = new List<dynamic>();
            foreach (Order o in res){
                list.Add(new
                {
                    o.Id,
                    o.CreationDate,
                    o.Status,
                    o.TotalPrice,
                    o.TransId,
                    o.UserId,
                    Email = _db.User.FirstOrDefault(x => x.Id == o.UserId).Email
                }); ;
            }
            return list;
        }
        public dynamic Get(int id)
        {
            var o = _db.Order.FirstOrDefault(x => x.Id == id);
            return new
            {
                o.Id,
                o.CreationDate,
                o.Status,
                o.TotalPrice,
                o.TransId,
                o.UserId,
                Email = _db.User.FirstOrDefault(x => x.Id == o.UserId).Email
            }; 
        }
        public IEnumerable<Order> GetUserOrder(int userId)
        {
            var res = _db.Order.Where(x => x.UserId == userId).ToList();
            return res;
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int id)
        {
            var res = _db.OrderDetail.Where(x => x.OrderId == id).ToList();
            return res;
        }

        public DbResponse Insert(OrderUpsert orderUpsert)
        {
            try
            {
                Order orderResult = new Order()
                {
                    CreationDate = DateTime.Now,
                    TotalPrice = orderUpsert.TotalPrice,
                    Status = false,
                    UserId = orderUpsert.UserId
                };
                _db.Order.Add(orderResult);
                _db.SaveChanges();
                foreach (var item in orderUpsert.OrderDetails)
                {
                    _db.OrderDetail.Add(
                        new OrderDetail()
                        {
                            FoodItemId = item.FoodItemId,
                            Quantity = item.Quantity,
                            OrderId = orderResult.Id
                        });
                }
                _db.SaveChanges();
                return new DbResponse()
                {
                    Result = true,
                    ExceptionMessage = "Order Added"
                };
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

        public DbResponse Update(int id, OrderUpsert orderUpsert)
        {
            try
            {
                var order = _db.Order.FirstOrDefault(x => x.Id == id);
                if (order != null)
                {
                    order.Status = orderUpsert.Status;
                    order.TransId = orderUpsert.TransId;
                    _db.SaveChanges();
                    return new DbResponse()
                    {
                        Result = true,
                        ExceptionMessage = "Order Updated"
                    };
                }
                else
                {
                    return new DbResponse()
                    {
                        Result = false,
                        ExceptionMessage = "Order does not Exists"
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

    public class OrderUpsert
    {
        //[Required]
        public DateTime CreationDate { get; set; }
        //[Required]
        public bool Status { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than {1}")]
        public int TotalPrice { get; set; }
        public string TransId { get; set; }
        [Required]
        public int UserId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
