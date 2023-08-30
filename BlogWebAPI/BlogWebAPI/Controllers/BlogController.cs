
using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;

namespace BlogWebAPI.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        BlogApplicationContext _db;
        UserManager<User> _userManager;
        IMapper mapper;
        
        public BlogController(BlogApplicationContext context, UserManager<User> userManager,IMapper _mapper)
        {
            _db = context;
            _userManager = userManager;
            mapper = _mapper;
        }
        
        [HttpGet]
        public IActionResult Users()
        {
            //System.ArgumentNullException: Value cannot be null. (Parameter 'providedPassword')
            //at Microsoft.AspNetCore.Identity.PasswordHasher`1.VerifyHashedPassword(TUser user, String hashedPassword, String providedPassword)
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode(401);
            }
            var users = _db.Users.Include(x => x.Subscribes).ToList();
            List<UserModel> userModels = new List<UserModel>();
            foreach (var user in users)
            {
                var userModel = mapper.Map<UserModel>(user);
                userModels.Add(userModel);
            }

            return Json(userModels);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Blogs()
        {
            var blogs = await _db.Blogs.Include(x => x.User).Include(x=> x.Articles).ToListAsync();
      
            List<BlogModel> blogModels = new List<BlogModel>();
            foreach (var blog in blogs)
            {
                var blogModel = mapper.Map<BlogModel>(blog);
                blogModels.Add(blogModel);
            }

            return  Json(blogModels);
        }

        public async Task<IActionResult> AddNewBlog(string name, int idUser)
        {
            if (string.IsNullOrEmpty(name) || idUser== 0)
            {
                return BadRequest();
            }
            if(await _db.Blogs.FirstAsync(x => x.Name == name && x.IdUser == idUser) != null)
            {
                return BadRequest();
            }
            Blog newBlog = new Blog();
            newBlog.Name = name;
            newBlog.IdUser = idUser;
            await _db.Blogs.AddAsync(newBlog);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog =await _db.Blogs.Include(x => x.User).Include(x => x.Articles).FirstAsync(x => x.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return Json(blog);
        }
        [HttpGet]
       
        public IActionResult Articles()
        {
            
            var articles = _db.Articles.Include(x => x.Blog).ToList();
            List<ArticleModel> articleModels = new List<ArticleModel>();

            foreach (var article in articles)
            {
                var articleModel = mapper.Map<ArticleModel>(article);
                articleModels.Add(articleModel);
            }
            return Json(articleModels);
        }
     
        public async Task<IActionResult> AddNewArticle(string title, string description , string? photo ,int idBlog)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || idBlog == 0)
            {
                return BadRequest();
            }
            if (await _db.Articles.FirstAsync(x => x.Title == title && x.Description == description && x.IdBlog == idBlog) != null)
            {
                return BadRequest();
            }
            var newArticle = new Article()
            {
                Title = title,
                Description = description,
                Photo = photo,
                IdBlog = idBlog
            };

            await _db.Articles.AddAsync(newArticle);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public IActionResult Subscribers()
        {
            var subs = _db.Subscribes.Include(x => x.User).ToList();
            List<SubscribeModel> subModels = new List<SubscribeModel>();
            
            foreach (var sub in subs)
            {
                var subModel = mapper.Map<SubscribeModel>(sub);
                subModels.Add(subModel);
            }
            return Json(subModels); 
        }

        [HttpGet]
        public IActionResult GetBlogsFromSubscribers(int userid)
        {
            var blogs = (from blog in _db.Blogs.Include(x => x.User)
                        from d in blog.User.Subscribes
                        where d.SubscriberId == userid
                        select blog).ToList();
            List<BlogModel> blogModels = new List<BlogModel>();
            foreach (var blog in blogs)
            {
                var blogModel = mapper.Map<BlogModel>(blog);
                blogModels.Add(blogModel);
            }
           
           return  Json(blogModels);
        }
        [HttpGet]
        public IActionResult GetArticlesFromBlogsSubscribers(int userid)
        {
          var articles = (from article in _db.Articles.Include(x => x.Blog).ThenInclude(b => b.User)
                          from a in article.Blog.User.Subscribes
                           where a.SubscriberId == userid
                           select article).ToList();
            List<ArticleModel> articleModels = new List<ArticleModel>();
            foreach (var article in articles)
            {
                var articleMap = mapper.Map<ArticleModel>(article);
                
                articleModels.Add(articleMap);
            }
            return Json(articleModels);
        }


    }
}
