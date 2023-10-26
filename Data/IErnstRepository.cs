using ERNST.Dto.Coach;
using ERNST.Dto.CoachClass;
using ERNST.Dto.Composition;
using ERNST.Dto.Header;
using ERNST.Dto.Lines;
using ERNST.Dto.Response;
using ERNST.Dto.Train;
using ERNST.Dto.TrainType;
using ERNST.Dto.User;
using ERNST.Dto.UserType;
using ERNST.Helper;
using ERNST.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERNST.Dto.Time_Table_Stations;

namespace Dating.Data
{
    public interface IErnstRepository
    {
        /// <summary>
        ///     This function is responsible for add any entity object from table in database
        /// </summary>
        /// <param name='entity'></param>
        /// <returns></returns>
        void Add<T>(T entity) where T : class;


        /// <summary>
        ///     This function is responsible for add any entity object from table in database
        /// </summary>
        /// <param name='entity'></param>
        /// <returns></returns>
        void Delete<T>(T entity) where T : class;


        /// <summary>
        ///     This function is responsible for save any query to database
        /// </summary>
        /// <param></param>
        /// <returns>
        ///     is the query terminated or executed successfully
        /// </returns>
        Task<bool> SaveAll();

        /*--------------------------------- Train Types --------------------------------------*/
        /// <summary>
        ///     This function is responsible for get all train types from database
        /// </summary>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of trains types in a pagination format
        /// </returns>
        Task<PagedList<TrainType>> GetTrainTypes(UserParam userParam);

        /// <summary>
        ///     This function is responsible for get all coach class to displayed in dropdown
        /// </summary>
        /// <param></param>
        /// <returns>
        ///     list of coach classes
        /// </returns>
        Task<List<TrainType>> GetTrainTypesForDropDown();


        /// <summary>
        ///     This function is responsible for get specific train type from database
        /// </summary>
        /// <param name="id">
        ///     the id of the train type that we want to return
        /// </param>
        /// <returns>
        ///     single train type 
        /// </returns>
        Task<TrainType> GetTrainType(int id);


        /*--------------------------------- Train --------------------------------------*/
        /// <summary>
        ///     This function is responsible for get all trains from database
        /// </summary>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of trains in a pagination format
        /// </returns>
        Task<PagedList<Train>> GetTrains(UserParam userParam);

        /// <summary>
        ///     This function is responsible for get all trains from
        ///     database which end date before 1/1/2050
        /// </summary>
        /// <param></param>
        /// <returns>
        ///     list of trains
        /// </returns>
        Task<List<Train>> GetTrainsForSchedule();

        /// <summary>
        ///     This function is responsible for check that this train number doesn't exist'
        /// </summary>
        /// <param name="number">
        ///     the number of the train that we check if it is exist 
        /// </param>
        /// <returns>
        ///     existed or not
        /// </returns>
        bool CheckTrainNumber(string number);


        /// <summary>
        ///     This function is responsible for get specific train type from database
        /// </summary>
        /// <param name="id">
        ///     the id of the train that we want to return
        /// </param>
        /// <returns>
        ///     single train
        /// </returns>
        Task<Train> GetTrain(int id);


        /*-------------------------------- Coach Class -----------------------------------*/
        /// <summary>
        ///     This function is responsible for get all coach classes from database
        /// </summary>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of coach classes in a pagination format
        /// </returns>
        Task<PagedList<CoachClass>> GetCoachClasses(UserParam userParam);

        /// <summary>
        ///     This function is responsible for get all coach class to displayed in dropdown
        /// </summary>
        /// <param></param>
        /// <returns>
        ///     list of coach classes
        /// </returns>
        Task<List<CoachClass>> GetCoachClassesForDropDown();

        /// <summary>
        ///     This function is responsible for get specific coach class from database
        /// </summary>
        /// <param name="id">
        ///     the id of the coach class type that we want to return
        /// </param>
        /// <returns>
        ///     single coach class
        /// </returns>
        Task<CoachClass> GetCoachClass(int id);


        /*---------------------------------- Coach ----------------------------------------*/
        /// <summary>
        ///     This function is responsible for get all coaches from database
        /// </summary>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of coaches in a pagination format
        /// </returns>
        Task<PagedList<Coach>> GetCoaches(UserParam userParam);

        /// <summary>
        ///     This function is responsible for get all coaches to displayed in dropdown
        /// </summary>
        /// <param></param>
        /// <returns>
        ///     list of coaches
        /// </returns>
        Task<List<Coach>> GetCoachesForDropDown();

        /// <summary>
        ///     This function is responsible for get specific coaches from database
        /// </summary>
        /// <param name="id">
        ///     the id of the coach that we want to return
        /// </param>
        /// <returns>
        ///     single coach
        /// </returns>
        Task<Coach> GetCoach(int id);

        /*---------------------------------- Workshop Coach ----------------------------------------*/
        /// <summary>
        ///     This function is responsible for get all workshop coaches from database
        /// </summary>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of workshop coaches in a pagination format
        /// </returns>
        Task<PagedList<WorkshopCoach>> GetWorkshopCoaches(UserParam userParam);

        /// <summary>
        ///     This function is responsible for get specific workshop coach from database
        /// </summary>
        /// <param name="id">
        ///     the id of the workshop coach that we want to return
        /// </param>
        /// <returns>
        ///     single workshop coach
        /// </returns>
        Task<WorkshopCoach> GetWorkshopCoach(int id);

        /*---------------------------------- Composition ----------------------------------------*/
        /// <summary>
        ///     This function is responsible for get all compositions from database
        /// </summary>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of compositions in a pagination format
        /// </returns>
        Task<PagedList<Composition>> GetCompositions(UserParam userParam);

        /// <summary>
        ///     This function is responsible for get all compositions to displayed in dropdown
        /// </summary>
        /// <param name="id">
        ///     the id of the composition
        /// </param>
        /// <returns>
        ///     all related coaches for specific composition
        /// </returns>
        string GetCoachesForComposition(int id);

        /// <summary>
        ///     This function is responsible for get all compositions to displayed in dropdown
        /// </summary>
        /// <param></param>
        /// <returns>
        ///     list of compositions
        /// </returns>
        Task<List<Composition>> GetCompositionsForDropDown();

        /// <summary>
        ///     This function is responsible for get specific composition from database
        /// </summary>
        /// <param name="id">
        ///     the id of the composition that we want to return
        /// </param>
        /// <returns>
        ///     single composition
        /// </returns>
        Task<Composition> GetComposition(int id);


        /// <summary>
        ///     This function is responsible for check that if this composition editable or not
        /// </summary>
        /// <param name="id">
        ///     the id of the composition that we check if it is exist 
        /// </param>
        /// <returns>
        ///     it is editable or not
        /// </returns>
        Task<bool> IsCompositionEditable(int id);

        /*---------------------------------- Composition Records ----------------------------------------*/
        /// <summary>
        ///     This function is responsible for get all composition records
        ///     which is related to one composition from database
        /// </summary>
        /// <param name="id">
        ///     id of the composition 
        /// </param>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of composition records
        /// </returns>
        List<CompositionRecord> GetCompositionRecords(int id,UserParam userParam);


        /// <summary>
        ///     This function is responsible for delete old composition records while update
        /// </summary>
        /// <param name="id">
        ///     the id of the composition
        /// </param>
        /// <returns>
        ///     deleted or not
        /// </returns>
        bool DeleteCompositionRecords(int id);



        /*---------------------------------- UserType ----------------------------------------*/

        /// <summary>
        ///     This function is responsible for get all user types from database
        /// </summary>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of user types in a pagination format
        /// </returns>
        Task<PagedList<UserType>> GetUserTypes(UserParam userParam);

        /// <summary>
        ///     This function is responsible for get specific user type from database
        /// </summary>
        /// <param name="id">
        ///     the id of the composition that we want to return
        /// </param>
        /// <returns>
        ///     single user type
        /// </returns>
        Task<UserType> GetUserType(int id);

        /*---------------------------------- User ----------------------------------------*/
        /// <summary>
        ///     This function is responsible for get all users from database
        /// </summary>
        /// </summary>
        /// <param name="userParam">
        ///     this object include pagination information such as number of page wanted,
        ///     search keyword, etc.
        /// </param>
        /// <returns>
        ///     list of users in a pagination format
        /// </returns>
        Task<PagedList<User>> GetUsers(UserParam userParam);

        /// <summary>
        ///     This function is responsible for get specific user from database
        /// </summary>
        /// <param name="id">
        ///     the id of the user that we want to return
        /// </param>
        /// <returns>
        ///     single user
        /// </returns>
        Task<User> GetUser(int id);

        /****************             Lines                *************/

        Task<PagedList<Lines>> GetLines(UserParam userParam);
        Task<Lines> UpdateLines(int id);

        /***------------------- Schadule train Line  -----------------*/
        Task<PagedList<Schedule>> GetSchedule(UserParam userParam);
        Task<Schedule> updateSchedule(int id);
        Task<List<Schedule>> GetScheduleForDropDown();
       /*------------------------------- ticket type --------------------------------*/

        Task<PagedList<TicketTypes>> GetTicketTypes(UserParam userParam);

        Task<TicketTypes> GetTicketTypes(int id);

        /*------------------------------- Passenger type --------------------------------*/
        Task<PagedList<PassengersType>> GetPassengerType(UserParam userParam);

        Task<PassengersType> GetPassengerType(int id);
       

        /*------------------------------- Time_Table_Station --------------------------------*/
        Task<PagedList<Time_Tabale_Stations>> GetTime_Table_Station(UserParam userParam);
        Task<Time_Tabale_Stations> GetTime_Table_Stations(int id);
        List<TimeTableStationForUpdate> GetTimeTableStationForSchedule(int id);
        List<Time_Tabale_Stations> GetTimeTableStationForScheduleOnUpdate(int id);

        int GetLineBySchedule(int id);
        ///////////////////////////***************************************  Line Stations  **************************************** ////////////////////
        Task<PagedList<LineStations>> GetLineStation(UserParam userParam);
        Task<List<LineStations>> LineStation(int id);

        Task<LineStations> updateLineStation(int id);
        List<Stations> GetStationsForLine(int id);


        Task<PagedList<Stations>> GetStations(UserParam userParam);
        Task<List<Stations>> GetStationsDropdown(UserParam userParam);
        Task<bool> checkTrain(int id);
        Task<Schedule> GetScheduleByid(int id);

        Task<Stations> GetStationsID(int id);
        Task<Stations> GetStationsOC(int idOparationCode);
        Task<bool> CheckStationId(int id);
        /*------------------------------- Region --------------------------------*/
        Task<PagedList<Region>> GetRegion(UserParam userParam);
        Task<List<Region>> GetRegionForDropDown(UserParam userParam);
        Task<Region> GetRegion(int id);
        Task<bool> CheckRegionId(int id);
        /*------------------------------- Governorate --------------------------------*/
        Task<PagedList<Governorate>> GetGovernorate(UserParam userParam);

        Task<Governorate> GetGovernorate(int id);
        Task<bool> CheckGovernorateId(int id);
        /*------------------------------- GovRegion --------------------------------*/
        Task<PagedList<Gov_Regions>> GetGovRegion(UserParam userParam);
        Task<Gov_Regions> GetGovRegion(int id);
        Task<List<Gov_Regions>> GetGovRegions(int id);
        Task<Gov_Regions> GetGovRegionitem(int idgov, int idregion);
        Task<bool> CheckGovRegionId(int id);

    }
}
