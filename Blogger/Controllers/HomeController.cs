using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogger.Data;
using Blogger.Data.FileManager;
using Blogger.Data.Repository;
using Blogger.Models;
using Blogger.Models.Comments;
using Blogger.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogger.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repo;
        private readonly IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        // GET: /<controller>/
        public IActionResult Index(int pageNumber, string category)
        {
            if (pageNumber < 0)
                return RedirectToAction("Index", new { pageNumber = 1, category }); // set defaults

            var vm = _repo.GetAllPosts(pageNumber, category);

            return View(vm);
        }

        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);

            return View(post);
        }

        // Streaming images
        [HttpGet("/Image/{iamge}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf(".") + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{image}");
        }

        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.PostId });

            var post = _repo.GetPost(vm.PostId);
            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTimeOffset.Now
                });

                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTimeOffset.Now
                };

                _repo.AddSubComment(comment);
            }

            await _repo.SavageChangesAsync();

            return RedirectToAction("Post", new { id = vm.PostId });
        }
    }
}
