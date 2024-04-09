using Jewelry.Data.Entities;
using Jewelry.Data;
using Jewelry.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Jewelry.Models;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Jewelry.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class AdminPageController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IJewelryRepository _repository;
        private readonly UserManager<StoreUser> _userManager;
        private readonly SignInManager<StoreUser> _signInManger;
        private readonly ILogger<AdminPageController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminPageController(IMailService mailService, JewelryContext context, IJewelryRepository repository, UserManager<StoreUser> userManager, SignInManager<StoreUser> signInManager, ILogger<AdminPageController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _mailService = mailService;
            _repository = repository;
            _userManager = userManager;
            _signInManger = signInManager;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult AccountManagement()
        {
            var users = _repository.GetAllUsers();
            return View(users);
        }

        //public IActionResult SearchUsers(string searchText)
        //{
        //    // Thực hiện tìm kiếm người dùng dựa trên searchText
        //    var users = _repository.SearchUsers(searchText);

        //    // Trả về một phần giao diện người dùng (partial view) chứa kết quả tìm kiếm
        //    return PartialView("_UserTable", users);
        //}

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult SaveUser(string userId, string email, string firstName, string lastName, string phoneNumber, string address)
        {
            var user = _repository.GetUserById(userId);
            if (user != null)
            {
                user.Email = email;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.PhoneNumber = phoneNumber;
                user.Address = address;

                if (_repository.SaveAll())
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUsers(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email đã tồn tại.");
                    return View(model);
                }
                var user = new StoreUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    if (model.IsEmployee)
                    {
                        await _userManager.AddToRoleAsync(user, "manager");
                    }

                    await _signInManger.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("AccountManagement", "AdminPage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Lỗi đăng nhập");

            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUserModal(string email, string firstName, string lastName, string phoneNumber, string address, string password, bool isEmployee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _userManager.FindByEmailAsync(email);
                    if (existingUser != null)
                    {
                        return Json(new { success = false, errorMessage = "Email already exists." });
                    }

                    var user = new StoreUser
                    {
                        UserName = email,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        Address = address,
                    };
                    var result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {

                        if (isEmployee)
                        {
                            await _userManager.AddToRoleAsync(user, "manager");
                        }
                        var roles = await _userManager.GetRolesAsync(user);
                        await _signInManger.SignInAsync(user, isPersistent: false);

                        return Json(new { success = true, email = user.Email, firstName = user.FirstName, lastName = user.LastName, phoneNumber = user.PhoneNumber, address = user.Address, roles = roles });
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding category: {ex}");
                    return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                }
            }

            return Json(new { success = false, errorMessage = "Invalid data." });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = _repository.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            _repository.DeleteUser(user);

            if (_repository.SaveAll())
            {
                return Json(new { success = true });
            }
            else
            {
                return StatusCode(500);
            }
        }
        public IActionResult SupplierManagement()
        {
            var Supplier = _repository.GetAllSupplier();
            return View(Supplier);
        }

        public async Task<IActionResult> AddSupplier(string name, string email, string phoneNumber, string address)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isCategoryExists = _repository.IsEmailExists(email);
                    if (isCategoryExists)
                    {
                        // Trả về JSON object thông báo lỗi
                        return Json(new { success = false, errorMessage = "Email already exists." });
                    }

                    var supplier = new Supplier
                    {
                        Name = name,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        Address = address
                    };

                    _repository.AddEntity(supplier);

                    if (_repository.SaveAll())
                    {
                        // Trả về JSON object thông báo thành công
                        return Json(new { success = true, name = supplier.Name, email = supplier.Email, phoneNumber = supplier.PhoneNumber, address = supplier.Address });
                    }
                    else
                    {
                        // Trả về JSON object thông báo lỗi
                        return Json(new { success = false, errorMessage = "Failed to save changes." });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding category: {ex}");
                    // Trả về JSON object thông báo lỗi
                    return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                }
            }

            // Trả về JSON object thông báo lỗi nếu dữ liệu không hợp lệ
            return Json(new { success = false, errorMessage = "Invalid data." });
        }

        public IActionResult SaveSupplier(int supplierId, string name, string email, string phoneNumber, string address)
        {
            var existingSupplier = _repository.GetSupplierByEmail(email);
            if (existingSupplier != null)
            {
                ModelState.AddModelError(string.Empty, "Email đã tồn tại.");
                return Json(new { success = false, message = "Email đã tồn tại." });
            }
            var supplier = _repository.GetSupplierById(supplierId);
            if (supplier != null)
            {
                supplier.Name = name;
                supplier.Email = email;
                supplier.PhoneNumber = phoneNumber;
                supplier.Address = address;

                if (_repository.SaveAll())
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }

        public async Task<IActionResult> DeleteSupplier(int supplierId)
        {
            var supplier = _repository.GetSupplierById(supplierId);
            if (supplier == null)
            {
                return NotFound();
            }

            _repository.DeleteSupplier(supplier);

            if (_repository.SaveAll())
            {
                return Json(new { success = true });
            }
            else
            {
                return StatusCode(500);
            }
        }

        //public IActionResult SearchSupplier(string searchText)
        //{
        //    var supplier = _repository.SearchSupplier(searchText);
        //    return PartialView("_SupplierTable", supplier);
        //}

        public IActionResult CategoryManagement()
        {
            var category = _repository.GetAllCategory();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string name)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isCategoryExists = _repository.IsCategoryExists(name);
                    if (isCategoryExists)
                    {
                        return Json(new { success = false, errorMessage = "Tên danh mục sản phẩm này đã tồn tại." });
                    }
                    var category = new ProductCategory
                    {
                        Name = name,
                    };
                    _repository.AddEntity(category);

                    if (_repository.SaveAll())
                    {
                        return Json(new { success = true, categoryName = category.Name });
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Failed to save changes." });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding category: {ex}");
                    return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                }
            }
            return Json(new { success = false, errorMessage = "Tên danh mục là trường bắt buộc." });
        }

        [HttpPost]
        public async Task<IActionResult> AddSize(string name)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isSizeExists = _repository.IsSizeExists(name);
                    if (isSizeExists)
                    {
                        return Json(new { success = false, errorMessage = "Size này đã tồn tại." });
                    }

                    var size = new Size
                    {
                        Name = name,
                    };

                    _repository.AddEntity(size);

                    if (_repository.SaveAll())
                    {
                        return Json(new { success = true, sizeId = size.Id, sizeName = size.Name });
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Failed to save changes." });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding size: {ex}");
                    return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                }
            }

            // Trả về JSON object thông báo lỗi nếu dữ liệu không hợp lệ
            return Json(new { success = false, errorMessage = "Tên size là trường bắt buộc." });
        }

        [HttpPost]
        public async Task<IActionResult> AddPurity(string name)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isPurityExists = _repository.IsPurityExists(name);
                    if (isPurityExists)
                    {
                        return Json(new { success = false, errorMessage = "Độ tinh khiết này đã tồn tại." });
                    }

                    var purity = new Purity
                    {
                        Name = name,
                    };

                    _repository.AddEntity(purity);

                    if (_repository.SaveAll())
                    {
                        return Json(new { success = true, purityId = purity.Id, purityName = purity.Name });
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Failed to save changes." });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding purity: {ex}");
                    return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                }
            }

            // Trả về JSON object thông báo lỗi nếu dữ liệu không hợp lệ
            return Json(new { success = false, errorMessage = "Độ tinh khiết là trường bắt buộc." });
        }

        [HttpPost]
        public async Task<IActionResult> AddMaterial(string name)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isMaterialExists = _repository.IsMaterialExists(name);
                    if (isMaterialExists)
                    {
                        return Json(new { success = false, errorMessage = "Chất liệu này đã tồn tại." });
                    }

                    var material = new Material
                    {
                        Name = name,
                    };

                    _repository.AddEntity(material);

                    if (_repository.SaveAll())
                    {
                        return Json(new { success = true, materialId = material.Id, materialName = material.Name });
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Failed to save changes." });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding material: {ex}");
                    return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                }
            }

            // Trả về JSON object thông báo lỗi nếu dữ liệu không hợp lệ
            return Json(new { success = false, errorMessage = "Tên chât liệu là trường bắt buộc." });
        }


        public IActionResult SaveCategory(int categoryId, string name)
        {
            var category = _repository.GetCategoryById(categoryId);
            if (category != null)
            {
                category.Name = name;

                if (_repository.SaveAll())
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = _repository.GetCategoryById(categoryId);
            if (category == null)
            {
                return NotFound();
            }
            _repository.DeleteCategory(category);

            if (_repository.SaveAll())
            {
                return Json(new { success = true });
            }
            else
            {
                return StatusCode(500);
            }
        }

        //public IActionResult SearchCategory(string searchText)
        //{
        //    var category = _repository.SearchCategory(searchText);
        //    return PartialView("_CategoryTable", category);
        //}

        public IActionResult ProductManagement()
        {
            var product = _repository.GetConfirmedProductItems();
            return View(product);
        }

        public IActionResult SaveProduct(int productItemId, int productId, decimal? salesPrice, string? warrantyInformation, string? productName)
        {
            var product = _repository.GetProductById(productId);
            var existingProductItem = _repository.GetProductItemById(productItemId);
            if (existingProductItem != null)
            {
                if (salesPrice != null && salesPrice != 0 &&
                   salesPrice != existingProductItem.SalesPrice?.OrderByDescending(pp => pp.EffectiveDate).FirstOrDefault()?.Price)
                {
                    var sPrice = new SalesPrice
                    {
                        Price = (decimal)salesPrice,
                        EffectiveDate = DateTime.Now
                    };
                    existingProductItem.SalesPrice.Add(sPrice);
                }
                if (warrantyInformation != null)
                {
                    product.WarrantyInformation = warrantyInformation;
                }
                if (productName != null)
                {
                    product.Name = productName;
                }
                if (_repository.SaveAll())
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        public string GetCategoryName(int id)
        {
            var category = _repository.GetCategoryById(id);
            return category.Name;
        }

        public IActionResult AddProduct()
        {
            var categories = _repository.GetCategory();
            ViewBag.categories = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model, List<IFormFile> img)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isProductExists = _repository.IsProductExists(model.Name);
                    if (isProductExists)
                    {
                        ModelState.AddModelError("CustomError", "Product already exists.");
                        return View(model);
                    }
                    var existingCategory = _repository.GetCategoryById(model.Category.Id);
                    var product = new Product
                    {
                        Category = existingCategory,
                        Name = model.Name,
                        Description = model.Description,
                        WarrantyInformation = model.WarrantyInformation
                    };

                    // Lưu sản phẩm vào database
                    _repository.AddEntity(product);

                    // Lưu hình ảnh vào thư mục wwwroot/img và cập nhật đường dẫn vào database
                    foreach (var image in img)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "img", uniqueFileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        var productImage = new ProductImage
                        {
                            Product = product,
                            Name = image.FileName,
                            UrlImage = "/img/" + uniqueFileName
                        };
                        _repository.AddEntity(productImage);
                    }

                    if (_repository.SaveAll())
                    {
                        return RedirectToAction("AddInventoryReceipt", "AdminPage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save changes.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding product: {ex}");
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductModal(int categoryId, string name, string description, string warrantyInformation, List<IFormFile> img)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isProductExists = _repository.IsProductExists(name);
                    if (isProductExists)
                    {
                        return Json(new { success = false, errorMessage = "Product already exists." });
                    }
                    var existingCategory = _repository.GetCategoryById(categoryId);
                    var product = new Product
                    {
                        Category = existingCategory,
                        Name = name,
                        Description = description,
                        WarrantyInformation = warrantyInformation
                    };

                    // Lưu sản phẩm vào database
                    _repository.AddEntity(product);

                    // Lưu hình ảnh vào thư mục wwwroot/img và cập nhật đường dẫn vào database
                    foreach (var image in img)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "img", uniqueFileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        var productImage = new ProductImage
                        {
                            Product = product,
                            Name = image.FileName,
                            UrlImage = "/img/" + uniqueFileName
                        };
                        _repository.AddEntity(productImage);
                    }

                    if (_repository.SaveAll())
                    {
                        return Json(new { success = true, productId = product.Id, name = product.Name });
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Failed to save changes." });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding product: {ex}");
                    return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                }
            }
            return Json(new { success = false, errorMessage = "Invalid data." });
        }

        public IActionResult InventoryReceiptManagement()
        {
            var inventory = _repository.GetAllInventoryReceipt();
            return View(inventory);
        }

        public IActionResult InventoryDetailManagement(int id)
        {
            var inventory = _repository.GetDetailsByIvnetoryReceipt(id);
            return View(inventory);
        }

        public string GetSupplierName(int id)
        {
            var supplier = _repository.GetSupplierById(id);
            return supplier.Name;
        }

        public IActionResult AddInventoryReceipt()
        {
            var categories = _repository.GetCategory();
            ViewBag.categories = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            var purity = _repository.GetAllPurity();
            ViewBag.purity = purity.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            var suppliers = _repository.GetAllSupplier();
            ViewBag.suppliers = suppliers.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            var products = _repository.GetAllProducts();
            ViewBag.products = products.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            var sizes = _repository.GetAllSize();
            ViewBag.sizes = sizes.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            var materials = _repository.GetAllMaterial();
            ViewBag.materials = materials.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            List<AddInventoryReceiptViewModel> viewModelList = new List<AddInventoryReceiptViewModel>();

            return View(viewModelList);
        }

        [HttpPost]
        public async Task<JsonResult> AddInventoryReceipt([FromBody] List<AddInventoryReceiptViewModel> inventoryReceipts)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var isSupplier = _repository.GetSupplierById(inventoryReceipts.FirstOrDefault().SupplierId);
                var inventoryReceipt = new InventoryReceipt
                {
                    User = user,
                    Supplier = isSupplier,
                    CreationDate = DateTime.Now,
                    Confirmation = false,
                    Note = ""
                };
                _repository.AddEntity(inventoryReceipt);
                foreach (var item in inventoryReceipts)
                {
                    var purchasePrice = new PurchasePrice
                    {
                        Price = item.PurchasePrice,
                        EffectiveDate = DateTime.Now
                    };
                    var salesPrice = new SalesPrice
                    {
                        Price = item.PurchasePrice,
                        EffectiveDate = DateTime.Now
                    };

                    ProductItem productItem;
                    var existingProductItem = _repository.GetProductItemByProductIdSizeIdMaterialPurityId(item.ProductId, item.SizeId, item.MaterialId, item.PurityId);

                    if (existingProductItem != null)
                    {
                        existingProductItem.Quantity += item.Quantity;

                        if (existingProductItem.PurchasePrice == null)
                        {
                            existingProductItem.PurchasePrice = new List<PurchasePrice>();
                        }
                        existingProductItem.PurchasePrice.Add(purchasePrice);

                        if (existingProductItem.SalesPrice == null)
                        {
                            existingProductItem.SalesPrice = new List<SalesPrice>();
                        }
                        existingProductItem.SalesPrice.Add(salesPrice);

                        productItem = existingProductItem;
                    }
                    else
                    {
                        var isProduct = _repository.GetProductById(item.ProductId);
                        var isSize = _repository.GetSizeById(item.SizeId);
                        var isMaterial = _repository.GetMaterialById(item.MaterialId);
                        var isPurity = _repository.GetPurityById(item.PurityId);

                        // Tạo mới ProductItem
                        productItem = new ProductItem
                        {
                            Product = isProduct,
                            Sizes = isSize,
                            Materials = isMaterial,
                            Purity = isPurity,
                            Weight = item.Weight,
                            PurchasePrice = new List<PurchasePrice> { purchasePrice },
                            SalesPrice = new List<SalesPrice> { salesPrice },
                            Quantity = item.Quantity
                        };

                        _repository.AddEntity(productItem);
                    }


                    // Tạo mới InventoryReceiptDetails
                    var inventoryReceiptDetails = new InventoryReceiptDetails
                    {
                        ProductItem = productItem,
                        InventoryReceipt = inventoryReceipt,
                        Quantity = item.Quantity
                    };
                    _repository.AddEntity(inventoryReceiptDetails);
                }

                await _repository.SaveChangesAsync();
            }

            return Json(new { redirectUrl = Url.Action("InventoryReceiptManagement", "AdminPage") });
        }


        public IActionResult SaveInventoryReceipt(int inventoryId, string note)
        {
            var inventory = _repository.GetInventoryReceiptById(inventoryId);
            if (inventory != null)
            {
                inventory.Note = note;
                if (_repository.SaveAll())
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }

        [Authorize(Roles = "admin")]
        public IActionResult ConfirmationInventory(int inventoryId)
        {
            var inventory = _repository.GetInventoryReceiptById(inventoryId);
            if (inventory != null)
            {
                inventory.ConfirmationDate = DateTime.Now;
                inventory.Confirmation = true;
                if (_repository.SaveAll())
                {
                    return Json(new { success = true, confirmationDate = inventory.ConfirmationDate?.ToString("M/d/yyyy h:mm:ss tt") });
                }
            }
            return Json(new { success = false });
        }

        public IActionResult OrderManagement()
        {
            var orderProduct = _repository.GetAllOrders();
            return View(orderProduct);
        }

        public IActionResult AddOrder()
        {
            var products = _repository.GetAllProducts();
            ViewBag.products = products.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            ViewBag.materials = new List<SelectListItem>();
            ViewBag.purity = new List<SelectListItem>();
            ViewBag.sizes = new List<SelectListItem>();
            List<AddOrderViewModel> viewModelList = new List<AddOrderViewModel>();
            return View(viewModelList);
        }

        [HttpPost]
        public async Task<JsonResult> AddOrder([FromBody] List<AddOrderViewModel> model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var paymentId = _repository.GetPaymentById(2);
                    var statusCategory = _repository.GetStatusCategoryById(1);
                    var order = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderNumber = Guid.NewGuid().ToString(),
                        User = user,
                        DeliveryAddress = model.FirstOrDefault().Address,
                        PhoneNumber = model.FirstOrDefault().PhoneNumber,
                        ConsigneeName = model.FirstOrDefault().Customer,
                        Note = model.FirstOrDefault().Note,
                        PaymentMethod = paymentId,
                        Status = new List<Status>
                    {
                        new Status
                        {
                            StatusCategory = statusCategory,
                            UpdateDate = DateTime.Now,
                            User = user,
                            Note = model.FirstOrDefault().Note
                        }
                    }
                    };
                    _repository.AddEntity(order);
                    foreach (var item in model)
                    {
                        var orderItem = new OrderItem
                        {
                            Order = order,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        };
                        _repository.AddEntity(orderItem);
                    }
                    await _repository.SaveChangesAsync();
                }
            }
            return Json(new { redirectUrl = Url.Action("OrderManagement", "AdminPage") });
        }

        public IActionResult GetPrice(int productId, int sizeId, int materialId, int purityId)
        {
            var productItem = _repository.GetProductItemByProductIdSizeIdMaterialPurityId(productId, sizeId, materialId, purityId);
            if (productItem != null)
            {
                var latestPrice = productItem.SalesPrice.OrderByDescending(sp => sp.EffectiveDate).FirstOrDefault();

                if (latestPrice != null)
                {
                    return Json(new { success = true, price = latestPrice.Price });
                }
            }

            return Json(new { success = false });
        }

        public JsonResult GetMaterials(int productId)
        {
            var productItems = _repository.GeMaterialsByProduct(productId);
            var material = productItems.Select(pi => pi.Materials).Distinct();
            return Json(material);
        }

        public JsonResult GetPurity(int productId, int materialId)
        {
            var productItems = _repository.GetProductItemsByProductPurity(productId, materialId);
            var purity = productItems.Select(pi => pi.Purity).Distinct();
            return Json(purity);
        }

        public JsonResult GetSizes(int productId, int materialId, int purityId)
        {
            var productItems = _repository.GetProductItemsByProductPuritySize(productId, materialId, purityId);
            var sizes = productItems.Select(pi => pi.Sizes).Distinct();
            return Json(sizes);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int orderId, string note)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var order = _repository.GetOrderById(orderId);
                if (order != null)
                {
                    // Get the next status
                    var status = _repository.GetStatusByOrder(orderId);
                    if (status.StatusCategory.Id == 1)
                    {
                        // Lấy danh sách các OrderItem trong Order
                        var orderItems = order.Items;

                        // Duyệt qua từng OrderItem
                        foreach (var item in orderItems)
                        {
                            // Lấy ProductItem tương ứng
                            var productItem = item.Product;

                            // Giảm số lượng ProductItem
                            productItem.Quantity -= item.Quantity;

                            // Cập nhật ProductItem
                            _repository.UpdateEntity(productItem);
                        }

                        // Lưu thay đổi
                        await _repository.SaveChangesAsync();
                    }

                    var nextStatusId = status.StatusCategory.Id + 1;
                    if (nextStatusId > 5) // If the status is "Thành công", don't change it
                    {
                        return Json(new { success = false });
                    }

                    var nextStatus = _repository.GetStatusCategoryById(nextStatusId);

                    if (nextStatus != null)
                    {
                        var newStatus = new Status
                        {
                            Order = order,
                            StatusCategory = nextStatus,
                            UpdateDate = DateTime.Now,
                            User = user,
                            Note = note
                        };
                        _repository.AddEntity(newStatus);
                        await _repository.SaveChangesAsync();
                        return Json(new { success = true, status = nextStatus.Name });
                    }
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId, string note)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var order = _repository.GetOrderById(orderId);
                if (order != null)
                {
                    var cancelledStatus = _repository.GetStatusCategoryById(6);

                    if (cancelledStatus != null)
                    {
                        var newStatus = new Status
                        {
                            Order = order,
                            StatusCategory = cancelledStatus,
                            UpdateDate = DateTime.Now,
                            User = user,
                            Note = note
                        };
                        _repository.AddEntity(newStatus);
                        await _repository.SaveChangesAsync();
                        return Json(new { success = true, status = cancelledStatus.Name });
                    }
                }
            }
            return Json(new { success = false });
        }

        public IActionResult PrintOrder(int id)
        {
            var order = _repository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public IActionResult GetOrderDetails(int orderId)
        {
            var order = _repository.GetOrderByIds(orderId);
            if (order != null)
            {
                var products = order.Items.Select(item => new
                {
                    name = item.Product.Product.Name,
                    category = item.Product.Product.Category.Name,
                    material = item.Product.Materials.Name,
                    size = item.Product.Sizes.Name,
                    purity = item.Product.Purity.Name,
                    weight = item.Product.Weight,
                    quantity = item.Quantity,
                    warrantyInformation = item.Product.Product.WarrantyInformation,
                    salesPrice = item.Product.SalesPrice.OrderByDescending(sp => sp.EffectiveDate).FirstOrDefault().Price,
                    imageUrl = item.Product.Product.Img.FirstOrDefault()?.UrlImage
                });

                return Json(new { success = true, products = products });
            }

            return Json(new { success = false });
        }

        public IActionResult Status(int orderId)
        {
            var status = _repository.GetAllStatusByOrder(orderId);
            return Json(new { success = true, status = status });
        }

        public IActionResult WarrantyManagement()
        {
            var orders = _repository.GetAllOrders();
            ViewBag.orders = orders.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Id.ToString() });
            var warranty = _repository.GetAllWarranty();
            return View(warranty);
        }

        [HttpPost]
        public async Task<IActionResult> AddWarrantyModal(int orderId, decimal price, string warrantyInformation, DateTime appointmentDate, string note)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    var existingOrder = _repository.GetOrderById(orderId);
                    var warranty = new Warranty
                    {
                        Order = existingOrder,
                        Price = price,
                        WarrantyInformation = warrantyInformation,
                        CreationDate = DateTime.Now,
                        AppointmentDate = appointmentDate,
                        Note = note,
                        User = user
                    };
                    _repository.AddEntity(warranty);

                    if (_repository.SaveAll())
                    {
                        return Json(new
                        {
                            success = true,
                            warrantyId = warranty.Id,
                            orderId = warranty.Order.Id,
                            warrantyInformation = warranty.WarrantyInformation,
                            price = warranty.Price,
                            creationDate = warranty.CreationDate,
                            appointmentDate = warranty.AppointmentDate,
                            note = warranty.Note,
                            firstName = warranty.User.FirstName,
                            lastName = warranty.User.LastName
                        });
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Failed to save changes." });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding product: {ex}");
                    return Json(new { success = false, errorMessage = "An error occurred while processing your request." });
                }
            }
            return Json(new { success = false, errorMessage = "Invalid data." });
        }

        public async Task<IActionResult> DeleteWarranty(int warrantyId)
        {
            var warranty = _repository.GetWarrantyById(warrantyId);
            if (warranty == null)
            {
                return NotFound();
            }
            _repository.DeleteWarranty(warranty);

            if (_repository.SaveAll())
            {
                return Json(new { success = true });
            }
            else
            {
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> ConfirmationWarranty(int warrantyId)
        {
            var user = await _userManager.GetUserAsync(User);
            var warranty = _repository.GetWarrantyById(warrantyId);
            if (warranty != null)
            {
                warranty.ReturnDate = DateTime.Now;
                warranty.EmployeeName = user.FirstName + " " + user.LastName;
                if (_repository.SaveAll())
                {
                    return Json(new { success = true, returnDate = warranty.ReturnDate?.ToString("M/d/yyyy h:mm:ss tt"), lastName = user.LastName, firstName = user.FirstName });
                }
            }
            return Json(new { success = false });
        }
        public IActionResult PrintWarranty(int id)
        {
            var warranty = _repository.GetWarrantyById(id);
            return View(warranty);
        }

        public IActionResult InventoryReport()
        {
            var model = new InventoryReportViewModel
            {
                Products = _repository.GetAllProducts(),
                SelectedYear = DateTime.Now.Year,
                SelectedMonth = DateTime.Now.Month
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GetInventoryReport(int productId, int year, int month)
        {
            var inventoryReport =  _repository.GetInventoryReport(productId, year, month);
            return Json(inventoryReport);
        }

        public IActionResult MonthlySalesReport()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetSalesReport(int year, int month)
        {
            var salesReport = _repository.GetMonthlySalesReport(year, month);
            return Json(salesReport);
        }

    }
}
