using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.Train;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        // Create an object of IErnstRepository interface 
        private readonly IErnstRepository _repo;

        // Create an object of IMapper interface 
        private readonly IMapper _mapper;

        // Initialize the objects
        public TrainsController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        // Purpose: Get all trains as a pagination result
        // Method: GET
        // Route: /api/trains
        public async Task<IActionResult> GetTrains([FromQuery]UserParam userParams)
        {
            try
            {
                // Get all trains from database 
                var trainsInDb = await _repo.GetTrains(userParams);

                // Map the results in another model
                var trains = _mapper.Map<IEnumerable<TrainForDisplay>>(trainsInDb);

                // Create pagination object depends on the returned values
                var pagination = new
                {
                    CurrentPage = trainsInDb.CurrentPage,
                    PageSize = trainsInDb.PageSize,
                    TotalCount = trainsInDb.TotalCount,
                    TotalPages = trainsInDb.TotalPages
                };

                // Return the results as json object
                return Ok(new { Trains = trains, Pagination = pagination });
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }

        }
        

        [HttpGet("forSchedule")]
        // Purpose: Get all train to display in drop down
        // Method: GET
        // Route: /api/trains/forSchedule
        public async Task<IActionResult> GetTrainsForSchedule()
        {
            try
            {
                // Get all trains
                var trains = await _repo.GetTrainsForSchedule();

                // Return the results as json object
                return Ok(new { Trains = trains });
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{number}/checkNumber")]
        // Purpose: Check train number
        // Method: GET
        // Route: /api/trains/1/checkNumber
        public IActionResult CheckTrainNumber(string number)
        {
            try
            {
                // Check if any train with the given number is already exist
                var isNumberExist = _repo.CheckTrainNumber(number);

                // Return the results as json object
                return Ok(isNumberExist);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        // Purpose: Create new train
        // Method: POST
        // Route: /api/trains
        public async Task<IActionResult> AddTrain(Train train)
        {
            try
            {
                // Check if the new train number is already exist
                var isTrainNumberExist =  _repo.CheckTrainNumber(train.Number);

                // If it true: return server error
                if (isTrainNumberExist) return BadRequest("تم ادخال رقم القطار هذا من قبل !");

                // Get current date time
                var dateTimeNow = DateTime.UtcNow;

                // Parse start date of new train
                var startDateAsDateTime = DateTime.Parse(train.StartDate);

                // Parse end date of new train
                var endDateAsDateTime = DateTime.Parse(train.EndDate);

                // Check if start date is less than current date
                if(startDateAsDateTime < dateTimeNow)
                    return BadRequest("يجب ان يكون تاريخ البدايه اكبر من تاريخ اليوم !");

                // Check if end date is less than or equal start date
                if (endDateAsDateTime <= startDateAsDateTime)
                    return BadRequest("يجب ان يكون تاريخ النهاية اكبر من تاريخ البدايه !");

                // Calculate the difference between end date and start
                var diff = (endDateAsDateTime - startDateAsDateTime).Days;

                // Check if the difference is less than 30
                if (diff < 30)
                    return BadRequest("يجب ان يكون الفرق بين تاريخ النهاية والبدايه اكبر من  ثلاثون يوما !");

                // Create the nre train
                _repo.Add(train);

                // Save changes in database
                await _repo.SaveAll();

                // Return the results as json object
                return Ok(train);
            }
            catch (Exception e)
            {
                // Return bad request in case of any server error
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        // Purpose: Update train
        // Method: PUT
        // Route: /api/trains/1
        public async Task<IActionResult> UpdateTrain(int id, Train train)
        {
            try
            {
                // Get current date time
                var dateTimeNow = DateTime.UtcNow;


                // Parse start date of new train
                var startDateAsDateTime = DateTime.Parse(train.StartDate);

                // Parse end date of new train
                var endDateAsDateTime = DateTime.Parse(train.EndDate);

                // Get the already exist train in database 
                var oldTrain = await _repo.GetTrain(id);

                // Update end date of old train by the new value minus day of start date of new train  
                oldTrain.EndDate = startDateAsDateTime.AddDays(-1).ToString();

                // Save changes in the database 
                await _repo.SaveAll();

                // Check if start date is less than current date
                if (startDateAsDateTime < dateTimeNow)
                    return BadRequest("يجب ان يكون تاريخ البدايه اكبر من تاريخ اليوم !");

                // Check if end date is less than or equal start date
                if (endDateAsDateTime <= startDateAsDateTime)
                    return BadRequest("يجب ان يكون تاريخ النهاية اكبر من تاريخ البدايه !");

                // Calculate the difference between end date and start
                var diff = (endDateAsDateTime - startDateAsDateTime).Days;

                // Check if the difference is less than 30
                if (diff < 30)
                    return BadRequest("يجب ان يكون الفرق بين تاريخ النهاية والبدايه اكبر من  ثلاثون يوما !");

                train.Id = 0;

                // Create the new train as a new record
                _repo.Add(train);

                // Save changes in the database
                await _repo.SaveAll();

                return Ok(train);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}