using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhatHaveYouPlayed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WhatHaveYouPlayed.Data;
using AspNetCoreGeneratedDocument;

namespace WhatHaveYouPlayed.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserHomeBlog");
            }
            return View();
        }
        /*
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Privacy()
        {
            var user = await _userManager.FindByNameAsync("Bartek");
            ViewBag.Test = await _userManager.GetEmailAsync(user);
            ViewBag.User = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }
        */
        [HttpGet]
        public IActionResult UserHomeBlog()
        {
            ViewBag.PostImagePath = _configuration.GetValue<string>("Paths:PostImagePath");
            List<BlogPost> blogs = new List<BlogPost>();
            blogs = _applicationDbContext.BlogPosts
                .Include(user => user.Author)
                .ToList();
            return View(blogs);
        }

        [HttpGet]
        public IActionResult AllGames(string searchString)
        {
            ViewBag.GameImagePath = _configuration.GetValue<string>("Paths:GameImagePath");
            var games = _applicationDbContext.GameDatas
                .Include(prod => prod.Producent)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.Name.Contains(searchString));
                ViewData["CurrentFilter"] = searchString;
            }
            List<GameData> gamesList = games.ToList();
            return View(games);
        }

        [HttpGet]
        public IActionResult MyGames(int Id = 1/*, string status = "NotAdded"*/)
        {
            ViewBag.GameImagePath = _configuration.GetValue<string>("Paths:GameImagePath");
            var user = _applicationDbContext.Users
                .Where(name => name.UserName == User.Identity.Name)
                .First();
            var usergamedatas = new List<UserGameData>();

            if(Id != 1)
            {
                usergamedatas = _applicationDbContext.UsersGamesDatas
                .Include(game => game.Game)
                .Include(state => state.State)
                .Where(us => us.UserId == user.Id)
                .Where(state => state.StateId == Id)
                .ToList();
            }
            else
            {
                usergamedatas = _applicationDbContext.UsersGamesDatas
                .Include(game => game.Game)
                .Include(state => state.State)
                .Where(us => us.UserId == user.Id)
                .ToList();
            }
            return View(usergamedatas);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}