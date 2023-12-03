using DomainModule.BaseRepo;
using DomainModule.Dto.Pass;
using DomainModule.Entity.Pass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.RepositoryInterface.Pass
{
    public interface IAccountDetailsRepository : BaseRepositoryInterface<AccountDetails>
    {
        AccountDetailModel GetAccountDetailsModel(string userId);
        AccountExportDTO GetAccountDetailsWithDecryptedData(string userId);

    }
}
