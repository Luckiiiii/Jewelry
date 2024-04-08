using Jewelry.Data;
using Jewelry.Data.Entities;
using Jewelry.Models;
using Jewelry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Jewelry.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IJewelryRepository _repository;
        private readonly UserManager<StoreUser> _userManager;

        public AppController(IMailService mailService, JewelryContext context, IJewelryRepository repository, UserManager<StoreUser> userManager)
        {
            _mailService = mailService;
            _repository = repository;
            _userManager = userManager;
        }
        //[Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            //var results = _context.Products.ToList();
            return View();
        }

        [HttpGet("contact")]
        //[Authorize(Roles = "manager")]
        [Authorize(Roles = "admin")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
          

            return View();

        }

        [HttpPost("contact")]
        
        public IActionResult Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                _mailService.SendMessage("luckyphuocs@gmail.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            return View();
        }
        [Authorize(Roles = "manager")]
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
        //[Authorize(Roles = "admin")]
        public IActionResult Shop() 
        {
            var results = _repository.GetAllProducts();
            var priceRanges = new Dictionary<int, (decimal MinPrice, decimal MaxPrice)>();
            var categories = _repository.GetAllCategory();
            foreach (var product in results)
            {
                priceRanges[product.Id] = _repository.GetProductItemPriceRange(product.Id);
            }

            ViewBag.PriceRanges = priceRanges;
            ViewBag.Categories = categories;
            return View(results);
        }

        public IActionResult ProductDetail(int id)
        {
            var results = _repository.GetProductById(id);
            return View(results);
        }

        public IActionResult GetProductsByCategory(int? categoryId)
        {
            IEnumerable<Product> products;
            if (categoryId.HasValue)
            {
                products = _repository.GetProductsByCategory(categoryId.Value);
            }
            else
            {
                products = _repository.GetAllProducts();
            }

            var priceRanges = new Dictionary<int, (decimal MinPrice, decimal MaxPrice)>();
            foreach (var product in products)
            {
                priceRanges[product.Id] = _repository.GetProductItemPriceRange(product.Id);
            }

            ViewBag.PriceRanges = priceRanges;
            return PartialView("_ProductList", products);
        }

        //public IActionResult GetProductsByName(string name)
        //{
        //    var products = _repository.GetProductsByName(name);
        //    var priceRanges = new Dictionary<int, (decimal MinPrice, decimal MaxPrice)>();
        //    foreach (var product in products)
        //    {
        //        priceRanges[product.Id] = _repository.GetProductItemPriceRange(product.Id);
        //    }

        //    ViewBag.PriceRanges = priceRanges;
        //    return PartialView("_ProductList", products);
        //}
        public IActionResult GetProductsByName(string name, int? categoryId)
        {
            IEnumerable<Product> products;
            if (categoryId.HasValue)
            {
                products = _repository.GetProductsByCategoryAndName(categoryId.Value, name);
            }
            else
            {
                products = _repository.GetProductsByName(name);
            }

            var priceRanges = new Dictionary<int, (decimal MinPrice, decimal MaxPrice)>();
            foreach (var product in products)
            {
                priceRanges[product.Id] = _repository.GetProductItemPriceRange(product.Id);
            }

            ViewBag.PriceRanges = priceRanges;
            return PartialView("_ProductList", products);
        }

        public IActionResult GetPurity(int productId, int materialId)
        {
            var productItems = _repository.GetProductItemsByProductPurity(productId, materialId);
            var purity = productItems.Select(pi => pi.Purity).Distinct();

            return Json(purity);
        }

        public IActionResult GetSizes(int productId, int materialId, int purityId)
		{
			var productItems = _repository.GetProductItemsByProductPuritySize(productId, materialId, purityId);
			var sizes = productItems.Select(pi => pi.Sizes).Distinct();

			return Json(sizes);
		}

		public IActionResult GetPrice(int productId, int sizeId, int materialId, int purityId)
		{
			var productItem = _repository.GetProductItemByProductIdSizeIdMaterialPurityId(productId, sizeId, materialId, purityId);
			if (productItem != null)
            {
				var latestPrice = productItem.SalesPrice.OrderByDescending(sp => sp.EffectiveDate).FirstOrDefault();

                if (latestPrice != null)
				{
					return Json(new { success = true, price = latestPrice.Price, weight = productItem.Weight, productItemId = productItem.Id });
				}
			}

			return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult Checkout(string cartData)
        {
            var cart = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>(cartData);
            var paymentMethods = _repository.GetAllPayments();
            ViewBag.PaymentMethods = paymentMethods;
            if (cart == null) return Json(new { success = false });
            return View("Checkout", cart);
        }


        [HttpPost]
        public async Task<IActionResult> CheckoutOrder(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = JsonConvert.DeserializeObject<IEnumerable<CartViewModel>>(model.CartData);
                if (cart == null || !cart.Any())
                {
                    ModelState.AddModelError("", "Giỏ hàng không có sản phẩm nào.");
                    return View(model);
                }
                var paymentId = _repository.GetPaymentById(model.PaymentMethodId);
                var statusCategory = _repository.GetStatusCategoryById(1);
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ModelState.AddModelError("", "Người dùng không tồn tại.");
                    return RedirectToAction("Login", "Account");
                }
                if(model.Note == null)
                {
                    model.Note = "";
                }
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = Guid.NewGuid().ToString(),
                    User = user,
                    DeliveryAddress = model.DeliveryAddress,
                    PhoneNumber = model.PhoneNumber,
                    ConsigneeName = $"{user.FirstName} {user.LastName}",
                    Note = model.Note,
                    PaymentMethod = paymentId,
                    Items = cart.Select(item => new OrderItem
                    {
                        ProductId = item.productItemId,
                        Quantity = item.quantity,
                        UnitPrice = item.price
                    }).ToList(),
                    Status = new List<Status>
                    {
                        new Status
                        {
                            StatusCategory = statusCategory,
                            UpdateDate = DateTime.Now,
                            User = user,
                            Note = model.Note
                        }
                    }
                };

                _repository.AddEntity(order);
                await _repository.SaveChangesAsync();

                return RedirectToAction("OrderConfirmation", new { order.OrderNumber });
            }

            return View(model);
        }

        public IActionResult OrderConfirmation(string orderNumber)
        {
            var order = _repository.OrderConfirmation(orderNumber);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> HistoryOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                ModelState.AddModelError("", "Vui lòng đăng nhập vào hệ thống.");
                return RedirectToAction("Login", "Account");
            }
            var order = _repository.GetOrderByUser(user);

            return View(order);
        }

        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) 
            {
                var orderItems = _repository.GetOrderItemByOrder(orderId);
                return View(orderItems);
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng đăng nhập vào hệ thống.");
                return View();
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AccountManagement()
        {
            return View();
        }
    }
}