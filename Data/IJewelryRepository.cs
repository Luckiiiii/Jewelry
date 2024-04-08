using Jewelry.Data.Entities;

namespace Jewelry.Data
{
    public interface IJewelryRepository
    {
       
        IEnumerable<Product> GetAllProducts();
        IEnumerable<ProductItem> GetConfirmedProductItems();
        IEnumerable<Product> GetProductsWithAllItemsZeroQuantity();
        List<ProductItem> GetProductItemsByProductPuritySize(int productId, int materialId, int purityId);
        List<ProductItem> GetProductItemsByProductPurity(int productId, int materialId);
        List<ProductItem> GeMaterialsByProduct(int productId);
        IEnumerable<ProductItem> GetAllProductItems();
        ProductItem GetProductItemById(int productId);
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        IEnumerable<Product> GetProductsByName(string name);
        IEnumerable<Product> GetProductsByCategoryAndName(int categoryId, string name);
        Product GetProductById(int productId);
        public bool IsProductExists(string product);
        IEnumerable<Payments> GetAllPayments();
        Payments GetPaymentById(int paymentId);
        StatusCategory GetStatusCategoryById(int stateId);
        Order OrderConfirmation(string orderNumber);
        bool SaveAll();
        Task<int> SaveChangesAsync();
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        Order GetOrderById(int orderId);
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
        bool IsMaterialExists(string material);
        bool IsSizeExists(string size);
        bool IsPurityExists(string purity);
        IEnumerable<ProductCategory> SearchCategory(string searchText);

        IEnumerable<ProductCategory> GetCategory();

        IEnumerable<Size> GetAllSize();
        Size GetSizeById(int sizeId);
        IEnumerable<Material> GetAllMaterial();
        Material GetMaterialById(int materialId);

        InventoryReceipt GetInventoryReceiptById(int inventoryId);
        IEnumerable<InventoryReceiptDetails> GetAllInventoryReceiptDetails();
        IEnumerable<InventoryReceipt> GetAllInventoryReceipt();
        InventoryReceiptDetails GetInventoryReceiptDetailsById(int inventoryId);
        IEnumerable<InventoryReceiptDetails> GetDetailsByIvnetoryReceipt(int inventoryReceiptId);
        ProductItem GetProductItemByProductIdSizeIdMaterialPurityId(int productId, int sizeId, int materialId, int purityId);
        (decimal MinPrice, decimal MaxPrice) GetProductItemPriceRange(int productId);

        //order
        Status GetStatusByOrder(int orderId);
        Status GetStatusById(int statusId);
        Order GetOrderByIds(int orderId);
        IEnumerable<OrderItem> GetOrderItemByOrder(int orderId);
        public IEnumerable<Order> GetOrderByUser(StoreUser user);
        IEnumerable<Status> GetAllStatusByOrder(int orderId);
        Purity GetPurityById(int purityId);
        IEnumerable<Purity> GetAllPurity();

        //warranty
        IEnumerable<Warranty> GetAllWarranty();
        void DeleteWarranty(Warranty warranty);
        Warranty GetWarrantyById(int warrantyId);

        //Report
        IEnumerable<InventoryReceiptDetails> GetInventoryReport(int productId, int year, int month);

        void UpdateEntity(object entity);


    }
}