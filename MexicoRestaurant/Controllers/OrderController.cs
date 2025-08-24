using MexicoRestaurant.Data;
using MexicoRestaurant.Models;
using MexicoRestaurant.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MexicoRestaurant.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Repository<Product> _products;
        private Repository<Order> _orders;
        private readonly UserManager<ApplicationUser> _userManger;

        public OrderController (ApplicationDbContext context ,UserManager<ApplicationUser>userManger)
        {
            _context = context;
            _userManger = userManger;
            _products = new Repository<Product>(context);
            _orders = new Repository<Order>(context);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create ()
        {
            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel") ?? new OrderVM
            {
                orderItems = new List<OrderItems>(),
                Products = await _products.GetAllAsync()
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddItem (int ProductId , int proQty)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null)
            {
                return NotFound();
            }
            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel") ?? new OrderVM
            {
                orderItems = new List<OrderItems>(),
                Products = await _products.GetAllAsync()
            };
            var exostingItem = model.orderItems.FirstOrDefault(i => i.ProductId == ProductId);

            if (exostingItem != null)
            {
                exostingItem.Quantity += proQty;

            }else
            {
                model.orderItems.Add(new OrderItems
                {
                    ProductId = ProductId,
                    ProductName = product.Name,
                    Quantity = proQty,
                    Price = product.Price
                });
            }
            model.TotalAmount = model.orderItems.Sum(i => i.Price * i.Quantity);

            HttpContext.Session.Set("OrderViewModel", model);
            return RedirectToAction("Create");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel");
            if (model == null || model.orderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder ()
        {
            var model = HttpContext.Session.Get<OrderVM>("OrderViewModel");
            if (model == null || model.orderItems.Count == 0)
            {
                return RedirectToAction("Create");
            }
            //Create a new Order Entity
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                UserId = _userManger.GetUserId(User),
            };

            //Add OrderItems to the Order entity

            foreach (var item in model.orderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);

                if (product == null || product.Stock < item.Quantity)
                {
                    TempData["ErrorMessage"] = $" Sorry, The Product {item.ProductName}is not available in the requested quantity.\"";
                    return RedirectToAction("Cart");
                }

                product.Stock -= item.Quantity;
                _context.Products.Update(product);

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }
            await _context.SaveChangesAsync();
            await _orders.AddAsync(order);

            HttpContext.Session.Remove("OrderViewModel");

            return RedirectToAction("ViewOrders");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewOrders ()
        {
            var userId = _userManger.GetUserId(User);
            var userOrders = await _orders.GetAllByIdAsync(userId, "UserId", new QueryOptions<Order>
            {
                Includes = "OrderItems.Product"
            });
            return View(userOrders);
        }
    }
}
