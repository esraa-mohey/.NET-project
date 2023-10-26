using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.Header;
using ERNST.Dto.LineStation;
using ERNST.Dto.Response;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineStationsController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;
        public LineStationsController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetLineStations([FromQuery]UserParam userParam)
        {
            try
            {
                var LineStationDB = await _repo.GetLineStation(userParam);
                var LineStationDBs = _mapper.Map<IEnumerable<GetLineStation>>(LineStationDB);

                var pagination = new
                {
                    CurrentPage = LineStationDB.CurrentPage,
                    PageSize = LineStationDB.PageSize,
                    TotalCount = LineStationDB.TotalCount,
                    TotalPages = LineStationDB.TotalPages
                };

                return Ok(new { LineStationDB = LineStationDBs, Pagination = pagination });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }

        }



        [HttpPost]
        public async Task<IActionResult> AddLineStations(LineStationDto LineStation)
        {
            try
            {
               
                foreach(var item in LineStation.StationsId)
                {
                    LineStations lineStations = new LineStations();
                    lineStations.LinesId = LineStation.LinesId;
                    lineStations.StationsId = item;

                    _repo.Add(lineStations);
                }

                await _repo.SaveAll();

                return Ok(LineStation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLineStations(int id, LineStationUpdate lineStations)
        {
            try
            {
                var lineStationsDB = await _repo.updateLineStation(id);

                var line = await _repo.UpdateLines(lineStations.LinesId);

                line.ArName = lineStations.LineAr;
                line.EnName = lineStations.LineEn;

                var LineStationDBList = await _repo.LineStation(lineStations.LinesId);

                foreach (var items in LineStationDBList)
                {

                    _repo.Delete(items);
                }
               
                foreach (var item in lineStations.StationsId)
                {
                  LineStations lineStationss = new LineStations();
                  lineStationss.LinesId = id;
                  lineStationss.StationsId = item;
                   _repo.Add(lineStationss);
                }

                // lineStationsDB.LinesId = lineStations.LinesId;
                // lineStationsDB.StationsId = lineStations.StationsId;

                await _repo.SaveAll();

                return Ok(lineStationsDB);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("{idLine}/Getstations")]
        public async Task<IActionResult> LineToStations(int idLine)
        {
            try
            {
                var LineStationDB = await _repo.LineStation(idLine);
                var LineStationDBs = _mapper.Map<IEnumerable<GetLineStation>>(LineStationDB);


                return Ok(new { LineStationDB = LineStationDBs});

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }       

        }      

    }       
}