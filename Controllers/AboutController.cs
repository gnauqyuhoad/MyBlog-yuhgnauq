using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Resume()
        {
            return View();
        }
        public IActionResult Viewuser()
        {
            return View();
        }
        private readonly BlogDbContext dbContext;
        public AboutController(BlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult View()
        {
            IEnumerable<Blog> lstBlogs = dbContext.Blogs.ToList();
            return View(lstBlogs);
        }


        public IActionResult Create()
        {
            Blog blog = new Blog();
            List<Category> categories = dbContext.Categories.ToList();
            ViewBag.categories = categories;
            return View(blog);
        }

        [BindProperty]
        public Blog Blog { get; set; }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(int id)
        {
            if (ModelState.IsValid)
            {
                Blog.Category = dbContext.Categories.First(cate => cate.Id == Blog.CateId);
                dbContext.Blogs.Add(Blog);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Viewuser");
        }

        public IActionResult Delete(int id)
        {
            var postToDelete = dbContext.Blogs.FirstOrDefault(u => u.Id == id);
            if (postToDelete != null)
            {
                dbContext.Blogs.Remove(postToDelete);
                dbContext.SaveChanges();
                return RedirectToAction("Viewuser");
            }
            return RedirectToAction("Viewuser");
        }

        //public IActionResult Details()
        //{
        //    Blog blog = new Blog();
        //    List<Category> categories = dbContext.Categories.ToList();
        //    ViewBag.categories = categories;
        //    return View(blog);
        //}

        public IActionResult Details(int id)
        {
            Blog = dbContext.Blogs.Include("Category").FirstOrDefault(blog => blog.Id == id);
            if (Blog != null)
            {
                return View(Blog);
            }
            return RedirectToAction("Viewuser");
        }

        public IActionResult Edit(int id)
        {
            Blog = dbContext.Blogs.FirstOrDefault(blog => blog.Id == id);
            if (Blog != null)
            {
                List<Category> categories = dbContext.Categories.ToList();
                ViewBag.categories = categories;
                return View(Blog);
            }
            return RedirectToAction("Viewuser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Viewuser", "About");
            }
            if (ModelState.IsValid)
            {

                Blog.Category = dbContext.Categories.First(cate => cate.Id == Blog.CateId);
                dbContext.Update(Blog);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Viewuser", "About");
        }
    }
}
