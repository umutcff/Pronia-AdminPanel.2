using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaUmut.Contexts;
using ProniaUmut.Helpers;
using ProniaUmut.Models;
using ProniaUmut.ViewModels.ProductViewModel;

namespace ProniaUmut.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]

    public class ProductController(AppDBContext _context, IWebHostEnvironment _envoriment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(x => x.Category).ToListAsync();

            List<ProductGetVM> result = new List<ProductGetVM>();
            foreach (var product in products)
            {
                ProductGetVM vm = new ProductGetVM()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = product.Category.Name,
                    MainImagePath = product.MainImagePath,
                    HoverImagePath = product.HoverImagePath,
                };
                result.Add(vm);
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await SendItems();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            await SendItems();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            foreach (var tagId in vm.TagIds)
            {
                var isExistTag = await _context.Tags.AnyAsync(x => x.Id == tagId);
                if (!isExistTag)
                {
                    ModelState.AddModelError("TagIds", "Bele bir Tag tapilmadi!");
                    return View(vm);
                }
            }



            if (!vm.MainImage.CheckType())
            {
                ModelState.AddModelError("MainImage", "Yalniz sekil daxil ede bilersen!");
                return View(vm);
            }

            if (!vm.MainImage.CheckSize(2))
            {
                ModelState.AddModelError("MainImage", "Sekil 3-Mb-dan artiq ola bilmez!");
                return View(vm);
            }

            if (!vm.HoverImage.CheckType())
            {
                ModelState.AddModelError("HoverImage", "Yalniz sekil daxil ede bilersen!");
                return View(vm);
            }

            if (!vm.HoverImage.CheckSize(2))
            {
                ModelState.AddModelError("HoverImage", "Sekil 3-Mb-dan artiq ola bilmez!");
                return View(vm);
            }

            foreach (var addImages in vm.AdditionalImages)
            {
                if (!addImages.CheckType())
                {
                    ModelState.AddModelError("AdditionalImages", "Yalniz sekil daxil ede bilersen!");
                    return View(vm);
                }

                if (!addImages.CheckSize(2))
                {
                    ModelState.AddModelError("AdditionalImages", "Sekil 3-Mb-dan artiq ola bilmez!");
                    return View(vm);
                }

            }

            string folderPath = Path.Combine(_envoriment.WebRootPath, "assets", "images", "website-images");

            /* string uniqueMainPath = Guid.NewGuid().ToString() + vm.MainImage.FileName;
             string mainPath = $@"{_envoriment.WebRootPath}/assets/images/website-images/{uniqueMainPath}";
             FileStream mainStream = new FileStream(mainPath, FileMode.Create);
             await vm.MainImage.CopyToAsync(mainStream);

             string uniqueHoverPath = Guid.NewGuid().ToString() + vm.HoverImage.FileName;
             string hoverPath = $@"{_envoriment.WebRootPath}/assets/images/website-images/{uniqueHoverPath}";
             FileStream hoverStream = new FileStream(hoverPath, FileMode.Create);
             await vm.HoverImage.CopyToAsync(hoverStream);*/

            string uniqueMainPath = await vm.MainImage.SaveFileAsync(folderPath);
            string uniqueHoverPath = await vm.HoverImage.SaveFileAsync(folderPath);



            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                CategoryId = vm.CategoryId,
                Price = vm.Price,
                MainImagePath = uniqueMainPath,
                HoverImagePath = uniqueHoverPath,
                ProductTags = [],
                ProductImages = []
            };


            foreach (var img in vm.AdditionalImages)
            {
                string uniqueImagePath = await img.SaveFileAsync(folderPath);

                ProductImage productImage = new()
                {
                    ImagePath = uniqueImagePath,
                    Product = product
                };

                product.ProductImages.Add(productImage);

            }

            foreach (var tagId in vm.TagIds)
            {
                ProductTag productTag = new ProductTag()
                {
                    TagId = tagId,
                    Product = product
                };
                product.ProductTags.Add(productTag);
            }

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedProduct = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

            if (deletedProduct == null)
            {
                return NotFound();
            }

            _context.Products.Remove(deletedProduct);

            string folderPath = Path.Combine(_envoriment.WebRootPath, "assets", "images", "web-images");
            string deleteMainPath = Path.Combine(folderPath, deletedProduct.MainImagePath);
            string deleteHoverPath = Path.Combine(folderPath, deletedProduct.HoverImagePath);


            ExtensionMethods.DeleteFile(deleteMainPath);
            ExtensionMethods.DeleteFile(deleteHoverPath);

            foreach (var prImage in deletedProduct.ProductImages)
            {
                string imagePath = Path.Combine(folderPath, prImage.ImagePath);

                ExtensionMethods.DeleteFile(imagePath);

            }



            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            await SendItems();


            var updatedProduct = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);


            if (updatedProduct == null)
            {
                return NotFound();
            }


            ProductUpdateVM vm = new ProductUpdateVM()
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                CategoryId = updatedProduct.CategoryId,
                MainImagePath = updatedProduct.MainImagePath,
                HoverImagePath = updatedProduct.HoverImagePath,
                AdditionalImagePaths = updatedProduct.ProductImages.Select(x => x.ImagePath).ToList(),
                AdditionalImageIds = updatedProduct.ProductImages.Select(x => x.Id).ToList()
            };
            return View(vm);

        }


        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVM vm)
        {
            await SendItems();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            if (!vm.MainImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("MainImage", "Yalniz sekil daxil ede bilersen!");
                return View(vm);
            }

            if (!vm.MainImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("MainImage", "Sekil 3-Mb-dan artiq ola bilmez!");
                return View(vm);
            }

            if (!vm.HoverImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("HoverImage", "Yalniz sekil daxil ede bilersen!");
                return View(vm);
            }

            if (!vm.HoverImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("HoverImage", "Sekil 3-Mb-dan artiq ola bilmez!");
                return View(vm);
            }

            foreach (var addImages in vm.Images ?? [])
            {
                if (!addImages.CheckType())
                {
                    ModelState.AddModelError("AdditionalImages", "Yalniz sekil daxil ede bilersen!");
                    return View(vm);
                }

                if (!addImages.CheckSize(2))
                {
                    ModelState.AddModelError("AdditionalImages", "Sekil 3-Mb-dan artiq ola bilmez!");
                    return View(vm);
                }

            }

            var existProduct = await _context.Products.Include(x => x.ProductTags).Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (existProduct == null)
            {
                return BadRequest();
            }

            existProduct.Name = vm.Name;
            existProduct.Description = vm.Description;
            existProduct.Price = vm.Price;
            existProduct.CategoryId = vm.CategoryId;
            existProduct.ProductTags = [];
            /*existProduct.ProductImages = [];*/

            foreach (var tagId in vm.TagIds)
            {
                ProductTag productTag = new ProductTag()
                {
                    TagId = tagId,
                    ProductId = existProduct.Id,
                };

                existProduct.ProductTags.Add(productTag);
            }




            string folderPath = Path.Combine(_envoriment.WebRootPath, "assets", "images", "website-images");

            if (vm.MainImage != null)
            {
                string newMainImagePath = await vm.MainImage.SaveFileAsync(folderPath);

                string existMainPath = Path.Combine(folderPath, existProduct.MainImagePath);
                ExtensionMethods.DeleteFile(existMainPath);

                existProduct.MainImagePath = newMainImagePath;
            }

            if (vm.HoverImage != null)
            {
                string newHoverImagePath = await vm.HoverImage.SaveFileAsync(folderPath);

                string existHoverPath = Path.Combine(folderPath, existProduct.HoverImagePath);
                ExtensionMethods.DeleteFile(existHoverPath);

                existProduct.HoverImagePath = newHoverImagePath;
            }

            var existImages=existProduct.ProductImages.ToList();

            foreach (var image in existImages)
            {
                var isExistImageId = vm.AdditionalImageIds?.Any(x => x == image.Id) ?? false;

                if (!isExistImageId)
                {
                    string deletedImagePath = Path.Combine(folderPath, image.ImagePath);
                    ExtensionMethods.DeleteFile(deletedImagePath);
                    existProduct.ProductImages.Remove(image);
                  
                }
            }

            foreach (var img in vm.Images ?? [])
            {
                string uniqueImagePath = await img.SaveFileAsync(folderPath);

                ProductImage productImage = new()
                {
                    ImagePath = uniqueImagePath,
                    ProductId = existProduct.Id
                };

                existProduct.ProductImages.Add(productImage);

            }

            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.Select(product => new ProductGetVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name,
                MainImagePath = product.MainImagePath,
                HoverImagePath = product.HoverImagePath,
                TagNames = product.ProductTags.Select(x => x.Tag.Name).ToList(),
                AdditionalImagePaths = product.ProductImages.Select(x => x.ImagePath).ToList()
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        private async Task SendItems()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            var tags = await _context.Tags.ToListAsync();
            ViewBag.Tags = tags;

        }
    }
}
