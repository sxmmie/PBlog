using Blogger.Models;
using Blogger.Models.Comments;
using Blogger.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data.Repository
{
    public interface IRepository
    {
        // Task<List<Post>> GetAllPosts();
        List<Post> GetAllPosts();
        //IndexViewModel GetAllPosts(int pageNumber);
        IndexViewModel GetAllPosts(int pageNumber, string category, string search);

        void UpdatePost(Post post);
        void AddPost(Post post);

        Post GetPost(int id);
        void RemvePost(int id);

        void AddSubComment(SubComment comment);

        Task<bool> SavageChangesAsync();
    }
}
