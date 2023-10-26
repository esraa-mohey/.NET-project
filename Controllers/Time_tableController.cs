using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dating.Data;
using ERNST.Dto;
using ERNST.Dto.Coach;
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
    public class Time_tableController : ControllerBase
    {
        private readonly IErnstRepository _repo;
        private readonly IMapper _mapper;
        public Time_tableController(IErnstRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetSchaduleTrainLine([FromQuery]UserParam userParam)
        {
            try
            {
                if (userParam.Pagination)
                {
                    var trainsInDb = await _repo.GetSchedule(userParam);

                    var trains = _mapper.Map<IEnumerable<ScheduleDto>>(trainsInDb);

                    

                    foreach (var item in trains)
                    {
                        item.TimeTableStations = _repo.GetTimeTableStationForSchedule(item.Id);
                        item.LineId = _repo.GetLineBySchedule(item.Id);
                    }

                    var pagination = new
                    {
                        CurrentPage = trainsInDb.CurrentPage,
                        PageSize = trainsInDb.PageSize,
                        TotalCount = trainsInDb.TotalCount,
                        TotalPages = trainsInDb.TotalPages
                    };

                    return Ok(new { schedule = trains, Pagination = pagination });
                }
                var scheduleClassesInDb = await _repo.GetScheduleForDropDown();

                foreach (var item in scheduleClassesInDb)
                {
                    item.TimeTableStations = _repo.GetTimeTableStationForSchedule(item.Id);
                    item.LineId = _repo.GetLineBySchedule(item.Id);
                }

                var scheduleClasses = _mapper.Map<ICollection<ScheduleDto>>(scheduleClassesInDb);

                return Ok(new { Time_table = scheduleClasses });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSchedule(ScheduleDto schedule)
        {
            try
            {
                var trainCheck = await _repo.checkTrain(schedule.TrainId);

                if (trainCheck == false) {
                    DateTime startDate = DateTime.Parse(schedule.StartDate);
                    DateTime expiredate = DateTime.Parse(schedule.ExpireDate);

                    if ((expiredate - startDate).TotalDays >= 30)
                    {
                        var scheduleModel = new Schedule
                        {
                            ExpireDate = schedule.ExpireDate,
                            StartDate = schedule.StartDate,
                            Suspended = schedule.Suspended,
                            TrainId = schedule.TrainId,

                        };

                        _repo.Add(scheduleModel);

                        await _repo.SaveAll();

                        return Ok(scheduleModel);
                    }
                    else
                    {
                        return BadRequest("مراجعة التاريخ");
                    }
                }
                else
                {
                    return BadRequest("يوجد جدول من قبل");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, ScheduleDto schedule)
        {
            try
            {
                var scheduleInDb = await _repo.updateSchedule(id);

                if (scheduleInDb.ExpireDate != schedule.ExpireDate || scheduleInDb.StartDate != schedule.StartDate) { 
                scheduleInDb.ExpireDate = schedule.StartDate;
                //scheduleInDb.StartDate = schedule.StartDate;
                //scheduleInDb.TrainId = schedule.TrainId;
                //scheduleInDb.Suspended = schedule.Suspended;
               

                DateTime startDate = DateTime.Parse(schedule.StartDate);
                DateTime expiredate = DateTime.Parse(schedule.ExpireDate);

                if ((expiredate - startDate).TotalDays >= 30)
                {
                    var scheduleModel = new Schedule
                    {
                        ExpireDate = schedule.ExpireDate,
                        StartDate = schedule.StartDate,
                        Suspended = schedule.Suspended,
                        TrainId = schedule.TrainId,

                    };

                    _repo.Add(scheduleModel);

                    await _repo.SaveAll();

                    return Ok();
                }
                    else
                    {
                        return BadRequest("مراجعة التاريخ");
                    }

                }
                else
                {
                    DateTime startDate = DateTime.Parse(schedule.StartDate);
                    DateTime expiredate = DateTime.Parse(schedule.ExpireDate);

                    if ((expiredate - startDate).TotalDays >= 30)
                    {
                        var scheduleModel = new Schedule
                        {
                            ExpireDate = schedule.ExpireDate,
                            StartDate = schedule.StartDate,
                            Suspended = schedule.Suspended,
                            TrainId = schedule.TrainId,

                        };

                        await _repo.SaveAll();

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, CoachForUpdateStatus coachForUpdateStatus)
        {
            try
            {
                var schedule = await _repo.GetScheduleByid(id);

                schedule.Suspended = coachForUpdateStatus.Status;

                await _repo.SaveAll();

                return Ok(schedule);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}