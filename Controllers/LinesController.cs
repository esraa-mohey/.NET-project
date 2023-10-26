using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto.Header;
using ERNST.Dto.Lines;
using ERNST.Dto.Response;
using ERNST.Helper;
using ERNST.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERNST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;
        public LinesController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetLines([FromQuery]UserParam userParam)
        {
            try
            {
                var userParamDb = await _repo.GetLines(userParam);

                var lines = _mapper.Map<IEnumerable<LinesDto>>(userParamDb);

                foreach (var item in lines)
                {
                    item.Stations = _repo.GetStationsForLine(item.Id);
                }

                var pagination = new
                {
                    CurrentPage = userParamDb.CurrentPage,
                    PageSize = userParamDb.PageSize,
                    TotalCount = userParamDb.TotalCount,
                    TotalPages = userParamDb.TotalPages
                };

                return Ok(new { Lines = lines, Pagination = pagination });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }

        }

        /*[HttpPost("AddAndUpdateLine")]
        public async Task<BaseHeaderResponse> AddAndUpdateLine([FromBody] LineAdd requestBody)
        {
            BaseHeaderResponse settingListReturnDto = new BaseHeaderResponse();
            try
            {
                if (requestBody.Id == 0) { 
                        Lines lines = new Lines();
                        lines.Id = requestBody.Id;
                        lines.ArName = requestBody.ArName;
                        lines.EnName = requestBody.EnName;
                        _repo.Add(lines);
                        if (await _repo.SaveAll()) { 
                        settingListReturnDto.responseHeader = await _repo.BuildSuccesResponse(requestBody, null, "Line Controller", "Add Lines", ResponseCodeHelper.SuccessgetDataMessageEn, ResponseCodeHelper.SuccessgetDataMessageAr, ResponseCodeHelper.SuccessMessageEn);
                        return settingListReturnDto;
                        }
                        else
                        {
                            settingListReturnDto.responseHeader = await _repo.BuildSuccesResponse(requestBody, null, "Line Controller", "Add Lines", ResponseCodeHelper.addDoneEn, ResponseCodeHelper.addDoneAr, ResponseCodeHelper.developerHelper);
                            return settingListReturnDto;
                        }
                     }
                else
                {
                    var returnData = await _repo.UpdateLines(requestBody.Id);
                    returnData.ArName = requestBody.ArName;
                    returnData.EnName = requestBody.EnName;
                    if (await _repo.SaveAll())
                    {
                        settingListReturnDto.responseHeader = await _repo.BuildSuccesResponse(requestBody, null, "Line Controller", "Update Lines", ResponseCodeHelper.SuccessgetDataMessageEn, ResponseCodeHelper.SuccessgetDataMessageAr, ResponseCodeHelper.SuccessMessageEn);
                        return settingListReturnDto;
                    }
                    else
                    {
                        settingListReturnDto.responseHeader = await _repo.BuildSuccesResponse(requestBody, null, "Line Controller", "Update Lines", ResponseCodeHelper.addDoneEn, ResponseCodeHelper.addDoneAr, ResponseCodeHelper.developerHelper);
                        return settingListReturnDto;
                    }
                }
            }
            catch (Exception exception)
            {
                settingListReturnDto.responseHeader = await _repo.BuildExceptionResponse(requestBody, null, "Line Controller", "Update Lines", ResponseCodeHelper.ErrorMessageEn, ResponseCodeHelper.ErrorMessageAr, exception);
                return settingListReturnDto;


            }
        }   */


        [HttpPost]
        public async Task<IActionResult> LinesType(Lines LinesType)
        {
            try
            {
                _repo.Add(LinesType);

                await _repo.SaveAll();

                return Ok(LinesType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassengersType(int id, Lines lineRequest)
        {
            try
            {
                var lineRequestDB = await _repo.UpdateLines(id);

                lineRequestDB.EnName = lineRequest.EnName;
                lineRequestDB.ArName = lineRequest.ArName;

                await _repo.SaveAll();

                return Ok(lineRequestDB);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


    }
}