using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogger.Models;
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
            var post = _ctx.Posts.FirstOrDefault(p => p.Id == id);
            
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
    }
}
