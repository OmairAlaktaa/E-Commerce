﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProkodersECommerce.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

}
