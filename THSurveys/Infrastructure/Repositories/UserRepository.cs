using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Model;
using Infrastructure;   

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly THSurveysContext _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("Null UnitOfWork supplied");
                _unitOfWork = unitOfWork as THSurveysContext;
        }

        public UserProfile GetUserByName(string userName)
        {
            var user = (UserProfile)_unitOfWork.UserProfiles.Where(u => u.UserName == userName).FirstOrDefault();
            return user;
        }
    }
}
