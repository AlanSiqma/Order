using System.ComponentModel.DataAnnotations;

namespace Order.MVC.Models
{
    public class OrderEntity
    {
        public OrderEntity()
        {
            this.DateOrder = DateTime.Now.AddHours(-3);
        }
        [Key]
        public int OrderId { get; set; }

        [Display(Name = "Nome do Cliente")]
        [Required(ErrorMessage = "O nome do cliente é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Data do Pedido")]
        public DateTime DateOrder { get; set; }

        [Display(Name = "Data de Entrega")]
        public DateTime? DateReady { get; set; }

        [Display(Name = "Data de Retirada")]
        public DateTime? DateDelivered { get; set; }

        [Display(Name = "Observações")]
        public string? Observation { get; set; }

        public void ReadyOrder()
        {
            this.DateReady = DateTime.Now;
        }

        public void DeliverOrder()
        {
            this.DateDelivered = DateTime.Now;
        }

        public decimal Value
        {
            get
            {
                decimal result = 0;

                if (this.ListProducts != null && this.ListProducts.Count >= 0)
                    result = this.ListProducts.Sum(x => x.Value);

                return result;
            }
        }

        public virtual List<ProductEntity> ListProducts { get; set; } = new List<ProductEntity>();
        public virtual List<OrderProductEntity> OrderProducts { get; set; } = new List<OrderProductEntity>();
    }
}