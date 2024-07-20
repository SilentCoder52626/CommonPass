using DomainModule.Dto.Pass;
using DomainModule.RepositoryInterface.Pass;
using DomainModule.ServiceInterface.Pass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extensions;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Areas.API.Controllers
{
    [Route("api/AccountDetails")]
    [ApiController]
    public class AccountDetailsApiController : Controller
    {
        private readonly IAccountDetailsRepository _accountDetailsRepo;
        private readonly IAccountDetailsService _accountDetailsService;

        public AccountDetailsApiController(IAccountDetailsService accountDetailsService, IAccountDetailsRepository accountDetailsRepo)
        {
            _accountDetailsService = accountDetailsService;
            _accountDetailsRepo = accountDetailsRepo;
        }
        [Authorize("Accounts-View")]
        [HttpGet("")]
        public IActionResult GetAccountDetails()
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                var data = _accountDetailsRepo.GetAccountDetailsModel(userId);

                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Account Details Fetched Successfully",
                    Data = data
                        
                });
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize("Accounts-DecryptPassword")]
        [HttpGet("DecryptPassword")]
        public IActionResult DecryptPassword(int accountId)
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                var Password = _accountDetailsService.DecryptAndShowPassword(accountId, userId);

                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Password Decrypted Successfully",
                    Data = new
                    {
                        Password = Password
                    }
                });
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize("Accounts-DecryptPassword")]
        [HttpGet("DecryptedDetails/{accountId}")]
        public IActionResult DecryptedDetails(int accountId)
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                var Password = _accountDetailsService.DecryptAndShowPassword(accountId, userId);
                var Details = _accountDetailsRepo.GetQueryable().FirstOrDefault(a => a.Id == accountId) ?? throw new Exception("Account details not found.");
                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Password Decrypted Successfully",
                    Data = new AccountDetailsDto
                    {
                        Account = Details.Account,
                        Pass = Password,
                        Id = Details.Id,
                        UserId = Details.UserId,
                        UserName = Details.Name
                    }
                });
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Authorize("Accounts-AddOrUpdate")]
        [HttpDelete("RemoveAccount/{accountId}")]
        public IActionResult RemoveAccount(int accountId)
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                _accountDetailsService.RemoveAccount(accountId, userId);

                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Account Details Deleted Successfully.",
                });
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [Authorize("Accounts-AddOrUpdate")]
        [HttpPost("AddOrUpdate")]
        public IActionResult AddOrUpdate(AccountDetailsDto model)
        {
            try
            {
                var userId = GetCurrentUserExtension.GetCurrentUserId(this);
                var Id = _accountDetailsService.AddOrUpdate(model, userId);

                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Accounts Updated Successfully",
                    Data = new
                    {
                        Id = Id
                    }
                });
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }

        }
        
    }
}
