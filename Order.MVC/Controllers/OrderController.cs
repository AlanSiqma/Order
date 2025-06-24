using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Order.MVC.Data;
using Order.MVC.Models;
using Order.MVC.ViewModels;

namespace Order.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderContext _context;

        public OrderController(OrderContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderEntity.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderEntity = await _context.OrderEntity
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderEntity == null)
            {
                return NotFound();
            }

            return View(orderEntity);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Name,DateOrder,DateReady,Observation")] OrderEntity orderEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderEntity);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderEntity = await _context.OrderEntity.FindAsync(id);
            if (orderEntity == null)
            {
                return NotFound();
            }
            return View(orderEntity);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Name,DateOrder,DateReady,Observation")] OrderEntity orderEntity)
        {
            if (id != orderEntity.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderEntityExists(orderEntity.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orderEntity);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderEntity = await _context.OrderEntity
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderEntity == null)
            {
                return NotFound();
            }

            return View(orderEntity);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderEntity = await _context.OrderEntity
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (orderEntity != null)
            {
                // Primeiro remove os relacionamentos
                if (orderEntity.OrderProducts != null && orderEntity.OrderProducts.Any())
                {
                    _context.OrderProduct.RemoveRange(orderEntity.OrderProducts);
                }

                // Depois remove o pedido
                _context.OrderEntity.Remove(orderEntity);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Pedido excluído com sucesso!";
            return RedirectToAction(nameof(TrackOrders));
        }

        private bool OrderEntityExists(int id)
        {
            return _context.OrderEntity.Any(e => e.OrderId == id);
        }
        // GET: Order/CreateWithProducts
        public async Task<IActionResult> CreateWithProducts()
        {
            var viewModel = new OrderWithProductsViewModel
            {
                Order = new OrderEntity { DateOrder = DateTime.Now },
                AvailableProducts = await _context.ProductEntity.ToListAsync()
            };
            return View(viewModel);
        }

        // POST: Order/CreateWithProducts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithProducts(OrderWithProductsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure order has current date if not provided
                    if (viewModel.Order.DateOrder == default)
                    {
                        viewModel.Order.DateOrder = DateTime.Now;
                    }

                    // Make sure Observation is not null
                    viewModel.Order.Observation ??= string.Empty;

                    // Add the order
                    _context.Add(viewModel.Order);
                    await _context.SaveChangesAsync();

                    // Debug line to confirm OrderId was generated
                    Console.WriteLine($"Created Order with ID: {viewModel.Order.OrderId}");

                    // Add order-product relationships with quantities
                    if (viewModel.ProductQuantities != null)
                    {
                        foreach (var pq in viewModel.ProductQuantities.Where(pq => pq.Quantity > 0))
                        {
                            var orderProduct = new OrderProductEntity
                            {
                                OrderId = viewModel.Order.OrderId,
                                ProductId = pq.ProductId,
                                Quantity = pq.Quantity
                            };

                            _context.OrderProduct.Add(orderProduct);
                        }

                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Order created successfully!";
                    return RedirectToAction(nameof(OrderDetails), new { id = viewModel.Order.OrderId });
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error saving order: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the order. Please try again.");
                }
            }

            // If we get here, something failed
            viewModel.AvailableProducts = await _context.ProductEntity.ToListAsync();
            return View(viewModel);
        }
        // GET: Order/TrackOrders
        public async Task<IActionResult> TrackOrders()
        {
            var orders = await _context.OrderEntity
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .OrderByDescending(o => o.DateOrder)
                .ToListAsync();

            return View(orders);
        }

        // GET: Order/OrderDetails/5
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderEntity = await _context.OrderEntity
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (orderEntity == null)
            {
                return NotFound();
            }

            return View(orderEntity);
        }
        // GET: Order/MarkAsReady/5
        public async Task<IActionResult> MarkAsReady(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderEntity = await _context.OrderEntity.FindAsync(id);
            if (orderEntity == null)
            {
                return NotFound();
            }

            // Use the ReadyOrder method from the OrderEntity model
            orderEntity.ReadyOrder();

            _context.Update(orderEntity);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Order marked as ready!";
            return RedirectToAction(nameof(TrackOrders));
        }
        // Adicione ao OrderController
        [HttpGet]
        public async Task<IActionResult> MarkAsDelivered(int id)
        {
            var order = await _context.OrderEntity.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            order.DeliverOrder();
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(TrackOrders));
        }

        [HttpGet]
        public async Task<IActionResult> OrderBoard()
        {
            // Recupera pedidos da última hora
            var cutoffTime = DateTime.Now.AddHours(-4);

            var orders = await _context.OrderEntity
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                //.Where(o => o.DateOrder >= cutoffTime)
                .OrderByDescending(o => o.DateOrder)
                .ToListAsync();

            return View(orders);
        }


    }
}
