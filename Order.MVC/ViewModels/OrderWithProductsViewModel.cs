using Order.MVC.Models;

namespace Order.MVC.ViewModels
{
    public class OrderWithProductsViewModel
    {
        public OrderEntity Order { get; set; }
        public List<ProductEntity> AvailableProducts { get; set; } = new List<ProductEntity>();
        public List<int> SelectedProductIds { get; set; } = new List<int>();
        public List<ProductQuantity> ProductQuantities { get; set; } = new List<ProductQuantity>();

        public decimal Total =>
            ProductQuantities?.Sum(pq =>
                AvailableProducts?.FirstOrDefault(p => p.ProductId == pq.ProductId)?.Value * pq.Quantity
            ) ?? 0;
    }

    public class ProductQuantity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
