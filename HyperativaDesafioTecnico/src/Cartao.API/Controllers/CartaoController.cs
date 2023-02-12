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


    [Route("api/[controller]")]
    [ApiController]
    public class CartaoController : ControllerBase
    {

        private readonly ICartaoAppService _cartaoAppService;

        public CartaoController(ICartaoAppService cartaoAppService)
        {
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
        [HttpPost]
        public CartaoCreateResponse Post([FromBody] CartaoCreateRequest cartaoNovo)
        {
            //Cadastrar Cartao

            var retCartao = new CartaoCreateResponse();
            

            if (_cartaoAppService.ValidarNumeroCartao(cartaoNovo.numero) == false)
            {
                retCartao.message = "Número inválido!";
                Response.StatusCode = BadRequest().StatusCode;
                return retCartao;
            }

            //var cartaoParaCadastro

            return retCartao;
        }

        [HttpPost]
        public CartaoCreateResponse PostFile(IFormFile fileLoadCartao)
        {
            CartaoCreateResponse cartaoCreateResponse = new();

            try
            {
                if (fileLoadCartao != null )
                {
                    //await _fileService.SaveFiles(files);
                    Response.StatusCode = Ok().StatusCode;
                    
                }
                else
                {
                    Response.StatusCode =  BadRequest().StatusCode;
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500 ;
                cartaoCreateResponse.message = "Ocorreu um erro ao processar o arquivo.";
            }

            return cartaoCreateResponse;

        }
    }
}
