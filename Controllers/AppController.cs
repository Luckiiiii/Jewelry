using Jewelry.Data;
using Jewelry.Data.Entities;
using Jewelry.Models;
using Jewelry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Jewelry.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IJewelryRepository _repository;
        private readonly UserManager<StoreUser> _userManager;

        public AppController(IMailService mailService, JewelryContext context, IJewelryRepository repository, UserManager<StoreUser> userManager)
        {
            _mailService = mailService;
            _repository = repository;
            _userManager = userManager;
        }
        //[Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            //var results = _context.Products.ToList();
            return View();
        }

        [HttpGet("contact")]
        //[Authorize(Roles = "manager")]
        [Authorize(Roles = "admin")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
          

            return View();

        }

        [HttpPost("contact")]
        
        public IActionResult Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                _mailService.SendMessage("luckyphuocs@gmail.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            return View();
        }
        [Authorize(Roles = "manager")]
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
        //[Authorize(Roles = "admin")]
        public IActionResult Shop() 
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AccountManagement()
        {
            return View();
        }
    }
}