using BlogDataLibrary.Models;

namespace BlogDataLibrary.Data
{
    public interface ISqlData
    {
        void AddPost(PostModel post);
        UserModel Authenticate(string UserName, string password);
        List<ListPostModel> ListPosts();
        void Register(string UserName, string firstName, string lastName, string password);
        ListPostModel ShowPostDetails(int id);
        void InsertUser(UserModel user);

    }
}