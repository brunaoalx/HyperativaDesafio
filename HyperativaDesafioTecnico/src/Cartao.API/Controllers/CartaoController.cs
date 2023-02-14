using Microsoft.AspNetCore.Mvc;
using HyperativaDesafio.API.Contracts.Request;
using HyperativaDesafio.API.Contracts.Response;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Net;
using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Entities;
using AutoMapper;
using HyperativaDesafio.Infra.Util;
using Serilog;
using HyperativaDesafio.Domain.Services;
using System.Text.Json;

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
        [HttpGet("~/api/v1/Cartao/ConsultarNumeroCartao")]
        public CartaoGetResponse ConsultarNumeroCartao([FromQuery] CartaoGetRequest cartaoConsultado)
        {
            var dadosRetorno = new CartaoGetResponse();

            try
            {
                Log.Information("ConsultarNumeroCartao - Inicio - Consultar Cartao : {cartao}", SecurityService.MascararNumeroCartao(cartaoConsultado.numero));

                if (SecurityService.ValidaNumeroCartao(cartaoConsultado.numero) == false)
                {
                    dadosRetorno.numero = cartaoConsultado.numero;
                    dadosRetorno.message = "Numero de cartão inválido";
                    Response.StatusCode = BadRequest().StatusCode;
                }
                else
                {

                    //Consultar Cartão
                    var cartaoParaConsulta = _mapper.Map<CartaoGetRequest, Cartao>(cartaoConsultado);

                    var cartaoLocalizado = _cartaoAppService.ObterCartaoPorHashNumero(cartaoParaConsulta.numeroHash).FirstOrDefault();

                    if (cartaoLocalizado != null)
                    {
                        dadosRetorno.numero = cartaoLocalizado.numeroMascara + "|" + cartaoLocalizado.numeroHash.Substring(0, 15);
                        dadosRetorno.message = "Cartao localizado";
                        Response.StatusCode = Ok().StatusCode;
                    }
                    else
                    {
                        dadosRetorno.message = "Cartao não localizado";
                        Response.StatusCode = NotFound().StatusCode;
                    }
                }

                Log.Information("ConsultarNumeroCartao - Fim - Retorno : {retorno}", JsonSerializer.Serialize(dadosRetorno));
            }
            catch (Exception ex)
            {

                Log.Error("ConsultarNumeroCartao - Erro : {erro}", JsonSerializer.Serialize(ex));
                dadosRetorno.message = "Ocorreu um erro inesperado, tente novamente.";
                Response.StatusCode = 500;
            }
            
            return dadosRetorno;
        }

        // POST api/<CartaoController>
        [HttpPost("~/api/v1/Cartao/CadastraCartaoAvulso")]        
        public CartaoCreateResponse CadastraCartaoAvulso([FromBody] CartaoCreateRequest cartaoNovo)
        {
            //Cadastrar Cartao

            var retCartao = new CartaoCreateResponse();

            try
            {

                Log.Information("CadastraCartaoAvulso - Inicio - Cadastrar Cartao : {cartao}", SecurityService.MascararNumeroCartao(cartaoNovo.numero));

                if (_cartaoAppService.ValidarNumeroCartao(cartaoNovo.numero) == false)
                {
                    retCartao.message = "Número inválido!";
                    Response.StatusCode = BadRequest().StatusCode;
                }
                else
                {
                    var cartaoParaCadastro = _mapper.Map<CartaoCreateRequest, Cartao>(cartaoNovo);
                    _cartaoAppService.CadastrarCartaoManual(cartaoParaCadastro);
                    retCartao.message = "Cartao Cadastrado com Sucesso.";

                    Response.StatusCode = Ok().StatusCode;
                    
                }

                Log.Information("CadastraCartaoAvulso - Fim - Retorno : {retorno}", JsonSerializer.Serialize(retCartao));

            }
            catch (Exception ex)
            {
                Log.Error("CadastraCartaoAvulso - Erro : {erro}", JsonSerializer.Serialize(ex));
                retCartao.message = "Erro ao cadastrar cartao.";
                Response.StatusCode = 500;
            }

            return retCartao;

        }

        [HttpPost("~/api/v1/Cartao/CadastrarCartaoViaArquivo")]
        public CadastrarCartaoViaArquivoResponse CadastrarCartaoViaArquivo(IFormFile fileLoadCartao)
        {
            CadastrarCartaoViaArquivoResponse retorno = new();

            try
            {

                Log.Information("CadastrarCartaoViaArquivo - Inicio - Arquivo: {arquivo}", JsonSerializer.Serialize(fileLoadCartao.FileName));

                if (fileLoadCartao != null)
                {

                    string pathToSave = Environment.CurrentDirectory + "\\Arquivos_Carga";

                    string fullpath = FileUtil.SaveFile(fileLoadCartao, pathToSave);

                    if (string.IsNullOrEmpty(fullpath))
                        throw new Exception($"Ero ao salvar o arquivo {fileLoadCartao.FileName} no path: {pathToSave}.");

                    var retornoCadastro =  _cartaoAppService.ProcessarArquivo(fullpath);

                    retorno = _mapper.Map<CadastrarCartaoViaArquivoResponse>(retornoCadastro);

                    Response.StatusCode = Ok().StatusCode;
                    
                }
                else
                {
                    Response.StatusCode = BadRequest().StatusCode;
                    retorno.resultadoProcessamento = "Arquivo Não Recebido";
                }

                Log.Information("CadastrarCartaoViaArquivo - Fim - Retorno : {retorno}", JsonSerializer.Serialize(retorno));

            }
            catch (Exception ex)
            {
                Log.Error("CadastrarCartaoViaArquivo - Erro : {erro}", JsonSerializer.Serialize(ex));
                Response.StatusCode = 500;
                retorno.resultadoProcessamento = "Ocorreu um erro ao processar o arquivo.";
            }

            return retorno;

        }
    }
}
