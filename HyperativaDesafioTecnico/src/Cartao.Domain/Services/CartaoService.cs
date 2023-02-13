using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;
using HyperativaDesafio.Infra.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperativaDesafio.Domain.Services
{
    public class CartaoService : ServiceBase<Cartao>, ICartaoService
    {

        private readonly ICartaoRepository _cartaoRepository;


        public CartaoService(ICartaoRepository cartaoRepository)
            : base(cartaoRepository)
        {
            _cartaoRepository = cartaoRepository;
        }

        public string GerarHashNumeroCartao(string numeroCartao)
        {
            /*
             PCI DSS Requirement 3.4
             All PAN’s strong one-way hash functions
             Fonte: https://www.pcidssguide.com/pci-dss-requirements/
             On: 2023-02-11

             */
            return SecurityService.GerarHashSha256(numeroCartao);
        }

        public string GerarMascaraNumeroCartao(string numeroCartao)
        {

            try
            {

                if (ValidarNumeroCartao(numeroCartao) == false)
                    throw new Exception("Numero inválido para Cartao de Credito.");

                return SecurityService.MarcararNumeroCartao(numeroCartao);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool ValidarNumeroCartao(string numeroCartao)
        {

            return SecurityService.ValidaNumeroCartao(numeroCartao);

        }

        public Cartao CadastrarCartaoManual(Cartao novoCartao)
        {

            try
            {
                Lote loteManual = _cartaoRepository.ObtemLoteParaCadastroManual();

                novoCartao.lote = loteManual.id;

                var cartaoCadastrado = _cartaoRepository.CadastrarCartao(novoCartao);

                return cartaoCadastrado;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<Cartao> ObterCartaoPorHashNumero(string hashNumber)
        {
            return _cartaoRepository.ObterCartaoPorHashNumero(hashNumber);
        }

        public void Add(Cartao cartao)
        {

            CadastrarCartaoManual(cartao);

        }

        public ResumoProcessamento ProcessarArquivo(string caminhoCompletoArquivo)
        {
            ResumoProcessamento resumoProcessamento = new("", 0);
            Lote loteArquivo = new();
            try
            {
                //Arquivo Existe?
                if (FileUtil.FileExist(caminhoCompletoArquivo) == false)
                {
                    resumoProcessamento.resultadoProcessamento = "Arquivo Não Localizado.";
                    throw new Exception($"Arquivo não localizado no caminho: {caminhoCompletoArquivo}.");
                }

                string[] linhas = File.ReadAllLines(caminhoCompletoArquivo);
                int qtdeRegistrosOk = 0;
                int qtdeRegistrosErro = 0;
                int numLinhaAtual = 0;


                foreach (var linha in linhas)
                {

                    DetalheProcessamentoArquivo detalheProcessamentoArquivo = new();

                    //Header
                    if (numLinhaAtual == 0)
                    {
                        
                        detalheProcessamentoArquivo = ValidarHeader(linha);
                        resumoProcessamento.detalheProcessamentoArquivo.Add(detalheProcessamentoArquivo);

                        if (detalheProcessamentoArquivo.retorno == "OK") 
                        {
                            loteArquivo = _cartaoRepository.ObterLoteParaArquivo(linha);
                            
                            if(loteArquivo== null)
                            {
                                throw new Exception("Erro ao criar o Lote");
                            }

                            qtdeRegistrosOk++; 
                        
                        
                        } else {
                            throw new Exception("Erro ao localizar as informações do Lote");
                        }

                        continue;
                    }

                    //Trailer
                    if (linha.Substring(0, 4) == "LOTE")
                    {

                        detalheProcessamentoArquivo = ValidarTrailer(linha,numLinhaAtual);
                        resumoProcessamento.detalheProcessamentoArquivo.Add(detalheProcessamentoArquivo);
                        if (detalheProcessamentoArquivo.retorno == "OK") { qtdeRegistrosOk++; } else { qtdeRegistrosErro++; }
                        break;
                    }

                    //Conteudo
                    detalheProcessamentoArquivo = ValidaLinhaConteudo(linha,numLinhaAtual);

                    if (detalheProcessamentoArquivo.retorno == "OK") {

                        Cartao novoCartao = new();

                        novoCartao.lote = loteArquivo.id;
                        novoCartao.numeroMascara = SecurityService.MarcararNumeroCartao(linha.Substring(8, 18));
                        novoCartao.numeracaoNoLote = linha.Substring(1, 1);
                        novoCartao.dataCadastro = DateTime.Now;
                        novoCartao.numeroHash = SecurityService.GerarHashSha256(linha.Substring(8, 18));

                        var cartaoCadastrado = _cartaoRepository.CadastrarCartao(novoCartao);

                        if (cartaoCadastrado == null) {
                            detalheProcessamentoArquivo.retorno = "ERRO";
                            detalheProcessamentoArquivo.mensagem += "Erro ao cadastrar o cartao";
                        }
                        else 
                        {
                            detalheProcessamentoArquivo.mensagem += $"Cadastrado {cartaoCadastrado.numeroMascara}.";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                resumoProcessamento.resultadoProcessamento = ex.Message;
            }

            return resumoProcessamento;

        }

        private DetalheProcessamentoArquivo ValidaLinhaConteudo(string linha,int numLinha)
        {
            DetalheProcessamentoArquivo detalhe = new();
            detalhe.linha = numLinha;
            detalhe.retorno = "OK";
            detalhe.mensagem = "OK";

            //Layout linha
            if(linha.Length != 51)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem = $"Linha fora do layout, esperado [51] recebido {linha.Length}";

            }

            //Identificador linha
            if (linha.Substring(0, 1).ToUpper() != "C")
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem = $"[01-01] esperado [C] recebido {linha.Substring(0, 1)}";

            }

            //Numero do cartao
            if ( ValidarNumeroCartao(linha.Substring(8, 18)) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem = $"Número do cartao inválido}";

            }


            return detalhe;
        }

        private DetalheProcessamentoArquivo ValidarHeader(string linhaHeader)
        {
            DetalheProcessamentoArquivo detalhe = new();
            detalhe.linha = 0;
            detalhe.retorno = "OK";
            detalhe.mensagem = "OK";

            if (linhaHeader.Length != 51)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem = $"Linha fora do layout, esperado [51] recebido {linhaHeader.Length}. ";
            }



            if(linhaHeader.Substring(0, 29).Trim().ToUpper() != "DESAFIO-HYPERATIVA")
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[01-29] esperado [DESAFIO-HYPERATIVA] recebido {linhaHeader.Substring(0, 29)}";
            }

            if ( DateOnly.TryParse(linhaHeader.Substring(29, 8), out _) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[30-37] esperado [yyyyMMdd] recebido {linhaHeader.Substring(29, 8)}";
            }


            if (int.TryParse(linhaHeader.Substring(45, 6),out _) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[46-51] esperado [NNNNNN] recebido {linhaHeader.Substring(45, 6)}";
            }


            return detalhe;

        }

        private string ValidarLinhaConteudo(string linhaConteudo)
        {
            return "";
        }

        private DetalheProcessamentoArquivo ValidarTrailer(string linhaTrailer, int numLinha)
        {
            DetalheProcessamentoArquivo detalhe = new();
            detalhe.linha = numLinha;
            detalhe.retorno = "OK";
            detalhe.mensagem = "OK";

            if (linhaTrailer.Length != 51)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem = $"Linha fora do layout, esperado 51 recebido {linhaTrailer.Length}.";
            }


            if (linhaTrailer.Substring(0, 4).ToUpper() != "LOTE" || int.TryParse(linhaTrailer.Substring(4, 4), out _) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[01-08] Esperado LOTENNNN recebido {linhaTrailer.Substring(0, 8)}.";
            }

            if (int.TryParse(linhaTrailer.Substring(8, 14), out _) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[09-14] Esperado NNNNNN recebido {linhaTrailer.Substring(8, 6)}.";
            }


            return detalhe;

        }

    }
}
