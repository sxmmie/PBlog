using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogger.Models;
using Blogger.Models.Comments;
using Blogger.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _ctx;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<Post> GetAllPosts()
        {
            var posts = _ctx.Posts.ToList();

            return posts;
        }

        public IndexViewModel GetAllPosts(int pageNumber, string category)
        {
            Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };

            var pageSize = 5;
            var skipAmount = pageSize * (pageNumber - 1);

            var query = _ctx.Posts.AsQueryable();

            if (String.IsNullOrEmpty(category))
                query = query.Where(x => InCategory(x));

            var postsCount = query.Count();

            var posts = new IndexViewModel
            {
                PageNumber = pageNumber,
                PageCount = (int) Math.Ceiling((double)postsCount / pageSize),
                NextPage = postsCount > skipAmount + pageSize,
                Category = category,
                Posts = query.Skip(skipAmount).Take(pageSize).ToList()
            };

            return posts;
        }

        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
        }

        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }

        public Post GetPost(int id)
        {
            var post = _ctx.Posts
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.Id == id);
            
            return post;
        }

        public void RemvePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }

        public async Task<bool> SavageChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }

        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }
    }
}
