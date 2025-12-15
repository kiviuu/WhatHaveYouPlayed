using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhatHaveYouPlayed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WhatHaveYouPlayed.Data;

namespace WhatHaveYouPlayed.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public GameController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult SetStatus(string Id)
        {
            ViewBag.GameImagePath = _configuration.GetValue<string>("Paths:GameImagePath");
            var game = _applicationDbContext.GameDatas
                .Include(prod => prod.Producent)
                .Where(id => id.GameId == Id)
                .FirstOrDefault();

            var userGameData = new UserGameData()
            {
                GameId = game.GameId,
                Game = game
            };
            return View(userGameData);
        }
        
        [HttpPost]
        public async Task<IActionResult> SetStatus(UserGameData USG)
        {
            var game = _applicationDbContext.GameDatas
                .Include(prod => prod.Producent)
                .Where(id => id.GameId == USG.GameId)
                .FirstOrDefault();

            var user = _applicationDbContext.Users
                .Where(u => u.UserName == User.Identity.Name)
                .First();
            var usergamedata = new UserGameData()
            {
                GameId = USG.GameId,
                UserId = user.Id,
                StateId = USG.StateId,
                AddDate = DateTime.Now
            };

            var result = _applicationDbContext.UsersGamesDatas
                .Where(o => o.GameId == usergamedata.GameId)
                .Where(o => o.UserId == usergamedata.UserId)
                .Any();

            if (ModelState.IsValid)
            {
                if (!result)
                {
                    _applicationDbContext.UsersGamesDatas.Add(usergamedata);
                    await _applicationDbContext.SaveChangesAsync();
                    return RedirectToAction("AllGames", "Home");
                }
                else
                {
                   
                    var record = _applicationDbContext.UsersGamesDatas
                            .Where(o => o.GameId == usergamedata.GameId)
                            .Where(o => o.UserId == usergamedata.UserId)
                            .FirstOrDefault();
                    if (usergamedata.StateId != record.StateId)
                    {
                        record.StateId = usergamedata.StateId;
                        _applicationDbContext.UsersGamesDatas.Update(record);
                        await _applicationDbContext.SaveChangesAsync();
                    }
                    //return RedirectToAction("UpdateStatus","Game");
                }
            }
            return RedirectToAction("AllGames","Home");
        }
        /*
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(UserGameData userGameData)
        {

            var user = _applicationDbContext.Users
                .Where(u => u.UserName == User.Identity.Name)
                .First();

            var record = _applicationDbContext.UsersGamesDatas
                    .Where(o => o.GameId == userGameData.GameId)
                    .Where(o => o.UserId == userGameData.UserId)
                    .FirstOrDefault();

            if (userGameData.StateId != record.StateId)
            {
                record.StateId = userGameData.StateId;
                _applicationDbContext.UsersGamesDatas.Update(record);
                await _applicationDbContext.SaveChangesAsync();
            }
            return RedirectToAction("SetStatus",userGameData);
        }
        */
    }
}
