using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Server.Data.Entities
{
    public class ReceiptItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Item Item { get; set; }

        [Required]
        [ForeignKey(nameof(Item))]
        public Guid ItemId { get; set; }

        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        public Basket Basket { get; set; }

        [Required]
        [ForeignKey(nameof(Basket))]
        public Guid BasketId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }

        [MaxLength(100)]
        public string OfferText { get; set; }
    }
}
