using DomainModule.Dto.Pass;
using DomainModule.Entity.Pass;
using DomainModule.Enums;
using DomainModule.Exceptions;
using DomainModule.Helper;
using DomainModule.RepositoryInterface;
using DomainModule.RepositoryInterface.Pass;
using DomainModule.ServiceInterface.Pass;
using InfrastructureModule.Repository;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModule.Service.Pass
{
    public class AccountDetailsService : IAccountDetailsService
    {
        private readonly IAccountDetailsRepository _detailsRepo;
        private readonly AppSettingsRepositoryInterface _settingRepo;
        private readonly IUnitOfWork _unitOfWork;
        public AccountDetailsService(IAccountDetailsRepository detailsRepo, IUnitOfWork unitOfWork, AppSettingsRepositoryInterface settingRepo)
        {
            _detailsRepo = detailsRepo;
            _unitOfWork = unitOfWork;
            _settingRepo = settingRepo;
        }

        public int AddOrUpdate(AccountDetailsDto dto, string userId)
        {
            try
            {
                using (var tx = _unitOfWork.BeginTransaction())
                {

                    var CipherKey = _settingRepo.GetByKey(AppSettingsEnum.EncryptionKey.ToString(), userId)?.Value ?? throw new CustomException("Please set your private encryption key first.");
                    var AccountDetails = new AccountDetails();
                    if (dto.Id > 0)
                    {
                        AccountDetails = _detailsRepo.GetById(dto.Id) ?? throw new CustomException("Account details not found.");
                        AccountDetails.Account = dto.Account;
                        AccountDetails.Name = dto.UserName;
                        if (dto.Pass != AccountDetails.DefaultPasswordString)
                        {
                            AccountDetails.Pass = CustomPasswordEncrypter.EncryptString(dto.Pass, CipherKey);
                        }
                        AccountDetails.UserId = userId;
                        _detailsRepo.Update(AccountDetails);
                    }
                    else
                    {
                        AccountDetails.Account = dto.Account;
                        AccountDetails.Name = dto.UserName;
                        AccountDetails.Pass = CustomPasswordEncrypter.EncryptString(dto.Pass, CipherKey);
                        AccountDetails.UserId = userId;

                        _detailsRepo.Insert(AccountDetails);
                    }
                    _unitOfWork.Complete();
                    tx.Commit();
                    return AccountDetails.Id;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string DecryptAndShowPassword(int id, string userId)
        {
            try
            {
                var CipherKey = _settingRepo.GetByKey(AppSettingsEnum.EncryptionKey.ToString(), userId)?.Value ?? throw new CustomException("Please set your private encryption key first.");
                var AccountDetails = _detailsRepo.GetQueryable().Where(a => a.Id == id && a.UserId == userId).FirstOrDefault() ?? throw new CustomException("Account details not found.");

                var pass = CustomPasswordEncrypter.DecryptString(AccountDetails.Pass, CipherKey);

                return pass;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ReEncryptPassword(string userId, string newKey)
        {
            using (var tx = _unitOfWork.BeginTransaction())
            {
                var CipherKey = _settingRepo.GetByKey(AppSettingsEnum.EncryptionKey.ToString(), userId)?.Value ?? throw new CustomException("Please set your private encryption key first.");

                var AccountDetailsOfUser = _detailsRepo.GetQueryable().Where(a => a.UserId == userId).ToList();
                foreach (var account in AccountDetailsOfUser)
                {
                    var currentPass = CustomPasswordEncrypter.DecryptString(account.Pass, CipherKey);
                    account.Pass = CustomPasswordEncrypter.EncryptString(currentPass, newKey);
                    _detailsRepo.Update(account);
                }
                _unitOfWork.Complete();
                tx.Commit();

            }
        }
        public void ReEncryptPasswordWithoutCommit(string userId, string newKey)
        {

            var CipherKey = _settingRepo.GetByKey(AppSettingsEnum.EncryptionKey.ToString(), userId)?.Value ?? throw new CustomException("Please set your private encryption key first.");

            var AccountDetailsOfUser = _detailsRepo.GetQueryable().Where(a => a.UserId == userId).ToList();
            foreach (var account in AccountDetailsOfUser)
            {
                var currentPass = CustomPasswordEncrypter.DecryptString(account.Pass, CipherKey);
                account.Pass = CustomPasswordEncrypter.EncryptString(currentPass, newKey);
                _detailsRepo.Update(account);
            }
            _unitOfWork.Complete();

        }

       

    }
}
