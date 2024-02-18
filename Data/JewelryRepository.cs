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

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts was called");
                return _context.Products
                   .OrderBy(p => p.Title)
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
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

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                _logger.LogInformation("GetAllProducts was call");
                return _context.Products
                    .Where(p => p.Category == category)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
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


        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
