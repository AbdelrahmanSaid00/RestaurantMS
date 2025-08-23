using MexicoRestaurant.Data;
using MexicoRestaurant.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace MexicoRestaurant.Controllers
{
    public class ProductController : Controller
    {
        private IRepository<Product> products;
        private IRepository<Category> categories;
        private IRepository<Ingredient> ingredients;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            products = new Repository<Product>(context);
            categories = new Repository<Category>(context);
            ingredients = new Repository<Ingredient>(context);
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task <IActionResult> Index()
        {
            return View(await products.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit (int id)
        {
            ViewBag.Categories = await categories.GetAllAsync();
            ViewBag.Ingredients = await ingredients.GetAllAsync();
            if (id == 0)
            {
                ViewBag.Operation = "Add";
                return View(new Product());
            }else
            {
                Product product = await products.GetByIdAsync(id, new QueryOptions<Product>
                {
                    Includes = "productIngredients.Ingredient,Category"
                });
                ViewBag.Operation = "Edit";
                return View(product);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit (Product product, int[] ingredientIds , int catId)
        {
            ViewBag.Inredients = await ingredients.GetAllAsync();
            ViewBag.Categories = await categories.GetAllAsync();
             if (ModelState.IsValid)
             {
                 if (product.ImageFile !=null)
                 {
                     string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                     string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                     string filePath = Path.Combine(uploadFolder, uniqueFileName);
                     using (var fileStream = new FileStream(filePath, FileMode.Create))
                     {
                            await product.ImageFile.CopyToAsync(fileStream);
                     }
                    product.ImageUrl = uniqueFileName;
                 }
                 if (product.ProductId == 0)
                 {
                    
                    product.CategoryId = catId;
                    
                   foreach (int id in ingredientIds)
                   {
                       product.productIngredients?.Add(new ProductIngredient { IngredientId = id , ProductId = product.ProductId});
                   }
                    await products.AddAsync(product);
                    return RedirectToAction("Index", "Product");
                 }
                else
                {
                    var existingProduct = await products.GetByIdAsync(product.ProductId, new QueryOptions<Product> { Includes = "productIngredients" });
                    
                    if (existingProduct == null)
                    {
                        ModelState.AddModelError("", "Product not found.");
                        ViewBag.Ingredients = await ingredients.GetAllAsync();
                        ViewBag.Categories = await categories.GetAllAsync();
                        return View (product);
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Stock = product.Stock;
                    existingProduct.CategoryId = catId;

                    // Update image if a new one is uploaded
                    if (product.ImageFile != null)
                    {
                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(fileStream);
                        }
                        existingProduct.ImageUrl = uniqueFileName;
                    }

                    //Update ingredients

                    existingProduct.productIngredients?.Clear();
                    foreach (int id in ingredientIds)
                    {
                        existingProduct.productIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = existingProduct.ProductId });
                    }

                    try
                    {
                        await products.UdateAsunc(existingProduct);
                    }catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Error : { ex.GetBaseException().Message}");
                        ViewBag.Ingredients = await ingredients.GetAllAsync();
                        ViewBag.Categories = await categories.GetAllAsync();
                        return View(product);
                    }
                }
                return RedirectToAction("Index", "Product");
            }
            else
             {
                 ViewBag.Ingredients = await ingredients.GetAllAsync();
                 ViewBag.Categories = await categories.GetAllAsync();
                 return View(product);
             }
        }

        [HttpPost]
        public async Task<IActionResult> Delete (int id)
        {
            try
            {
                await products.DeleteAsync(id);
                return RedirectToAction("Index");
            }catch
            {
                ModelState.AddModelError("", "Product not found");
                return RedirectToAction("Index");
            }
        }
    }
}
