using Jewelry.Data.Entities;

namespace Jewelry.Data
{
    public interface IJewelryRepository
    {
       
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);

        bool SaveAll();
        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        Order GetOrderById(string username, int id);
        void AddEntity(object model);

        IEnumerable<StoreUser> GetAllUsers();
        StoreUser GetUserById(string userId);
        IEnumerable<StoreUser> SearchUsers(string searchText);
        void DeleteUser(StoreUser user);
    }
}