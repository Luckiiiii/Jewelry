using AutoMapper;
using Jewelry.Data;
using Jewelry.Data.Entities;
using Jewelry.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace Jewelry.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IJewelryRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IJewelryRepository repository, 
            ILogger<OrdersController> logger, 
            IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;
                var result = _repository.GetAllOrdersByUser(username, includeItems);
                return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(result));

            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to get orders: {ex}");
                return BadRequest("failed to get orders");
            }

        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, id);
                if (order != null) return Ok(_mapper.Map<Order, OrderViewModel>(order));
                else return NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to get orders: {ex}");
                return BadRequest("failed to get orders");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);
                    /*var newOrder = new Order()
                    {
                        OrderDate = model.OrdrDate,
                        OrderNumber = model.OrderNumber,
                        Id = model.OrderId
                    };*/
                    

                    if (newOrder.OrderDate == DateTime.MinValue) 
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    newOrder.User = currentUser;

                    _repository.AddEntity(newOrder);
                    if (_repository.SaveAll())
                    {
                        /*var vm = new OrderViewModel()
                        {
                            OrderId = newOrder.Id,
                            OrdrDate = newOrder.OrderDate,
                            OrderNumber = newOrder.OrderNumber,
                        };*/
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order, OrderViewModel>(newOrder));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");

            }
            return BadRequest("Failed to save new order");
        }
    }
}
