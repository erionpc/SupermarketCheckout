using Checkout.Server.Data.Entities.Enums;
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

        public DateTime? ClosedOn { get; set; }

        public BasketStatus Status { get; set; } = BasketStatus.Active;

        [Column(TypeName = "decimal(9,2)")]
        public decimal TotalPrice { get; set; } = 0;

        public ICollection<BasketItem> BasketItems { get; set; } 
            = new List<BasketItem>();

        public ICollection<ReceiptItem> ReceiptItems { get; set; }
            = new List<ReceiptItem>();

    }
}
