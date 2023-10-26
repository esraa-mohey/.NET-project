using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.TrainType;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainTypesController : ControllerBase
    {
        // Create an object of IErnstRepository interface 
        private readonly IErnstRepository _repo;

        // Create an object of IMapper interface 
        private readonly IMapper _mapper;

        // Initialize the objects
        public TrainTypesController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        
        [HttpGet]
        // Purpose: Get Train Types
        // Method: GET
        // Route: /api/trainTypes
        public async Task<IActionResult> GetTrainTypes([FromQuery]UserParam userParam)
        {
            try
            {
                // Check if user want train types in a pagination format
                if (userParam.Pagination)
                {
                    // Get all train types from database 
                    var trainTypesForPaginationInDb = await _repo.GetTrainTypes(userParam);

                    // Map the results in another model
                    var trainTypesForPagination = _mapper
                        .Map<IEnumerable<TrainTypeForDisplay>>(trainTypesForPaginationInDb);

                    // Create pagination object depends on the returned values
                    var pagination = new
                    {
                        CurrentPage = trainTypesForPaginationInDb.CurrentPage,
                        PageSize = trainTypesForPaginationInDb.PageSize,
                        TotalCount = trainTypesForPaginationInDb.TotalCount,
                        TotalPages = trainTypesForPaginationInDb.TotalPages
                    };

                    // Return the results as json object
                    return Ok(new { TrainTypes = trainTypesForPagination, Pagination = pagination });
                }

                // Get all train types from database 
                var trainTypesInDb = await _repo.GetTrainTypesForDropDown();

                // Map the results in another model
                var trainTypes = _mapper.Map<IEnumerable<TrainTypeForDisplay>>(trainTypesInDb);

                // Return the results as json object
                return Ok(new { TrainTypes = trainTypes });
            }
            catch(Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
            
        }

        [HttpPost]
        // Purpose: Create new train type
        // Method: POST
        // Route: /api/trainTypes
        public async Task<IActionResult> AddTrainType(TrainType trainType)
        {
            try
            {
                // Create new train type 
                _repo.Add(trainType);

                // Save changes in the database 
                await _repo.SaveAll();

                // Return the results as json object
                return Ok(trainType);
            }
            catch(Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        // Purpose: Update train type
        // Method: PUT
        // Route: /api/trainTypes/1
        public async Task<IActionResult> UpdateTrainType(int id, TrainType trainType)
        {
            try
            {
                // Get train by the given id
                var trainTypeInDb = await _repo.GetTrainType(id);

                // Override the old values by the new values 
                trainTypeInDb.DescEn = trainType.DescEn;
                trainTypeInDb.DescAr = trainType.DescAr;

                // Save changes in the database
                await _repo.SaveAll();

                // Return the results as json object
                return Ok(trainTypeInDb);
            }
            catch(Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
            
        }
    }
}