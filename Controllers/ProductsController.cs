using Jewelry.Data;
using Jewelry.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Jewelry.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        private readonly IJewelryRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IJewelryRepository repository, ILogger<ProductsController> logger) 
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                return Ok(_repository.GetAllProducts());
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to get products: {ex}");
                return BadRequest("failed to get products");
            }
            
        }

    }
}
