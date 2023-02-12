using Microsoft.AspNetCore.Mvc;
using HyperativaDesafio.API.Contracts.Request;
using HyperativaDesafio.API.Contracts.Response;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Net;
using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Entities;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HyperativaDesafio.API.Controllers
{


    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartaoController : ControllerBase
    {

        private readonly ICartaoAppService _cartaoAppService;
        private readonly IMapper _mapper;

        public CartaoController(ICartaoAppService cartaoAppService, IMapper mapper)
        {
            _mapper = mapper;
            _cartaoAppService = cartaoAppService;
        }

        // GET: api/<CartaoController>
        [HttpGet]
        public CartaoGetResponse Get(CartaoGetRequest cartaoConsultado)
        {
            //Consultar Cartão
            return new CartaoGetResponse();
        }

        // POST api/<CartaoController>
        [HttpPost("~/api/v1/Cartao/CadastraCartaoAvulso")]        
        public CartaoCreateResponse CadastraCartaoAvulso([FromBody] CartaoCreateRequest cartaoNovo)
        {
            //Cadastrar Cartao

            var retCartao = new CartaoCreateResponse();

            try
            {
                

                if (_cartaoAppService.ValidarNumeroCartao(cartaoNovo.numero) == false)
                {
                    retCartao.message = "Número inválido!";
                    Response.StatusCode = BadRequest().StatusCode;
                    return retCartao;
                }

                var cartaoParaCadastro = _mapper.Map<CartaoCreateRequest, Cartao>(cartaoNovo);

                _cartaoAppService.CadastrarCartaoManual(cartaoParaCadastro);

                retCartao.message = "Cartao Cadastrado com Sucesso.";
                Response.StatusCode = Ok().StatusCode;

                

            }
            catch (Exception)
            {
                retCartao.message = "Erro ao cadastrar cartao.";
                Response.StatusCode = 500;
            }

            return retCartao;

        }

        [HttpPost("~/api/v1/Cartao/CadastrarCartaoViaArquivo")]
        public CartaoCreateResponse CadastrarCartaoViaArquivo(IFormFile fileLoadCartao)
        {
            CartaoCreateResponse cartaoCreateResponse = new();

            try
            {
                if (fileLoadCartao != null)
                {
                    //await _fileService.SaveFiles(files);
                    Response.StatusCode = Ok().StatusCode;

                }
                else
                {
                    Response.StatusCode = BadRequest().StatusCode;
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                cartaoCreateResponse.message = "Ocorreu um erro ao processar o arquivo.";
            }

            return cartaoCreateResponse;

        }
    }
}
