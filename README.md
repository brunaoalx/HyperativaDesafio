# HyperativaDesafio

## Bruno Alexandre

### Tecnologias Utilizadas:

- AspNetCore Net6
- Automapper
- Dapper
- SQLITE3
- JWT 
- Serilog
- Swagger Open API
- Postman

#### Setup

*Banco de Dados*

SQLITE3

Para instalação recomendo seguir instruções do Link abaixo:

https://www.alura.com.br/artigos/sqlite-da-instalacao-ate-primeira-tabela?gclid=EAIaIQobChMIiLDG3YOM_QIV_RXUAR2kvQNnEAAYASAAEgIJKfD_BwE

Basta realizar a instalação e configuração do Sqlite nas variaveis de ambiente, o arquivo de banco de dados usado pela aplicação já é fornecido juntamente com o programa.

Certifique de que há o arquivo hyperativaDesafio.db dentro da pasta: HyperativaDesafioTecnico\src\Cartao.API\DataBase.

Caso o arquivo hyperativaDesafio.db, abra o SQLITE3, crie uma base de dados com esse nome no diretório HyperativaDesafioTecnico\src\Cartao.API\DataBase.

Acesse a pasta HyperativaDesafioTecnico\src\Cartao.API\DataBase\Scripts, lá há todos os scripts para realizar o setup no banco de dados.

#### Log

Utilizamos a biblioteca Serilog pra gravação de logs, os arquivos são salvos na pasta "\HyperativaDesafioTecnico\src\Cartao.API\log"


#### Documentação interativa da API

Utilize a documentação interativa da API disponibilizada, acesse o link abaixo, abrirá a página do PostMan com toda a documentação da API, clique no botão "Run in PostMan" no canto superior da tela para trazer toda a collectio diretamente para sua máquina e realizar os testes. 

Link para a documentação interativa da API
https://documenter.getpostman.com/view/10193758/2s93CEvG89


#### O Desafio

Criação de API para cadastro e consulta de número de cartão completo
Você precisa criar uma API com os seguintes requisitos:
- End-point para autenticação do usuário

- - O cliente deve realizar uma autenticação (JWT ou OAuth2) para realizar o uso da API.

- End-point para inserção de dados

- - O cliente poderá inserir os dados através de requisições informando um único cartão ou a partir de arquivo TXT a API.

- Defina o contrato da API com os padrões a serem adotados para integração.

- Escolha o banco de dados que achar melhor e a estrutura que achar mais adequada.
- Por serem dados sensíveis toda informação deve ser armazenada de maneira segura no banco de dados.

- End-point para consulta de dados
- - O cliente consulta se determinado número de cartão completo existe na base de dados e retorna um identificador único do sistema;

- Requisitos Obrigatórios
- - Logar as requisições de uso da API e seus retornos.
- - Usar linguagem C# com Framework .NET ou Python com Flask ou Django;

- Requisitos Opcionais (Não necessário)
- - Ter uma cobertura de teste unitários relativamente boa.
- - Utilizar criptografia (end-to-end encryption) para tráfego de informações.

