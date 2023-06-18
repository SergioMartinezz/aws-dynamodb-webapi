using DynamoDb.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace DynamoDb.Products.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private IProductsRepository _repository;
        public ProductsController(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(string name = "")
        {
            if(!string.IsNullOrEmpty(name))
            {
                var products = await _repository.Find(new SearchRequest { Name = name });
                return View(new ProductViewModel
                {
                    Products = products,
                    ResultsType = ResultsType.Search
                });
            }
            else
            {
                var products = await _repository.All();
                return View(products);
            }
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Products/CreateOrUpdate.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductInputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("~/Views/Products/CreateOrUpdate.cshtml", model);
                await _repository.Add(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("~/Views/Products/CreateOrUpdate.cshtml", model);
            }
        }

        [HttpGet]
        [Route("Edit/{productId}")]
        public async Task<IActionResult> Edit(Guid productId)
        {
            return await TakeProduct(productId, InputType.Update);

            //var product = await _repository.Single(productId);

            //ViewBag.productId = productId;
            //return View("~/Views/Products/CreateOrUpdate.cshtml", new ProductInputModel
            //{
            //    Name = product.Name,
            //    Price = product.Price,
            //    Stock = product.Stock,
            //    InputType = InputType.Update
            //});
        }

        [HttpPost]
        [Route("Edit/{productId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid productId, ProductInputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("~/Views/Products/CreateOrUpdate.cshtml", model);
                await _repository.Update(productId, model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {

                return View("~/Views/Products/CreateOrUpdate.cshtml", model);
            }
        }

        [HttpGet]
        [Route("Delete/{productId}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            await _repository.Remove(productId);

            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> TakeProduct(Guid productId, InputType inputType)
        {
            var product = await _repository.Single(productId);

            ViewBag.productId = productId;
            ViewData["Providers"] = product.Providers;
            
            return View("~/Views/Products/CreateOrUpdate.cshtml", new ProductInputModel
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                InputType = inputType
            });
        }

        [HttpGet]
        [Route("AddProvider/{productId}")]
        public async Task<IActionResult> AddProvider(Guid productId)
        {
            return await TakeProduct(productId, InputType.AddProvider);
        }

        [HttpPost]
        [Route("AddProvider/{productId}")]
        public async Task<IActionResult> AddProvider(Guid productId, ProductInputModel model)
        {
            try
            {
                model.InputType = InputType.AddProvider;
                await _repository.Update(productId, model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("~/Views/Products/CreateOrUpdate.cshtml");
            }
        }

        [HttpGet]
        [Route("DeleteProvider/{productId}/{provider}")]
        public async Task<IActionResult> DeleteProvider(Guid productId, string provider)
        {
            try
            {
                var product = await _repository.Single(productId);

                var model = new ProductInputModel()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    Providers = new List<string>() { provider },
                    InputType = DynamoDb.Contracts.InputType.DeleteProvider
                };

                await _repository.Update(productId, model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
