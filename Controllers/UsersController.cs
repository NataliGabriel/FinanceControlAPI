using FinanceControlAPI.Data;
using FinanceControlAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceControlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
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
        [HttpGet("Email")]
        public async Task<ActionResult<int>> GetUserByEmail([FromQuery] string email)
        {
            var user = await _financeContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound(); 
            }

            return user.Id;
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
            _financeContext.Users.Add(users);
            await _financeContext.SaveChangesAsync();

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
