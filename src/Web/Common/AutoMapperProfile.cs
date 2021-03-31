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
            CreateMap<ReservationInputModel, Reservation>();
            CreateMap<Reservation, ReservationPeriod>();
            CreateMap<EmployeeData, EmployeeDataViewModel>();
            CreateMap<EmployeeDataViewModel, EmployeeDataViewModel>();
            CreateMap<EmployeeData, EmployeeInputModel>();
            CreateMap<ApplicationUser, EmployeeDataViewModel>();
        }
    }
}
