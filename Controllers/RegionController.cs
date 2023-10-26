 using AutoMapper;
using Dating.Data;
using ERNST.Dto.Region;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;
        //  private int IdIdentifer;
        // private static string ErrorMessageForID = "this Id is already been used ";
        // private int number;

        public RegionController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegion([FromQuery]UserParam userParam)
        {
            try
            {
                if (userParam.Pagination)
                {
                    var RegionInDB = await _repo.GetRegion(userParam);
                    var Region = _mapper.Map<ICollection<RegionForDisplay>>(RegionInDB);

                    var pagination = new
                    {
                        CurrentPage = RegionInDB.CurrentPage,
                        PageSize = RegionInDB.PageSize,
                        TotalCount = RegionInDB.TotalCount,
                        TotalPages = RegionInDB.TotalPages
                    };
                    return Ok(new { Regions = Region, Pagination = pagination });
                }
                var regionInDb = await _repo.GetRegionForDropDown(userParam);

                var region = _mapper.Map<ICollection<RegionForDisplay>>(regionInDb);
                return Ok(new { Region = region });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        [HttpGet("{idRegion}")]
        public async Task<IActionResult> GetRegionByID(int idRegion)
        {
            try
            {
                var RegionInDB = await _repo.GetRegion(idRegion);
                var Region = _mapper.Map<ICollection<RegionForDisplay>>(RegionInDB);



                return Ok(new { Stat = Region });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            //
        }


        [HttpPost]
        public async Task<IActionResult> AddRegion(Region region)
        {
            try
            {
                var isExist = await _repo.CheckRegionId(region.Id);

                if (isExist) return BadRequest("This Id is already exist");

                _repo.Add(region);

                await _repo.SaveAll();

                return Ok(region);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegion(int id, Region Region)
        {
            try
            {
                var RegionInDB = await _repo.GetRegion(id);

                RegionInDB.Id = RegionInDB.Id;
                RegionInDB.EnName = Region.EnName;
                RegionInDB.ArName = Region.ArName;

                await _repo.SaveAll();

                return Ok(RegionInDB);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}