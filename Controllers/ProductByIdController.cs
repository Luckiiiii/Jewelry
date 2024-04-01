using Jewelry.Data.Entities;
using Jewelry.Data;
using Microsoft.AspNetCore.Mvc;

namespace Jewelry.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductByIdController : Controller
    {
        private readonly IJewelryRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductByIdController(IJewelryRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> Get(int id)
        {
            try
            {
                return Ok(_repository.GetProductById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to get products: {ex}");
                return BadRequest("failed to get products");
            }

        }

    }
}
