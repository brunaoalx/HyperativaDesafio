using AutoMapper;
using HyperativaDesafio.API.Contracts.Response;
using HyperativaDesafio.Domain.Entities;


namespace HyperativaDesafio.API.AutoMapper
{
    public class DomainToContractMappingProfile : Profile
    {
        public DomainToContractMappingProfile()
        {
            CreateMap<Cartao, CartaoGetResponse>();
             
        }

    }
}
