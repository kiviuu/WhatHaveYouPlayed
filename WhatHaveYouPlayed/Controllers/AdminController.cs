using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WhatHaveYouPlayed.Data;
//using WhatHaveYouPlayed.Data.Migrations;
using WhatHaveYouPlayed.Models;
using WhatHaveYouPlayed.Models.Dto;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting;

namespace WhatHaveYouPlayed.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(ApplicationDbContext applicationDbContext, IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = applicationDbContext;
            _configuration = configuration;
            _environment = webHostEnvironment;
            _userManager = userManager;
        }
        public async Task<PanelDto> CreatePanelDto()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var panelDto = new PanelDto()
            {
                userMessages = _context.UsersMessages
                .Include(obj => obj.User)
                .OrderByDescending(dt => dt.CreateTime)
                .ToList(),
                User = user,
                StatusMessage = new List<string>()
            };
            return panelDto;
        }
        public async Task<IActionResult> AdminPanel()
        {
            var panelDto = await CreatePanelDto();
            return View(panelDto);
        }



        //Posts Actions
        public async Task<IActionResult> CreatePost()
        {
            ViewBag.ImageAcceptString = "image/,.png,.jpeg,.jpg,.gif,.jfif";
            var panelDto = await CreatePanelDto();
            return View(panelDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PanelDto dto)
        {
            //Correct image formats
            List<string> ImageFormats = new List<string>
            {
                "png",
                "jpg",
                "jpeg",
                "gif",
                "jfif"
            };
            string ImageFormatsString = null;

            //Creating accept string for html file input
            foreach(string format in ImageFormats)
            {
                ImageFormatsString += $",.{format}";
            }
            ViewBag.ImageAcceptString = "image/" + ImageFormatsString;


            var postGuid = Guid.NewGuid();
            var panelDto = await CreatePanelDto();

            //Title and Content required validation
            if ((dto.BlogPost.Title == null) || (dto.BlogPost.Title == ""))
            {
                panelDto.StatusMessage.Add("Remember to type Title!");
                return View(panelDto);
            }
            if ((dto.BlogPost.Content == null) || (dto.BlogPost.Content == ""))
            {
                panelDto.StatusMessage.Add("Remember to type Content!");
                return View(panelDto);
            }

            var post = new BlogPost()
            {
                PostId = postGuid.ToString(),
                Title = dto.BlogPost.Title.Trim(),
                Content = dto.BlogPost.Content.Trim(),
                AuthorId = panelDto.User.Id,
                PostDate = DateTime.Now
            };

            //Saving the added image
            if (dto.PostImage != null)
            {
                //Getting only the format type
                string imageFormat = dto.PostImage.ContentType.Substring(dto.PostImage.ContentType.IndexOf("/") + 1);
                //Image format validation
                if (!(ImageFormats.Contains(imageFormat)))
                {
                    panelDto.StatusMessage.Add($"Remember the Image must have one of these formats: {ImageFormatsString}");
                    return View(panelDto);
                }

                var uniqueImageName = postGuid.ToString() + "_" + dto.PostImage.FileName;
                var path = Path.Combine(_environment.ContentRootPath,"wwwroot","PostImg", uniqueImageName);
                var stream = new FileStream(path, FileMode.Create);
                await dto.PostImage.CopyToAsync(stream);
                post.PostImgSrc = uniqueImageName;

                stream.Close();
            }
            else
            {
                post.PostImgSrc = null;
            } 


            //Adding post to the database
            try
            {
                _context.BlogPosts.Add(post);
                await _context.SaveChangesAsync();
                panelDto.StatusMessage.Add("New Post has been created!");
            }
            catch
            {
                panelDto.StatusMessage.Add("Failed to create new Post!");

            }
            return View(panelDto);
        }

        public async Task<IActionResult> DeletePost()
        {
            var panelDto = await CreatePanelDto();
            panelDto.blogPosts = _context.BlogPosts
                .OrderByDescending(dt => dt.PostDate)
                .ToList();
            return View(panelDto);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(PanelDto dto)
        {
            var panelDto = await CreatePanelDto();
            panelDto.blogPosts = _context.BlogPosts.ToList();
            if ((dto.BlogPost.PostId == null) || (dto.BlogPost.PostId == ""))
            {
                panelDto.StatusMessage.Add("Remember to type the correct Post Id!");
                return View(panelDto);
            }
            try
            {
                var postToDelete = _context.BlogPosts.Where(id => id.PostId == dto.BlogPost.PostId).First();
                if(postToDelete.PostImgSrc != null)
                {
                    DeleteContentFile(postToDelete.PostImgSrc, "PostImg");
                }
                _context.BlogPosts.Remove(postToDelete);
                await _context.SaveChangesAsync();
                panelDto.StatusMessage.Add("The Post has been deleted!");
            }
            catch
            {
                panelDto.StatusMessage.Add("Failed to remove the Post with this Id!");
            }
            panelDto.blogPosts = _context.BlogPosts.ToList();
            return View(panelDto);
        }



        //Games Actions
        public async Task<IActionResult> AddGame()
        {
            ViewBag.ImageAcceptString = "image/,.png,.jpeg,.jpg,.gif,.jfif";
            var panelDto = await CreatePanelDto();
            return View(panelDto);
        }

        [HttpPost]
        public  async Task<IActionResult> AddGame(PanelDto dto)
        {
            var panelDto = await CreatePanelDto();
            //Fields required validation
            if ((dto.GameData.Name == null) || (dto.GameData.Producent.CompanyName == null) || (dto.GameImage == null))
            {
                panelDto.StatusMessage.Add("Remember: all fields are required!");
                return View(panelDto);
            }
            if(_context.GameDatas.Where(p => p.Name == dto.GameData.Name).Count() != 0)
            {
                panelDto.StatusMessage.Add("Failed: game with this name already exist!");
                return View(panelDto);
            }


            //Correct image formats
            List<string> ImageFormats = new List<string>
            {
                "png",
                "jpg",
                "jpeg",
                "gif",
                "jfif"
            };
            string ImageFormatsString = null;

            //Creating accept string for html file input
            foreach (string format in ImageFormats)
            {
                ImageFormatsString += $",.{format}";
            }
            ViewBag.ImageAcceptString = "image/" + ImageFormatsString;


            var gameGuid = Guid.NewGuid();
            
            panelDto.Producents = _context.Producents.ToList();
            var producentsName = new List<string>();
            foreach(var producent in panelDto.Producents)
            {
                producentsName.Add(producent.CompanyName);
            }

            var game = new GameData()
            {
                GameId = gameGuid.ToString(),
                Name = dto.GameData.Name.Trim(),
                ReleaseDate = dto.GameData.ReleaseDate,
                PegiAge = dto.GameData.PegiAge
            };


            if (!(producentsName.Contains(dto.GameData.Producent.CompanyName)))
            {
                _context.Producents.Add(new Producent { CompanyName = dto.GameData.Producent.CompanyName });
                await _context.SaveChangesAsync();
            }

            game.ProducentId = _context.Producents.Where(cname => cname.CompanyName == dto.GameData.Producent.CompanyName).First().ProdId;


            //Saving the added image
            if (dto.GameImage != null)
            {
                //Getting only the format type
                string imageFormat = dto.GameImage.ContentType.Substring(dto.GameImage.ContentType.IndexOf("/") + 1);
                //Image format validation
                if (!(ImageFormats.Contains(imageFormat)))
                {
                    panelDto.StatusMessage.Add($"Remember the Image must have one of these formats: {ImageFormatsString}");
                    return View(panelDto);
                }

                var uniqueImageName = gameGuid.ToString() + "_" + dto.GameImage.FileName;
                var path = Path.Combine(_environment.ContentRootPath, "wwwroot", "GameImg", uniqueImageName);
                var stream = new FileStream(path, FileMode.Create);
                await dto.GameImage.CopyToAsync(stream);
                game.ImgSrc = uniqueImageName;

                stream.Close();
            }


            //Adding post to the database
            try
            {
                _context.GameDatas.Add(game);
                await _context.SaveChangesAsync();
                panelDto.StatusMessage.Add("New Game has been added!");
            }
            catch
            {
                panelDto.StatusMessage.Add("Failed to add new Game!");

            }

            return View(panelDto);
        }

        public async Task<IActionResult> DeleteGame()
        {
            var panelDto = await CreatePanelDto();
            panelDto.games = _context.GameDatas
                .Include(p => p.Producent)
                .ToList();
            return View(panelDto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGame(PanelDto dto)
        {
            var panelDto = await CreatePanelDto();
            panelDto.games = _context.GameDatas.Include(p => p.Producent).ToList();
            if ((dto.GameData.GameId == null) || (dto.GameData.GameId == ""))
            {
                panelDto.StatusMessage.Add("Remember to type the correct Game Id!");
                return View(panelDto);
            }
            try
            {
                var gameToDelete = _context.GameDatas.Where(id => id.GameId == dto.GameData.GameId).First();
                if (gameToDelete.ImgSrc != null)
                {
                    DeleteContentFile(gameToDelete.ImgSrc, "GameImg");
                }
                _context.GameDatas.Remove(gameToDelete);
                await _context.SaveChangesAsync();
                panelDto.StatusMessage.Add("The Game has been deleted!");
            }
            catch
            {
                panelDto.StatusMessage.Add("Failed to remove the Game with this Id!");
            }
            panelDto.games = _context.GameDatas.Include(p => p.Producent).ToList();
            return View(panelDto);
        }


        
        public async Task<IActionResult> DeleteMessage(PanelDto dto, int messageId)
        {
            var panelDto = await CreatePanelDto();
            try
            {
                var message = _context.UsersMessages.Where(p => p.MessageId == messageId).FirstOrDefault();
                _context.UsersMessages.Remove(message);
                await _context.SaveChangesAsync();
                panelDto.StatusMessage.Add("The message has been deleted!");
            }
            catch
            {
                panelDto.StatusMessage.Add("Failed to delete the message!");
            }
            return RedirectToAction("AdminPanel", panelDto);
        }
        


        //Delete content file 
        private void DeleteContentFile(string imageName, string foldersPath)
        {
            var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", foldersPath, imageName);
            if (System.IO.File.Exists(imagePath))
            {
                FileInfo fileInfo = new FileInfo(imagePath);
                System.IO.File.Delete(imagePath);
                fileInfo.Delete();
            }
        }
        
    }
}
