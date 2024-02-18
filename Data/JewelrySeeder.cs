using Jewelry.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
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
                user = new StoreUser() { FirstName = "Lucky", LastName = "Phuoc", Email = "luckyphuocs@gmail.com", UserName = "luckyphuocs@gmail.com" };
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
                managerUser = new StoreUser() { FirstName = "Manager", LastName = "User", Email = "manager@gmail.com", UserName = "manager@gmail.com" };
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

            if (!_context.Products.Any())
            {
                // Need to create the Sample Data
                var file = Path.Combine(_environment.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(file);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
                _context.Products.AddRange(products);
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

                _context.SaveChanges();

            }
        }
    }
}
