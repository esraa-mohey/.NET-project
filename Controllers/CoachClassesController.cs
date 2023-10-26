using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.CoachClass;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachClassesController : ControllerBase
    {
        // Create an object of IErnstRepository interface 
        private readonly IErnstRepository _repo;

        // Create an object of IMapper interface 
        private readonly IMapper _mapper;

        // Initialize the objects
        public CoachClassesController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
       

        [HttpGet]
        // Purpose: Get Coach Classes
        // Method: GET
        // Route: /api/coachClasses
        public async Task<IActionResult> GetCoachClasses([FromQuery]UserParam userParams)
        {
            try
            {
                // Check if user want coach classes as a pagination format
                if (userParams.Pagination)
                {
                    // Get all coach classes from database 
                    var coachClassesForPaginationInDb = await _repo.GetCoachClasses(userParams);

                    // Map the results in another model
                    var coachClassesForPagination = _mapper
                        .Map<ICollection<CoachClassForDisplay>>(coachClassesForPaginationInDb);

                    // Create pagination object depends on the returned values
                    var pagination = new
                    {
                        CurrentPage = coachClassesForPaginationInDb.CurrentPage,
                        PageSize = coachClassesForPaginationInDb.PageSize,
                        TotalCount = coachClassesForPaginationInDb.TotalCount,
                        TotalPages = coachClassesForPaginationInDb.TotalPages
                    };

                    // Return the results as json object
                    return Ok(new { CoachClasses = coachClassesForPagination, Pagination = pagination });
                }

                // Get all coach classes from database 
                var coachClassesInDb = await _repo.GetCoachClassesForDropDown();

                // Map the results in another model
                var coachClasses = _mapper.Map<ICollection<CoachClassForDisplay>>(coachClassesInDb);

                // Create pagination object depends on the returned values
                return Ok(new { CoachClasses = coachClasses });
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
        // Route: /api/coachClasses
        public async Task<IActionResult> AddCoachClass(CoachClass coachClass)
        {
            try
            {
                // Create new coach class
                _repo.Add(coachClass);

                // Save changes in the database 
                await _repo.SaveAll();

                // Create pagination object depends on the returned values
                return Ok(coachClass);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        // Purpose: Update coach class
        // Method: PUT
        // Route: /api/coachClasses/1
        public async Task<IActionResult> UpdateCoachClasses(int id, CoachClass coachClass)
        {
            try
            {
                // Get train by the given id
                var coachClassInDb = await _repo.GetCoachClass(id);

                // Override the old values by the new values 
                coachClassInDb.ArName = coachClass.ArName;
                coachClassInDb.EnName = coachClass.EnName;
                coachClassInDb.Suspended = coachClass.Suspended;

                // Save changes in the database
                await _repo.SaveAll();

                // Return the results as json object
                return Ok(coachClassInDb);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
        }
    }
}