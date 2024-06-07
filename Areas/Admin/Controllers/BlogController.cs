using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;

namespace MyBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly BlogDbContext dbContext;
        public BlogController(BlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
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
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var postToDelete = dbContext.Blogs.FirstOrDefault(u => u.Id == id);
            if (postToDelete != null)
            {
                dbContext.Blogs.Remove(postToDelete);
                dbContext.SaveChanges();
                return RedirectToAction("Delete");
            }
            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Blog");
            }
            if (ModelState.IsValid)
            {

                Blog.Category = dbContext.Categories.First(cate => cate.Id == Blog.CateId);
                dbContext.Update(Blog);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index", "Blog");
        }
        /*private readonly BlogDbContext dbContext;

        public BlogController(BlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: Admin/Blog
        public async Task<IActionResult> Index()
        {
            return View(await dbContext.Blogs.ToListAsync());
        }

        // GET: Admin/Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await dbContext.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blog/Create
        public IActionResult blog()
        {
            Blog blog= new Blog();

            List<Category> categories = dbContext.Categories.ToList();
            ViewBag.categories = categories;
            return View(blog);
        }

        // POST: Admin/Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(int id)
        {
            if (ModelState.IsValid)
            {
                Blog.Category = dbContext.Categories.First(cate => cate.Id == Blog.CateId);
                dbContext.Blogs.Add(Blog);
                dbContext.SaveChanges();
            }
            return RedirectToAction("View");
        }
        *//*public async Task<IActionResult> Create([Bind("Id,TieuDe,NoiDung,TacGia,CateId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(blog);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);*//*
    }

        // GET: Admin/Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await dbContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TieuDe,NoiDung,TacGia,CateId")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(blog);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await dbContext.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await dbContext.Blogs.FindAsync(id);
            if (blog != null)
            {
                dbContext.Blogs.Remove(blog);
            }

            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return dbContext.Blogs.Any(e => e.Id == id);
        }*/
    }
}
