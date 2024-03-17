using Jewelry.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Net;
using System.Text.Json;

namespace Jewelry.Data
{
    public class JewelrySeeder
    {
        private readonly JewelryContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<StoreUser> _userManager;
        private readonly SignInManager<StoreUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IServiceProvider _serviceProvider;

        public JewelrySeeder(SignInManager<StoreUser>signInManager, JewelryContext context, IWebHostEnvironment environment, UserManager<StoreUser> userManager, IServiceProvider serviceProvider /*RoleManager<IdentityRole> roleManager*/)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceProvider = serviceProvider;
            //_roleManager = roleManager;

        }

        public async Task SeedAsync()
        {
            // Tạo vai trò "admin" nếu chưa tồn tại

            /*var adminRole = new IdentityRole("admin");
            await _roleManager.CreateAsync(adminRole);*/


            // Tạo người dùng "luckyphuocs@gmail.com" nếu chưa tồn tại
            /*var user = await _userManager.FindByEmailAsync("luckyphuocs@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Lucky",
                    LastName = "Phuoc",
                    Email = "luckyphuocs@gmail.com",
                    UserName = "luckyphuocs@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result.Succeeded)
                {
                    // Gán vai trò "admin" cho người dùng
                    await _userManager.AddToRoleAsync(user, "admin");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create new user in seeder. Errors: {errors}");
                }
            }*/

            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var adminRoleExists = await roleManager.RoleExistsAsync("admin");
            var managerRoleExists = await roleManager.RoleExistsAsync("manager");
            if (!adminRoleExists)
            {
                var adminRole = new IdentityRole("admin");
                await roleManager.CreateAsync(adminRole);
            }
            if (!managerRoleExists)
            {
                var managerRole = new IdentityRole("manager");
                await roleManager.CreateAsync(managerRole);
            }

            _context.Database.EnsureCreated();
            StoreUser user = await _userManager.FindByEmailAsync("luckyphuocs@gmail.com");
            if (user == null)
            {
                user = new StoreUser() { Address = "mot", PhoneNumber = "0123456789", FirstName = "Lucky", LastName = "Phuoc", Email = "luckyphuocs@gmail.com", UserName = "luckyphuocs@gmail.com" };
                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    //throw new InvalidOperationException("Could not create new user in seeder");
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create new user in seeder. Errors: {errors}");
                }
                
            }
            var isInRole = await _userManager.IsInRoleAsync(user, "admin");
            if (!isInRole)
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }


            StoreUser managerUser = await _userManager.FindByEmailAsync("manager@gmail.com");
            if (managerUser == null)
            {
                managerUser = new StoreUser() { Address = "mot", PhoneNumber = "0123456789", FirstName = "Manager", LastName = "User", Email = "manager@gmail.com", UserName = "manager@gmail.com" };
                var result = await _userManager.CreateAsync(managerUser, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Could not create new manager user in seeder. Errors: {errors}");
                }

                // Gán vai trò "manager" cho người dùng
               
            }
            var manager = await _userManager.IsInRoleAsync(managerUser, "manager");
            if (!manager)
            {
                await _userManager.AddToRoleAsync(managerUser, "manager");
            }
            if (!_context.Material.Any())
            {
                _context.Material.AddRange(
                    new Material
                    {
                        Name = "Vàng"
                    },
                    new Material
                    {
                        Name = "Bạc"
                    });
                _context.SaveChanges();
            }
            if (!_context.Size.Any())
            {
                var sizes = new List<Size>();

                for (int i = 6; i <= 20; i++)
                {
                    sizes.Add(new Size {Name = i.ToString() });
                }

                _context.Size.AddRange(sizes);
                _context.SaveChanges();
            }
            if (!_context.Supplier.Any())
            {
                _context.Supplier.AddRange(
                    new Supplier
                    {
                        Name = "Công Ty Cổ Phần Đầu Tư Và Kinh Doanh Vàng Việt Nam",
                        Address = "Phòng Số 1, Tầng 4, Tòa Nhà Đào Duy Anh, Số 9 Đào Duy Anh, Hà Nội",
                        Email = "info@vietnamgoldcorp.com",
                        PhoneNumber = "0335772269",
                    },
                    new Supplier
                    {
                        Name = "Công Ty TNHH Vàng Bạc Đá Quý Huy Thành",
                        Address = "135 Hàng Bạc, Hoàn Kiếm, Hà Nội",
                        Email = "cskh@htj.vn",
                        PhoneNumber = "0339264656",
                    }
                );
            }

            //if (!_context.PurchasePrice.Any())
            //{
            //    _context.PurchasePrice.AddRange(
            //        new PurchasePrice { Price = 100, EffectiveDate = DateTime.UtcNow },
            //        new PurchasePrice { Price = 100, EffectiveDate = DateTime.UtcNow }
            //    );
            //    _context.SaveChanges();
            //}

            //if (!_context.SalesPrice.Any())
            //{
            //    _context.SalesPrice.AddRange(
            //        new SalesPrice { Price = 120, EffectiveDate = DateTime.UtcNow },
            //        new SalesPrice { Price = 120, EffectiveDate = DateTime.UtcNow }
            //    );
            //    _context.SaveChanges();
            //}

            //if (!_context.ProductItems.Any())
            //{
            //    var size = _context.Size.FirstOrDefault(s => s.Id == 1);
            //    var material = _context.Material.FirstOrDefault(s => s.Id == 1);
            //    var product = _context.Products.FirstOrDefault(s => s.Id == 1);

            //    if (size != null && material != null && product != null)
            //    {
            //        var purchasePrice1 = new PurchasePrice
            //        {
            //            Price = 10000000,
            //            EffectiveDate = DateTime.UtcNow
            //        };

            //        var salesPrice1 = new SalesPrice
            //        {
            //            Price = 20000000,
            //            EffectiveDate = DateTime.UtcNow
            //        };

            //        var purchasePrice2 = new PurchasePrice
            //        {
            //            Price = 20000000,
            //            EffectiveDate = DateTime.UtcNow
            //        };

            //        var salesPrice2 = new SalesPrice
            //        {
            //            Price = 40000000,
            //            EffectiveDate = DateTime.UtcNow
            //        };

            //        _context.ProductItems.AddRange(
            //            new ProductItem
            //            {
            //                Sizes = size,
            //                Materials = material,
            //                Product = product,
            //                PurchasePrice = new List<PurchasePrice> { purchasePrice1 },
            //                SalesPrice = new List<SalesPrice> { salesPrice1 },
            //                Quantity = 100
            //            },
            //            new ProductItem
            //            {
            //                Sizes = size,
            //                Materials = material,
            //                Product = product,
            //                PurchasePrice = new List<PurchasePrice> { purchasePrice2 },
            //                SalesPrice = new List<SalesPrice> { salesPrice2 },
            //                Quantity = 150
            //            });

            //        _context.SaveChanges();
            //    }
            //}

            //var size = _context.Size.FirstOrDefault(s => s.Id == 1);
            //var material = _context.Material.FirstOrDefault(s => s.Id == 1);
            //var product = _context.Products.Where(p => p.Id == 9).FirstOrDefault();
            //if (product != null)
            //{
            //    product.Item = new List<ProductItem>()
            //    {
            //        new ProductItem
            //        {
            //            Sizes = size,
            //            Materials = material,
            //            PurchasePrice = null,
            //            SalesPrice = null,
            //            Quantity = 100,

            //        },
            //        new ProductItem
            //        {
            //            Sizes = size,
            //            Materials = material,
            //            PurchasePrice = null,
            //            SalesPrice = null,
            //            Quantity = 150,

            //        }
            //    };
            //}

            //if (!_context.ProductItems.Any())
            //{
            // Need to create the Sample Data
            //var file = Path.Combine(_environment.ContentRootPath, "Data/art.json");
            //var json = File.ReadAllText(file);
            //var products = JsonSerializer.Deserialize<IEnumerable<ProductItem>>(json);
            //_context.ProductItems.AddRange(products);
            /*var order = _context.Orders.Where(o => o.Id == 1).FirstOrDefault();
            if (order != null)
            {
                order.User = user;
                order.Items = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        Product = products.First(),
                        Quantity = 5,
                        UnitPrice = products.First().Price
                    }
                };
            }*/

            /*var order = new Order()
            {

                OrderDate = DateTime.Today,
                OrderNumber = "1000",
                //order.User = user,
                Items = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        Product = products.First(),
                        Quantity = 5,
                        UnitPrice = products.First().Price
                    }
                }
            };
            order.User = user;
            _context.Orders.Add(order);
            _context.SaveChanges();*/
            //      var order = _context.Orders.Where(o => o.Id == 1).FirstOrDefault();
            //      if (order != null)
            //      {
            //          order.User = user;
            //          order.Items = new List<OrderItem>()
            //{
            //  new OrderItem()
            //  {
            //    Product = products.First(),
            //    Quantity = 5,
            //    UnitPrice = products.First().Price
            //  }
            //};
            //      }

            //    _context.SaveChanges();

            //}
        }
    }
}
