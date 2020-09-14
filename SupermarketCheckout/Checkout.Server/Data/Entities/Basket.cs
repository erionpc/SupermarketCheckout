using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Server.Data.Entities
{
    public class Basket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public PointOfSale PointOfSale { get; set; }

        /// <summary>
        /// Point of sale id
        /// </summary>
        [Required]
        [ForeignKey(nameof(PointOfSale))]
        public Guid PosId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public ICollection<BasketItem> BasketItems { get; set; } 
            = new List<BasketItem>();
    }
}
