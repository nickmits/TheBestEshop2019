using Microsoft.ESportShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.ESportShop.Web.ViewModels;
using Microsoft.ESportShop.ApplicationCore.Entities;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Microsoft.ESportShop.Web.Controllers.Api
{
    public class CatalogController : BaseApiController
    {
        private readonly ICatalogViewModelService _catalogViewModelService;
        private readonly IHostingEnvironment env;

        public CatalogController(ICatalogViewModelService catalogViewModelService, IHostingEnvironment env)
        {
            _catalogViewModelService = catalogViewModelService;
            this.env = env;
        }

        [HttpGet]
        public async Task<IActionResult> List(int? brandFilterApplied, int? typesFilterApplied, int? page)
        {
            var itemsPage = 10;
            return Ok(await _catalogViewModelService.GetCatalogItems(page ?? 0, itemsPage, brandFilterApplied, typesFilterApplied));
        }

        [Route("/UpdateProduct")]
        [HttpPost]
        public async Task<IActionResult> UpdateProduct([FromBody]CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToPage("/Index");
            }
            await _catalogViewModelService.UpdateCatalogItemPrice(productDetails.Id, productDetails.Price);

            return Ok();
        }

        [Route("/DeleteProduct")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct([FromBody]CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToPage("/Index");
            }
            await _catalogViewModelService.DeleteCatalogItem(productDetails.Id);

            return Ok();
        }


        [Route("/CreateProduct")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm]CatalogItemDTO newProduct)
        {
            IFormFile ProductImage = newProduct.Picture;


            var newProd = await _catalogViewModelService.AddCatalogItem(new CatalogItem
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                PictureUri = @"\images\",
                CatalogType = await _catalogViewModelService.AddCatalogType(newProduct.CatalogType),
                CatalogBrand = await _catalogViewModelService.AddCatalogBrand(newProduct.CatalogBrand)
            });

            if (ProductImage != null && ProductImage.Length > 0)
            {
                if (ProductImage.ContentType.Contains("png") || ProductImage.ContentType.Contains("jpg") || ProductImage.ContentType.Contains("jpeg"))
                {
                    var imageLocal = @"\images\products\" + newProd.Id + '.' + ProductImage.ContentType.Split('/')[1];
                    var filePath = env.WebRootPath + imageLocal;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProductImage.CopyToAsync(stream);
                    }

                    newProd.PictureUri = imageLocal;
                    await _catalogViewModelService.UpdateCatalogItem(newProd);
                }
            }
            return Ok();
        }

    }
}
