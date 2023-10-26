using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.GovRegion;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovRegionController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;

        public GovRegionController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetGovRegion([FromQuery]UserParam userParam)
        {
            try
            {
                var Gov_RegionsInDB = await _repo.GetGovRegion(userParam);
                var Gov_Regions = _mapper.Map<ICollection<GovRegionForDisplay>>(Gov_RegionsInDB);

                var pagination = new
                {
                    CurrentPage = Gov_RegionsInDB.CurrentPage,
                    PageSize = Gov_RegionsInDB.PageSize,
                    TotalCount = Gov_RegionsInDB.TotalCount,
                    TotalPages = Gov_RegionsInDB.TotalPages
                };
                return Ok(new { GovRegion = Gov_Regions, Pagination = pagination });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddGovRegion(GovRegDTO GovRegion)
        {
            try
            {
                var isExist = await _repo.CheckGovRegionId(GovRegion.Id);

                if (isExist) return BadRequest("This Id is already exist");

                Gov_Regions gov_Regions = new Gov_Regions();
                gov_Regions.Id = GovRegion.Id;
                gov_Regions.GovernorateId = GovRegion.GovernorateId;
                gov_Regions.RegionId = GovRegion.RegionId;

                _repo.Add(gov_Regions);

                await _repo.SaveAll();

                return Ok(GovRegion);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGovRegion(int id, Gov_Regions GovRegion)
        {
            try
            {
                var Gov_RegionsInDB = await _repo.GetGovRegion(id);

                Gov_RegionsInDB.GovernorateId = GovRegion.GovernorateId;
                Gov_RegionsInDB.RegionId = GovRegion.RegionId;
               /*Gov_RegionsInDB.GovarName = GovRegion.GovarName;
                Gov_RegionsInDB.GovenName = GovRegion.GovenName; */

                await _repo.SaveAll();

                return Ok(Gov_RegionsInDB);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("{idRegion}/GetAllGoverment")]

        public async Task<IActionResult> GetAllGovRegion(int idRegion)
        {
            try
            {
                var Gov_RegionsInDB = await _repo.GetGovRegions(idRegion);
                var Gov_Regions = _mapper.Map<ICollection<GovRegionForDisplay>>(Gov_RegionsInDB);



                return Ok(new { AllGov = Gov_Regions });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            //
        }
    }

}