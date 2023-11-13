using AutoMapper;
using System;
using ToolsApp.EntityFramework;
using ToolsApp.Models;


namespace ToolsApp.App_Start
{
    public static class Mapper
    {
        private static IMapper _mapper;
        public static void RegisterMappings()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                #region User
                cfg.CreateMap<AspNetUser, QL_UserViewModel>()
                                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dto => dto.Username, opt => opt.MapFrom(src => src.UserName))
                                .ForMember(dto => dto.Fullname, opt => opt.MapFrom(src => src.Fullname))
                                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email));

                cfg.CreateMap<QL_UserViewModel, AspNetUser>()
                                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.Username))
                                .ForMember(dto => dto.Fullname, opt => opt.MapFrom(src => src.Fullname))
                                .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email));
                #endregion

                #region Page
                cfg.CreateMap<Page, QLKTPPageViewModel>()
                                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dto => dto.Controler, opt => opt.MapFrom(src => src.Controler))
                                .ForMember(dto => dto.Action, opt => opt.MapFrom(src => src.Action))
                                .ForMember(dto => dto.Code, opt => opt.MapFrom(src => src.Code))
                                .ForMember(dto => dto.Info, opt => opt.MapFrom(src => src.Info))
                                .ForMember(dto => dto.UserCreate, opt => opt.MapFrom(src => src.UserCreate))
                                .ForMember(dto => dto.UserFullnameCreate, opt => opt.MapFrom(src => src.UserFullnameCreate))
                                .ForMember(dto => dto.DatetimeCreate, opt => opt.MapFrom(src => src.DatetimeCreate))
                                .ForMember(dto => dto.UserUpdate, opt => opt.MapFrom(src => src.UserUpdate))
                                .ForMember(dto => dto.UserFullnameUpdate, opt => opt.MapFrom(src => src.UserFullnameUpdate))
                                .ForMember(dto => dto.DatetimeUpdate, opt => opt.MapFrom(src => src.DatetimeUpdate))
                                .ForMember(dto => dto.isDelete, opt => opt.MapFrom(src => src.isDelete))
                                .ForMember(dto => dto.UserDelete, opt => opt.MapFrom(src => src.UserDelete))
                                .ForMember(dto => dto.UserFullnameDelete, opt => opt.MapFrom(src => src.UserFullnameDelete))
                                .ForMember(dto => dto.DatetimeDelete, opt => opt.MapFrom(src => src.DatetimeDelete));

                cfg.CreateMap<QLKTPPageViewModel, Page>()
                                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dto => dto.Controler, opt => opt.MapFrom(src => src.Controler))
                                .ForMember(dto => dto.Action, opt => opt.MapFrom(src => src.Action))
                                .ForMember(dto => dto.Code, opt => opt.MapFrom(src => src.Code))
                                .ForMember(dto => dto.Info, opt => opt.MapFrom(src => src.Info))
                                .ForMember(dto => dto.UserCreate, opt => opt.MapFrom(src => src.UserCreate))
                                .ForMember(dto => dto.UserFullnameCreate, opt => opt.MapFrom(src => src.UserFullnameCreate))
                                .ForMember(dto => dto.DatetimeCreate, opt => opt.MapFrom(src => src.DatetimeCreate))
                                .ForMember(dto => dto.UserUpdate, opt => opt.MapFrom(src => src.UserUpdate))
                                .ForMember(dto => dto.UserFullnameUpdate, opt => opt.MapFrom(src => src.UserFullnameUpdate))
                                .ForMember(dto => dto.DatetimeUpdate, opt => opt.MapFrom(src => src.DatetimeUpdate))
                                .ForMember(dto => dto.isDelete, opt => opt.MapFrom(src => src.isDelete))
                                .ForMember(dto => dto.UserDelete, opt => opt.MapFrom(src => src.UserDelete))
                                .ForMember(dto => dto.UserFullnameDelete, opt => opt.MapFrom(src => src.UserFullnameDelete))
                                .ForMember(dto => dto.DatetimeDelete, opt => opt.MapFrom(src => src.DatetimeDelete));
                #endregion

                #region Menu
                cfg.CreateMap<Menu, QLKTPMenuViewModel>()
                                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dto => dto.MenuName, opt => opt.MapFrom(src => src.MenuName))
                                .ForMember(dto => dto.ParentId, opt => opt.MapFrom(src => src.ParentId))
                                .ForMember(dto => dto.PageId, opt => opt.MapFrom(src => src.PageId))
                                .ForMember(dto => dto.Icon, opt => opt.MapFrom(src => src.Icon))
                                .ForMember(dto => dto.OrderNo, opt => opt.MapFrom(src => src.OrderNo))
                                .ForMember(dto => dto.UserCreate, opt => opt.MapFrom(src => src.UserCreate))
                                .ForMember(dto => dto.UserFullnameCreate, opt => opt.MapFrom(src => src.UserFullnameCreate))
                                .ForMember(dto => dto.DatetimeCreate, opt => opt.MapFrom(src => src.DatetimeCreate))
                                .ForMember(dto => dto.UserUpdate, opt => opt.MapFrom(src => src.UserUpdate))
                                .ForMember(dto => dto.UserFullnameUpdate, opt => opt.MapFrom(src => src.UserFullnameUpdate))
                                .ForMember(dto => dto.DatetimeUpdate, opt => opt.MapFrom(src => src.DatetimeUpdate))
                                .ForMember(dto => dto.isDelete, opt => opt.MapFrom(src => src.isDelete))
                                .ForMember(dto => dto.UserDelete, opt => opt.MapFrom(src => src.UserDelete))
                                .ForMember(dto => dto.UserFullnameDelete, opt => opt.MapFrom(src => src.UserFullnameDelete))
                                .ForMember(dto => dto.DatetimeDelete, opt => opt.MapFrom(src => src.DatetimeDelete));

                cfg.CreateMap<QLKTPMenuViewModel, Menu>()
                                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                                .ForMember(dto => dto.MenuName, opt => opt.MapFrom(src => src.MenuName))
                                .ForMember(dto => dto.ParentId, opt => opt.MapFrom(src => src.ParentId))
                                .ForMember(dto => dto.PageId, opt => opt.MapFrom(src => src.PageId))
                                .ForMember(dto => dto.Icon, opt => opt.MapFrom(src => src.Icon))
                                .ForMember(dto => dto.OrderNo, opt => opt.MapFrom(src => src.OrderNo))
                                .ForMember(dto => dto.UserCreate, opt => opt.MapFrom(src => src.UserCreate))
                                .ForMember(dto => dto.UserFullnameCreate, opt => opt.MapFrom(src => src.UserFullnameCreate))
                                .ForMember(dto => dto.DatetimeCreate, opt => opt.MapFrom(src => src.DatetimeCreate))
                                .ForMember(dto => dto.UserUpdate, opt => opt.MapFrom(src => src.UserUpdate))
                                .ForMember(dto => dto.UserFullnameUpdate, opt => opt.MapFrom(src => src.UserFullnameUpdate))
                                .ForMember(dto => dto.DatetimeUpdate, opt => opt.MapFrom(src => src.DatetimeUpdate))
                                .ForMember(dto => dto.isDelete, opt => opt.MapFrom(src => src.isDelete))
                                .ForMember(dto => dto.UserDelete, opt => opt.MapFrom(src => src.UserDelete))
                                .ForMember(dto => dto.UserFullnameDelete, opt => opt.MapFrom(src => src.UserFullnameDelete))
                                .ForMember(dto => dto.DatetimeDelete, opt => opt.MapFrom(src => src.DatetimeDelete));
                #endregion






            })
            {

            };
            _mapper = mapperConfiguration.CreateMapper();
        }

        //internal static object MapFrom(Qr_Event model)
        //{
        //    throw new NotImplementedException();
        //}

        #region User
        public static QL_UserViewModel MapFrom(AspNetUser data)
        {
            return _mapper.Map<AspNetUser, QL_UserViewModel>(data);
        }
        public static AspNetUser MapFrom(QL_UserViewModel data)
        {
            return _mapper.Map<QL_UserViewModel, AspNetUser>(data);
        }
        public static AspNetUser MapFrom(QL_UserViewModel data_view, AspNetUser data_entity)
        {
            return _mapper.Map(data_view, data_entity);
        }
        #endregion

        #region Page
        public static QLKTPPageViewModel MapFrom(Page data)
        {
            return _mapper.Map<Page, QLKTPPageViewModel>(data);
        }
        public static Page MapFrom(QLKTPPageViewModel data)
        {
            return _mapper.Map<QLKTPPageViewModel, Page>(data);
        }
        public static Page MapFrom(QLKTPPageViewModel data_view, Page data_entity)
        {
            return _mapper.Map(data_view, data_entity);
        }
        #endregion


        #region Menu
        public static QLKTPMenuViewModel MapFrom(Menu data)
        {
            return _mapper.Map<Menu, QLKTPMenuViewModel>(data);
        }
        public static Menu MapFrom(QLKTPMenuViewModel data)
        {
            return _mapper.Map<QLKTPMenuViewModel, Menu>(data);
        }
        public static Menu MapFrom(QLKTPMenuViewModel data_view, Menu data_entity)
        {
            return _mapper.Map(data_view, data_entity);
        }
        #endregion


    }
}