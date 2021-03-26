using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab4_Cs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCCoreAndEF.Controllers
{
    public class HomeController : Controller
    {
        private Assigment1DbContext _Assign1Context;


        public HomeController(Assigment1DbContext context)
        {
            _Assign1Context = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddBlogPost()
        {
            return View();
        }
        public IActionResult Register() {
            return View();
        }

        public IActionResult CreateUser(User u)
        {
            RedirectToPage("Register");
            if (!ModelState.IsValid)
                return RedirectToAction("Register");
            _Assign1Context.Users.Add(u);
            _Assign1Context.SaveChanges();

            HttpContext.Session.SetString("FirstName", u.FirstName);
            HttpContext.Session.SetString("LastName", u.LastName);
            HttpContext.Session.SetString("EmailAddress", u.EmailAddress);
            HttpContext.Session.SetInt32("UserId", u.UserId);
            HttpContext.Session.SetInt32("Roleid", u.RoleId);

            return RedirectToAction("Index");
        }
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult EditBlog(int id)
        {
            var blog = (from b in _Assign1Context.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();

            return View(blog);
        }


        public IActionResult Login()
        {
            return View();
        }
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

        public IActionResult EditBlogPost(int id)
        {
            var editPost = (from b in _Assign1Context.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();
            return View(editPost);
        }

        public IActionResult ModifyBlog(BlogPost blogPost)
        {
            var editPosted = (from b in _Assign1Context.BlogPosts where b.BlogPostId == blogPost.BlogPostId select b).FirstOrDefault();
            editPosted.Title = blogPost.Title;
            editPosted.Content = blogPost.Content;
            editPosted.Posted = DateTime.Now;
            _Assign1Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteBlog(int id)
        {
            var deletePosted = (from b in _Assign1Context.BlogPosts where b.BlogPostId == id select b).FirstOrDefault();
            var deleteComment = (from item in _Assign1Context.Comments where item.BlogPostId == id select item).ToList();
            foreach (Comment c in deleteComment)
            {
                _Assign1Context.Comments.Remove(c);
            }
            _Assign1Context.BlogPosts.Remove(deletePosted);
            _Assign1Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddPosted(BlogPost newBlogPost)
        {
            _Assign1Context.BlogPosts.Add(newBlogPost);
            _Assign1Context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult DisplayFullBlogPost(int id)
        {
            var subset = _Assign1Context.BlogPosts.Include(x => x.user).Include(y => y.comment).ThenInclude(z => z.user);
            var blogToDisplay = (from b in subset where b.BlogPostId == id select b).FirstOrDefault();
            return View(blogToDisplay);
        }

        public IActionResult AddComment(Comment comment)
        {
            _Assign1Context .Comments.Add(comment);
            _Assign1Context.SaveChanges();
            return RedirectToAction("DisplayFullBlogPost", new { id = comment.BlogPostId });
        }
    }
}
