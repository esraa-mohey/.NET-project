using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.Coach;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachesController : ControllerBase
    {
        // Create an object of IErnstRepository interface 
        private readonly IErnstRepository _repo;

        // Create an object of IMapper interface 
        private readonly IMapper _mapper;

        // Initialize the objects
        public CoachesController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        // Purpose: Get Coaches
        // Method: GET
        // Route: /api/coaches
        public async Task<IActionResult> GetCoaches([FromQuery]UserParam userParams)
        {
            try
            {
                // Check if user want coaches in a pagination format
                if (userParams.Pagination)
                {
                    // Get all coaches from database 
                    var coachesForPaginationInDb = await _repo.GetCoaches(userParams);

                    // Map the results in another model
                    var coachesForPagination = _mapper
                        .Map<ICollection<CoachForDisplay>>(coachesForPaginationInDb);

                    // Create pagination object depends on the returned values
                    var pagination = new
                    {
                        CurrentPage = coachesForPaginationInDb.CurrentPage,
                        PageSize = coachesForPaginationInDb.PageSize,
                        TotalCount = coachesForPaginationInDb.TotalCount,
                        TotalPages = coachesForPaginationInDb.TotalPages
                    };

                    // Return the results as json object
                    return Ok(new { Coachs = coachesForPagination, Pagination = pagination });
                }

                // Get all coaches from database 
                var coachesInDb = await _repo.GetCoachesForDropDown();

                // Map the results in another model
                var coaches = _mapper.Map<ICollection<CoachForDisplay>>(coachesInDb);

                // Create pagination object depends on the returned values
                return Ok(new { Coachs = coaches });
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        // Purpose: Create new Class
        // Method: GET
        // Route: /api/coaches
        public async Task<IActionResult> AddCoach(Coach coach)
        {
            try
            {
                // Calculate Seats count by multiply columns count in rows count
                coach.SeatsCount = coach.ColsCount * coach.RowsCount;

                // Create new coach
                _repo.Add(coach);

                // Save changes in the database 
                await _repo.SaveAll();

                // Create pagination object depends on the returned values
                return Ok(coach);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        // Purpose: Update coach
        // Method: PUT
        // Route: /api/coaches/1
        public async Task<IActionResult> UpdateCoach(int id, Coach coach)
        {
            try
            {
                // Get coach by the given id
                var coachInDb = await _repo.GetCoach(id);

                // Override the old values by the new values 
                coachInDb.ArName = coach.ArName;
                coachInDb.EnName = coach.EnName;
                coachInDb.CoachClassId = coach.CoachClassId;
                coachInDb.RowsCount = coach.RowsCount;
                coachInDb.ColsCount = coach.ColsCount;
                coachInDb.SeatsCount = coach.RowsCount * coach.ColsCount;
                coachInDb.SeatsNumber = coach.SeatsNumber;
                coachInDb.CountOfCoaches = coach.CountOfCoaches;

                // Save changes in the database
                await _repo.SaveAll();

                // Return the results as json object
                return Ok(coachInDb);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }

        }


        [HttpPut("{id}/status")]
        // Purpose: Update coach status
        // Method: PUT
        // Route: /api/coaches/1/status
        public async Task<IActionResult> ChangeStatus(int id, CoachForUpdateStatus coachForUpdateStatus)
        {
            try
            {
                // Get coach by the given id
                var coachInDb = await _repo.GetCoach(id);

                // Override the old values by the new values 
                coachInDb.Suspended = coachForUpdateStatus.Status;

                // Save changes in the database
                await _repo.SaveAll();

                // Return the results as json object
                return Ok(coachInDb);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }

        }
    }
}