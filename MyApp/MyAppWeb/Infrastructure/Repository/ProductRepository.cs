using MyApp.Data;
using MyApp.Infrastructure.IRepository;
using MyApp.Models;
using MyAppWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var productDB = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (productDB != null)
            {
                productDB.Name = productDB.Name;
                productDB.Description = productDB.Description;
                productDB.Price = productDB.Price;
                if (productDB.ImageUrl != null)
                {
                    productDB.ImageUrl = productDB.ImageUrl;
                }
            }
        }
    }
}
