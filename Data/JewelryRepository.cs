using Jewelry.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Jewelry.Data
{
    public class JewelryRepository : IJewelryRepository
    {
        private readonly JewelryContext _context;
        private readonly ILogger <JewelryRepository> _logger;
        private readonly UserManager<StoreUser> _userManager;

        public JewelryRepository(JewelryContext context, ILogger<JewelryRepository> logger, UserManager<StoreUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        //public IEnumerable<Order> GetAllOrders()
        //{
        //    return _context.Orders
        //        .Include(o=>o.Items)
        //        .ThenInclude(i=>i.Product)
        //        .ToList(); 
        //}

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _context.Orders
                  .Include(o => o.Items)
                  .ThenInclude(i => i.Product)
                  .ToList();
            }
            else
            {
                return _context.Orders
                  .ToList();
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return _context.Orders
                  .Where(o => o.User.UserName == username)
                  .Include(o => o.Items)
                  .ThenInclude(i => i.Product)
                  .ToList();
            }
            else
            {
                return _context.Orders
                  .ToList();
            }
        }

        //public Order GetOrderById(string username, int id)
        //{
        //    return _context.Orders
        //        .Include(o => o.Items)
        //        .ThenInclude(i => i.Product)
        //        .Where(p => p.Id == id && p.User.UserName == username) 
        //        .FirstOrDefault();
        //}

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts was called");
                return _context.Products
                    .Include(i => i.Category)
                    .Include(i => i.Img)
                    .Include(i => i.Item)
                    .OrderBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        public IEnumerable<ProductItem> GetAllProductItems()
        {
            try
            {
                _logger.LogInformation("GetAllProductItems was called");
                return _context.ProductItems
                    .Include(p=>p.Product).ThenInclude(i=>i.Img)
                    .Include(p => p.Product).ThenInclude(i => i.Category)
                    .Include(p=>p.Materials)
                    .Include(p => p.Sizes)
                    .Include(p=>p.PurchasePrice)
                    .Include(p=>p.SalesPrice)
                    .OrderBy(p => p.Product)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all product item: {ex}");
                return null;
            }
        }

        public IEnumerable<ProductItem> GetConfirmedProductItems()
        {
            try
            {
                _logger.LogInformation("GetConfirmedProductItems was called");

                var productItemIds = _context.InventoryReceiptDetails
                    .Where(ird => ird.InventoryReceipt.Confirmation == true)
                    .Select(ird => ird.ProductItem.Id)
                    .ToList();

                var productItems = _context.ProductItems
                    .Where(p => productItemIds.Contains(p.Id))
                    .Include(p => p.Product).ThenInclude(i => i.Img)
                    .Include(p => p.Product).ThenInclude(i => i.Category)
                    .Include(p => p.Materials)
                    .Include(p => p.Sizes)
                    .Include(p => p.PurchasePrice)
                    .Include(p => p.SalesPrice)
                    .Include (p => p.Purity)
                    .OrderBy(p => p.Product)
                    .ToList();

                return productItems;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get confirmed product items: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsWithAllItemsZeroQuantity()
        {
            try
            {
                _logger.LogInformation("GetProductsWithAllItemsZeroQuantity was called");

                var products = _context.Products
                    .Where(p => p.Item.All(i => i.Quantity == 0))
                    .OrderBy(p => p.Name)
                    .ToList();

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products with all items zero quantity: {ex}");
                return null;
            }
        }

        public ProductItem GetProductItemById(int productId)
        {
            try
            {
                _logger.LogInformation("GetProductItemById was called");
                return _context.ProductItems
                    .Include(i => i.Materials)
                    .Include(i => i.SalesPrice)
                    .Include(i => i.PurchasePrice)
                    .Include(i => i.Product)
                    .ThenInclude(p => p.Img)
                    .FirstOrDefault(s => s.Id == productId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get productItems by ID: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            try
            {
                _logger.LogInformation("GetProductsByCategory was call");
                return _context.Products
                    .Include(c=>c.Category)
                    .Include(i => i.Img)
                    .Include(i => i.Item)
                    .Where(p => p.Category.Id == categoryId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        public Product GetProductById(int productId)
        {
            try
            {
                _logger.LogInformation("GetProductById was called");
                return _context.Products
                    .Include(i=>i.Img)
                    .Include(i=>i.Category)
                    .Include(i=>i.Item)
                    .ThenInclude(p=>p.Sizes)
                    .Include(i => i.Item)
                    .ThenInclude(p => p.SalesPrice)
                    .Include(i => i.Item)
                    .ThenInclude(p => p.Materials)
                    .FirstOrDefault(s => s.Id == productId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get product by ID: {ex}");
                return null;
            }
        }
        public IEnumerable<Product> GetProductsByName(string name)
        {
            return _context.Products
                .Include(c => c.Category)
                .Include(i => i.Img)
                .Include(i => i.Item)
                .Where(p => p.Name.Contains(name)).ToList();
        }
        public IEnumerable<Product> GetProductsByCategoryAndName(int categoryId, string name)
        {
            return _context.Products
                .Include(c => c.Category)
                .Include(i => i.Img)
                .Include(i => i.Item)
                .Where(p => p.Category.Id == categoryId && p.Name.Contains(name)).ToList();
        }
        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }
        public IEnumerable<Product> SearchProduct(string searchText)
        {
            try
            {
                _logger.LogInformation($"Searching for Supplier with term: {searchText}");

                // Thực hiện tìm kiếm người dùng trong cơ sở dữ liệu dựa trên searchText
                var results = _context.Products
                    .Where(u => u.Category.Name.Contains(searchText)
                                || u.Name.Contains(searchText)
                                || u.WarrantyInformation.Contains(searchText))
                    .OrderBy(u => u.Name)
                    .ToList();
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to search products: {ex}");
                return null;
            }
        }
        public bool IsProductExists(string product)
        {
            return _context.Products.Any(s => s.Name == product);
        }

        public IEnumerable<StoreUser> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("GetAllUsers was called");
                return _context.Users
                   .OrderBy(u => u.UserName)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all users: {ex}");
                return null;
            }
        }
        public StoreUser GetUserById(string userId)
        {
            try
            {
                _logger.LogInformation("GetUserById was called");
                return _context.Users.FirstOrDefault(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user by ID: {ex}");
                return null;
            }
        }


        public void DeleteUser(StoreUser user)
        {
            _context.Users.Remove(user);
        }

        public IEnumerable<StoreUser> SearchUsers(string searchText)
        {
            try
            {
                _logger.LogInformation($"Searching for users with term: {searchText}");

                // Thực hiện tìm kiếm người dùng trong cơ sở dữ liệu dựa trên searchText
                var results = _context.Users
                    .Where(u => u.Email.Contains(searchText)
                                || u.FirstName.Contains(searchText)
                                || u.LastName.Contains(searchText)
                                || u.PhoneNumber.Contains(searchText)
                                || u.Address.Contains(searchText))
                    .OrderBy(u => u.UserName)
                    .ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to search users: {ex}");
                return null;
            }
        }

        public IEnumerable<Supplier> GetAllSupplier()
        {
            try
            {
                _logger.LogInformation("GetAllSupplier was called");
                return _context.Supplier
                   .OrderBy(p => p.Name)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Supplier: {ex}");
                return null;
            }
        }
        public bool IsEmailExists(string email)
        {
            return _context.Supplier.Any(s => s.Email == email);
        }

        public Supplier GetSupplierById(int supplierId)
        {
            try
            {
                _logger.LogInformation("GetSupplierById was called");
                return _context.Supplier.FirstOrDefault(s => s.Id == supplierId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get supplier by ID: {ex}");
                return null;
            }
        }
        public Supplier GetSupplierByEmail(string email)
        {
            try
            {
                _logger.LogInformation($"GetSupplierByEmail was called with email: {email}");

                // Truy vấn cơ sở dữ liệu để lấy nhà cung cấp có email tương ứng
                return _context.Supplier.FirstOrDefault(s => s.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get supplier by email: {ex}");
                return null;
            }
        }

        public void DeleteSupplier(Supplier supplier)
        {
            _context.Supplier.Remove(supplier);
        }

        public IEnumerable<Supplier> SearchSupplier(string searchText)
        {
            try
            {
                _logger.LogInformation($"Searching for Supplier with term: {searchText}");

                // Thực hiện tìm kiếm người dùng trong cơ sở dữ liệu dựa trên searchText
                var results = _context.Supplier
                    .Where(u => u.Email.Contains(searchText)
                                || u.Name.Contains(searchText)
                                || u.PhoneNumber.Contains(searchText)
                                || u.Address.Contains(searchText))
                    .OrderBy(u => u.Name)
                    .ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to search users: {ex}");
                return null;
            }
        }

        public IEnumerable<ProductCategory> GetCategory()
        {
            return _context.ProductCategory.ToList();
        }

        public IEnumerable<ProductCategory> GetAllCategory()
        {
            try
            {
                _logger.LogInformation("GetAllCategory was called");
                return _context.ProductCategory
                   .OrderBy(p => p.Name)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Category: {ex}");
                return null;
            }
        }
        public ProductCategory GetCategoryById(int categoryId)
        {
            try
            {
                _logger.LogInformation("GetCategoryById was called");
                return _context.ProductCategory.FirstOrDefault(s => s.Id == categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get category by ID: {ex}");
                return null;
            }
        }
        public void DeleteCategory(ProductCategory category)
        {
            _context.ProductCategory.Remove(category);
        }
        public bool IsCategoryExists(string category)
        {
            return _context.ProductCategory.Any(s => s.Name == category);
        }
        public bool IsMaterialExists(string material)
        {
            return _context.Material.Any(s => s.Name == material);
        }
        public bool IsSizeExists(string size)
        {
            return _context.Size.Any(s => s.Name == size);
        }
        public bool IsPurityExists(string purity)
        {
            return _context.Purity.Any(s => s.Name == purity);
        }
        public IEnumerable<ProductCategory>SearchCategory(string searchText)
        {
            try
            {
                _logger.LogInformation($"Searching for Supplier with term: {searchText}");

                // Thực hiện tìm kiếm người dùng trong cơ sở dữ liệu dựa trên searchText
                var results = _context.ProductCategory
                    .Where(u => u.Name.Contains(searchText))
                    .OrderBy(u => u.Name)
                    .ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to search users: {ex}");
                return null;
            }
        }

        public IEnumerable<Size> GetAllSize()
        {
            try
            {
                _logger.LogInformation("GetAllSize was called");
                return _context.Size
                   .OrderBy(p => p.Name)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Size: {ex}");
                return null;
            }
        }
        public Size GetSizeById(int sizeId)
        {
            try
            {
                _logger.LogInformation("GetSizeById was called");
                return _context.Size.FirstOrDefault(s => s.Id == sizeId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get size by ID: {ex}");
                return null;
            }
        }

        public IEnumerable<Material> GetAllMaterial()
        {
            try
            {
                _logger.LogInformation("GetAllMaterial was called");
                return _context.Material
                   .OrderBy(p => p.Name)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Size: {ex}");
                return null;
            }
        }
        public Material GetMaterialById(int materialId)
        {
            try
            {
                _logger.LogInformation("GetMaterialById was called");
                return _context.Material.FirstOrDefault(s => s.Id == materialId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Material by ID: {ex}");
                return null;
            }
        }

        public InventoryReceipt GetInventoryReceiptById(int inventoryId)
        {
            try
            {
                _logger.LogInformation("GetInventoryReceiptById was called");
                return _context.InventoryReceipt.FirstOrDefault(s => s.Id == inventoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Inventory by ID: {ex}");
                return null;
            }
        }

        public IEnumerable<InventoryReceipt> GetAllInventoryReceipt()
        {
            try
            {
                _logger.LogInformation("GetAllInventoryReceipt was called");
                return _context.InventoryReceipt
                    .Include(i => i.User)
                    .Include(i => i.Supplier)
                   .OrderByDescending(p => p.CreationDate)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all InventoryReceipt: {ex}");
                return null;
            }
        }

        public IEnumerable<InventoryReceiptDetails>GetDetailsByIvnetoryReceipt(int inventoryReceiptId)
        {
            try
            {
                _logger.LogInformation("GetDetailsByIvnetoryReceipt was called");
                return _context.InventoryReceiptDetails
                    .Where(d => d.InventoryReceipt.Id == inventoryReceiptId)
                    .Include(i => i.ProductItem)
                    .ThenInclude(p => p.PurchasePrice)
                    .Include(i => i.ProductItem)
                    .ThenInclude(p => p.Product)
                    .Include(i => i.ProductItem)
                    .ThenInclude(s => s.Sizes)
                    .Include(i => i.ProductItem)
                    .ThenInclude(m => m.Materials)
                    .Include(i => i.ProductItem)
                    .ThenInclude(m => m.Purity)
                    .Include(i => i.InventoryReceipt)
                    .OrderBy(p => p.Id)
                    .ToList();
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Inventory by ID: {ex}");
                return null;
            }
        }

        public IEnumerable<InventoryReceiptDetails> GetAllInventoryReceiptDetails()
        {
            try
            {
                _logger.LogInformation("GetAllInventoryReceiptDetails was called");
                return _context.InventoryReceiptDetails
                   .Include(i => i.ProductItem)
                   .ThenInclude(p => p.Product)
                   .Include(i => i.ProductItem)
                   .ThenInclude(s => s.Sizes)
                   .Include(i => i.ProductItem)
                   .ThenInclude(m => m.Materials)
                   .Include(i => i.InventoryReceipt)
                   .ThenInclude(s => s.Supplier)
                   .Include(i => i.ProductItem)
                   .ThenInclude(s => s.PurchasePrice)
                   .OrderBy(p => p.Id)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all InventoryReceiptDetails: {ex}");
                return null;
            }
        }

        public InventoryReceiptDetails GetInventoryReceiptDetailsById(int inventoryId)
        {
            try
            {
                _logger.LogInformation("GetInventoryReceiptDetailsById was called");
                return _context.InventoryReceiptDetails.FirstOrDefault(s => s.Id == inventoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Inventory Details by ID: {ex}");
                return null;
            }
        }

        public ProductItem GetProductItemByProductIdSizeIdMaterialPurityId(int productId, int sizeId, int materialId, int purityId)
        {
            return _context.ProductItems
                .Include(pi => pi.Product)
                .Include(pi => pi.Sizes)
                .Include(pi => pi.Materials)
                .Include(p => p.PurchasePrice)
                .Include(p => p.SalesPrice)
                .Include(p => p.Purity)
                .FirstOrDefault(pi => pi.Product.Id == productId && pi.Sizes.Id == sizeId && pi.Materials.Id == materialId && pi.Purity.Id == purityId);
        }

		public List<ProductItem> GetProductItemsByProductPuritySize(int productId, int materialId, int purityId)
		{
			return _context.ProductItems
				.Include(pi => pi.Product)
				.Include(pi => pi.Sizes)
				.Include(pi => pi.Materials)
                .Include(pi => pi.Purity)
				.Include(p => p.PurchasePrice)
				.Include(p => p.SalesPrice)
				.Where(pi => pi.Product.Id == productId && pi.Materials.Id == materialId && pi.Purity.Id == purityId)
				.ToList();
        }
        public List<ProductItem> GetProductItemsByProductPurity(int productId, int materialId)
        {
            return _context.ProductItems
                .Include(pi => pi.Product)
                .Include(pi => pi.Sizes)
                .Include(pi => pi.Materials)
                .Include(p => p.PurchasePrice)
                .Include(p => p.SalesPrice)
                .Include(pi => pi.Purity)
                .Where(pi => pi.Product.Id == productId && pi.Materials.Id == materialId)
                .ToList();
        }

        public List<ProductItem> GeMaterialsByProduct(int productId)
        {
            return _context.ProductItems
                .Include(pi => pi.Product)
                .Include(pi => pi.Sizes)
                .Include(pi => pi.Materials)
                .Include(p => p.PurchasePrice)
                .Include(p => p.SalesPrice)
                .Include(pi => pi.Purity)
                .Where(pi => pi.Product.Id == productId)
                .ToList();
        }

        public IEnumerable<Payments> GetAllPayments()
        {
            try
            {
                _logger.LogInformation("GetAllPayments was called");
                return _context.Payments
                   .OrderBy(p => p.Name)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Payment: {ex}");
                return null;
            }
        }

        public Payments GetPaymentById(int paymentId)
        {
            try
            {
                _logger.LogInformation("GetPaymentById was called");
                return _context.Payments.FirstOrDefault(s => s.Id == paymentId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get payment by ID: {ex}");
                return null;
            }
        }
        public StatusCategory GetStatusCategoryById(int stateId)
        {
            try
            {
                _logger.LogInformation("GetStatusCategoryById was called");
                return _context.StatusCategory.FirstOrDefault(s => s.Id == stateId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get StatusCategory by ID: {ex}");
                return null;
            }
        }

        public Order OrderConfirmation(string orderNumber)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
        }
        public void UpdateEntity(object entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public (decimal MinPrice, decimal MaxPrice) GetProductItemPriceRange(int productId)
        {
            var prices = _context.ProductItems
                .Include(pi => pi.SalesPrice)
                .Include(pi => pi.InventoryReceiptDetails)
                .ThenInclude(ird => ird.InventoryReceipt)
                .Where(pi => pi.Product.Id == productId && pi.InventoryReceiptDetails.Any(ird => ird.InventoryReceipt.ConfirmationDate.HasValue))
                .SelectMany(pi => pi.SalesPrice)
                .OrderByDescending(sp => sp.EffectiveDate)
                .Select(sp => sp.Price)
                .ToList();

            return prices.Any() ? (prices.Min(), prices.Max()) : (0, 0);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                _logger.LogInformation("GetAllOrder was called");
                return _context.Orders
                    .Include(i => i.PaymentMethod)
                    .Include(i => i.User)
                    .Include(i => i.Items)
                    .Include(i => i.Status)
                    .ThenInclude(p => p.StatusCategory)
                    .OrderByDescending(p => p.OrderDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Order: {ex}");
                return null;
            }
        }

        public Order GetOrderById(int orderId)
        {
            try
            {
                _logger.LogInformation("GetOrderById was called");
                return _context.Orders
                    .Include(i => i.Items)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(i=>i.Product)
                    .Include(i => i.Items)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(i => i.Purity)
                    .Include(i => i.Items)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(i => i.SalesPrice)
                    .Include(i=>i.User)
                    .Include(i=>i.PaymentMethod)
                    .Include(i => i.Status)
                    .FirstOrDefault(s => s.Id == orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Order by ID: {ex}");
                return null;
            }
        }
        public Order GetOrderByIds(int orderId)
        {
            try
            {
                _logger.LogInformation("GetOrderByIds was called");
                return _context.Orders
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.Product)
                                .ThenInclude(p => p.Category)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.Product)
                                .ThenInclude(p => p.Img)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.Materials)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.Sizes)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.SalesPrice)
                    .Include(o => o.Status)
                    .Include(o => o.Items)
                        .ThenInclude(p => p.Product)
                            .ThenInclude(p => p.Purity)
                    .FirstOrDefault(s => s.Id == orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Order by ID: {ex}");
                return null;
            }
        }


        public Status GetStatusById(int statusId)
        {
            try
            {
                _logger.LogInformation("GetStatusById was called");
                return _context.Status.FirstOrDefault(s => s.Id == statusId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Status by ID: {ex}");
                return null;
            }
        }

        public Status GetStatusByOrder(int orderId)
        {
            try
            {
                _logger.LogInformation("GetStatusByOrder was called");
                return _context.Status
                    .Include(i => i.User)
                    .Include(i => i.StatusCategory)
                    .OrderByDescending(s => s.UpdateDate)
                    .FirstOrDefault(s => s.Order.Id == orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Status by Order: {ex}");
                return null;
            }
        }

        public IEnumerable<Status> GetAllStatusByOrder(int orderId)
        {
            try
            {
                _logger.LogInformation("GetAllStatusByOrder was called");
                return _context.Status
                    .Include(i => i.User)
                    .Include(i => i.Order)
                    .Include(i => i.StatusCategory)
                    .Where(p => p.Order.Id == orderId)
                    .OrderBy(p => p.UpdateDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Status: {ex}");
                return null;
            }
        }

        public Purity GetPurityById(int purityId)
        {
            try
            {
                _logger.LogInformation("GetPurityById was called");
                return _context.Purity.FirstOrDefault(s => s.Id == purityId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Purity by ID: {ex}");
                return null;
            }
        }

        public IEnumerable<Purity> GetAllPurity()
        {
            try
            {
                _logger.LogInformation("GetAllPurity was called");
                return _context.Purity
                   .OrderBy(p => p.Name)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Purity: {ex}");
                return null;
            }
        }
        public IEnumerable<Order> GetOrderByUser(StoreUser user)
        {
            try
            {
                _logger.LogInformation("GetOrderByUser was called");
                return _context.Orders
                    .Include(p => p.User)
                    .Include(p => p.Status)
                    .ThenInclude(p=>p.StatusCategory)
                    .Include(p => p.PaymentMethod)
                    .Include(p => p.Items)
                    .ThenInclude(p => p.Product)
                    .Where(o => o.User == user)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Order: {ex}");
                return null;
            }
        }

        public IEnumerable<OrderItem> GetOrderItemByOrder(int orderId)
        {
            try
            {
                _logger.LogInformation("GetOrderItemByOrder was called");
                return _context.OrderItem
                    .Include(p => p.Order)
                        .ThenInclude(p => p.Status)
                            .ThenInclude(p => p.StatusCategory)
                    .Include(p => p.Product)
                        .ThenInclude(p => p.Product)
                            .ThenInclude(p => p.Img)
                    .Include(p => p.Product)
                        .ThenInclude(p => p.Sizes)
                    .Include(p => p.Product)
                        .ThenInclude (p => p.Materials)
                    .Include(p => p.Product)
                        .ThenInclude(p => p.Purity)
                    .Include(p => p.Product)
                        .ThenInclude(p => p.SalesPrice)
                    .Where(o => o.Order.Id == orderId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Order: {ex}");
                return null;
            }
        }

        public IEnumerable<Warranty> GetAllWarranty()
        {
            try
            {
                _logger.LogInformation("GetAllWarranty was called");
                return _context.Warranties
                    .Include(i => i.User)
                    .Include(i => i.Order)
                    .ThenInclude(i => i.Items)
                    .ThenInclude(i=>i.Product)
                    .ThenInclude(i=>i.Product)
                    .ThenInclude(i=>i.Img)
                   .OrderByDescending(p => p.CreationDate)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all Warranty: {ex}");
                return null;
            }
        }
        public void DeleteWarranty(Warranty warranty)
        {
            _context.Warranties.Remove(warranty);
        }

        public Warranty GetWarrantyById(int warrantyId)
        {
            try
            {
                _logger.LogInformation("GetWarrantyById was called");
                return _context.Warranties
                    .Include(p => p.Order)
                    .ThenInclude(p => p.Items)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(p => p.Product)
                    .FirstOrDefault(s => s.Id == warrantyId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Warranty by ID: {ex}");
                return null;
            }
        }

        public IEnumerable<InventoryReceiptDetails> GetInventoryReport(int productId, int year, int month)
        {
            return _context.InventoryReceiptDetails
                .Include(ird => ird.ProductItem)
                    .ThenInclude(ird => ird.Product)
                .Include(ird => ird.ProductItem)
                    .ThenInclude(ird => ird.Sizes)
                .Include(ird => ird.ProductItem)
                    .ThenInclude(ird => ird.Materials)
                .Include(ird => ird.ProductItem)
                    .ThenInclude(ird => ird.Purity)
                .Include(ird => ird.ProductItem)
                    .ThenInclude(ird => ird.PurchasePrice)
                .Include(ird => ird.InventoryReceipt)
                .Where(ird => ird.ProductItem.Product.Id == productId &&
                  ird.InventoryReceipt.ConfirmationDate != null &&
                  ird.InventoryReceipt.ConfirmationDate.Value.Year == year &&
                  ird.InventoryReceipt.ConfirmationDate.Value.Month == month)
                .ToList();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
