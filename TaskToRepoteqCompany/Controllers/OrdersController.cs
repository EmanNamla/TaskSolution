using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskToRepoteqCompany.BLL.Repositoies.Abstraction;
using TaskToRepoteqCompany.BLL.UnitOfWork;
using TaskToRepoteqCompany.DAL.Contexts;
using TaskToRepoteqCompany.DAL.Models;
using TaskToRepoteqCompany.PL.ViewModels;
using TaskToRepoteqCompany.PL.ViewModels.Order;
using TaskToRepoteqCompany.PL.ViewModels.Product;

namespace TaskToRepoteqCompany.PL.Controllers
{
    public class OrdersController : Controller
    {

        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly DbContext _dbContext;

        public OrdersController(IUnitofWork unitofWork, IMapper mapper, EcommerceDbContext dbContext)
        {
            this._unitofWork = unitofWork;
            this._mapper = mapper;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        }

        public IActionResult Index(string CustomerName, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 2)
        {
            var query = _unitofWork.Repository<Order>().GetAllAsync().Result;

            //  Search 
            if (!string.IsNullOrEmpty(CustomerName))
            {
                query = query.Where(p => p.CustomerName.ToLower().Contains(CustomerName.ToLower()));
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

            var Orders = query
                .OrderByDescending(p => p.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedOrders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(Orders);

            var viewModel = new OrderIndexViewModel
            {
                Orders = mappedOrders,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = totalPages,
                CustomarName = CustomerName,
                DateFrom = fromDate,
                DateTo = toDate
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _unitofWork.Repository<Product>().GetAllAsync();
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> SaveOrder([FromBody] List<OrderViewModel> orderItems)
        {
            try
            {
                if (!orderItems.Any())
                {
                    return BadRequest("No order items provided."); 
                }

                var customerName = orderItems[0].CustomerName; 

                var order = new Order
                {
                    CustomerName = customerName,
                    Price = orderItems.Sum(item => item.Price * item.Quantity), 
                    Quantity = orderItems.Sum(item => item.Quantity), 
                    Total=orderItems.Sum(item=>item.Total)                                                  // Set other properties as needed
                };

                await _unitofWork.Repository<Order>().AddAsync(order);
                await _unitofWork.CompleteAsync();

                foreach (var orderItem in orderItems)
                {
                    foreach (var productId in orderItem.ProductIds)
                    {
                        var orderProduct = new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = productId
                        };

                        await _unitofWork.Repository<OrderProduct>().AddAsync(orderProduct);
                    }

                    await _unitofWork.CompleteAsync(); 
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving orders: {ex.Message}");
                return View(orderItems); 
            }
        }


    }
}