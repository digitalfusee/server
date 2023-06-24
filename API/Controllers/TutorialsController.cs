using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/courses/{courseId}/[controller]")]
    [ApiController]
    public class TutorialsController : ControllerBase
    {
        private readonly CourseDataContext _context;

        public TutorialsController(CourseDataContext context)
        {
            _context = context;
        }

        // GET: api/Tutorials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tutorial>>> GetTutorials(int courseId)
        {
            return await _context.Tutorials.Where(m => m.CourseId == courseId).ToListAsync();
        }

        // GET: api/Tutorials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tutorial>> GetTutorial(int id)
        {
            var tutorial = await _context.Tutorials.FindAsync(id);

            if (tutorial == null)
            {
                return NotFound("Tutorial not found.");
            }

            return tutorial;
        }

        // PUT: api/Tutorials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTutorial(int id, Tutorial tutorial)
        {
            // check if the course exists
            var course = await _context.Courses.FindAsync(tutorial.CourseId);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // check if the tutorial exists
            var existingTutorial = await _context.Tutorials.FindAsync(id);

            if (existingTutorial == null)
            {
                return NotFound("Tutorial not found.");
            }

            // check if the tutorial title already exists
            var existingTutorialTitle = await _context.Tutorials.Where(m => m.CourseId == tutorial.CourseId && m.Title == tutorial.Title).FirstOrDefaultAsync();
            if (existingTutorialTitle != null && existingTutorialTitle.Id != id)
            {
                return BadRequest("Tutorial already exists.");
            }

            // update the tutorial
            existingTutorial.Title = tutorial.Title;
            existingTutorial.Description = tutorial.Description;
            existingTutorial.CourseId = tutorial.CourseId;
            existingTutorial.VideoURL = tutorial.VideoURL;
            existingTutorial.Steps = tutorial.Steps;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TutorialExists(id))
                {
                    return NotFound("Tutorial not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingTutorial);
        }

        // POST: api/Tutorials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tutorial>> PostTutorial(int courseId, [FromBody] Tutorial tutorial)
        {
            // check if course exists
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // check if tutorial exists
            var existingTutorial = await _context.Tutorials.Where(m => m.CourseId == courseId && m.Title == tutorial.Title).FirstOrDefaultAsync();
            if (existingTutorial != null)
            {
                return BadRequest("Tutorial already exists.");
            }

            // set the courseId for the new tutorial
            tutorial.CourseId = courseId;

            // add tutorial to course and save changes
            _context.Tutorials.Add(tutorial);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTutorial), new { courseId, id = tutorial.Id }, tutorial);
        }

        // DELETE: api/Tutorials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutorial(int id)
        {
            var tutorial = await _context.Tutorials.FindAsync(id);
            if (tutorial == null)
            {
                return NotFound("Tutorial not found.");
            }

            _context.Tutorials.Remove(tutorial);
            await _context.SaveChangesAsync();

            return Ok("Tutorial deleted.");
        }

        private bool TutorialExists(int id)
        {
            return _context.Tutorials.Any(e => e.Id == id);
        }
    }
}
