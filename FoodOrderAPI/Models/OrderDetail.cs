using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderAPI.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public int FoodItemId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [ForeignKey("FoodItemId")]
        public FoodItem FoodItem { get; set; }
    }
}
