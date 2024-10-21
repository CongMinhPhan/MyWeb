using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        void Save();
    }
}
