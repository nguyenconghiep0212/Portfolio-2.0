using BackEnd.Contexts;
using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BackEnd.Controllers.CityController;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoiController : ControllerBase
    {
        private readonly PoiContext _context;

        public PoiController(PoiContext context)
        {
            _context = context;
        }

        // POST/List
        public class PoiResponse
        {
            public IEnumerable<Poi> Data { get; set; }
            public int From { get; set; }
            public int To { get; set; }
            public int Total { get; set; }
        }
        [HttpPost("List")]
        public async Task<ActionResult<PoiResponse>> GetAll([FromBody] ListRequest request)
        {
            var data = await _context.Poi.Skip(request.offset).Take(request.limit).ToListAsync();
            var totalCount = await _context.Poi.CountAsync();
            var response = new PoiResponse
            {
                Data = data,
                From = request.offset,
                To = request.offset + data.Count,
                Total = totalCount
            };
            return response;
        }

        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<Poi>> Get(int id)
        {
            var poi = await _context.Poi.FindAsync(id);
            if (poi == null)
                return NotFound();
            return poi;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Poi>> Create(Poi poi)
        {
            _context.Poi.Add(poi);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = poi.Id }, poi);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, Poi poi)
        {
            if (id != poi.Id)
                return BadRequest();

            _context.Entry(poi).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Poi.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var poi = await _context.Poi.FindAsync(id);
            if (poi == null)
                return NotFound();

            _context.Poi.Remove(poi);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}