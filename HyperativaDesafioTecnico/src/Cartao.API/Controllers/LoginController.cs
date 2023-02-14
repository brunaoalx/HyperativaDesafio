using AutoMapper;
using HyperativaDesafio.API.Contracts.Request;
using HyperativaDesafio.API.Contracts.Response;
using HyperativaDesafio.API.Helper;
using HyperativaDesafio.Application.Interfaces;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace HyperativaDesafio.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IUsuarioAppService _usuarioAppService;
        private readonly IMapper _mapper;

        public LoginController(IUsuarioAppService usuarioAppService, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioAppService = usuarioAppService;
        }

        [HttpPost("~/api/v1/Cartao/Login")]

        public LoginResponse Login(LoginRequest loginRequest)
        {

            var retornoLogin = new LoginResponse();


            try
            {

                Log.Information("Login - Inicio - Usuario : {login}", loginRequest.login);


                var usuarioLogin = _mapper.Map<LoginRequest, Usuario>(loginRequest);

                var usuarioLocalizado = _usuarioAppService.GetUsuario(usuarioLogin.login, usuarioLogin.senha);

                if (usuarioLocalizado.id > 0)
                {
                    var token = TokenHelper.GetToken(usuarioLocalizado);
                    retornoLogin.token = token;
                    retornoLogin.message = "Sucesso";
                    Response.StatusCode = Ok().StatusCode;
                }
                else
                {
                    retornoLogin.message = "Usuario ou senha invalidos.";
                    Response.StatusCode = NotFound().StatusCode;
                }

                Log.Information("Login - Fim - Retorno : {retorno}", JsonSerializer.Serialize(retornoLogin));

            }
            catch (Exception ex)
            {
                Log.Error("Login - Erro - Retorno : {erro}", JsonSerializer.Serialize(ex));
                Response.StatusCode = 500;
                retornoLogin.message = "Ocorreu um erro, tente novamente.";

            }

            return retornoLogin;
        }

    }
}
