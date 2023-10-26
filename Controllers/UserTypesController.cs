using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.UserType;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypesController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;

        public UserTypesController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTypes([FromQuery]UserParam userParams)
        {
            try
            {
                var userTypesInDb = await _repo.GetUserTypes(userParams);

                var userTypes = _mapper.Map<ICollection<UserTypeForDisplay>>(userTypesInDb);

                var pagination = new
                {
                    CurrentPage = userTypesInDb.CurrentPage,
                    PageSize = userTypesInDb.PageSize,
                    TotalCount = userTypesInDb.TotalCount,
                    TotalPages = userTypesInDb.TotalPages
                };

                return Ok(new { UserTypes = userTypes, Pagination = pagination });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddUserType(UserType userType)
        {
            try
            {
                _repo.Add(userType);

                await _repo.SaveAll();

                return Ok(userType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserType(int id, UserType userType)
        {
            try
            {
                var userTypeInDb = await _repo.GetUserType(id);

                userTypeInDb.TypeName = userType.TypeName;
                userTypeInDb.Privilages = userType.Privilages;

                await _repo.SaveAll();

                return Ok(userTypeInDb);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
