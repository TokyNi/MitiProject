using AutoMapper;
using MitiConsulting.ApplicationCore.DTOs;
using MitiConsulting.Domain.Interfaces;
using MitiConsulting.Domain.Models;

namespace MitiConsulting.Application.Profil
    {
    public class RapportProfile : Profile
    {
            public RapportProfile()
            {
                CreateMap<CreatRapportDTO,Rapport>().ForMember(r => r.IdRapport ,opt => opt.Ignore());
                CreateMap<Rapport,UpdateRapportDTO>().ForAllMembers(opt=> opt.Ignore());
                CreateMap<Rapport,RapportDTO>();
                CreateMap<Rapport, ListeDTO>();

                // CreateMap<RapportViewModel,CreatRapportDTO>();
                // CreateMap<RapportViewModel,UpdateRapportDTO>();
            }
    }
}