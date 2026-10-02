using Microsoft.AspNetCore.Mvc;

namespace ProjKlitov.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductRepository _repository;

        public ProductsController(ProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetProductsAsync();
            return View(products);
        }
    }

}
