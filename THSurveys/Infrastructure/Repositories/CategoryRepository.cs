using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Model;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly THSurveysContext _db = new THSurveysContext();

        public IQueryable<Category> GetAll()
        {
            var categories = (from c in _db.Categories
                              select c);
            return categories;
        }

        public Category GetCategory(long Id)
        {
            var category = (from c in _db.Categories
                            where c.CategoryId == Id
                            select c).FirstOrDefault();
            return category;
        }

        public void Add(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }
    }
}
