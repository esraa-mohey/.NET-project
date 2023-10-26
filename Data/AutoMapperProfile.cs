using AutoMapper;
using ERNST.Dto.Coach;
using ERNST.Dto.CoachClass;
using ERNST.Dto.Composition;
using ERNST.Dto.CompositionRecord;
using ERNST.Dto.Region;
using ERNST.Dto.Governorate;
using ERNST.Dto.Stations;
using ERNST.Dto.Train;
using ERNST.Dto.TrainType;
using ERNST.Dto.User;
using ERNST.Dto.UserType;
using ERNST.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERNST.Dto.GovRegion;
using ERNST.Dto.WorkshopCoach;
using ERNST.Dto.LineStation;
using ERNST.Dto;

namespace ERNST.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TrainType, TrainTypeForDisplay>();

            CreateMap<Train, TrainForDisplay>()
                .ForMember(dest => dest.TrainTypeArName, opt => { opt.MapFrom(src => src.TrainType.DescAr); })
                .ForMember(dest => dest.TrainTypeEnName, opt => { opt.MapFrom(src => src.TrainType.DescEn); })
                .ForMember(dest => dest.CompositionArName, opt => { opt.MapFrom(src => src.Composition.ArName); })
                .ForMember(dest => dest.CompositionEnName, opt => { opt.MapFrom(src => src.Composition.EnName); });

            CreateMap<CoachClass, CoachClassForDisplay>();
            CreateMap<Schedule, StationsForDisplay>();
            CreateMap<Schedule, ScheduleDto>()
            .ForMember(dest => dest.TrainNumber, opt =>
             {
                 opt.MapFrom(src => src.Train.Number);

             });

            CreateMap<Coach, CoachForDisplay>()
                .ForMember(dest => dest.CoachClassEnName, opt => { opt.MapFrom(src => src.CoachClass.EnName); })
                .ForMember(dest => dest.CoachClassArName, opt => { opt.MapFrom(src => src.CoachClass.ArName); });

            CreateMap<WorkshopCoach, WorkshopCoachForDisplay>()
                .ForMember(dest => dest.CoachEnName, opt => { opt.MapFrom(src => src.Coach.EnName); })
                .ForMember(dest => dest.CoachArName, opt => { opt.MapFrom(src => src.Coach.ArName); });

            CreateMap<Composition, CompositionForDisplay>();

            CreateMap<CompositionRecord, CompositionRecordForDisplay>()
                .ForMember(dest => dest.CoachArName, opt => { opt.MapFrom(src => src.Coach.ArName); });

            CreateMap<UserType, UserTypeForDisplay>();


            CreateMap<User, UserForDisplay>()
                .ForMember(dest => dest.UserTypeName, opt =>
                {
                    opt.MapFrom(src => src.UserType.TypeName);
                });

            ///////////////////////////////////////////////////////////////////


            // CreateMap<Gov_Regions, RegionForDisplay>()


            CreateMap<Gov_Regions, GovRegionForDisplay>()
               .ForMember(dest => dest.GovarName, opt =>
               {
                   opt.MapFrom(src => src.Governorate.ArName);
               })
                .ForMember(dest => dest.GovenName, opt =>
                {
                    opt.MapFrom(src => src.Governorate.EnName);
                })
               .ForMember(dest => dest.RegionName, opt =>
               {
                   opt.MapFrom(src => src.Region.ArName);
               });

            CreateMap<LineStations, GetLineStation>()
   .ForMember(dest => dest.LineAr, opt =>
   {
       opt.MapFrom(src => src.Lines.ArName);
   })
      .ForMember(dest => dest.LineStationId, opt =>
      {
          opt.MapFrom(src => src.Id);
      })
    .ForMember(dest => dest.LineEn, opt =>
    {
        opt.MapFrom(src => src.Lines.EnName);
    })
   .ForMember(dest => dest.StationsAr, opt =>
   {
       opt.MapFrom(src => src.Stations.ArName);
   })
   .ForMember(dest => dest.StationsEn, opt =>
   {
       opt.MapFrom(src => src.Stations.EnName);
   })
   .ForMember(dest => dest.OperationCode, opt =>
   {
       opt.MapFrom(src => src.Stations.OperationCode);
   });

            CreateMap<Stations, StationsForDisplay>()
                   .ForMember(dest => dest.GovernorateId, opt =>
                   {
                       opt.MapFrom(src => src.GovRegions.GovernorateId);

                   })
               .ForMember(dest => dest.RegionId, opt =>
               {
                   opt.MapFrom(src => src.GovRegions.RegionId);

               })
               .ForMember(dest => dest.RegionName, opt =>
               {
                   opt.MapFrom(src => src.GovRegions.Region.ArName);

               })
                 .ForMember(dest => dest.GovRegionsId, opt =>
                 {
                     opt.MapFrom(src => src.GovRegionsId);

                 })
            .ForMember(dest => dest.GovernorateName, opt =>
            {
                opt.MapFrom(src => src.GovRegions.Governorate.ArName);

            });


        }
    }
}