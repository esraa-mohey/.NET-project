using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.User;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParam userParams)
        {
            try
            {
                var usersInDb = await _repo.GetUsers(userParams);

                var users = _mapper.Map<ICollection<UserForDisplay>>(usersInDb);

                var pagination = new
                {
                    CurrentPage = usersInDb.CurrentPage,
                    PageSize = usersInDb.PageSize,
                    TotalCount = usersInDb.TotalCount,
                    TotalPages = usersInDb.TotalPages
                };

                return Ok(new { Users = users, Pagination = pagination });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                _repo.Add(user);

                await _repo.SaveAll();

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            try
            {
                var userInDb = await _repo.GetUser(id);

                userInDb.Email = user.Email;
                userInDb.FirstName = user.FirstName;
                userInDb.LastName = user.LastName;
                userInDb.Password = user.Password;
                userInDb.Mobile = user.Mobile;
                userInDb.Status = user.Status;
                userInDb.Gender = user.Gender;
                userInDb.UserTypeId = user.UserTypeId;

                await _repo.SaveAll();

                return Ok(userInDb);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}