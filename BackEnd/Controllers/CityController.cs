using BackEnd.Contexts;
using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all actions in this controller

    public class CityController : ControllerBase
    {
        private readonly CityContext _context;

        public CityController(CityContext context)
        {
            _context = context;
        }

        // GET: api/City
        public class CityResponse
        {
            public IEnumerable<City> Data { get; set; }
            public int From { get; set; }
            public int To { get; set; }
            public int Total { get; set; }
        }
        [HttpPost("list")]
        public async Task<ActionResult<CityResponse>> GetCity([FromBody] ListRequest request)
        {
            var City = await _context.City.Skip(request.offset).Take(request.limit).ToListAsync();
            var totalCount = await _context.City.CountAsync();
            var response = new CityResponse
            {
                Data = City,
                From = request.offset,
                To = request.offset + City.Count,
                Total = totalCount
            };
            return response;

        }

        // GET: api/City/5
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<City>> Getcity(long id)
        {
            var city = await _context.City.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/City/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Putcity(long id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cityExists(id))
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

        // POST: api/City
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<City>> Createcity(City city)
        {
            _context.City.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Getcity), new { id = city.Id }, city);
        }

        // DELETE: api/City/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Deletecity(long id)
        {
            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.City.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool cityExists(long id)
        {
            return _context.City.Any(e => e.Id == id);
        }
    }
}
