using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WhatHaveYouPlayed.Data;
//using WhatHaveYouPlayed.Data.Migrations;
using WhatHaveYouPlayed.Models;
using WhatHaveYouPlayed.Models.Dto;

namespace WhatHaveYouPlayed.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext applicationDbContext, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = applicationDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<AccountDto> CreateAccountModelAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var role = await _userManager.GetRolesAsync(user);
            var stats = new StatsDto()
            {
                Playing = _context.UsersGamesDatas
                 .Where(usr => usr.User.Id == user.Id)
                 .Where(stat => stat.StateId == 2)
                 .Count(),
                Complete = _context.UsersGamesDatas
                 .Where(usr => usr.User.Id == user.Id)
                 .Where(stat => stat.StateId == 3)
                 .Count(),
                Planning = _context.UsersGamesDatas
                 .Where(usr => usr.User.Id == user.Id)
                 .Where(stat => stat.StateId == 4)
                 .Count(),
                Awaiting = _context.UsersGamesDatas
                 .Where(usr => usr.User.Id == user.Id)
                 .Where(stat => stat.StateId == 5)
                 .Count(),
                Dropped = _context.UsersGamesDatas
                 .Where(usr => usr.User.Id == user.Id)
                 .Where(stat => stat.StateId == 6)
                 .Count(),
                Added = _context.UsersGamesDatas
                 .Where(usr => usr.User.Id == user.Id)
                 .Count()
            };
            var accountData = new AccountDto()
            {
                Username = user.UserName,
                User = user,
                Email = user.Email,
                CreateDate = user.CreateDate,
                Role = role.FirstOrDefault(),
                Statistics = stats
                //ChangeData = null
            };
            return accountData;
        }

        public async Task<IActionResult> Overview()
        {
            var accountData = await CreateAccountModelAsync();
            return View(accountData);
        }

        public async Task<IActionResult> Stats()
        {
            var accountData = await CreateAccountModelAsync();
            return View(accountData);
        }

        [Route("Overview/ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            var accountData = await CreateAccountModelAsync();
            return View(accountData);
        }

        [HttpPost]
        [Route("Overview/ChangePassword")]
        public async Task<IActionResult> ChangePassword(AccountDto dto)
        {
            var accountData = await CreateAccountModelAsync();

            //accountData.ChangeData = dto;
            //if(accountData.ChangeData.NewPassword == accountData.ChangeData.ConfirmPassword)
            //await _userManager.ChangePasswordAsync(accountData.User, accountData.ChangeData.OldPassword, accountData.ChangeData.NewPassword);

            var result = await _userManager.ChangePasswordAsync(accountData.User, dto.ChangeData.OldPassword, dto.ChangeData.NewPassword);
            await _signInManager.RefreshSignInAsync(accountData.User);
            //else
            //{
            //ModelState.AddModelError(string.Empty,"Cannot Change Password!");
            //}
            if(result.Succeeded)
                accountData.StatusMessage = "The Password has been changed successfuly!";
            else
            {
                accountData.StatusMessage = "Failed to change the Password!";
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(accountData);
        }
        /*
        [Route("Overview/ChangeEmail")]
        public async Task<IActionResult> ChangeEmail()
        {
            var accountData = await CreateAccountModelAsync();
            return View(accountData);
        }
        [HttpPost]
        [Route("Overview/ChangeEmail")]
        public async Task<IActionResult> ChangeEmail(AccountDto dto)
        {
            var accountData = await CreateAccountModelAsync();

            //if((accountData.User.Email != dto.ChangeData.NewEmail) || (accountData.User.Email == dto.ChangeData.OldEmail))
            //{
            var code = await _userManager.GenerateChangeEmailTokenAsync(accountData.User, dto.ChangeData.NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var result = await _userManager.ChangeEmailAsync(accountData.User, dto.ChangeData.NewEmail, code);
            //}
            if (result.Succeeded)
                accountData.StatusMessage = "The Email has been changed successfuly!";
            else
            {
                accountData.StatusMessage = "Failed to change the Email!";
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(accountData);
        }
        */

        [Route("Overview/DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var accountData = await CreateAccountModelAsync();
            return View(accountData);
        }

        [HttpPost]
        [Route("Overview/DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(AccountDto dto)
        {
            var accountData = await CreateAccountModelAsync();
            var result = await _userManager.CheckPasswordAsync(accountData.User, dto.ChangeData.OldPassword);
            if ((accountData.User.Email == dto.ChangeData.OldEmail) || (accountData.User.Id == dto.ChangeData.ConfirmDeleteId) || result)
            {
                await _userManager.DeleteAsync(accountData.User);
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                accountData.StatusMessage = "Failed to delete account!";
                accountData.ChangeData = null;
                return View(accountData);
            }
        }

        [Route("Overview/SendMessageToAdmin")]
        public async Task<IActionResult> SendMessageToAdmin()
        {
            var accountData = await CreateAccountModelAsync();
            return View(accountData);
        }

        [HttpPost]
        [Route("Overview/SendMessageToAdmin")]
        public async Task<IActionResult> SendMessageToAdmin(AccountDto dto)
        {
            var accountData = await CreateAccountModelAsync();
            if((dto.UserMessage.Topic == null) || (dto.UserMessage.Topic == ""))
            {
                accountData.StatusMessage = "Remember to type the Title!";
                return View(accountData);
            }
            if ((dto.UserMessage.Content == null) || (dto.UserMessage.Content == ""))
            {
                accountData.StatusMessage = "Remember to type the Content!";
                return View(accountData);
            }
            var message = new UserMessage()
            {
                UserId = accountData.User.Id,
                Topic = dto.UserMessage.Topic.Trim(),
                Content = dto.UserMessage.Content.Trim(),
                CreateTime = DateTime.Now
            };
            try
            {
                _context.UsersMessages.Add(message);
                await _context.SaveChangesAsync();
                accountData.StatusMessage = "The Message has been sent!";
            }
            catch
            {
                accountData.StatusMessage = "Failed to send message to Admin!";
            }
            return View(accountData);
        }
    }
}