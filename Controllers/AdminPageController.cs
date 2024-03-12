using Jewelry.Data.Entities;
using Jewelry.Data;
using Jewelry.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Jewelry.Models;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Jewelry.Controllers
{
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

        public IActionResult SearchUsers(string searchText)
        {
            // Thực hiện tìm kiếm người dùng dựa trên searchText
            var users = _repository.SearchUsers(searchText);

            // Trả về một phần giao diện người dùng (partial view) chứa kết quả tìm kiếm
            return PartialView("_UserTable", users);
        }


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

        public async Task<IActionResult> AddUsers(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email already exists.");
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

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }
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

        public async Task<IActionResult> AddSupplier(AddSupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isEmailExists = _repository.IsEmailExists(model.Email);
                    if (isEmailExists)
                    {
                        ModelState.AddModelError("CustomError", "Email already exists.");
                        return View(model);
                    }

                    var supplier = new Supplier
                    {
                        Name = model.Name,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address
                    };

                    _repository.AddEntity(supplier); 

                    if (_repository.SaveAll())
                    {               
                        return RedirectToAction("SupplierManagement", "AdminPage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save changes."); 
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding supplier: {ex}");
                    ModelState.AddModelError("", "An error occurred while processing your request."); 
                }
            }
            return View(model);
        }

        public IActionResult SaveSupplier(int supplierId, string name, string email, string phoneNumber, string address)
        {
            //var existingSupplier = _repository.GetSupplierByEmail(email);
            //if (existingSupplier != null)
            //{
            //    ModelState.AddModelError(string.Empty, "Email already exists.");
            //    return Json(new { success = false, message = "Email already exists." });
            //}
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

        public IActionResult SearchSupplier(string searchText)
        {
            var supplier = _repository.SearchSupplier(searchText);
            return PartialView("_SupplierTable", supplier);
        }

        public IActionResult CategoryManagement()
        {
            var category = _repository.GetAllCategory();
            return View(category);
        }

        public async Task<IActionResult>AddCategory(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isCategoryExists = _repository.IsCategoryExists(model.Name);
                    if (isCategoryExists)
                    {
                        ModelState.AddModelError("CustomError", "Category already exists.");
                        return View(model);
                    }

                    var category = new ProductCategory
                    {
                        Name = model.Name,
                    };

                    _repository.AddEntity(category);

                    if (_repository.SaveAll())
                    {
                        return RedirectToAction("CategoryManagement", "AdminPage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save changes.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error adding supplier: {ex}");
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                }
            }
            return View(model);
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

        public IActionResult SearchCategory(string searchText)
        {
            var category = _repository.SearchCategory(searchText);
            return PartialView("_CategoryTable", category);
        }

        public IActionResult ProductManagement()
        {

            var product = _repository.GetAllProducts();
            return View(product);
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
                        return RedirectToAction("AccountManagement", "AdminPage");
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

        public IActionResult InventoryReceiptManagement()
        {
            var inventory = _repository.GetAllInventoryReceiptDetails();
            return View(inventory);
        }

        public string GetSupplierName(int id)
        {
            var supplier = _repository.GetSupplierById(id);
            return supplier.Name;
        }

        public IActionResult AddInventoryReceipt()
        {
            
            var suppliers = _repository.GetAllSupplier();
            ViewBag.suppliers = suppliers.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name});
            var products = _repository.GetAllProducts();
            ViewBag.products = products.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            var sizes = _repository.GetAllSize();
            ViewBag.sizes = sizes.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            var materials = _repository.GetAllMaterial();
            ViewBag.materials = materials.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            //var suppliersid = _repository.GetAllSupplier();
            //ViewBag.suppliersid = suppliersid.Select(c => new SelectListItem { Value = c.Name, Text = c.Id.ToString() });
            var productsid = _repository.GetAllProducts();
            ViewBag.productsid = productsid.Select(c => new SelectListItem { Value = c.Name, Text = c.Id.ToString() });
            var sizesid = _repository.GetAllSize();
            ViewBag.sizesid = sizesid.Select(c => new SelectListItem { Value = c.Name, Text = c.Id.ToString() });
            var materialsid = _repository.GetAllMaterial();
            ViewBag.materialsid = materialsid.Select(c => new SelectListItem { Value = c.Name, Text = c.Id.ToString() });
            List<AddInventoryReceiptViewModel> viewModelList = new List<AddInventoryReceiptViewModel>();
          
            return View(viewModelList);
        }

        [HttpPost]
        public async Task<JsonResult> AddInventoryReceipt([FromBody] List<AddInventoryReceiptViewModel> inventoryReceipts)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                foreach (var item in inventoryReceipts)
                {
                    ProductItem productItem;
                    var existingProductItem = _repository.GetProductItemByProductIdSizeIdMaterialId(item.ProductId, item.SizeId, item.MaterialId);

                    if (existingProductItem != null)
                    {
                        // Cập nhật số lượng và giá nhập
                        existingProductItem.Quantity += item.Quantity;
                        if(existingProductItem.PurchasePrice != null)
                        {
                            // Cập nhật giá nhập
                            existingProductItem.PurchasePrice.Price = item.PurchasePrice;
                            existingProductItem.PurchasePrice.EffectiveDate = DateTime.Now;
                        }
                        else
                        {
                            // Tạo mới PurchasePrice
                            existingProductItem.PurchasePrice = new PurchasePrice { Price = item.PurchasePrice, EffectiveDate = DateTime.Now };
                        }
                        // Kiểm tra xem SalesPrice có phải là null không
                        if (existingProductItem.SalesPrice != null)
                        {
                            // Cập nhật giá bán
                            existingProductItem.SalesPrice.Price = item.PurchasePrice; // Giá SalesPrice ban đầu giống như giá PurchasePrice
                            existingProductItem.SalesPrice.EffectiveDate = DateTime.Now;
                        }
                        {
                            // Tạo mới SalesPrice
                            existingProductItem.SalesPrice = new SalesPrice { Price = item.PurchasePrice, EffectiveDate = DateTime.Now }; // Giá SalesPrice ban đầu giống như giá PurchasePrice
                        }

                        productItem = existingProductItem;
                    }
                    else
                    {
                        // Tạo mới PurchasePrice và SalesPrice
                        var purchasePrice = new PurchasePrice { Price = item.PurchasePrice, EffectiveDate = DateTime.Now };
                        var salesPrice = new SalesPrice { Price = item.PurchasePrice, EffectiveDate = DateTime.Now }; // Giá SalesPrice ban đầu giống như giá PurchasePrice
                        var isProduct = _repository.GetProductById(item.ProductId);
                        var isSize = _repository.GetSizeById(item.SizeId);
                        var isMaterial = _repository.GetMaterialById(item.MaterialId);
                        // Tạo mới ProductItem
                        productItem = new ProductItem
                        {
                            Product = isProduct,
                            Sizes = isSize,
                            Materials = isMaterial,
                            PurchasePrice = purchasePrice,
                            SalesPrice = salesPrice,
                            Quantity = item.Quantity
                        };

                        _repository.AddEntity(productItem);
                    }

                    var isSupplier = _repository.GetSupplierById(item.SupplierId);

                    var inventoryReceipt = new InventoryReceipt
                    {
                        User = user,
                        Supplier = isSupplier,
                        CreationDate = DateTime.Now,
                        Confirmation = false,
                        Note = ""
                    };
                    // Tạo mới InventoryReceiptDetails
                    var inventoryReceiptDetails = new InventoryReceiptDetails
                    {
                        ProductItem = productItem,
                        InventoryReceipt = inventoryReceipt,
                        Quantity = item.Quantity
                    };
                    _repository.AddEntity(inventoryReceipt);
                    _repository.AddEntity(inventoryReceiptDetails);
                }
                await _repository.SaveChangesAsync();
            }

            return Json(inventoryReceipts.Count + " record(s) inserted.");
        }

        public IActionResult SaveInventoryReceipt(int inventoryId,string note)
        {
            var inventory = _repository.GetInventoryReceiptDetailsById(inventoryId);
            if (inventoryId != null)
            {
                inventory.InventoryReceipt.Note = note;
                if (_repository.SaveAll())
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }

    }


}
