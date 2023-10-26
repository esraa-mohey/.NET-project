using AutoMapper;
using Dating.Data;
using ERNST.Dto.Stations;
using ERNST.Helper;
using ERNST.Model;
//using System.Object;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;
        // private int IdIdentifer;
        // private static string ErrorMessageForID = "this Id is already been used ";
        //private int number;
        //
        public StationController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetStations([FromQuery]UserParam userParam)
        {
            try
            {
                if (userParam.Pagination)
                {
                    var StationInDB = await _repo.GetStations(userParam);
                    var stations = _mapper.Map<IEnumerable<StationsForDisplay>>(StationInDB);

                    var pagination = new
                    {
                        CurrentPage = StationInDB.CurrentPage,
                        PageSize = StationInDB.PageSize,
                        TotalCount = StationInDB.TotalCount,
                        TotalPages = StationInDB.TotalPages
                    };

                    return Ok(new { Stations = stations, Pagination = pagination });

                }
                else
                {
                    var stations = await _repo.GetStationsDropdown(userParam);
                    return Ok(new { Stations = stations });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        [HttpGet("{OprationCodeStation}")]
        public async Task<IActionResult> GetStationsByOC(int OcStation)
        {
            try
            {
                var StationInDB = await _repo.GetStationsOC(OcStation);
                var Station = _mapper.Map<ICollection<StationsForDisplay>>(StationInDB);



                return Ok(new { Stat = Station });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            //
        }

        [HttpGet("{idStation}")]
        public async Task<IActionResult> GetStationsByID(int idStation)
        {
            try
            {
                var StationInDB = await _repo.GetStationsID(idStation);
                var Station = _mapper.Map<ICollection<StationsForDisplay>>(StationInDB);



                return Ok(new { Stat = Station });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            //
        }



        [HttpPost]
        public async Task<IActionResult> AddStations(StationRequest stationRequest)
        {
            try
            {
                var isExist = await _repo.CheckStationId(stationRequest.Id);

                if (isExist) return BadRequest("This Id is already exist");

                Stations station = new Stations();
                var govInDb = await _repo.GetGovernorate(stationRequest.GovernorateId);
                var regInDb = await _repo.GetRegion(stationRequest.RegionId);

                var gov_region = await _repo.GetGovRegionitem(govInDb.Id, regInDb.Id);

                station.GovRegionsId = gov_region.Id;

                station.Id = stationRequest.Id;
                station.OperationCode = stationRequest.OperationCode;
                station.ArName = stationRequest.ArName;
                station.EnName = stationRequest.EnName;

                _repo.Add(station);

                await _repo.SaveAll();

                return Ok(station);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStations(int id, StationRequest station)
        {
            try
            {
                var StationInDB = await _repo.GetStationsID(id);
                var GovInDB = await _repo.GetGovernorate(station.GovernorateId);
                var RegInDB = await _repo.GetRegion(station.RegionId);

                var govregon = await _repo.GetGovRegionitem(GovInDB.Id, RegInDB.Id);

                StationInDB.Id = StationInDB.Id;
                StationInDB.OperationCode = StationInDB.OperationCode;
                StationInDB.EnName = station.EnName;
                StationInDB.ArName = station.ArName;
                StationInDB.GovRegionsId = govregon.Id;

                await _repo.SaveAll();

                return Ok(StationInDB);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}