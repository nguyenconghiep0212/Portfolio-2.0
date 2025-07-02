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
using BackEnd.DTO;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IncidentContext _context;

        public IncidentController(IncidentContext context)
        {
            _context = context;
        }

        // GET: api/ListIncident
        public class ListResponse
        {
            public IEnumerable<Incident> Data { get; set; }
            public int From { get; set; }
            public int To { get; set; }
            public int Total { get; set; }
        }
        [HttpPost("List")]
        public async Task<ActionResult<ListResponse>> GetIncidents([FromBody] ListRequest request)
        {
            var query = from incident in _context.Incident
                        join city in _context.City on incident.City_Code equals city.Code.ToString()
                        select new IncidentWithCityNameDto
                        {
                            Id = incident.Id,
                            Name = incident.Name,
                            CityName = city.Name,
                            CityCode = city.Code,
                            Severity = incident.Severity,
                            Description = incident.Description,
                            LatLng = incident.LatLng,
                            Time = incident.Time,
                            Solved = incident.Solved,
                            Dispatcher_Team_Id = incident.Dispatcher_Team_Id
                        };

            if (!string.IsNullOrEmpty(request.name))
                query = query.Where(i => i.Name.Contains(request.name));

            if (request.cityCode != null && request.cityCode.Any())
                query = query.Where(i => request.cityCode.Contains(i.CityCode));

            if (request.severity != null && request.severity.Any())
                query = query.Where(i => request.severity.Contains(i.Severity));

            if (request.date != null)
                query = query.Where(i => i.Time >= request.date.From && i.Time <= request.date.To);

            var totalCount = await query.CountAsync();

            var incidents = await query
                .Skip(request.offset)
                .Take(request.limit)
                .ToListAsync();

            var response = new
            {
                Data = incidents,
                From = request.offset,
                To = request.offset + incidents.Count,
                Total = totalCount
            };

            return Ok(response);
        }

        // GET: api/Incident/5
        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<Incident>> GetIncident(long id)
        {
            var incidents = await _context.Incident.FindAsync(id);

            if (incidents == null)
            {
                return NotFound();
            }

            return incidents;
        }

        // PUT: api/Incident/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> PutIncident(long id, Incident incidents)
        {
            if (id != incidents.Id)
            {
                return BadRequest();
            }

            _context.Entry(incidents).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(id))
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

        // POST: api/Incident
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        public async Task<ActionResult<Incident>> CreateIncident(Incident incidents)
        {
            _context.Incident.Add(incidents);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncident), new { id = incidents.Id }, incidents);
        }

        [HttpPost("CreateMany")]
        public async Task<ActionResult<IEnumerable<Incident>>> CreateIncidentMany(Incident[] incidents)
        {
            // Ensure Id is not set for new incidents
            foreach (Incident item in incidents)
            {
                item.Id = 0;
            }
            _context.Incident.AddRange(incidents);
            await _context.SaveChangesAsync();

            // Optionally, you can return the list of created incidents
            return Ok(incidents);
        }

        // DELETE: api/Incident/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteIncident(long id)
        {
            var incident = await _context.Incident.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            _context.Incident.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncidentExists(long id)
        {
            return _context.Incident.Any(e => e.Id == id);
        }

        // GET: api/Incident/TotalByCity
        [HttpGet("TotalByCity")]
        public async Task<ActionResult<IEnumerable<object>>> GetTotalByCity()
        {
            var result = await (
                from city in _context.City
                join incident in _context.Incident
                    on city.Code equals incident.City_Code into incidentGroup
                select new
                {
                    CityName = city.Name,
                    Total = incidentGroup.Count()
                }
            ).ToListAsync();

            return Ok(result);
        }

        // GET: api/Incident/TotalBySeverity
        [HttpGet("TotalBySeverity")]
        public async Task<ActionResult<IEnumerable<object>>> GetTotalBySeverity()
        {
            var result = await _context.Incident
                .GroupBy(i => i.Severity)
                .Select(g => new
                {
                    Severity = g.Key,
                    Total = g.Count()
                }).ToListAsync();

            return Ok(result);
        }

        // GET: api/Incident/TotalBySolved
        [HttpGet("TotalBySolved")]
        public async Task<ActionResult<IEnumerable<object>>> GetTotalBySolved()
        {
            var result = await _context.Incident
                .GroupBy(i => i.Solved)
                .Select(g => new
                {
                    Solved = g.Key,
                    Total = g.Count()
                }).ToListAsync();

            return Ok(result);
        }
    }
}
