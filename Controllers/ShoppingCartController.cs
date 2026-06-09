using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TH_LTW_Buoi02.Extensions;
using TH_LTW_Buoi02.Models;
using TH_LTW_Buoi02.Repositories;

namespace TH_LTW_Buoi02.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await GetProductFromDatabase(productId);
            if (product == null) return NotFound();

            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity,
                ImageUrl = product.ImageUrl
            };

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        private async Task<Product> GetProductFromDatabase(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }
            ViewBag.Cart = cart;
            return View(new Order());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            order.UserId = user?.Id ?? string.Empty;
            order.OrderDate = DateTime.UtcNow;

            ModelState.Remove(nameof(order.UserId));
            ModelState.Remove(nameof(order.ApplicationUser));
            if (!ModelState.IsValid)
            {
                ViewBag.Cart = cart;
                return View(order);
            }

            if (string.IsNullOrEmpty(order.PaymentMethod))
            {
                order.PaymentMethod = "Tiền mặt";
            }

            var baseTotal = cart.Items.Sum(i => i.Price * i.Quantity);
            decimal discount = 0;
            if (!string.IsNullOrEmpty(order.VoucherCode))
            {
                var code = order.VoucherCode.Trim().ToUpper();
                if (code == "36THSHOP")
                {
                    discount = Math.Round(baseTotal * 0.15m);
                }
                else if (code == "GIAM10")
                {
                    discount = Math.Round(baseTotal * 0.10m);
                }
                else if (code == "GIAM50")
                {
                    discount = Math.Round(baseTotal * 0.50m);
                }
                else
                {
                    discount = 20000; // Flat discount
                }
            }

            order.TotalPrice = baseTotal + 30000 - discount;
            if (order.TotalPrice < 0) order.TotalPrice = 0;

            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");

            return View("OrderCompleted", order);
        }
    }
}
