﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Data;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly WebAppContext _context;
        public CourseController(WebAppContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Course> courses = _context.Courses.ToList();
            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null || id == 0) return NotFound();
            Course? course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if(course == null) return NotFound();
            ViewBag.Blogs = _context.Blogs.OrderByDescending(c => c.Id).Take(4).ToList();

            return View(course);
        }
    }
}