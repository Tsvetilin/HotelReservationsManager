using AutoMapper;
using Data.Models;
using Web.Models.Clients;
using Web.Models.InputModels;
using Web.Models.Reservations;
using Web.Models.Rooms;
using Web.Models.ViewModels;

namespace Web.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Reservation, ReservationViewModel>();
            CreateMap<ClientViewModel, ClientData>();
            CreateMap<ClientData, ClientViewModel>();
            CreateMap<EmployeeData, EmployeeDataViewModel>();
            CreateMap<ClientData, ClientInputModel>();
            CreateMap<ClientInputModel, ClientData>();
            CreateMap<ReservationInputModel, Reservation>();
            CreateMap<Reservation, ReservationPeriod>();
            CreateMap<ReservationViewModel, ReservationPeriod>();
            CreateMap<Reservation, ReservationInputModel>();
            CreateMap<EmployeeData, EmployeeDataViewModel>();
            CreateMap<EmployeeData, EmployeeInputModel>();
            CreateMap<ApplicationUser, EmployeeDataViewModel>();
            CreateMap<Room, RoomViewModel>();
            CreateMap<Room, RoomInputModel>();
            CreateMap<EmployeeData, ApplicationUser>();
            CreateMap<ApplicationUser, EmployeeInputModel>();
        }
    }
}
