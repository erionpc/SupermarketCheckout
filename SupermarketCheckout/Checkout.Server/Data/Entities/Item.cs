using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Server.Data.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Stock Keeping Unit. Unique identifier of the item.
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public ICollection<ItemPrice> Prices { get; set; } 
            = new List<ItemPrice>();
    }
}
