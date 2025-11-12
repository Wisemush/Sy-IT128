using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace BlogTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlData db = GetConnection();
            try
            {
                //Authenticate(db);
                //Register(db);
                //AddPost(db);
                ListPosts(db);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static SqlData GetConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();

            // Log connection string (mask secrets if present)
            string cs = config.GetConnectionString("SqlDb") ?? "<null>";
            Console.WriteLine($"Using connection string: {cs}");

            ISqlDataAccess dbAccess = new SqlDataAccess(config);
            SqlData db = new SqlData(dbAccess);
            return db;
        }

        private static UserModel GetCurrentUser(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            UserModel user = null;

            try
            {
                user = db.Authenticate(username, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }

            return user;
        }

        public static void Authenticate(SqlData db)
        {
            UserModel user = GetCurrentUser(db);
            if (user == null)
            {
                Console.WriteLine("Invalid credentials or could not connect to the database.");
            }
            else
            {
                Console.WriteLine($"Welcome, {user.UserName}");
            }
        }

        public static void Register(SqlData db)
        {
            Console.Write("Enter new username: ");
            var username = Console.ReadLine();

            Console.Write("Enter new password: ");
            var password = Console.ReadLine();

            Console.Write("Enter First name: ");
            var firstName = Console.ReadLine();

            Console.Write("Enter Last name: ");
            var lastName = Console.ReadLine();

            try
            {
                db.Register(username, firstName, lastName, password);
                Console.WriteLine("Registration successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registration failed: " + ex.Message);
            }
        }


        private static void AddPost(SqlData db)
        {
            UserModel user = GetCurrentUser(db);
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.WriteLine("Write body: "); string body = Console.ReadLine();
            PostModel post = new PostModel
            {
                Title = title,
                Body = body,
                DateCreated = DateTime.Now,
                UserId = user.Id
            };
            db.AddPost(post);
        }

        private static void ListPosts(SqlData db)
        {

            List<ListPostModel> posts = db.ListPosts();

            foreach (ListPostModel post in posts)
            {
                Console.WriteLine($"{post.Id}. Title: {post.Title} by {post.UserName}[{ post.DateCreated.ToString("yyyy-MM-dd")}]");

                Console.WriteLine($"{(post.Body.Length > 20 ? post.Body.Substring(0, 20) + " ... " : post.Body)}");

                Console.WriteLine();

            }    
                

        }

        private static void ShowPostDetails(SqlData db)
        {
            Console.Write("Enter a post ID: ");
            int id = Int32.Parse(Console.ReadLine());

            ListPostModel post = db.ShowPostDetails(id);
            Console.WriteLine(post.Title);
            Console.WriteLine($"by {post.FirstName} {post.LastName} [{post.UserName}]");
    

            Console.WriteLine();

            Console.WriteLine(post.Body);

            Console.WriteLine(post.DateCreated.ToString("MMM d yyyy"));

        }
    }
}
