using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERNST.Data;
using AutoMapper;
using ERNST.Dto.Train;
using ERNST.Model;
using Microsoft.EntityFrameworkCore;
using ERNST.Dto.TrainType;
using ERNST.Dto.CoachClass;
using ERNST.Dto.Coach;
using ERNST.Dto.Composition;
using ERNST.Dto.UserType;
using ERNST.Dto.User;
using ERNST.Helper;
using ERNST.Dto.Response;
using ERNST.Dto.Header;
using ERNST.Dto.Lines;
using ERNST.Dto.Time_Table_Stations;
using Microsoft.EntityFrameworkCore.Internal;


namespace Dating.Data
{
    public class ErnstRepository : IErnstRepository
    {
        private readonly ERNSTContext _context;

        public ErnstRepository(ERNSTContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /*------------------------------------- Train Types -------------------------------------*/
        public async Task<PagedList<TrainType>> GetTrainTypes(UserParam userParam)
        {
            // Get all train types 
            var trainTypesInDb = _context.TrainTypes
                .OrderBy(c => c.Id)
                .AsQueryable();

            // Check if client want to search by any attribute
            if (!string.IsNullOrEmpty(userParam.Key))
            {
                // Return train types which any of table attributes contains search keyword
                trainTypesInDb = trainTypesInDb.Where(c =>
                    c.DescAr.Contains(userParam.Key) ||
                    c.DescEn.Contains(userParam.Key) ||
                    c.Id.ToString().Contains(userParam.Key));
            }

            // Return train types as a pagedList for pagination
            return await PagedList<TrainType>
                .CreateAsync(trainTypesInDb, userParam.PageNumber, userParam.PageSize);
        }


        public async Task<List<TrainType>> GetTrainTypesForDropDown()
        {
            // Get all train types 
            var trainTypes = await _context.TrainTypes.ToListAsync();

            // return train types
            return trainTypes;
        }

        public async Task<TrainType> GetTrainType(int id)
        {
            // Get train type with the given id
            var trainType = await _context.TrainTypes.FirstOrDefaultAsync(c => c.Id == id);

            // Return train type
            return trainType;
        }


        /*---------------------------------------- Train ----------------------------------------*/
        public async Task<PagedList<Train>> GetTrains(UserParam userParam)
        {
            // Get all trains 
            var trainsInDb = _context.Trains
                .Include(c => c.Composition)
                .Include(c => c.TrainType)
                .OrderBy(c => c.Id)
                .AsQueryable();

            // Check if client want to search by any attribute
            if (!string.IsNullOrEmpty(userParam.Key))
            {
                // Return train types which any of table attributes contains search keyword
                trainsInDb = trainsInDb.Where(c => c.Id.ToString().Contains(userParam.Key) ||
                                                   c.TrainType.DescAr.Contains(userParam.Key) ||
                                                   c.TrainType.DescEn.Contains(userParam.Key) ||
                                                   c.TrainTypeId.ToString().Contains(userParam.Key) ||
                                                   c.Suspended.ToString().Contains(userParam.Key) ||
                                                   c.StartDate.ToString().Contains(userParam.Key) ||
                                                   c.EndDate.ToString().Contains(userParam.Key) ||
                                                   c.Number.ToString().Contains(userParam.Key) ||
                                                   c.Composition.ArName.Contains(userParam.Key) ||
                                                   c.Composition.EnName.Contains(userParam.Key));
            }

            // Change start date and end date format to be compatible with client 
            foreach (var item in trainsInDb)
            {
                item.StartDate = DateTime.Parse(item.StartDate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                item.EndDate = DateTime.Parse(item.EndDate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            }

            // Return trains as a pagedList for pagination
            return await PagedList<Train>.CreateAsync(trainsInDb, userParam.PageNumber, userParam.PageSize);
        }

        public async Task<List<Train>> GetTrainsForSchedule()
        {
            // Get trains which end date before 1/1/2050 
            var trains = await _context.Trains
                .Where(c=> DateTime.Parse(c.EndDate) < DateTime.Parse("1/1/2050"))
                .ToListAsync();

            // Filter trains by number to avoid return redundant trains 
            trains = trains
                .GroupBy(c => c.Number)
                .Select(g => g.First())
                .ToList();

            // Return trains
            return trains;
        }
        
        public async Task<Train> GetTrain(int id)
        {
            // Get train with the given id
            var train = await _context.Trains.FirstOrDefaultAsync(c => c.Id == id);

            // Return train
            return train;
        }


        public bool CheckTrainNumber(string number)
        {
            // Check if there is a train with the given number
            var ifExist = _context.Trains.Any(c => c.Number == number);

            // Return true or false
            return ifExist;
        }

        /*----------------------------- Coach Class -----------------------------*/

        public async Task<PagedList<CoachClass>> GetCoachClasses(UserParam userParam)
        {
            // Get all coach classes 
            var coachClassesInDb = _context.CoachClass
                .OrderBy(c => c.Id)
                .AsQueryable();

            // Check if client want to search by any attribute
            if (!String.IsNullOrEmpty(userParam.Key))
            {
                // Return coach class which any of table attributes contains search keyword
                coachClassesInDb = coachClassesInDb.Where(c => c.EnName.Contains(userParam.Key) ||
                                                               c.Id.ToString().Contains(userParam.Key) ||
                                                               c.Suspended.ToString().Contains(userParam.Key) ||
                                                               c.ArName.Contains(userParam.Key));
            }

            // Return coach class as a pagedList for pagination
            return await PagedList<CoachClass>.CreateAsync(coachClassesInDb, userParam.PageNumber, userParam.PageSize);
        }

        public async Task<List<CoachClass>> GetCoachClassesForDropDown()
        {
            // Get all coach classes except which are suspended
            var coachClasses = await _context.CoachClass.Where(c => !c.Suspended).ToListAsync();

            // Return coach class
            return coachClasses;
        }

        public async Task<CoachClass> GetCoachClass(int id)
        {
            // Get single coach class by id
            var coachClass = await _context.CoachClass.FirstOrDefaultAsync(c => c.Id == id);


            // Return coach class
            return coachClass;
        }


        /*------------------------------- Coach --------------------------------*/

        public async Task<PagedList<Coach>> GetCoaches(UserParam userParam)
        {
            // Get all coaches
            var coachesInDb = _context.Coaches.Include(c=> c.CoachClass).OrderBy(c => c.Id).AsQueryable();

            // Check if client want to search by any attribute
            if (!string.IsNullOrEmpty(userParam.Key))
            {
                // Return coaches which any of table attributes contain search keyword
                coachesInDb = coachesInDb.Where(c => c.Id.ToString().Contains(userParam.Key) ||
                                                     c.SeatsNumber.ToString().Contains(userParam.Key) ||
                                                     c.Suspended.ToString().Contains(userParam.Key) ||
                                                     c.CountOfCoaches.ToString().Contains(userParam.Key) ||
                                                     c.ColsCount.ToString().Contains(userParam.Key) ||
                                                     c.EnName.Contains(userParam.Key) ||
                                                     c.ArName.Contains(userParam.Key) || 
                                                     c.CoachClass.EnName.Contains(userParam.Key) ||
                                                     c.CoachClass.ArName.Contains(userParam.Key) ||
                                                     c.CoachClass.Id.ToString().Contains(userParam.Key) ||
                                                     c.RowsCount.ToString().Contains(userParam.Key) ||
                                                     c.SeatsCount.ToString().Contains(userParam.Key));
            }

            // Return coaches as a pagedList for pagination
            return await PagedList<Coach>
                .CreateAsync(coachesInDb, userParam.PageNumber, userParam.PageSize);
        }

        public async Task<List<Coach>> GetCoachesForDropDown()
        {
            // Get all coach classes except which are suspended
            var coaches = await _context.Coaches.Where(c=>!c.Suspended).ToListAsync();

            // Return coach class
            return coaches;
        }

        public async Task<Coach> GetCoach(int id)
        {
            // Get single coach by id
            var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.Id == id);

            // Return coach
            return coach;
        }

        /*-------------------------------- Workshop Coaches --------------------------------*/

        public async Task<PagedList<WorkshopCoach>> GetWorkshopCoaches(UserParam userParam)
        {
            // Get all workshop coaches 
            var workshopCoachesInDb = _context.WorkshopCoaches
                .Include(c=>c.Coach)
                .OrderBy(c => c.Id)
                .AsQueryable();

            // Check if client want to search by any attribute
            if (!string.IsNullOrEmpty(userParam.Key))
            {
                // Return workshop coaches which any of table attributes contain search keyword
                workshopCoachesInDb = workshopCoachesInDb
                    .Where(c => c.Available.ToString().Contains(userParam.Key) ||
                                c.InUse.ToString().Contains(userParam.Key) ||
                                c.InMaintenance.ToString().Contains(userParam.Key));
            }

            // Return coaches as a pagedList for pagination
            return await PagedList<WorkshopCoach>
                .CreateAsync(workshopCoachesInDb, userParam.PageNumber, userParam.PageSize);
        }

        public async Task<WorkshopCoach> GetWorkshopCoach(int id)
        {
            // Get single workshop coach by id
            var workshopCoach = await _context.WorkshopCoaches.FirstOrDefaultAsync(c => c.Id == id);

            // Return workshop coach
            return workshopCoach;
        }

        /*--------------------------------- Composition -----------------------------------*/

        public async Task<PagedList<Composition>> GetCompositions(UserParam userParam)
        {
            // Get all compositions
            var compositionInDb = _context.Compositions.OrderBy(c => c.Id).AsQueryable();

            // Check if client want to search by any attribute
            if (!string.IsNullOrEmpty(userParam.Key))
            {
                // Return compositions which any of table attributes contain search keyword
                compositionInDb = compositionInDb.Where(c => c.Id.ToString().Contains(userParam.Key) ||
                                                             c.ArName.Contains(userParam.Key) ||
                                                             c.EnName.Contains(userParam.Key));
            }

            // Return coaches as a pagedList for pagination
            return await PagedList<Composition>.CreateAsync(compositionInDb, userParam.PageNumber, userParam.PageSize);
        }

        public string GetCoachesForComposition(int id)
        {
            // Get all composition records by composition id
            var coachesForCompositions = _context.CompositionRecords
                .Include(c=>c.Coach)
                .Where(c => c.CompositionId == id)
                .ToList();

            // Filter composition records by coachId to avoid return redundant data 
            coachesForCompositions = coachesForCompositions
                .GroupBy(car => car.CoachId)
                .Select(g => g.First())
                .ToList();

            // First index in the list 
            var index = 0;

            // Last index in the list
            var lastIndex = coachesForCompositions.Count;

            // Final result
            var result = "";

            // Iterate over the list 
            foreach (var coachesForComposition in coachesForCompositions)
            {
                // Check if the current element is the last element in the array
                if (index == lastIndex-1)
                {
                    result += _context.CompositionRecords.Count(c => c.CompositionId == coachesForComposition.CompositionId && c.CoachId == coachesForComposition.CoachId) + " " +
                              coachesForComposition.Coach.ArName;
                }
                else
                {
                    result += _context.CompositionRecords.Count(c => c.CompositionId == coachesForComposition.CompositionId && c.CoachId == coachesForComposition.CoachId) + " " +
                              coachesForComposition.Coach.ArName + ",";
                }

                index++;

            }

            // Return the final result
            return result;
        }

        public async Task<List<Composition>> GetCompositionsForDropDown()
        {
            // Get all composition to be displayed in drop down list
            var compositionInDb = await _context.Compositions
                .OrderBy(c => c.Id)
                .ToListAsync();

            // Return compositions
            return compositionInDb;
        }

        public List<CompositionRecord> GetCompositionRecords(int id, UserParam userParam)
        {
            // Get all composition records by composition id
            var compositionRecordsInDb =  _context.CompositionRecords
                        .Include(c => c.Coach)
                        .OrderBy(c => c.Id)
                        .Where(c => c.CompositionId == id)
                        .ToList();

            // Check if client want to search by any attribute
            if (!string.IsNullOrEmpty(userParam.Key))
            {
                // Return composition records which any of table attributes contain search keyword
                compositionRecordsInDb = compositionRecordsInDb
                    .Where(c => c.Coach.EnName.Contains(userParam.Key)).ToList();
            }

            // Return composition records as a pagedList for pagination
            return compositionRecordsInDb;
        }

        public async Task<Composition> GetComposition(int id)
        {
            // Get composition by id
            var composition = await _context.Compositions.FirstOrDefaultAsync(c => c.Id == id);


            // Return composition
            return composition;
        }

        public async Task<bool> IsCompositionEditable(int id)
        {
            // Check if the composition with the given id is editable or not
            var result = await _context.Time_Table
                .Include(c => c.Train)
                .AnyAsync(c => c.Train.CompositionId == id);

            // Return the result 
            return result;
        }

        public bool DeleteCompositionRecords(int id)
        {
            // Get all composition records by the given composition id
            var compositionRecords = _context.CompositionRecords
                .Where(c => c.CompositionId == id)
                .ToList();

            // Remove all composition records
            _context.RemoveRange(compositionRecords);

            _context.SaveChanges();

            return true;
        }

        

        /*---------------------------------- User Type -----------------------------------*/

        public async Task<PagedList<UserType>> GetUserTypes(UserParam userParam)
        {
            // Get all user types
            var userTypesInDb = _context.UserTypes.OrderBy(c => c.Id).AsQueryable();

            // Check if client want to search by any attribute
            if (!String.IsNullOrEmpty(userParam.Key))
            {
                // Return user types which any of table attributes contain search keyword
                userTypesInDb = userTypesInDb.Where(c => c.TypeName.Contains(userParam.Key) ||
                                                         c.Privilages.Contains(userParam.Key));
            }

            // Return coaches as a pagedList for pagination
            return await PagedList<UserType>.CreateAsync(userTypesInDb, userParam.PageNumber, userParam.PageSize);
        }

        public async Task<UserType> GetUserType(int id)
        {
            // Get user type by the given id
            var userType = await _context.UserTypes.FirstOrDefaultAsync(c => c.Id == id);

            // Return user type
            return userType;
        }

        /*---------------------------------- User -------------------------------------*/

        public async Task<PagedList<User>> GetUsers(UserParam userParam)
        {
            // Get all users
            var usersInDb = _context.Users.Include(c=> c.UserType).OrderBy(c => c.Id).AsQueryable();

            // Check if client want to search by any attribute
            if (!String.IsNullOrEmpty(userParam.Key))
            {
                // Return users which any of table attributes contain search keyword
                usersInDb = usersInDb.Where(c => c.FirstName.Contains(userParam.Key) || c.LastName.Contains(userParam.Key) ||
                                                 c.Mobile.Contains(userParam.Key) || c.Password.Contains(userParam.Key) ||
                                                 c.Status.ToString().Contains(userParam.Key) || c.UserType.TypeName.Contains(userParam.Key) ||
                                                 c.Email.Contains(userParam.Key) || c.Gender.Contains(userParam.Key));
            }

            // Return coaches as a pagedList for pagination
            return await PagedList<User>.CreateAsync(usersInDb, userParam.PageNumber, userParam.PageSize);
        }

        public async Task<User> GetUser(int id)
        {
            // Get user by the given id
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);

            // Return user
            return user;
        }

        /************************************ Lines *********************************************/
        public async Task<PagedList<Lines>> GetLines(UserParam userParam)
        {
            var LinesDb = _context.Lines.AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                LinesDb = LinesDb.Where(c => c.ArName.ToString().Contains(userParam.Key) || c.EnName.ToString().Contains(userParam.Key)  ||
                                        c.Id.ToString().Contains(userParam.Key));
            }

            return await PagedList<Lines>.CreateAsync(LinesDb, userParam.PageNumber, userParam.PageSize);
        }

        public List<Stations> GetStationsForLine(int id)
        {
            var stations = _context.LineStations.Include(c => c.Stations).OrderBy(x=>x.Stations.OperationCode).Where(c => c.LinesId == id)
                .Select(c => c.Stations).ToList();

            return stations;
        }

        public async Task<Lines> UpdateLines(int id)
        {
            var Line = await _context.Lines.FirstOrDefaultAsync(c => c.Id == id);

            return Line;
        }


        /***************** ----------- Schedule Train Line ----------------------------------*/
        public async Task<PagedList<Schedule>> GetSchedule(UserParam userParam)
        {

            var RegionInDB = _context.Time_Table.Include(x=>x.Train).AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                RegionInDB = RegionInDB.Where(c => c.ExpireDate.Contains(userParam.Key));
            }

            foreach (var item in RegionInDB)
            {
                item.StartDate = DateTime.Parse(item.StartDate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                item.ExpireDate = DateTime.Parse(item.ExpireDate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            }

            return await PagedList<Schedule>.CreateAsync(RegionInDB, userParam.PageNumber, userParam.PageSize);

        }
        public async Task<Schedule> GetScheduleByid(int id)
        {
            var Time_Table = await _context.Time_Table.FirstOrDefaultAsync(c => c.Id == id);

            return Time_Table;
        }
        public async Task<List<Schedule>> GetScheduleForDropDown()
        {
            var scheduleClasses = await _context.Time_Table.Where(c => !c.Suspended).ToListAsync();

            foreach (var item in scheduleClasses)
            {
                item.StartDate = DateTime.Parse(item.StartDate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                item.ExpireDate = DateTime.Parse(item.ExpireDate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            }

            return scheduleClasses;
        }

        public async Task<Schedule> updateSchedule(int id)
        {
            var trainSchedule = await _context.Time_Table.LastOrDefaultAsync(c => c.Id == id);

            return trainSchedule;
        }






        /*------------------------------- ticket type --------------------------------*/
        public async Task<PagedList<TicketTypes>> GetTicketTypes(UserParam userParam)
        {
            var TicketTypeInDB = _context.TicketTypes.AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                TicketTypeInDB = TicketTypeInDB.Where(c => c.DescAr.Contains(userParam.Key) || c.DescEn.Contains(userParam.Key));
            }

            return await PagedList<TicketTypes>.CreateAsync(TicketTypeInDB, userParam.PageNumber, userParam.PageSize);

          
        }
        public async Task<TicketTypes> GetTicketTypes(int id)
        {
            var TicketTypeInDB = await _context.TicketTypes.FirstOrDefaultAsync(c => c.Id == id);

            return TicketTypeInDB;
        }


        /*------------------------------- Passenger type --------------------------------*/
        public async Task<PagedList<PassengersType>> GetPassengerType(UserParam userParam)
        {
            var PassengerTypeInDB = _context.PassengersType.AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                PassengerTypeInDB = PassengerTypeInDB.Where(c => c.DescAr.Contains(userParam.Key) || c.DescEn.Contains(userParam.Key));
            }

            return await PagedList<PassengersType>.CreateAsync(PassengerTypeInDB, userParam.PageNumber, userParam.PageSize);
        }

        public async Task <PassengersType> GetPassengerType(int id)
        {
            var PassengerTypeInDB = await _context.PassengersType.FirstOrDefaultAsync(c => c.Id == id);

            return PassengerTypeInDB;
        }
        /************************************ Update line station *****************************/

        /*------------------------------- Time_Table_Station --------------------------------*/
        public async Task<PagedList<Time_Tabale_Stations>> GetTime_Table_Station(UserParam userParam)
        {
            var StationInDB = _context.Time_Table_Stations.Include(c => c.LineStations).OrderBy(c => c.Id).AsQueryable();


            if (!String.IsNullOrEmpty(userParam.Key))
            {
                StationInDB = StationInDB.Where(c => c.LineStationsId.ToString().Contains(userParam.Key) || c.Arrival_time.Contains(userParam.Key)
                                               || c.Departure_time.Contains(userParam.Key) );
            }

            

            return await PagedList<Time_Tabale_Stations>.CreateAsync(StationInDB, userParam.PageNumber, userParam.PageSize);
        }
        public async Task<Time_Tabale_Stations> GetTime_Table_Stations(int id)
        {
            var RegionInDB = await _context.Time_Table_Stations.FirstOrDefaultAsync(c => c.Id == id);

            return RegionInDB;
        }

        public List<TimeTableStationForUpdate> GetTimeTableStationForSchedule(int id)
        {
            var timeTableStations = _context.Time_Table_Stations
                .Include(c=>c.LineStations.Lines)
                .Include(c=>c.LineStations.Stations)
                .Where(c => c.ScheduleId == id)
                .OrderBy(x=>x.LineStationsId)
                .ToList();

            var model = timeTableStations.Select(item => new TimeTableStationForUpdate()
            {
                Arrival_time = item.Arrival_time,
                Departure_time = item.Departure_time,
                StationId = item.LineStations.StationsId,
                StationName = item.LineStations.Stations.ArName,
                LineStationsId = item.LineStationsId
            })
                .ToList();

            return model;
        }

        public List<Time_Tabale_Stations> GetTimeTableStationForScheduleOnUpdate(int id)
        {
            var timeTableStations = _context.Time_Table_Stations
                .Include(c=>c.LineStations.Lines)
                .Include(c=>c.LineStations.Stations)
                .Where(c => c.ScheduleId == id)
                .ToList();

            return timeTableStations;
        }

        public int GetLineBySchedule(int id)
        {
            var result = 0;

            if (_context.Time_Table_Stations.Any(c=>c.ScheduleId == id))
            {
                result = _context.Time_Table_Stations
                    .Include(c => c.LineStations)
                    .First(c => c.ScheduleId == id).LineStations.LinesId;
            }
            

            return result;
        }
        /***************** ----------- Schedule Train Line ----------------------------------*/
        public async Task<PagedList<LineStations>> GetLineStation(UserParam userParam)
        {
            var LineStationDB = _context.LineStations.Include(c=>c.Stations).Include(c=>c.Lines).OrderBy(c => c.Stations.OperationCode).AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                LineStationDB = LineStationDB.Where(c => c.LinesId.ToString().Contains(userParam.Key) || c.StationsId.ToString().Contains(userParam.Key));
            }

            return await PagedList<LineStations>.CreateAsync(LineStationDB, userParam.PageNumber, userParam.PageSize);
        }
        
        public async Task<bool> checkTrain(int id)
        {
            var train = _context.TrainSchedule.Where(c => c.TrainId == id).FirstOrDefault();
            if (train != null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }
        public async Task<List<LineStations>> LineStation(int id)
        {
            var LineStationDB = _context.LineStations.Where(x=>x.LinesId==id).Include(c => c.Stations).OrderBy(x=>x.Stations.OperationCode).Include(c => c.Lines).ToListAsync();
  

            return await LineStationDB;
        }
        public async Task<LineStations> updateLineStation(int id)
        {
            var LineStationsDB = await _context.LineStations.FirstOrDefaultAsync(c => c.Id == id);

            return LineStationsDB;
        }


        /*------------------------------- Station --------------------------------*/
        public async Task<PagedList<Stations>> GetStations(UserParam userParam)
        {//GovRegionsId
            var StationInDB = _context.Stations.Include(c => c.GovRegions).Include(c => c.GovRegions.Governorate).Include(c=>c.GovRegions.Region).OrderBy(c => c.OperationCode).AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                StationInDB = StationInDB.Where(c => c.ArName.Contains(userParam.Key) || c.EnName.Contains(userParam.Key)
                                 || c.Id.ToString().Contains(userParam.Key)
                                 || c.OperationCode.ToString().Contains(userParam.Key)
                                 || c.GovRegions.Region.ArName.Contains(userParam.Key)
                                 || c.GovRegions.Governorate.ArName.Contains(userParam.Key)
                                 || c.GovRegions.Id.ToString().Contains(userParam.Key));
            }

            return await PagedList<Stations>.CreateAsync(StationInDB, userParam.PageNumber, userParam.PageSize);
        }
        public async Task<List<Stations>> GetStationsDropdown(UserParam userParam)
        {//GovRegionsId
            var StationInDB = _context.Stations.Include(c => c.GovRegions).Include(c => c.GovRegions.Governorate).Include(c => c.GovRegions.Region).OrderBy(c => c.OperationCode).AsQueryable().ToListAsync();


            return await StationInDB;
        }

        public async Task<Stations> GetStationsID(int id)
        {
            var StationInDB = await _context.Stations.FirstOrDefaultAsync(c => c.Id == id);

            return StationInDB;
        }
        public async Task<Stations> GetStationsOC(int idOparationCode)
        {
            var StationInDB = await _context.Stations.FirstOrDefaultAsync(c => c.OperationCode == idOparationCode);

            return StationInDB;
        }

        public async Task<bool> CheckStationId(int id)
        {
            var isExist = await _context.Stations.AnyAsync(c => c.Id == id);

            return isExist;
        }
        /*------------------------------- Region --------------------------------*/

        public async Task<PagedList<Region>> GetRegion(UserParam userParam)
        {
            var RegionInDB = _context.Region.OrderBy(c => c.Id).AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                RegionInDB = RegionInDB.Where(c => c.ArName.Contains(userParam.Key) || c.EnName.Contains(userParam.Key)
                    || c.Id.ToString().Contains(userParam.Key));
            }

            return await PagedList<Region>.CreateAsync(RegionInDB, userParam.PageNumber, userParam.PageSize);
        }
        public async Task<List<Region>> GetRegionForDropDown(UserParam userParam)
        {
            var coaches = await _context.Region.ToListAsync();

            return coaches;
        }
        public async Task<Region> GetRegion(int id)
        {
            var RegionInDB = await _context.Region.FirstOrDefaultAsync(c => c.Id == id);

            return RegionInDB;
        }

        public async Task<bool> CheckRegionId(int id)
        {
            var isExist = await _context.Region.AnyAsync(c => c.Id == id);

            return isExist;
        }

        /*------------------------------- Governorate --------------------------------*/
        public async Task<PagedList<Governorate>> GetGovernorate(UserParam userParam)
        {
            var GovernorateInDB = _context.Governorate.OrderBy(c => c.Id).AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                GovernorateInDB = GovernorateInDB.Where(c => c.ArName.Contains(userParam.Key) || c.EnName.Contains(userParam.Key)
                    || c.Id.ToString().Contains(userParam.Key));
            }
            return await PagedList<Governorate>.CreateAsync(GovernorateInDB, userParam.PageNumber, userParam.PageSize);
        }


        public async Task<Governorate> GetGovernorate(int id)
        {
            var GovernorateInDB = await _context.Governorate.FirstOrDefaultAsync(c => c.Id == id);

            return GovernorateInDB;
        }

        public async Task<bool> CheckGovernorateId(int id)
        {
            var isExist = await _context.Governorate.AnyAsync(c => c.Id == id);

            return isExist;
        }

        /*------------------------------- GovRegion --------------------------------*/
        public async Task<PagedList<Gov_Regions>> GetGovRegion(UserParam userParam)
        {
            var Gov_RegionsInDB = _context.Gov_Regions.Include(c => c.Region).Include(c => c.Governorate).OrderBy(c => c.Id).AsQueryable();

            if (!String.IsNullOrEmpty(userParam.Key))
            {
                Gov_RegionsInDB = Gov_RegionsInDB.Where(c => c.Region.ArName.Contains(userParam.Key) || c.Region.EnName.Contains(userParam.Key) || c.Region.Id.ToString().Contains(userParam.Key)
                                                     || c.Governorate.ArName.Contains(userParam.Key) || c.Governorate.EnName.Contains(userParam.Key) || c.Governorate.Id.ToString().Contains(userParam.Key));
            }
            return await PagedList<Gov_Regions>.CreateAsync(Gov_RegionsInDB, userParam.PageNumber, userParam.PageSize);
        }

        public async Task<Gov_Regions> GetGovRegionitem(int idgov, int idregion)
        {
            var GovDB = _context.Gov_Regions.Where(x => x.RegionId == idregion && x.GovernorateId == idgov).Include(c => c.Region).Include(c => c.Governorate).FirstOrDefaultAsync();


            return await GovDB;
        }

        public async Task<bool> CheckGovRegionId(int id)
        {
            var isExist = await _context.Gov_Regions.AnyAsync(c => c.Id == id);

            return isExist;
        }

        public async Task<Gov_Regions> GetGovRegion(int id)
        {
            
            var Gov_RegionsInDB = await _context.Gov_Regions.FirstOrDefaultAsync(c => c.Id == id);

            return Gov_RegionsInDB;
        }

        public async Task<List<Gov_Regions>> GetGovRegions(int id)
        {

            var Gov_RegionsInDB = await _context.Gov_Regions.Where(x => x.RegionId == id).Include(c => c.Governorate).Include(c => c.Region).ToListAsync();

            return Gov_RegionsInDB;
        }




    }
}
