using Jewelry.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o=>o.Items)
                .ThenInclude(i=>i.Product)
                .ToList(); 
        }

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

        public Order GetOrderById(string username, int id)
        {
            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(p => p.Id == id && p.User.UserName == username) 
                .FirstOrDefault();
        }

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
                    .FirstOrDefault(s => s.Id == productId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get productItems by ID: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                _logger.LogInformation("GetAllProducts was call");
                return _context.Products
                    .Where(p => p.Category.Name == category)
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
                return _context.Products.FirstOrDefault(s => s.Id == productId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get product by ID: {ex}");
                return null;
            }
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
                   .OrderBy(p => p.Id)
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

        public ProductItem GetProductItemByProductIdSizeIdMaterialId(int productId, int sizeId, int materialId)
        {
            return _context.ProductItems
                .Include(pi => pi.Product)
                .Include(pi => pi.Sizes)
                .Include(pi => pi.Materials)
                .Include(p => p.PurchasePrice)
                .Include(p => p.SalesPrice)
                .FirstOrDefault(pi => pi.Product.Id == productId && pi.Sizes.Id == sizeId && pi.Materials.Id == materialId);
        }

        

        public void UpdateEntity(object entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
