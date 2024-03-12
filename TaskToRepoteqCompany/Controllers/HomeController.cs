using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskToRepoteqCompany.BLL.Repositoies.Abstraction;
using TaskToRepoteqCompany.BLL.UnitOfWork;
using TaskToRepoteqCompany.DAL.Models;
using TaskToRepoteqCompany.Models;
using TaskToRepoteqCompany.PL.ViewModels;

namespace TaskToRepoteqCompany.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitofWork unitofWork;

        public HomeController(IUnitofWork unitofWork)
        {
            this.unitofWork = unitofWork;
        }

        public async Task< IActionResult> Index()
        {
            int productCount =await unitofWork.Repository<Product>().GetCountAsync();
            int orderCount =await unitofWork.Repository<Order>().GetCountAsync();

        
            var viewModel = new DashboardViewModel
            {
                ProductCount = productCount,
                OrderCount = orderCount
            };
            return View(viewModel);
        }
        
       
    }
}