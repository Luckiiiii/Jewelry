using Jewelry.Data.Entities;

namespace Jewelry.Data
{
    public interface IJewelryRepository
    {
       
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        Product GetProductById(int productId);
        public bool IsProductExists(string product);

        bool SaveAll();
        Task<int> SaveChangesAsync();
        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        Order GetOrderById(string username, int id);
        void AddEntity(object model);

        //User
        IEnumerable<StoreUser> GetAllUsers();
        StoreUser GetUserById(string userId);
        void DeleteUser(StoreUser user);
        IEnumerable<StoreUser> SearchUsers(string searchText);

        //Supplier
        IEnumerable<Supplier> GetAllSupplier();
        bool IsEmailExists(string email);
        Supplier GetSupplierById(int supplierId);
        Supplier GetSupplierByEmail(string email);
        void DeleteSupplier(Supplier supplier);
        IEnumerable<Supplier> SearchSupplier(string searchText);

        //Category
        IEnumerable<ProductCategory> GetAllCategory();
        ProductCategory GetCategoryById(int categoryId);
        void DeleteCategory(ProductCategory category);
        bool IsCategoryExists(string category);
        IEnumerable<ProductCategory> SearchCategory(string searchText);

        IEnumerable<ProductCategory> GetCategory();

        IEnumerable<Size> GetAllSize();
        Size GetSizeById(int sizeId);
        IEnumerable<Material> GetAllMaterial();
        Material GetMaterialById(int materialId);

        InventoryReceipt GetInventoryReceiptById(int inventoryId);
        IEnumerable<InventoryReceiptDetails> GetAllInventoryReceiptDetails();
        InventoryReceiptDetails GetInventoryReceiptDetailsById(int inventoryId);
        ProductItem GetProductItemByProductIdSizeIdMaterialId(int productId, int sizeId, int materialId);
        void UpdateEntity(object entity);


    }
}