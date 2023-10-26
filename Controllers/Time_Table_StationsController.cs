using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.Time_Table_Stations;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Time_Table_StationsController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;

        public Time_Table_StationsController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTime_Table_StationsController([FromQuery]UserParam userParam)
        {
            try
            {
                var stationInDb = await _repo.GetTime_Table_Station(userParam);
                var stations = _mapper.Map<IEnumerable<Time_Table_StationForDisplay>>(stationInDb);

                var pagination = new
                {
                    CurrentPage = stationInDb.CurrentPage,
                    PageSize = stationInDb.PageSize,
                    TotalCount = stationInDb.TotalCount,
                    TotalPages = stationInDb.TotalPages
                };

                return Ok(new { Time_Table_Stations = stations, Pagination = pagination });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Time_Table_Station(Add_time_table Time_Table)
        {
            try
            {

                foreach (var item in Time_Table.time_Table_StationForDisplays)
                {
                    Time_Tabale_Stations time_Table_Stations = new Time_Tabale_Stations();
                    time_Table_Stations.ScheduleId = Time_Table.Time_Table_Id;
                    time_Table_Stations.Arrival_time = item.Arrival_time;
                    time_Table_Stations.Departure_time = item.Departure_time;
                    time_Table_Stations.LineStationsId = item.LineStationsId;
                    time_Table_Stations.Dci = item.Dci;
                    _repo.Add(time_Table_Stations);
                }
                await _repo.SaveAll();

                return Ok(Time_Table);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Time_Table_Station(int id, [FromBody]JArray compositionRecords)
        {
            try
            {
                var lineRequestDb =  _repo.GetTimeTableStationForScheduleOnUpdate(id);

                foreach (var item in lineRequestDb)
                {
                    _repo.Delete(item);
                }

                await _repo.SaveAll();

                var newlineRequest = (List<TimeTableStationForUpdate>)JsonConvert.DeserializeObject(compositionRecords.ToString(), typeof(List<TimeTableStationForUpdate>));
                newlineRequest.Reverse();
                foreach (var item2 in newlineRequest)
                {
                    var tempItem = new Time_Tabale_Stations()
                    {
                        ScheduleId = id,
                        LineStationsId = item2.LineStationsId,
                        Departure_time = item2.Departure_time,
                        Arrival_time = item2.Arrival_time   ,
                        Dci = item2.Dci,
                    };

                    _repo.Add(tempItem);
                }

                await _repo.SaveAll();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
    }
}