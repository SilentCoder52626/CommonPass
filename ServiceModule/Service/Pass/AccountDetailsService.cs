using DomainModule.Dto.Pass;
using DomainModule.Entity.Pass;
using DomainModule.Enums;
using DomainModule.Exceptions;
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
                        if (dto.Password != AccountDetails.DefaultPasswordString)
                        {
                            AccountDetails.Password = EncryptString(dto.Password, CipherKey);
                        }
                        AccountDetails.UserId = userId;
                        _detailsRepo.Update(AccountDetails);
                    }
                    else
                    {
                        AccountDetails.Account = dto.Account;
                        AccountDetails.Password = EncryptString(dto.Password, CipherKey);
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

                var pass = DecryptString(AccountDetails.Password, CipherKey);

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
                    var currentPass = DecryptString(account.Password, CipherKey);
                    account.Password = EncryptString(currentPass, newKey);
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
                var currentPass = DecryptString(account.Password, CipherKey);
                account.Password = EncryptString(currentPass, newKey);
                _detailsRepo.Update(account);
            }
            _unitOfWork.Complete();

        }

        private static string EncryptString(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
                }
            }
        }

        private static string DecryptString(string cipherText, string key)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = cipherBytes.Take(16).ToArray();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes.Skip(16).ToArray()))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}
