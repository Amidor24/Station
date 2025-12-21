using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly HttpClient _httpClient;


        public StationsController(AppDBContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // GET: api/Stations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetStation()
        {
            return await _context.Station.ToListAsync();
        }

        // GET: api/Stations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> GetStation(Guid id)
        {
            var station = await _context.Station.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }

            return station;
        }

        // PUT: api/Stations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStation(Guid id, Station station)
        {
            if (id != station.Id)
            {
                return BadRequest();
            }

            _context.Entry(station).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(id))
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

        // POST: api/Stations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Station>> PostStation(Station station)
        {
            _context.Station.Add(station);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStation", new { id = station.Id }, station);
        }

        // DELETE: api/Stations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStation(Guid id)
        {
            var station = await _context.Station.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }

            _context.Station.Remove(station);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StationExists(Guid id)
        {
            return _context.Station.Any(e => e.Id == id);
        }

        [HttpPost("rain-data")]
        public async Task<IActionResult> GetRainData()
        {
            var url = "https://c2ai-cloud.com/api/ds/query";

            var bearerToken = "eyJrIjoibFl1V3FzSWJFZjVsc0s4cFVTeVNuMEVTRkY4aU5qdWoiLCJuIjoidHQiLCJpZCI6Mn0=";

            var payload = new
            {
                from = "1735689600",
                to = "1754524800",
                queries = new[]
                {
                    new
                    {
                        datasourceId = 2,
                        format = "table",
                        intervalMs = 86400000,
                        maxDataPoints = 1092,
                        refId = "A",
                        rawSql =
                        "SELECT UNIX_TIMESTAMP(DATE_TIME) DIV 86400 * 86400 AS `time`, " +
                        "avg(MEAS_1) AS `Intensité de Pluie (mm/h)`, " +
                        "avg(MEAS_2) AS `Pluie quotidienne (mm) `, " +
                        "avg(MEAS_3) AS `Pluie Totale (mm)` " +
                        "FROM ap_8503.ed_8543 " +
                        "WHERE DATE_TIME BETWEEN FROM_UNIXTIME(1731682718) AND FROM_UNIXTIME(1762009303) " +
                        "GROUP BY 1 ORDER BY UNIX_TIMESTAMP(DATE_TIME) DIV 86400 * 86400"
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int) response.StatusCode, responseContent);
            }

            return Ok(responseContent);
        }
    }
}
