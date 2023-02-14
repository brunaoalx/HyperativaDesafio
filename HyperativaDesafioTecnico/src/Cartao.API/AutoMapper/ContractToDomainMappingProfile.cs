using AutoMapper;
using HyperativaDesafio.API.Contracts.Request;
using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Services;

namespace HyperativaDesafio.API.AutoMapper
{
    public class ContractToDomainMappingProfile : Profile
    {
        
        public ContractToDomainMappingProfile()
        {
        
            CreateMap<CartaoCreateRequest,Cartao>()
                .ForMember(x => x.dataCadastro, dtNow => dtNow.MapFrom(dtNow => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")))
                .ForMember(x => x.numeroMascara, nMask => nMask.MapFrom( nMask => SecurityService.MascararNumeroCartao(nMask.numero)))
                .ForMember(x => x.numeroHash, nHash => nHash.MapFrom(nHash => SecurityService.GerarHashSha256(nHash.numero)));

            CreateMap<CartaoGetRequest, Cartao>()
                .ForMember(x => x.dataCadastro, dtNow => dtNow.MapFrom(dtNow => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")))
                .ForMember(x => x.numeroMascara, nMask => nMask.MapFrom(nMask => SecurityService.MascararNumeroCartao(nMask.numero)))
                .ForMember(x => x.numeroHash, nHash => nHash.MapFrom(nHash => SecurityService.GerarHashSha256(nHash.numero)));


            CreateMap<LoginRequest, Usuario>()
                .ForMember(x => x.senha, senhaHash => senhaHash.MapFrom(senhaHash => SecurityService.GerarHashSha256(senhaHash.senha)));


        }

    }
}
