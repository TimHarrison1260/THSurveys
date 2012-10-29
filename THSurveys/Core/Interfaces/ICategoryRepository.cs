using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Model;

namespace Core.Interfaces
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetAll();
        Category GetCategory(long Id);
        void Add(Category category);
    }
}
