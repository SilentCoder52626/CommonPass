using DomainModule.Dto.Pass;
using DomainModule.Exceptions;
using DomainModule.RepositoryInterface.Pass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NToastNotify;
using WebApp.Extensions;
using WebApp.Helper;

namespace WebApp.Areas.Pass.Controllers
{
    [Area("Pass")]
    public class AccountsController : Controller
    {
        private readonly IAccountDetailsRepository _accountRepo;
        private readonly IToastNotification _notify;

        public AccountsController(IAccountDetailsRepository accountRepo, IToastNotification notify)
        {
            _accountRepo = accountRepo;
            _notify = notify;
        }

        [Authorize("Accounts-View")]
        public IActionResult Index()
        {
            var userId = GetCurrentUserExtension.GetCurrentUserId(this);
            var model = _accountRepo.GetAccountDetailsModel(userId);
            return View(model);
        }
        [Authorize("Accounts-AddOrUpdate")]
        public IActionResult AddorUpdate(int? id)
        {
            try
            {
                var dto = new AccountDetailsDto();
                if(id.GetValueOrDefault() > 0)
                {
                    var entity = _accountRepo.GetById(id.GetValueOrDefault()) ?? throw new CustomException("Account details not found.");
                    dto.Id = entity.Id;
                    dto.UserId = entity.UserId;
                    dto.Account = entity.Account;
                    dto.Password = "EncrptedPassword";
                }
                return PartialView("Partial/_AddOrUpdate", dto);
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
