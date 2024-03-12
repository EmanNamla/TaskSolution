namespace TaskToRepoteqCompany.PL.ViewModels.Product
{
    public class ProductIndexViewModel
    {

        public IEnumerable<ProductViewModel> Products { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string ProductName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

    }
}
