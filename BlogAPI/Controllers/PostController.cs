using BlogDataLibrary;
using BlogDataLibrary.Data;
using BlogDataLibrary.Models;  // Add this - for ListPostModel
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]  // Add this
    [ApiController]               // Add this
    public class PostController : ControllerBase  // Change to ControllerBase for API
    {
        private readonly ISqlData _db;  // Add this field declaration

        public PostController(ISqlData db)  // Change from private to public
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("list")]  // Optional: makes URL cleaner
        public ActionResult ListPosts()
        {
            List<ListPostModel> posts = _db.ListPosts();
            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]  // Optional: makes URL cleaner
        public ActionResult ShowPostDetails(int id)
        {
            ListPostModel post = _db.ShowPostDetails(id);
            return Ok(post);
        }

        [Authorize]
        [HttpPost]
        [Route("add")]  // Optional: makes URL cleaner
        public ActionResult AddPost([FromBody] PostForm form)
        {
            PostModel post = new PostModel();
            post.Title = form.Title;
            post.Body = form.Body;
            post.DateCreated = DateTime.Now;
            post.UserId = GetCurrentUserId();
            _db.AddPost(post);

            return Ok("post Created.");
        }


        private int GetCurrentUserId()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                string id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                if (id != null)
                {
                    return Convert.ToInt32(id);
                }
            }
            return 0;
        }
    }
}