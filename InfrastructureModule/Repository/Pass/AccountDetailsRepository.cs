using DomainModule.BaseRepo;
using DomainModule.Dto.Pass;
using DomainModule.Entity.Pass;
using DomainModule.RepositoryInterface.Pass;
using InfrastructureModule.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModule.Repository.Pass
{
    public class AccountDetailsRepository : BaseRepository<AccountDetails>, IAccountDetailsRepository
    {
        public AccountDetailsRepository(AppDbContext context) : base(context)
        {
        }

        public AccountDetailModel GetAccountDetailsModel(string userId)
        {
            var model = new AccountDetailModel();
            model.Details =  GetQueryable().Where(a => a.UserId == userId).Select(x => new AccountDetailsDto()
            {
                Account = x.Account,
                Name = x.Name,
                Id = x.Id,
                Pass = AccountDetails.DefaultPasswordString,
                UserId = userId

            }).ToList();
            return model;
        }
    }
}
