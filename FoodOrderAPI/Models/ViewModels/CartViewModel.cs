using FoodOrderAPI.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderAPI.Models.ViewModels
{
    public class CartViewModel
    {
        private readonly ISession _session;
        private readonly ApplicationDBContext _db;
        public CartViewModel(ISession session, ApplicationDBContext db)
        {
            _session = session;
            _db = db;
        }

        public DbResponse AddToCart(OrderDetailVM orderDetail) {
            List<OrderDetailVM> sessionList = new List<OrderDetailVM>();
            if (string.IsNullOrEmpty(_session.GetString(SD.SessionCart)))
            {
                sessionList.Add(orderDetail);
                _session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                sessionList = _session.GetObject<List<OrderDetailVM>>(SD.SessionCart);
                if (!sessionList.Contains(orderDetail))
                {
                    sessionList.Add(orderDetail);
                    _session.SetObject(SD.SessionCart, sessionList);
                }
                else
                {
                    var orderDetailObj =
                        sessionList.FirstOrDefault(x => x.FoodItemId == orderDetail.FoodItemId);
                    sessionList.Remove(orderDetailObj);
                    orderDetailObj.Quantity += orderDetail.Quantity;
                    sessionList.Add(orderDetailObj);
                    _session.SetObject(SD.SessionCart, sessionList);
                }
            }
            return new DbResponse()
            {
                Result = true,
                ExceptionMessage = _session.GetObject<List<OrderDetailVM>>(SD.SessionCart).Count().ToString()
            };
        }

        public IEnumerable<OrderDetailVM> GetAllCartItems()
        {
            List<OrderDetailVM> sessionList = new List<OrderDetailVM>();
            if (!string.IsNullOrEmpty(_session.GetString(SD.SessionCart))) 
            {
                sessionList = _session.GetObject<List<OrderDetailVM>>(SD.SessionCart);
            }
            return sessionList;
        }

        public bool UpdateQuantity(int id, int quantity)
        {
            List<OrderDetailVM> sessionList = new List<OrderDetailVM>();
            sessionList = _session.GetObject<List<OrderDetailVM>>(SD.SessionCart);
            OrderDetailVM orderDetailObj = sessionList.FirstOrDefault(x => x.FoodItemId == id);
            if (sessionList.Contains(orderDetailObj))
            {
                sessionList.Remove(orderDetailObj);
                orderDetailObj.Quantity = quantity;
                sessionList.Add(orderDetailObj);
                _session.SetObject(SD.SessionCart, sessionList);
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            List<OrderDetailVM> sessionList = new List<OrderDetailVM>();
            sessionList = _session.GetObject<List<OrderDetailVM>>(SD.SessionCart);
            OrderDetailVM orderDetailObj = sessionList.FirstOrDefault(x => x.FoodItemId == id);
            if (sessionList.Contains(orderDetailObj))
            {
                sessionList.Remove(orderDetailObj);
                _session.SetObject(SD.SessionCart, sessionList);
                return true;
            }
            return false;
        }
    }

    public class OrderDetailVM
    {
        [Required]
        public int FoodItemId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than {1}")]
        public int Quantity { get; set; }
    }
}
