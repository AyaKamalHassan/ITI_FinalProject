using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagementApp.Data;
using CourseManagementApp.Models;
using CourseManagementApp.ViewModels;

namespace CourseManagementApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        // 1. READ ALL (Index)
        public IActionResult Index()
        {
            // Eager Loading للمدرب المسؤول
            var courses = _context.Courses.Include(c => c.Instructor).ToList();

            ViewBag.TotalCourses = courses.Count;
            ViewData["PageHeader"] = "Course Catalog & Training Programs";
            return View(courses);
        }

        // 2. DETAILS
        public IActionResult Details(int id)
        {
            var course = _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Trainees)
                .FirstOrDefault(c => c.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // 3. CREATE (GET)
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "Id", "Name");
            ViewData["ActionTitle"] = "Add New Course";
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "Id", "Name", vm.InstructorId);
                ViewData["ActionTitle"] = "Add New Course";
                return View(vm);
            }

            var course = new Course
            {
                Title = vm.Title,
                Description = vm.Description,
                DurationInHours = vm.DurationInHours.Value,
                Price = vm.Price.Value,
                InstructorId = vm.InstructorId.Value
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            TempData["Success"] = $" The course: '{course.Title}' added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // 4. EDIT (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            var vm = new CourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                DurationInHours = course.DurationInHours,
                Price = course.Price,
                InstructorId = course.InstructorId
            };

            ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "Id", "Name", course.InstructorId);
            ViewData["ActionTitle"] = " Edit Course Data ";
            return View(vm);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CourseViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Instructors = new SelectList(_context.Instructors.ToList(), "Id", "Name", vm.InstructorId);
                ViewData["ActionTitle"] = "Edit Course Data";
                return View(vm);
            }

            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            course.Title = vm.Title;
            course.Description = vm.Description;
            course.DurationInHours = vm.DurationInHours.Value;
            course.Price = vm.Price.Value;
            course.InstructorId = vm.InstructorId.Value;

            _context.SaveChanges();

            TempData["Success"] = "The course was Updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // 5. DELETE (GET confirmation)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefault(c => c.Id == id);

            if (course == null) return NotFound();

            ViewBag.Warning = "Note: Deleting the course will permanently remove it, along with the associated trainee records.";
            return View(course);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
                TempData["Success"] = "Course deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}