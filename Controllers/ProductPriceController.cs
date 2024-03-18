using Jewelry.Data.Entities;
using Jewelry.Data;
using Microsoft.AspNetCore.Mvc;

namespace Jewelry.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductPriceController : Controller
    {
        private readonly IJewelryRepository _repository;
        private readonly ILogger<ProductPriceController> _logger;

        public ProductPriceController(IJewelryRepository repository, ILogger<ProductPriceController> logger)
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
