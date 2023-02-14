using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Interfaces.Repositories;
using HyperativaDesafio.Domain.Interfaces.Services;
using HyperativaDesafio.Infra.Util;
using System.Globalization;

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

        //TODO Retirar os metodos que podem ser chamados diretos pela SecurityService


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

                return SecurityService.MascararNumeroCartao(numeroCartao);

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

                var cartaiJacadastrado = _cartaoRepository.ObterCartaoPorHashNumero(novoCartao.numeroHash).FirstOrDefault();

                if (cartaiJacadastrado == null)
                {

                    Lote loteManual = _cartaoRepository.ObtemLoteParaCadastroManual();

                    novoCartao.lote = loteManual.id;

                    var cartaoCadastrado = _cartaoRepository.CadastrarCartao(novoCartao);

                    return cartaoCadastrado;
                }
                else
                {
                    return cartaiJacadastrado;
                }
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
                int linhaTotalLinhaConteudo = 0;

                resumoProcessamento.qtdeTotalRegistros = linhas.Count();

                foreach (var linha in linhas)
                {

                    DetalheProcessamentoArquivo detalheProcessamentoArquivo = new();

                    #region Header
                    //Header
                    //TODO: Separar tratameto do Header em um método
                    if (numLinhaAtual == 0)
                    {

                        detalheProcessamentoArquivo = ValidarHeader(linha);

                        if (detalheProcessamentoArquivo.retorno == "OK")
                        {
                            loteArquivo = _cartaoRepository.ObterLoteParaArquivo(linha);

                            if (loteArquivo == null)
                            {
                                throw new Exception("Erro ao criar o Lote");
                            }

                            qtdeRegistrosOk++;


                        }
                        else
                        {
                            throw new Exception("Erro ao localizar as informações do Lote");
                        }

                        resumoProcessamento.detalheProcessamentoArquivo.Add(detalheProcessamentoArquivo);

                        numLinhaAtual++;
                        continue;
                    }

                    #endregion Header

                    #region Linha Vazia
                    //Linha Vazia
                    if (linha.Trim() == "")
                    {
                        detalheProcessamentoArquivo.linha = numLinhaAtual;
                        detalheProcessamentoArquivo.retorno = "ERRO";
                        detalheProcessamentoArquivo.mensagem = "Linha vazia";
                        resumoProcessamento.detalheProcessamentoArquivo.Add(detalheProcessamentoArquivo);
                        numLinhaAtual++;
                        qtdeRegistrosErro++;
                        resumoProcessamento.qtdeRegistrosErro = qtdeRegistrosErro;

                        numLinhaAtual++;
                        continue;
                    }
                    #endregion Linha Vazia

                    #region Trailer
                    //Trailer
                    //TODO: Separar tratameto do trailer em um método
                    if (linha.Substring(0, 4) == "LOTE")
                    {

                        detalheProcessamentoArquivo = ValidarTrailer(linha, numLinhaAtual);
                        resumoProcessamento.detalheProcessamentoArquivo.Add(detalheProcessamentoArquivo);

                        if (detalheProcessamentoArquivo.retorno == "OK") { qtdeRegistrosOk++; } else { qtdeRegistrosErro++; }

                        resumoProcessamento.qtdeRegistrosErro = qtdeRegistrosErro;
                        resumoProcessamento.qtdeRegistrosOk = qtdeRegistrosOk;

                        numLinhaAtual++;
                        continue;
                    }

                    #endregion Trailer

                    #region Conteudo
                    //Conteudo
                    //TODO: Separar tratameto do Conteudo em um método
                    detalheProcessamentoArquivo = ValidaLinhaConteudo(linha, numLinhaAtual);
                    linhaTotalLinhaConteudo++;
                    if (detalheProcessamentoArquivo.retorno == "OK")
                    {

                        Cartao novoCartao = new();

                        novoCartao.lote = loteArquivo.id;
                        novoCartao.numeroMascara = SecurityService.MascararNumeroCartao(linha.Substring(7, 18).Trim());
                        novoCartao.numeracaoNoLote = linha.Substring(1, 1);
                        novoCartao.dataCadastro = DateTime.Now;
                        novoCartao.numeroHash = SecurityService.GerarHashSha256(linha.Substring(7, 18).Trim());

                        if (_cartaoRepository.ObterCartaoPorHashNumero(novoCartao.numeroHash).Count() > 0)
                        {
                            qtdeRegistrosErro++;
                            detalheProcessamentoArquivo.retorno = "ERRO";
                            detalheProcessamentoArquivo.mensagem = "Cartao ja cadastrado";
                        }
                        else
                        {
                            var cartaoCadastrado = _cartaoRepository.CadastrarCartao(novoCartao);

                            if (cartaoCadastrado == null)
                            {
                                qtdeRegistrosErro++;
                                detalheProcessamentoArquivo.retorno = "ERRO";
                                detalheProcessamentoArquivo.mensagem = "Erro ao cadastrar o cartao. ";
                            }
                            else
                            {
                                qtdeRegistrosOk++;
                                detalheProcessamentoArquivo.mensagem = $"Cadastrado {cartaoCadastrado.numeroMascara}. ";
                            }
                        }
                    }
                    else
                    {
                        qtdeRegistrosErro++;
                    }

                    #endregion Conteudo

                    resumoProcessamento.detalheProcessamentoArquivo.Add(detalheProcessamentoArquivo);

                    resumoProcessamento.qtdeRegistrosErro = qtdeRegistrosErro;
                    resumoProcessamento.qtdeRegistrosOk = qtdeRegistrosOk;

                    numLinhaAtual++;
                }

            }
            catch (Exception ex)
            {
                resumoProcessamento.resultadoProcessamento = ex.Message;
            }

            if (resumoProcessamento.qtdeRegistrosErro > 0)
            {
                resumoProcessamento.resultadoProcessamento = "Processamento contém Erros.";
            }
            else
            {
                resumoProcessamento.resultadoProcessamento = "Processado com Sucesso";
            }

            return resumoProcessamento;

        }

        private DetalheProcessamentoArquivo ValidaLinhaConteudo(string linha, int numLinha)
        {
            DetalheProcessamentoArquivo detalhe = new();
            detalhe.linha = numLinha;

            //Layout linha
            if (linha.Length != 51)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem = $"Linha fora do layout, esperado [51] recebido {linha.Length}. ";

            }

            //Identificador linha
            if (linha.Substring(0, 1).ToUpper() != "C")
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[01-01] esperado [C] recebido {linha.Substring(0, 1)}. ";

            }

            //Numero do cartao
            if (ValidarNumeroCartao(linha.Substring(7, 18).Trim()) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += "Número do cartao inválido. ";

            }

            detalhe.retorno = string.IsNullOrEmpty(detalhe.retorno) ? "OK" : detalhe.retorno;
            detalhe.mensagem = string.IsNullOrEmpty(detalhe.mensagem) ? "OK" : detalhe.mensagem;



            return detalhe;
        }

        private DetalheProcessamentoArquivo ValidarHeader(string linhaHeader)
        {
            DetalheProcessamentoArquivo detalhe = new();
            detalhe.linha = 0;

            if (linhaHeader.Length != 51)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem = $"Linha fora do layout, esperado [51] recebido {linhaHeader.Length}. ";
            }


            if (linhaHeader.Substring(0, 29).Trim().ToUpper() != "DESAFIO-HYPERATIVA")
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[01-29] esperado [DESAFIO-HYPERATIVA] recebido {linhaHeader.Substring(0, 29)}. ";
            }

            bool dataValida = DateTime.TryParseExact(linhaHeader.Substring(29, 8), "yyyyMMdd", CultureInfo.InvariantCulture,
                                         DateTimeStyles.None, out _);

            if (dataValida == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[30-37] esperado [yyyyMMdd] recebido {linhaHeader.Substring(29, 8)}. ";
            }


            if (int.TryParse(linhaHeader.Substring(45, 6), out _) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[46-51] esperado [NNNNNN] recebido {linhaHeader.Substring(45, 6)}. ";
            }


            detalhe.retorno = string.IsNullOrEmpty(detalhe.retorno) ? "OK" : detalhe.retorno;
            detalhe.mensagem = string.IsNullOrEmpty(detalhe.mensagem) ? "OK" : detalhe.mensagem;

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

            if (linhaTrailer.Length != 51)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem = $"Linha fora do layout, esperado 51 recebido {linhaTrailer.Length}. ";
            }


            if (linhaTrailer.Substring(0, 4).ToUpper() != "LOTE" || int.TryParse(linhaTrailer.Substring(4, 4), out _) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[01-08] Esperado LOTENNNN recebido {linhaTrailer.Substring(0, 8)}. ";
            }

            if (int.TryParse(linhaTrailer.Substring(8, 14), out _) == false)
            {
                detalhe.retorno = "ERRO";
                detalhe.mensagem += $"[09-14] Esperado NNNNNN recebido {linhaTrailer.Substring(8, 6)}. ";
            }

            detalhe.retorno = string.IsNullOrEmpty(detalhe.retorno) ? "OK" : detalhe.retorno;
            detalhe.mensagem = string.IsNullOrEmpty(detalhe.mensagem) ? "OK" : detalhe.mensagem;


            return detalhe;

        }

    }
}
