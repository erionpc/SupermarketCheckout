using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Server.Data.Entities
{
    public class ItemPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Item Item { get; set; }

        [Required]
        [ForeignKey(nameof(Item))]
        public Guid ItemId { get; set; }

        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Amount { get; set; }

        public DateTime ValidFrom { get; set; } = DateTime.MinValue;

        public DateTime ValidTo { get; set; } = DateTime.MaxValue;
    }
}
