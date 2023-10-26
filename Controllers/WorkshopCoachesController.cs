using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.CoachClass;
using ERNST.Dto.WorkshopCoach;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopCoachesController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;

        public WorkshopCoachesController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetWorkshopCoaches([FromQuery]UserParam userParams)
        {
            try
            {
                var workshopCoachesForPaginationInDb = await _repo.GetWorkshopCoaches(userParams);

                var workshopCoachesForPagination = _mapper.Map<ICollection<WorkshopCoachForDisplay>>(workshopCoachesForPaginationInDb);

                var pagination = new
                {
                    CurrentPage = workshopCoachesForPaginationInDb.CurrentPage,
                    PageSize = workshopCoachesForPaginationInDb.PageSize,
                    TotalCount = workshopCoachesForPaginationInDb.TotalCount,
                    TotalPages = workshopCoachesForPaginationInDb.TotalPages
                };

                return Ok(new { WorkshopCoaches = workshopCoachesForPagination, Pagination = pagination });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddWorkshopCoach(WorkshopCoach workshopCoach)
        {
            try
            {
                _repo.Add(workshopCoach);

                await _repo.SaveAll();

                return Ok(workshopCoach);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkshopCoach(int id, WorkshopCoach workshopCoach)
        {
            try
            {
                var workshopCoachInDb = await _repo.GetWorkshopCoach(id);

                workshopCoachInDb.CoachId = workshopCoach.CoachId;
                workshopCoachInDb.Available = workshopCoach.Available;
                workshopCoachInDb.InMaintenance = workshopCoach.InMaintenance;
                workshopCoachInDb.InUse = workshopCoach.InUse;

                await _repo.SaveAll();

                return Ok(workshopCoachInDb);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}