using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.MVC.Models
{
    public class OrderProductEntity
    {
        [Key]
        public int OrderProductId { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual OrderEntity Order { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductEntity Product { get; set; }

        public int Quantity { get; set; }
    }
}
