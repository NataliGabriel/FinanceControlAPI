using FinanceControlAPI.Data;
using FinanceControlAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceControlAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly FinanceControlContext? _context;

        public FinanceController(FinanceControlContext financeContext) { _context = financeContext; }

        [HttpGet("FinanceID")]
        public async Task<ActionResult<int>> GetLastId()
        {
            var financeId = _context.Finance.OrderByDescending(e => e.Id).Select(e => e.Id).FirstOrDefault();

            if (financeId == null)
            {
                return NotFound();
            }

            return financeId;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Finance>> Getfinance(int id)
        {
            var finance = await _context.Finance.FindAsync(id);

            if (finance == null)
            {
                return NotFound();
            }

            return finance;
        }
        [HttpGet("ListFinances")]
        public async Task<ActionResult<IEnumerable<Finance>>> GetFinancesByUser([FromQuery] int idUser)
        {
            var financeByUser = await _context.Finance.Where(f => f.UserId == idUser).ToListAsync();
            //var financeByUser = await _context.Users.Where(c => c.Id == idUser).Select(c => c.Finances).FirstOrDefaultAsync();

            if (financeByUser == null)
            {
                return NotFound();
            }

            return Ok(financeByUser);
        }
        [HttpGet("ListFinancesByMonth")]
        public async Task<ActionResult<IEnumerable<Finance>>> GetFinancesByMonth([FromQuery] int month, [FromQuery] int year, [FromQuery] int id)
        {
            var financeByUser = await _context.Finance.Where(f => f.Date.Month == month && f.Date.Year == year).Where(f => f.Id == id).ToListAsync();
            //var financeByUser = await _context.Users.Where(c => c.Id == idUser).Select(c => c.Finances).FirstOrDefaultAsync();

            if (financeByUser == null)
            {
                return NotFound();
            }

            return Ok(financeByUser);
        }
        [HttpPost]
        public async Task<ActionResult<Finance>> AddFinance(Finance finance)
        {
            _context.Finance.Add(finance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Getfinance), new { id = finance.Id }, finance);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateFinance(int id, [FromBody] Finance updatedFinance)
        {
            if (id != updatedFinance.Id)
            {
                return BadRequest();
            }
            _context.Entry(updatedFinance).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Finance.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteFinance(int id)
        {
            var finance = await _context.Finance.FindAsync(id);
            if (finance == null)
            {
                return NotFound();
            }

            _context.Finance.Remove(finance);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
