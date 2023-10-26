using AutoMapper;
using Dating.Data;
using ERNST.Dto.Governorate;
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
    public class GovernorateController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;
        //  private int IdIdentifer;
        //  private static string ErrorMessageForID = "this Id is already been used ";
        //  private var number;

        public GovernorateController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetGovernorate([FromQuery]UserParam userParam)
        {
            try
            {
                var GovernorateInDB = await _repo.GetGovernorate(userParam);
                var Governorate = _mapper.Map<IEnumerable<GovernorteForDisplay>>(GovernorateInDB);

                var pagination = new
                {
                    CurrentPage = GovernorateInDB.CurrentPage,
                    PageSize = GovernorateInDB.PageSize,
                    TotalCount = GovernorateInDB.TotalCount,
                    TotalPages = GovernorateInDB.TotalPages
                };
                return Ok(new { Governorate = Governorate, Pagination = pagination });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        [HttpGet("{idGovernorate}")]
        public async Task<IActionResult> GetGovernorateByID(int idGovernorate)
        {
            try
            {
                var GovernorateInDB = await _repo.GetGovernorate(idGovernorate);
                var Governorate = _mapper.Map<ICollection<GovernorteForDisplay>>(GovernorateInDB);



                return Ok(new { Stat = Governorate });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            //
        }

        [HttpPost]
        public async Task<IActionResult> AddGovernorte(Governorate governorate)
        {
            try
            {
                var isExist = await _repo.CheckGovernorateId(governorate.Id);

                if (isExist) return BadRequest("This Id is already exist");
                
                _repo.Add(governorate);

                await _repo.SaveAll();

                return Ok(governorate);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            // }
            //  else return ErrorMessageForID;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGovernorate(int id, Governorate governorate)
        {
            try
            {
                var GovernorateInDB = await _repo.GetGovernorate(id);

                GovernorateInDB.Id = GovernorateInDB.Id;
                GovernorateInDB.EnName = governorate.EnName;
                GovernorateInDB.ArName = governorate.ArName;

                await _repo.SaveAll();

                return Ok(GovernorateInDB);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}