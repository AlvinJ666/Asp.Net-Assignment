using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab4_Cs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCCoreAndEF.Controllers
{
    public class HomeController : Controller
    {
        private readonly Assigment1DbContext _Assign1Context;//readonly means it can only be assigned in declaration or in constructor, can have multiple times of assignments
        //private readonly BlobServiceClient _blobServiceClient;

        //constructor, initialize the context object
        public HomeController(Assigment1DbContext context)//, BlobServiceClient blobServiceClient)
        {
            _Assign1Context = context;
            //_blobServiceClient = blobServiceClient;
        }

        //function to direct to home page, if has notification, pass to viewbag value for alertion
        public IActionResult Index(String notification)
        {
            if (notification != null)
                ViewBag.notification = notification;
            return View(_Assign1Context.BlogPosts.ToList());
        }

        //function to direct to blog adding page
        public IActionResult AddBlogPost()
        {
            return View();
        }

        //function to direct to register page, if has notification, pass to viewbag value for alertion
        public IActionResult Register(String notification) {
            if(notification!=null)
                ViewBag.notification = notification;
            return View();
        }

        //function to direct to user account creation page, pass by an user object for validation checking and set httpcontext session to stay login if succeed
        public IActionResult CreateUser(User u)
        {
            var existu = (from user in _Assign1Context.Users where user.EmailAddress == u.EmailAddress select user).FirstOrDefault();
            if (existu == null)
            {
                if (!ModelState.IsValid)
                    //direct to register page with alertion of email format or password length
                    return RedirectToAction("Register", new { notification = "Failed to create account, Please Check your Email format or password length" });
                _Assign1Context.Users.Add(u);
                _Assign1Context.SaveChanges();

                //set sessions
                HttpContext.Session.SetString("FirstName", u.FirstName);
                HttpContext.Session.SetString("LastName", u.LastName);
                HttpContext.Session.SetString("EmailAddress", u.EmailAddress);
                HttpContext.Session.SetInt32("UserId", u.UserId);
                HttpContext.Session.SetInt32("Roleid", u.RoleId);

                //direct to home page
                return RedirectToAction("Index");
            }
            //direct to registration page if failed to creat account, with alertion
            return RedirectToAction("Register", new { notification = "Failed to create account, account exists" });
        }

        //function to clear login sessions and direct to home page
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        //function to direct to login page
        public IActionResult Login()
        {
            return View();
        }

        //function to verify if another user log in
        public IActionResult Verify(User user)
        {
            var userToUpdate = (from u in _Assign1Context.Users where u.EmailAddress == user.EmailAddress && u.Password == user.Password select u).FirstOrDefault();
            if (userToUpdate == null)
                return RedirectToAction("Login");
            else {
                HttpContext.Session.SetString("FirstName",userToUpdate.FirstName);
                HttpContext.Session.SetString("LastName", userToUpdate.LastName);
                HttpContext.Session.SetString("EmailAddress", userToUpdate.EmailAddress);
                HttpContext.Session.SetInt32("UserId", userToUpdate.UserId);
                HttpContext.Session.SetInt32("Roleid", userToUpdate.RoleId);
            }
            return RedirectToAction("Index");
        }

        //function to direct to the selected blog page for editing
        public IActionResult EditBlogPost(int id)
        {
            var editPost = (from b in _Assign1Context.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();
            return View(editPost);
        }

        //function to update the blogpost, direct to home page after updating
        public IActionResult ModifyBlog(BlogPost blogPost)
        {
            var editPosted = (from b in _Assign1Context.BlogPosts where b.BlogPostId == blogPost.BlogPostId select b).FirstOrDefault();
            editPosted.Title = blogPost.Title;
            editPosted.Content = blogPost.Content;
            editPosted.Posted = DateTime.Now;
            _Assign1Context.SaveChanges();
            return RedirectToAction("Index", new { notification="Successfully update the blog!!"});
        }

        //function to delete the selected blogpost as well as the comments associates with the blog
        public IActionResult DeleteBlog(int id)
        {
            var deletePosted = (from b in _Assign1Context.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();
            var deleteComment = (from item in _Assign1Context.Comments where item.BlogPostId == id select item).ToList();
            var deletePhoto = (from photo in _Assign1Context.Photos where photo.BlogPostId == id select photo).ToList();
            foreach (Comment c in deleteComment)
            {
                _Assign1Context.Comments.Remove(c);
            }
            foreach (Photo p in deletePhoto)
            {
                _Assign1Context.Photos.Remove(p);
            }
            _Assign1Context.BlogPosts.Remove(deletePosted);
            _Assign1Context.SaveChanges();
            return RedirectToAction("Index", new { notification = "Successfully delete the blog!!" });
        }

        //function to add a blogpost to the blog list
        public IActionResult AddPosted(BlogPost newBlogPost)
        {
            _Assign1Context.BlogPosts.Add(newBlogPost);
            _Assign1Context.SaveChanges();
            return RedirectToAction("Index", new { notification = "Successfully add the blog!!" });
        }

        //function to query the blog post as well as it's author and the comments assciate with it.
        public IActionResult DisplayFullBlogPost(int id)
        {
            var subset = _Assign1Context.BlogPosts.Include(x => x.user).Include(y => y.comment).ThenInclude(z => z.user);
            var blogToDisplay = (from b in subset where b.BlogPostId == id select b).FirstOrDefault();
            return View(blogToDisplay);
        }

        //function to allow add a comment to the specific blog
        public IActionResult AddComment(Comment comment)
        {
            _Assign1Context .Comments.Add(comment);
            _Assign1Context.SaveChanges();
            return RedirectToAction("DisplayFullBlogPost", new { id = comment.BlogPostId });
        }

        //function to direct to the search page
        public IActionResult Search()
        {
            return View();
        }

        //function to query all matched results after searching, results are based on the selected section(title/content) to query, if not specifiy, compare with the queries results, show the most similarities
        public async Task<IActionResult> showSearchResult(string pre, string value)
        {
            try
            {
                if (pre.Equals("title"))
                {
                    var blogs = await _Assign1Context.BlogPosts.Where(b => b.Title.Contains(value)).ToListAsync();
                    return View("index", blogs);
                }
                else if (pre.Equals("content"))
                {
                    var blogs = await _Assign1Context.BlogPosts.Where(b => b.Content.Contains(value)).ToListAsync();
                    return View("index", blogs);
                }
                else
                {
                    var blogsT = await _Assign1Context.BlogPosts.Where(b => b.Title.Contains(value)).ToListAsync();
                    var blogsC = await _Assign1Context.BlogPosts.Where(b => b.Content.Contains(value)).ToListAsync();
                    if(blogsT.Count()> blogsC.Count())
                        return View("index", blogsT);
                    else
                        return View("index", blogsC);
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

    }
}
