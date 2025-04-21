using System.ComponentModel.DataAnnotations;

namespace ProkodersECommerce.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }

}
