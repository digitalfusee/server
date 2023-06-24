using Data.Contexts;
using Data.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/courses/{courseId}/quizzes/{quizId}/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly CourseDataContext _context;

        public QuestionsController(CourseDataContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions(int quizId)
        {
            return await _context.Questions.Where(q => q.QuizId == quizId).ToListAsync();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound("Question not found.");
            }

            return question;
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            // check if the question exists
            var existingQuestion = await _context.Questions.FindAsync(id);

            if (existingQuestion == null)
            {
                return NotFound("Question not found.");
            }

            // check if the quiz exists
            var quiz = await _context.Quizzes.FindAsync(existingQuestion.QuizId);
            if (quiz == null)
            {
                return NotFound("Quiz not found.");
            }

            // update the question
            existingQuestion.QuestionText = question.QuestionText;
            existingQuestion.Choices = question.Choices;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound("Question not found.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingQuestion);
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(int courseId, int tutorialId, int quizId, [FromBody] Question question)
        {
            // check if the quiz exists
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null)
            {
                return NotFound("Quiz not found.");
            }

            // check if the question already exists
            var questionExists = await _context.Questions.AnyAsync(q => q.QuestionText == question.QuestionText && q.QuizId == quiz.Id);
            if (questionExists)
            {
                return BadRequest("Question already exists.");
            }

            // check if the choices are valid
            var choices = question.Choices;
            if (choices == null || choices.Count < 2)
            {
                return BadRequest("Question must have at least 2 choices.");
            }

            // check if isCorrect is set for at least one choice
            var isCorrectSet = false;
            foreach (var choice in choices)
            {
                if (choice.IsCorrect)
                {
                    isCorrectSet = true;
                    break;
                }
            }

            if (!isCorrectSet)
            {
                return BadRequest("At least one choice must be correct.");
            }


            // create the question
            var newQuestion = new Question
            {
                Id = _context.Questions.Count() + 1,
                QuestionText = question.QuestionText,
                Choices = question.Choices,
                QuizId = quiz.Id
            };

            _context.Questions.Add(newQuestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestion), new { courseId = courseId, tutorialId, quizId, id = newQuestion.Id }, newQuestion);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound("Question not found.");
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return Ok("Question deleted.");
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
