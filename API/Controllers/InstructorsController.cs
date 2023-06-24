using Data.Contexts;
using Data.Models.Domain;
using Data.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly CourseDataContext _context;
        public InstructorsController(CourseDataContext context)
        {
            _context = context;
        }

        // GET: api/Instructors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructors()
        {
            return await _context.Instructors.ToListAsync();
        }

        // GET: api/Instructors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetInstructor(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            return instructor;
        }

        // PUT: api/Instructors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructor(int id, Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return BadRequest();
            }

            _context.Entry(instructor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Instructors
        //[HttpPost]
        //public async Task<ActionResult<Instructor>> PostInstructor(Instructor instructor)
        //{
        //    _context.Instructors.Add(instructor);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetInstructor", new { id = instructor.Id }, instructor);
        //}

        // DELETE: api/Instructors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(InstructorRegisterRequest request)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass all the required fields";
                return Ok(status);
            }

            // if (_context.Instructors.Any(u => u.Email == request.Email))
            if (_context.Instructors.Any(u => u.Username == request.Username))
            {
                status.StatusCode = 0;
                status.Message = "Invalid username.";
                return Ok(status);
            }

            CreatePasswordHash(request.Password,
                 out byte[] passwordHash,
                 out byte[] passwordSalt);

            var lastId = _context.Instructors.Any() ? _context.Instructors.Max(i => i.Id) : 0;


            var instructor = new Instructor
            {
                Id = lastId + 1,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                Province = request.Province,
                PostalCode = request.PostalCode,
                Country = request.Country,
                ProfilePicture = request.ProfilePicture,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                VerificationToken = CreateRandomToken()
            };

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
            status.StatusCode = 1;
            status.Message = "Instructor successfully registered! :D";
            return Ok(status);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(InstructorLoginRequest request)
        {
            //var instructor = await _context.Instructors.FirstOrDefaultAsync(u => u.Email == request.Email);
            var instructor = await _context.Instructors.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (instructor == null)
            {
                return Ok(new InstructorLoginResponse
                {
                    StatusCode = 0,
                    Message = "Instructor not found.",
                });
            }

            if (!VerifyPasswordHash(request.Password, instructor.PasswordHash, instructor.PasswordSalt))
            {
                return Ok(new InstructorLoginResponse
                {
                    StatusCode = 0,
                    Message = "Invalid password.",
                });
            }

            //if (instructor.VerifiedAt == null)
            //{
            //    return BadRequest("Not verified!");
            //}

            return Ok(new InstructorLoginResponse
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Username = instructor.Username,
                Email = instructor.Email,
                ProfilePicture = instructor.ProfilePicture,
                PhoneNumber = instructor.PhoneNumber,
                Address = instructor.Address,
                City = instructor.City,
                Province = instructor.Province,
                PostalCode = instructor.PostalCode,
                Country = instructor.Country,
                IsInstructor = instructor.IsInstructor,
                StatusCode = 1,
                Message = "Instructor successfully logged in! :D",
            });
        }

        // [HttpPost("verify")]
        // public async Task<IActionResult> Verify(string token)
        // {
        //     var instructor = await _context.Instructors.FirstOrDefaultAsync(u => u.VerificationToken == token);
        //     if (instructor == null)
        //     {
        //         return BadRequest("Invalid token.");
        //     }

        //     instructor.VerifiedAt = DateTime.Now;
        //     await _context.SaveChangesAsync();

        //     return Ok("Instructor verified! :)");
        // }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string username)
        {
            //var instructor = await _context.Instructors.FirstOrDefaultAsync(u => u.Email == email);
            var instructor = await _context.Instructors.FirstOrDefaultAsync(u => u.Username == username);
            if (instructor == null)
            {
                return BadRequest("Instructor not found.");
            }

            instructor.PasswordResetToken = CreateRandomToken();
            instructor.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return Ok("You may now reset your password.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var instructor = await _context.Instructors.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (instructor == null || instructor.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            instructor.PasswordHash = passwordHash;
            instructor.PasswordSalt = passwordSalt;
            instructor.PasswordResetToken = null;
            instructor.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password successfully reset. :)");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

    }
}
