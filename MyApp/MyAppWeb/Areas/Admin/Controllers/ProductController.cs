using Microsoft.AspNetCore.Mvc;
using MyApp.Migrations;
using MyApp.Models;
using MyApp.Data;
using MyApp.Infrastructure.IRepository;
using MyApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        // private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostingEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }
        #region APICALL
        public IActionResult AllProducts()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return Json(new { data = products });
        }
        #endregion
        public IActionResult Index()
        {
            //ProductVM ProductVM = new ProductVM();
            //ProductVM.Products = _unitOfWork.Product.GetAll();
            return View();
        }
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Product Product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(Product);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Create Done! ";
        //        return RedirectToAction("Index");
        //    }
        //    return View(Product);
        //}

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitOfWork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Product = _unitOfWork.Product.GetT(x => x.Id == id);
                if (vm.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductVM vm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string fileName = String.Empty;
                if (file != null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);
                    
                    if(vm.Product.ImageUrl!=null)
                    {
                        var oldTmagePath=Path.Combine(_hostingEnvironment.WebRootPath,vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldTmagePath))
                        {
                            System.IO.File.Delete(oldTmagePath);
                        }
                    }
                    
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    vm.Product.ImageUrl = @"\ProductImage\" + fileName;
                }
                if (vm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(vm.Product);
                    TempData["success"] = "Product Created Done!";
                }

                else
                {
                    _unitOfWork.Product.Update(vm.Product);
                    TempData["success"] = "Product Created Done!";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #region DeleteAPICALL
        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var Product = _unitOfWork.Product.GetT(x => x.Id == id);
        //    if (Product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(Product);
        //}
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var Product = _unitOfWork.Product.GetT(x => x.Id == id);
            if (Product == null)
            {
                return Json(new { success = false, Error = "Error" });
            }
            else
            {
                var oldTmagePath = Path.Combine(_hostingEnvironment.WebRootPath, Product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldTmagePath))
                {
                    System.IO.File.Delete(oldTmagePath);
                }
                _unitOfWork.Product.Delete(Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Delete Done! ";
                return Json(new { success = false, message = "Error in Fetching Data" });
            }
        }
        #endregion
    }
}
