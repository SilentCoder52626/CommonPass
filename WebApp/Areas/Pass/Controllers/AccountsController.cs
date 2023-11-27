using DomainModule.RepositoryInterface.Pass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extensions;

namespace WebApp.Areas.Pass.Controllers
{
    [Area("Pass")]
    public class AccountsController : Controller
    {
        private readonly IAccountDetailsRepository _accountRepo;

        public AccountsController(IAccountDetailsRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [Authorize("Accounts-View")]
        public IActionResult Index()
        {
            var userId = GetCurrentUserExtension.GetCurrentUserId(this);
            var model = _accountRepo.GetAccountDetailsModel(userId);
            return View(model);
        }
    }
}
