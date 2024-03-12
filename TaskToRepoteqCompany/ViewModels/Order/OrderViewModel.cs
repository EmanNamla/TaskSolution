using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using TaskToRepoteqCompany.DAL.Models;

namespace TaskToRepoteqCompany.PL.ViewModels.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string? CustomerName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal Total { get; set; }

        public List<int>? ProductIds { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; } = new HashSet<OrderProduct>();


    }
}
