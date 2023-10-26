using AutoMapper;
using Dating.Data;
using ERNST.Dto.Ticket;
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
    public class TicketTypesController : ControllerBase
    {
        
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;

        public TicketTypesController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTicketTypes([FromQuery]UserParam userParam)
        {
            try
            {
                var TicketTypesInDB = await _repo.GetTicketTypes(userParam);
                var TicketType = _mapper.Map<IEnumerable<TicketForDisplay>>(TicketTypesInDB);
                //var TicketType = _mapper.Map<IEnumerable<TicketForDisplay>>(TicketTypesInDB);

                var pagination = new
                {
                    CurrentPage = TicketTypesInDB.CurrentPage,
                    PageSize = TicketTypesInDB.PageSize,
                    TotalCount = TicketTypesInDB.TotalCount,
                    TotalPages = TicketTypesInDB.TotalPages
                };

                return Ok(new { TicketTypes = TicketType, Pagination = pagination });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTickets(TicketTypes ticketTypes)
        {
            try
            {
                _repo.Add(ticketTypes);

                await _repo.SaveAll();

                return Ok(ticketTypes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTicketsType(TicketTypes ticketTypes)
        {
            try
            {
                _repo.Delete(ticketTypes);

                await _repo.SaveAll();

                return Ok(ticketTypes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTickets(int id, TicketTypes ticketTypes)
        {
            try
            {
                var TicketTypesInDB = await _repo.GetTicketTypes(id);

                TicketTypesInDB.DescEn = ticketTypes.DescEn;
                TicketTypesInDB.DescAr = ticketTypes.DescAr;
                //test o
                await _repo.SaveAll();

                return Ok(TicketTypesInDB);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}