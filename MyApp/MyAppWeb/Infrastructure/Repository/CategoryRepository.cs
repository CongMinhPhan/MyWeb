﻿using MyApp.Data;
using MyApp.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var categoryDB =_context.Categories.FirstOrDefault(x=>x.Id==category.Id);
            if(categoryDB != null)
            {
                categoryDB.Name=category.Name;
                categoryDB.DisplayOrder=category.DisplayOrder; 
            }
        
        }
    }
}
