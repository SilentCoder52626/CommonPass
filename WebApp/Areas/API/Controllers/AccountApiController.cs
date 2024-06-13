using DomainModule.Dto.User;
using DomainModule.Entity;
using DomainModule.ServiceInterface;
using DomainModule.ServiceInterface.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceModule.Service;
using WebApp.Helper;
using WebApp.Models;
using WebApp.ViewModel;

namespace WebApp.Areas.API.Controllers
{
	[Route("api/account")]
	[ApiController]
	public class AccountApiController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly UserServiceInterface _userService;
		private readonly IJWTTokenGenerator _tokenGenerator;
		private readonly IConfiguration _configuration;
        public AccountApiController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJWTTokenGenerator tokenGenerator,
            IConfiguration configuration,
            UserServiceInterface userService)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
        }

        [AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody]LoginViewModel model)
		{
			try
			{

				var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new Exception("Incorrect Email or Password");

				var isSucceeded = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);
				if (isSucceeded.Succeeded)
				{
					var tokenDto = new JWTTokenDto()
					{
						UserId = user.Id,
						UserName = user.UserName,
						Email = user.Email,
						JwtKey = _configuration["JWT:Secret"]
					};
					var token = _tokenGenerator.GenerateToken(tokenDto);
					return Ok(token);
				}
				return BadRequest("Incorrect user name or password");

			}
			catch (Exception ex)
			{
				CommonLogger.LogError(ex.Message, ex);
				return BadRequest(ex.Message);
			}


		}
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel model)
        {
            try
            {
                var createDto = new UserDto()
                {
                    Name = model.Name,
                    EmailAddress = model.EmailAddress,
                    MobileNumber = model.MobileNumber,
                    Password = model.Password,
                    UserName = model.UserName,
                    Type = DomainModule.Entity.User.TypeGeneral,
                    CurrentSiteDomain = $"{Request.Scheme}://{Request.Host}",
                    Roles = new List<string>() { DomainModule.Entity.User.TypeGeneral.ToString() }

                };
                var userReponse = await _userService.Create(createDto);

                return Ok(new ApiResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User Created succesfully. Please Confirm your account",
                    Data = userReponse

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
