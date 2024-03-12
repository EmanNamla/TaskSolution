using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskToRepoteqCompany.DAL.Models;
using System.Reflection.Emit;

namespace TaskToRepoteqCompany.DAL.Configrations
{
    public class OrderProductConfig : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {

            builder.HasKey(Am => new { Am.OrderId, Am.ProductId });
            builder.HasOne(Am => Am.Product).WithMany(Am => Am.OrderProducts).HasForeignKey(Am => Am.ProductId);
            builder.HasOne(Am => Am.Order).WithMany(Am => Am.OrderProducts).HasForeignKey(Am => Am.OrderId);


        }
    }
    
}
