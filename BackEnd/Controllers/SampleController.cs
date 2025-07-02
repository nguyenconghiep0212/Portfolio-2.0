using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.Interfaces;
using BackEnd.Contexts;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly SampleContext _context;

        public SampleController(SampleContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        public class SampleResponse
        {
            public IEnumerable<Sample> Data { get; set; }
            public int From { get; set; }
            public int To { get; set; }
            public int Total { get; set; }
        }
        [HttpPost("List")]
        public async Task<ActionResult<SampleResponse>> GetTodoItems([FromBody] ListRequest request)
        {
            var sample = await _context.Sample.Skip(request.offset).Take(request.limit).ToListAsync();
            var totalCount = await _context.Sample.CountAsync();
            var response = new SampleResponse
            {
                Data = sample,
                From = request.offset,
                To = request.offset + sample.Count,
                Total = totalCount
            };
            return response;

        }

        // GET: api/TodoItems/5
        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<Sample>> GetTodoItem(long id)
        {
            var todoItem = await _context.Sample.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> PutTodoItem(long id, Sample todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        public async Task<ActionResult<Sample>> CreateTodoItem(Sample todoItem)
        {
            _context.Sample.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.Sample.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Sample.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.Sample.Any(e => e.Id == id);
        }
    }
}
