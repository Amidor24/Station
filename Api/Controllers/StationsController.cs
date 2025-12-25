using Api.Dtos;
using Application.Commands;
using Application.DTOs;
using Application.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController(HttpClient httpClient, IConfiguration configuration, IMediator mediator) : ControllerBase
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetStation()
        {
            var query = new GetAllStationsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStation(Guid id)
        {
            try
            {
                StationDto stationDto = await _mediator.Send(new GetStationByIdQuery(id));

                return stationDto == null ?  NotFound("Oops!!! Identifiant not correct.") :  Ok(stationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStation(Guid id, Station station)
        {
            try
            {
                return Ok((StationDto?) await _mediator.Send(new UpdateStationCommand(id, station.Name, station.Reference)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Station>> PostStation(Station station)
        {
            try
            {
                StationDto result = await _mediator.Send(new CreateStationCommand(station.Name, station.Reference));
                return CreatedAtAction("GetStation", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStation(Guid id)
        {
            try
            {
                bool result = await _mediator.Send(new DeleteStationCommand(id));

                return result ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Climate")]
        public async Task<IActionResult> GetRainData(GetClimate climate)
        {
            string url = "https://c2ai-cloud.com/api/ds/query";

            string? get = _configuration["Grafana:value"];

            string StartedAt = ToUnixTimestamp(climate.StartedAt).ToString();

            string EndedtAt = ToUnixTimestamp(climate.EndedtAt.AddDays(1)).ToString();

            string periode = climate.Periode.ToString() + "*" + climate.Periode.ToString(); // 86400 * 86400

            var payload = new
            {
                StartedAt,
                EndedtAt,
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
                        "SELECT UNIX_TIMESTAMP(DATE_TIME) DIV "+periode+" AS `time`, " +
                        "avg(MEAS_1) AS `Intensité de Pluie (mm/h)`, " +
                        "avg(MEAS_2) AS `Pluie quotidienne (mm) `, " +
                        "avg(MEAS_3) AS `Pluie Totale (mm)` " +
                        "FROM ap_8503.ed_8543 " +
                        "WHERE DATE_TIME BETWEEN FROM_UNIXTIME("+StartedAt+") AND FROM_UNIXTIME("+EndedtAt+") " +
                        "GROUP BY 1 ORDER BY UNIX_TIMESTAMP(DATE_TIME) DIV "+climate.Periode.ToString()+" * "+climate.Periode.ToString()+""
                    }
                }
            };


            string json = JsonSerializer.Serialize(payload);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", get);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int) response.StatusCode, responseContent);
            }

            // Deserialize Grafana response
            GrafanaResponse? grafanaResponse = JsonSerializer.Deserialize<GrafanaResponse>(responseContent);

            if (grafanaResponse?.Results?.A?.Frames?.Any() != true)
            {
                return NotFound("No data returned from Grafana.");
            }

            Frame frame = grafanaResponse.Results.A.Frames.First();

            int rowCount = frame.Data.Values[0].Count;

            // Map all rows to RainMeasurement objects
            List<GetClimateResponse> climateResponses = new(rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                climateResponses.Add(new GetClimateResponse
                {
                    Date = DateTimeOffset
                    .FromUnixTimeMilliseconds(Convert.ToInt64(frame.Data.Values[0][i].ToString()))
                    .UtcDateTime,
                    Intensity = Math.Round(Convert.ToDouble(frame.Data.Values[1][i].ToString()), 4),
                    DailyRain = Math.Round(Convert.ToDouble(frame.Data.Values[2][i].ToString()), 4),
                    TotalRain = Math.Round(Convert.ToDouble(frame.Data.Values[3][i].ToString()), 4)
                });
            }

            return Ok(climateResponses);
        }

        public static long ToUnixTimestamp(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Local)
            {
                dateTime = dateTime.ToUniversalTime();
            }
            else if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }

            DateTime unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long) (dateTime - unixEpoch).TotalSeconds;
        }
    }
}
