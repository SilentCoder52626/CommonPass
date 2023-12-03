using DomainModule.BaseRepo;
using DomainModule.Dto.Pass;
using DomainModule.Entity.Pass;
using DomainModule.Enums;
using DomainModule.Exceptions;
using DomainModule.Helper;
using DomainModule.RepositoryInterface;
using DomainModule.RepositoryInterface.Pass;
using InfrastructureModule.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InfrastructureModule.Repository.Pass
{
    public class AccountDetailsRepository : BaseRepository<AccountDetails>, IAccountDetailsRepository
    {
        private readonly AppSettingsRepositoryInterface _appSettingRepo;
        private readonly UserRepositoryInterface _userRepo;
        public AccountDetailsRepository(AppDbContext context, AppSettingsRepositoryInterface appSettingRepo, UserRepositoryInterface userRepo) : base(context)
        {
            _appSettingRepo = appSettingRepo;
            _userRepo = userRepo;
        }

        public AccountDetailModel GetAccountDetailsModel(string userId)
        {
            var model = new AccountDetailModel();
            model.Details = GetQueryable().Where(a => a.UserId == userId).Select(x => new AccountDetailsDto()
            {
                Account = x.Account,
                UserName = x.Name,
                Id = x.Id,
                Pass = AccountDetails.DefaultPasswordString,
                UserId = userId

            }).ToList();
            return model;
        }

        public AccountExportDTO GetAccountDetailsWithDecryptedData(string userId)
        {
            var model = new AccountExportDTO();
            var Key = _appSettingRepo.GetByKey(AppSettingsEnum.EncryptionKey.ToString(), userId)?.Value;
            if (Key == null)
            {
                throw new CustomException("Please set encryption key in appsetting.");
            }
            var Details = GetQueryable().Where(a => a.UserId == userId).Select(x => new AccountDetailsCreateDto()
            {
                Account = x.Account,
                UserName = x.Name,
                Pass = CustomPasswordEncrypter.DecryptString(x.Pass, Key),
                UserId = userId

            });
            var user = _userRepo.GetByIdString(userId) ?? throw new CustomException("User not authenticated.");
            model.DataTable =  DataTableConverter.ConvertToDataTable(Details);
            model.Company = "Common Pass";
            model.Name = user.Name;
            model.Email = user.Email;
            model.Phone = user.PhoneNumber;
            model.InfoText = "All the precious credentials are laid bare like a juicy secret, waiting for you to guard them with the fierceness of a dragon protecting its treasure. So, do us a favor, be the hero of this Excel adventure and set up the fortress of protection. Thank you, Defender of Spreadsheets!";
            return model;
        }

        
    }
}
