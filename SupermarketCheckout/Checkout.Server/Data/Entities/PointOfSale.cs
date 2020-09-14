using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Server.Data.Entities
{
    public class PointOfSale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Shop Shop { get; set; }

        [Required]
        [ForeignKey(nameof(Shop))]
        public Guid ShopId { get; set; }

        public ICollection<Basket> Baskets { get; set; } 
            = new List<Basket>();
    }
}
