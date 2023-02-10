using AutoMapper;
using HyperativaDesafio.API.Contracts.Request;
using HyperativaDesafio.Domain.Entities;

namespace HyperativaDesafio.API.AutoMapper
{
    public class ContractToDomainMappingProfile : Profile
    {
        public ContractToDomainMappingProfile()
        {
            CreateMap<CartaoCreateRequest,Cartao>();
        }

    }
}
