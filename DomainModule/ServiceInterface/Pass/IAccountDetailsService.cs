using DomainModule.Dto.Pass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.ServiceInterface.Pass
{
    public interface IAccountDetailsService
    {
        int AddOrUpdate(AccountDetailsDto dto,string userId);
        string DecryptAndShowPassword(int id,string userId);
        void ReEncryptPassword(string userId,string newKey);
        void ReEncryptPasswordWithoutCommit(string userId,string newKey);
        void RemoveAccount(int accountId, string userId);
    }
}
