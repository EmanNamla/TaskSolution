using TaskToRepoteqCompany.PL.ViewModels.Product;

namespace TaskToRepoteqCompany.PL.ViewModels.Order
{
    public class OrderIndexViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string CustomarName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
