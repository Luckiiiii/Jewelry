using Jewelry.Data.Entities;
using Jewelry.Data;
using Jewelry.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Jewelry.Models;

namespace Jewelry.Controllers
{
    public class AdminPageController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IJewelryRepository _repository;
        private readonly UserManager<StoreUser> _userManager;
        private readonly SignInManager<StoreUser> _signInManger;

        public AdminPageController(IMailService mailService, JewelryContext context, IJewelryRepository repository, UserManager<StoreUser> userManager, SignInManager<StoreUser> signInManager)
        {
            _mailService = mailService;
            _repository = repository;
            _userManager = userManager;
            _signInManger = signInManager;

        }
        public IActionResult AccountManagement(int page = 1)
        {
            int pageSize = 5;
            var users = _repository.GetAllUsers().Skip((page - 1) * pageSize).Take(pageSize);
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
    }
}
