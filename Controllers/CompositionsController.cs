using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.Composition;
using ERNST.Dto.CompositionRecord;
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
    public class CompositionsController : ControllerBase
    {
        // Create an object of IErnstRepository interface 
        private readonly IErnstRepository _repo;

        // Create an object of IMapper interface 
        private readonly IMapper _mapper;

        // Initialize the objects
        public CompositionsController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        // Purpose: Get Compositions
        // Method: GET
        // Route: /api/compositions
        public async Task<IActionResult> GetCompositions([FromQuery]UserParam userParams)
        {
            try
            {
                // Check if user want coaches in a pagination format
                if (userParams.Pagination)
                {
                    // Get all compositions from database 
                    var compositionsForPaginationInDb = await _repo.GetCompositions(userParams);

                    // Map the results in another model
                    var compositionsForPagination = _mapper
                        .Map<ICollection<CompositionForDisplay>>(compositionsForPaginationInDb);

                    // Get all coaches which are related to each composition and
                    // check if each composition editable or not
                    foreach (var item in compositionsForPagination)
                    {
                        item.Coaches = _repo.GetCoachesForComposition(item.Id);

                        var result = await _repo.IsCompositionEditable(item.Id);
                        result = !result;
                        item.IsEditable = result;
                    }

                    // Create pagination object depends on the returned values
                    var pagination = new
                    {
                        CurrentPage = compositionsForPaginationInDb.CurrentPage,
                        PageSize = compositionsForPaginationInDb.PageSize,
                        TotalCount = compositionsForPaginationInDb.TotalCount,
                        TotalPages = compositionsForPaginationInDb.TotalPages
                    };

                    // Return the results as json object
                    return Ok(new { Compositions = compositionsForPagination, Pagination = pagination });
                }

                // Get all compositions from database 
                var compositionsInDb = await _repo.GetCompositionsForDropDown();

                // Map the results in another model
                var compositions = _mapper.Map<ICollection<CompositionForDisplay>>(compositionsInDb);

                // Create pagination object depends on the returned values
                return Ok(new { Compositions = compositions });
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }

        }


        [HttpGet("{id}")]
        // Purpose: Get Composition Records
        // Method: GET
        // Route: /api/compositions/1
        public IActionResult GetCompositionRecords(int id, [FromQuery]UserParam userParams)
        {
            try
            {
                // Get composition records by the given composition id 
                var compositionRecordsInDb = _repo.GetCompositionRecords(id, userParams);

                // Map the results in another model
                var compositionRecords = _mapper
                    .Map<ICollection<CompositionRecordForDisplay>>(compositionRecordsInDb);

                // Create pagination object depends on the returned values
                return Ok(compositionRecords);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        // Purpose: Create new Composition
        // Method: GET
        // Route: /api/compositions
        public async Task<IActionResult> AddComposition(Composition composition)
        {
            try
            {
                // Create new composition
                _repo.Add(composition);

                // Save changes in the database 
                await _repo.SaveAll();

                // Create pagination object depends on the returned values
                return Ok(composition);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}")]
        // Purpose: Create new Composition Records
        // Method: GET
        // Route: /api/compositions/1
        public async Task<IActionResult> AddCompositionRecords(int id, [FromBody]JArray compositionRecords)
        {
            try
            {
                // Deserialize the given array to be list of composition record
                var item = (List<CompositionRecord>)JsonConvert.DeserializeObject(compositionRecords.ToString(), typeof(List<CompositionRecord>));

                // Iterate over the list of composition record and create each of them separately 
                foreach (var compositionRecord in item)
                {
                    compositionRecord.CompositionId = id;

                    _repo.Add(compositionRecord);
                }

                // Save changes in the database
                await _repo.SaveAll();

                // Create pagination object depends on the returned values
                return Ok(compositionRecords);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        // Purpose: Create new Composition
        // Method: PUT
        // Route: /api/compositions/1
        public async Task<IActionResult> UpdateComposition(int id, Composition composition)
        {
            try
            {
                // Get composition by the given id
                var compositionInDb = await _repo.GetComposition(id);

                // Override the old values by the new values    
                compositionInDb.ArName = composition.ArName;
                compositionInDb.EnName = composition.EnName;

                // Save changes in the database
                await _repo.SaveAll();

                // Return the results as json object
                return Ok(compositionInDb);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/CompositionRecords")]
        public async Task<IActionResult> UpdateCompositionRecords(int id, [FromBody]JArray compositionRecords)
        {
            try
            {
                // Deserialize the given array to be list of composition record
                var item = (List<CompositionRecord>)JsonConvert.DeserializeObject(compositionRecords.ToString(), typeof(List<CompositionRecord>));

                // Delete the old records 
                _repo.DeleteCompositionRecords(id);

                // Iterate over the list of composition record and create each of them separately 
                foreach (var compositionRecord in item)
                {
                    compositionRecord.CompositionId = id;

                    _repo.Add(compositionRecord);
                }

                // Save changes in the database
                await _repo.SaveAll();

                // Return the results as json object
                return Ok(item);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }

        }
    }
}