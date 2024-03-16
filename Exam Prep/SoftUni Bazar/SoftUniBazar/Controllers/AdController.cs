using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Data.Models;
using SoftUniBazar.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SoftUniBazar.Controllers
{
    [Authorize]
    public class AdController : Controller
    {
        private readonly BazarDbContext dbContext;

        public AdController(BazarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var all = await dbContext.Ads
                .Select(a => new AllAdsViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    ImageUrl = a.ImageUrl,
                    CreatedOn = a.CreatedOn.ToString(DataConstants.DateTimeFormat),
                    Category = a.Category.Name,
                    Description = a.Description,
                    Price = a.Price,
                    Owner = a.Owner.UserName.ToString(),
                })
                .ToListAsync();

            return View(all);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await GetCategoriesAsync();

            var form = new AddNewAdViewModel()
            {
                Categories = categories
            };

            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddNewAdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync();

                return View(model);
            }

            var ad = new Ad()
            {
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                CreatedOn = DateTime.Now,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                OwnerId = GetUserId()
            };

            await dbContext.Ads.AddAsync(ad);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var adToEdit = await dbContext.Ads
                .FirstOrDefaultAsync(a => a.Id == id);

            if (adToEdit == null)
            {
                return RedirectToAction("All");
            }

            var model = new AddNewAdViewModel()
            {
                Name = adToEdit.Name,
                Description = adToEdit.Description,
                ImageUrl = adToEdit.ImageUrl,
                Price = adToEdit.Price,
                CategoryId = adToEdit.CategoryId,
                Categories = await GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddNewAdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync();
                return View(model);
            }

            var adToEdit = await dbContext.Ads
                .FirstOrDefaultAsync(a => a.Id == id);

            if (adToEdit == null)
            {
                return RedirectToAction("All");
            }

            adToEdit.Name = model.Name;
            adToEdit.Description = model.Description;
            adToEdit.ImageUrl = model.ImageUrl;
            adToEdit.Price = model.Price;
            adToEdit.CategoryId = model.CategoryId;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("All");
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            bool adIsValid =  dbContext.Ads
                .Any(a => a.Id == id);

            bool alreadyAdded = dbContext.AdsBuyers
                .Any(ab => ab.BuyerId == GetUserId() && ab.AdId == id);

            if (!adIsValid || alreadyAdded)
            {
                return RedirectToAction("All");
            }

            var addToCart = new AdBuyer()
            {
                AdId = id,
                BuyerId = GetUserId()
            };

            await dbContext.AdsBuyers.AddAsync(addToCart);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Cart()
        {
            var userId = GetUserId();

            var cart = await dbContext.Ads
                .Include(e => e.Buyers)
                .Where(ab => ab.Buyers.Any(b => b.BuyerId == userId))
                .Where(ab => ab.OwnerId != userId)
                .Select(ab => new CartItemsViewModel()
                {
                    Id = ab.Id,
                    Name = ab.Name,
                    ImageUrl = ab.ImageUrl,
                    CreatedOn = ab.CreatedOn.ToString(DataConstants.DateTimeFormat),
                    Category = ab.Category.Name,
                    Description = ab.Description,
                    Price = ab.Price,
                    Owner = ab.Owner.UserName,
                })
                .ToListAsync();


            return View(cart);
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var adBuyer = await dbContext.AdsBuyers
                .FirstOrDefaultAsync(ab => ab.AdId == id && ab.BuyerId == GetUserId());
            

            if (adBuyer == null)
            {
                return RedirectToAction("Cart");
            }

            dbContext.AdsBuyers.Remove(adBuyer);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Cart");
        }

        public async Task<List<CategoriesViewModel>> GetCategoriesAsync()
        {
            var categories = await dbContext.Categories
                .Select(c => new CategoriesViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();

            return categories;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}
