using AutoMapper;
using Data.Models;
using Web.Models.Clients;
using Web.Models.Reservations;

namespace Web.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Reservation, ReservationViewModel>();
            CreateMap<ClientData, ClientViewModel>();
        }
    }
}
