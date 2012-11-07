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
        private readonly THSurveysContext _unitOfWork;

        public CategoryRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("Null UnitOfWork supplied");
            _unitOfWork = unitOfWork as THSurveysContext;
        }


        public IQueryable<Category> GetAll()
        {
            var categories = (from c in _unitOfWork.Categories
                              select c);
            return categories;
        }

        public Category GetCategory(long Id)
        {
            Category category;

            /*
             * Wrap the Linq inside a new instance of the THSurveysContext
             * so that it is closed once the category has been retrieved
             * 
             * Why?
             * If the context, as injected into the repository, is used to
             * retrieve the Category, if the returned category is being used
             * during the creation of a new Survey, which contains a reference
             * to this category, Entity Framework will throw the following error:
             * 
             * "An Entity object cannot be referenced by multiple instances of 
             * IEntityChangeTracker".  
             * This error is caused by having more than one instance of the 
             * context open when trying to perform an update.
             * 
             * Since this category does not need to be tracked, no updates should 
             * be performed with it, it is safe to close this context, which 
             * resolves the cause of the above error.
             * 
             * THIS WILL CAUSE PROBLEMS WHEN PROVIDING UNIT TESTS!!!!!!!!!!!!!!!!!!!
             * 
             */
//            using (THSurveysContext context = new THSurveysContext())
//            {
                category = (from c in _unitOfWork.Categories
                                where c.CategoryId == Id
                                select c).FirstOrDefault();
//            }

            return category;
        }

        public void Add(Category category)
        {
            _unitOfWork.Categories.Add(category);
            _unitOfWork.SaveChanges();
        }
    }
}
