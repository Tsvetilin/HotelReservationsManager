using AutoMapper;
using Data.Models;
using Web.Models.Clients;
using Web.Models.InputModels;
using Web.Models.Reservations;
using Web.Models.ViewModels;

namespace Web.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Reservation, ReservationViewModel>();
            CreateMap<ClientData, ClientViewModel>();
            CreateMap<EmployeeData, EmployeeDataViewModel>();          
            CreateMap<EmployeeData, EmployeeInputModel>();
            CreateMap<ApplicationUser, EmployeeDataViewModel>();
            CreateMap<Room, RoomDataViewModel>();
        }
    }
}
