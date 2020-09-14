using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Server.Data.Entities
{
    public class BasketItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Basket Basket { get; set; }

        [Required]
        [ForeignKey(nameof(Basket))]
        public Guid BasketId { get; set; }

        public Item Item { get; set; }

        [Required]
        [ForeignKey(nameof(Item))]
        public Guid ItemId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
