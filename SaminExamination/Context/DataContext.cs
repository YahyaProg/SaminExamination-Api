using Microsoft.EntityFrameworkCore;
using SaminExamination.Models;

namespace SaminExamination.Context
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<BuyInvoice> BuyInvoices { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<InvoiceSell> invoiceSells { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
