using AutoMapper;
using Dating.Data;
using ERNST.Dto.PassengersType;
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
    public class PassengersTypeController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;

        public PassengersTypeController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPassengersType([FromQuery]UserParam userParam)
        {
            try
            {
                var PassengersTypeInDB = await _repo.GetPassengerType(userParam);
                var passengersType = _mapper.Map<IEnumerable<PassengerTypeForDisplay>>(PassengersTypeInDB);

                var pagination = new
                {
                    CurrentPage = PassengersTypeInDB.CurrentPage,
                    PageSize = PassengersTypeInDB.PageSize,
                    TotalCount = PassengersTypeInDB.TotalCount,
                    TotalPages = PassengersTypeInDB.TotalPages
                };

                return Ok(new { PassengersType = passengersType, Pagination = pagination });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }



        [HttpPost]
        public async Task<IActionResult> AddPassengersType(PassengersType passengersType)
        {
            try
            {
                _repo.Add(passengersType);

                await _repo.SaveAll();

                return Ok(passengersType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePassengersType(PassengersType passengersType)
        {
            try
            {
                _repo.Delete(passengersType);

                await _repo.SaveAll();

                return Ok(passengersType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassengersType(int id, PassengersType passengersType)
        {
            try
            {
                var passengersTypeInDB = await _repo.GetPassengerType(id);

                passengersTypeInDB.DescEn = passengersType.DescEn;
                passengersTypeInDB.DescAr = passengersType.DescAr;

                await _repo.SaveAll();

                return Ok(passengersTypeInDB);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
