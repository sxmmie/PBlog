using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogger.Models;
using Blogger.Models.Comments;
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

        public List<Post> GetAllPosts(string category)
        {
            Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };

            return _ctx.Posts
               .Where(post => InCategory(post))
               .ToList();

            /*return _ctx.Posts
                .Where(p => p.Category.ToLower().Equals(category.ToLower()))
                .ToList();*/
        }

        /*public async Task<List<Post>> GetAllPosts()
        {
            var posts = await _ctx.Posts.ToListAsync();

            return posts;
        }*/

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
