using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskToRepoteqCompan.PL.Helpers;
using TaskToRepoteqCompany.BLL.Repositoies.Abstraction;
using TaskToRepoteqCompany.BLL.UnitOfWork;
using TaskToRepoteqCompany.DAL.Models;
using TaskToRepoteqCompany.PL.ViewModels.Product;

namespace TaskToRepoteqCompany.PL.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitofWork unitofWork;
        private readonly IMapper mapper;

        public ProductsController(IUnitofWork unitofWork, IMapper mapper)
        {
           
            this.unitofWork = unitofWork;
            this.mapper = mapper;
        }

        #region Index Action
        public IActionResult Index(string productName, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 2)
        {
            var query = unitofWork.Repository<Product>().GetAllAsync().Result;

            //  Search 
            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => p.Name.ToLower().Contains(productName.ToLower()));
            }
            if (fromDate != null)
            {
                query = query.Where(p => p.Date >= fromDate);
            }
            if (toDate != null)
            {
                query = query.Where(p => p.Date <= toDate);
            }

            // Pagination
            int totalCount = query.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var products = query
                .OrderByDescending(p => p.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);

            var viewModel = new ProductIndexViewModel
            {
                Products = mappedProducts,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = totalPages,
                ProductName = productName,
                DateFrom = fromDate,
                DateTo = toDate
            };

            return View(viewModel);
        } 
        #endregion

        #region Create Actions
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {

            if (ModelState.IsValid)
            {
                if (productViewModel.Image != null)
                {
                    productViewModel.ImageName = DocumentSettings.UploadFile(productViewModel.Image, "Images");
                }

                var MappedProduct = mapper.Map<ProductViewModel, Product>(productViewModel);
                await unitofWork.Repository<Product>().AddAsync(MappedProduct);
                await unitofWork.CompleteAsync();
                var Product = mapper.Map<Product, ProductViewModel>(MappedProduct);
                return RedirectToAction("Index", Product);
            }

            return View(productViewModel);
        }
        #endregion

        #region Edit Actions
        public async Task<IActionResult> Edit(int? id, string ViewName = "Edit")
        {
            if (id == null)
            {
                return BadRequest();
            }

            var Product = await unitofWork.Repository<Product>().GetByIdAsync(id.Value);
            var MappedProduct = mapper.Map<Product, ProductViewModel>(Product);
            if (Product == null)
            {
                return NotFound();
            }
            return View(ViewName, MappedProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit([FromRoute] int id, ProductViewModel ProductVM)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await unitofWork.Repository<Product>().GetByIdAsync(id);

                if (existingProduct == null)
                {
                    return NotFound();
                }

                if (ProductVM.Image is not null)
                {
                    // Upload the new image
                    ProductVM.ImageName = DocumentSettings.UploadFile(ProductVM.Image, "Images");

                    // Delete the image 
                    if (!string.IsNullOrEmpty(existingProduct.ImageName))
                    {
                        DocumentSettings.DeleteFile(existingProduct.ImageName, "Images");

                    }
                }
                unitofWork.Repository<Product>().Detach(existingProduct);

                // Update  Product details
                var mappedProduct = mapper.Map<ProductViewModel, Product>(ProductVM);
                 unitofWork.Repository<Product>().Update(mappedProduct);
                 await unitofWork.CompleteAsync();
                var Product = mapper.Map<Product, ProductViewModel>(mappedProduct);

                return RedirectToAction("Index", Product);
            }

            return View(ProductVM);
        }
        #endregion

        #region Delete Actions
        public Task<IActionResult> Delete(int id)
        {
            return Edit(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id, ProductViewModel ProductVM)
        {

            if (id != ProductVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var MappedProduct = mapper.Map<ProductViewModel, Product>(ProductVM);
                 unitofWork.Repository<Product>().Delete(MappedProduct);
                int count = await unitofWork.CompleteAsync();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(ProductVM.ImageName, "Images");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(ProductVM);
            }
        } 
        #endregion
    }
}
