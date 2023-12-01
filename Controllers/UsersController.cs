using FinanceControlAPI.Data;
using FinanceControlAPI.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceControlAPI.Negócio;

namespace FinanceControlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private Extensao _ = new();
        private readonly FinanceControlContext _financeContext;
        public UsersController(FinanceControlContext financeContext) { _financeContext = financeContext; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _financeContext.Users.ToListAsync();
        }
        [HttpGet("UserID")]
        public async Task<ActionResult<int>> GetLastId()
        {
            var financeId = _financeContext.Users.OrderByDescending(e => e.Id).Select(e => e.Id).FirstOrDefault();

            if (financeId == null)
            {
                return NotFound();
            }

            return financeId;
        }
        [HttpGet("password")]
        public async Task<ActionResult<bool>> GetPasswordByEmail([FromQuery] string email)
        {
            var user = await _financeContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("Email não encontrado");
            }
            return Ok(await _.SendEmail(user, false));
            return true;
        }
        [HttpGet("Email")]
        public async Task<ActionResult<int>> CheckUserByEmail([FromQuery] string email)
        {
            var user = await _financeContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return Ok();
            }

            return Unauthorized();
        }
        [HttpGet("login")]
        public async Task<ActionResult<Users>> GetUserByLogin([FromQuery] string email, [FromQuery] string password)
        {
           var user = await _financeContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        
        [HttpPost]
        public async Task<ActionResult<Users>> AddUser(Users users)
        {
            if( await _financeContext.Users.FirstOrDefaultAsync(u => u.Email == users.Email) != null)
            {
                return Unauthorized("Email já cadastrado.");
            }
            _financeContext.Users.Add(users);
            await _financeContext.SaveChangesAsync();
            await _.SendEmail(users, true);
            return CreatedAtAction(nameof(GetUsers), new { id = users.Id }, users);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Users updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }
            _financeContext.Entry(updatedUser).State = EntityState.Modified;
            try
            {
                await _financeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_financeContext.Users.Any(e => e.Id == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _financeContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _financeContext.Users.Remove(user);
            await _financeContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
