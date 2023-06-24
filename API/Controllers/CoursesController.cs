using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CourseDataContext _context;

        public CoursesController(CourseDataContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound("Course not found.");
            }

            return course;
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            var existingCourse = await _context.Courses.FindAsync(id);

            if (existingCourse == null)
            {
                return NotFound("Course not found.");
            }

            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            existingCourse.ImageURL = course.ImageURL;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingCourse);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            // check if course already exists
            var existingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.Title == course.Title);

            if (existingCourse != null)
            {
                return BadRequest("Course already exists.");
            }

            // check if image is null
            if (course.ImageURL == null)
            {
                return BadRequest("ImageURL is required.");
            }

            // check if image is valid
            if (!course.ImageURL.Contains("http"))
            {
                return BadRequest("ImageURL is not valid.");
            }

            // check if instructor exists
            var instructor = await _context.Instructors.FindAsync(course.InstructorId);

            if (instructor == null)
            {
                return BadRequest("Instructor not found.");
            }

            course.Instructor = instructor;
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }


        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok("Course deleted.");
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
