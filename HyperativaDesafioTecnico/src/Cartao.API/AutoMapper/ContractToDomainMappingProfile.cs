using AutoMapper;
using HyperativaDesafio.API.Contracts.Request;
using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Entities;

namespace HyperativaDesafio.API.AutoMapper
{
    public class ContractToDomainMappingProfile : Profile
    {
        private readonly ICartaoAppService _cartaoAppService;

        public ContractToDomainMappingProfile(ICartaoAppService cartaoAppService)
        {
            _cartaoAppService = cartaoAppService;

            CreateMap<CartaoCreateRequest,Cartao>()
                .ForMember(x => x.dataCadastro, dtNow => dtNow.MapFrom(dtNow => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")))
                .ForMember(x => x.numeroMascara, nMask => nMask.MapFrom( nMask => _cartaoAppService.GerarMascaraNumeroCartao(nMask.numero)))
                .ForMember(x => x.numeroHash, nHash => nHash.MapFrom(nHash => _cartaoAppService.GerarMascaraNumeroCartao(nHash.numero)));
        }

    }
}
