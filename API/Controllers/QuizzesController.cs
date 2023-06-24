using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/courses/{courseId}/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly CourseDataContext _context;

        public QuizzesController(CourseDataContext context)
        {
            _context = context;
        }

        // GET: api/Quizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes(int courseId)
        {
            return await _context.Quizzes.Where(q => q.CourseId == courseId).ToListAsync();
        }

        // GET: api/Quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);

            if (quiz == null)
            {
                return NotFound("Quiz not found.");
            }

            return quiz;
        }

        // PUT: api/Quizzes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id, Quiz quiz)
        {
            // check if the quiz exists
            var existingQuiz = await _context.Quizzes.FindAsync(id);

            if (existingQuiz == null)
            {
                return NotFound("Quiz not found.");
            }

            // check if the course exists
            var course = await _context.Courses.FindAsync(existingQuiz.CourseId);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // check if the quiz Title already exists
            // var existingQuizTitle = await _context.Quizzes.Where(q => q.Title == quiz.Title && q.CourseId == course.Id).FirstOrDefaultAsync();
            // if (existingQuizTitle != null)
            // {
            //     return BadRequest("Quiz already exists.");
            // }

            // update the quiz
            existingQuiz.Title = quiz.Title;
            existingQuiz.Description = quiz.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
                {
                    return NotFound("Quiz not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingQuiz);
        }

        // POST: api/Quizzes
        [HttpPost]
        public async Task<ActionResult<Quiz>> PostQuiz(int courseId, [FromBody] Quiz quiz)
        {
            var course = await _context.Courses.FindAsync(courseId);

            // check if course exists
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // check if quiz already exists
            var existingQuiz = await _context.Quizzes.Where(q => q.Title == quiz.Title && q.CourseId == courseId).FirstOrDefaultAsync();
            if (existingQuiz != null)
            {
                return BadRequest("Quiz already exists.");
            }

            // set the courseId of the new quiz
            quiz.CourseId = courseId;

            // add quiz to course and save changes
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuiz), new { courseId = courseId, id = quiz.Id }, quiz);
        }

        // DELETE: api/Quizzes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound("Quiz not found.");
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return Ok("Quiz deleted.");
        }

        private bool QuizExists(int id)
        {
            return _context.Quizzes.Any(e => e.Id == id);
        }
    }
}
