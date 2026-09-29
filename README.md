# InfinityPart

Sistema de gerenciamento de produtos, clientes, marcas, categorias, pedidos e administradores, desenvolvido em C# com .NET 9, utilizando uma arquitetura em camadas e aplicação Desktop baseada em Windows Forms.

O projeto foi desenvolvido com foco em organização, separação de responsabilidades, reutilização de código e comunicação entre uma aplicação Desktop e uma API.

---

## Sobre o projeto

O InfinityPart é um sistema de gerenciamento voltado para uma loja de produtos, permitindo controlar diferentes áreas do negócio através de uma aplicação Desktop.

Entre as principais funcionalidades estão:

* Autenticação de administradores
* Gerenciamento de clientes
* Gerenciamento de administradores
* Gerenciamento de produtos
* Gerenciamento de marcas
* Gerenciamento de categorias
* Gerenciamento de pedidos
* Dashboard administrativo
* Controle de estoque
* Cálculo de valores de pedidos
* Regra de desconto para pagamento via PIX
* Sistema de auditoria
* Hash de senhas
* Validações de dados
* Interface gráfica personalizada

---

# Arquitetura

O projeto utiliza uma arquitetura dividida em camadas:

```text
┌──────────────────────────────┐
│      InfinityPart.Desktop    │
│        Windows Forms         │
└──────────────┬───────────────┘
               │
               │ HTTP / JSON
               ▼
┌──────────────────────────────┐
│        InfinityPart.API      │
│        Controllers / HTTP    │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│    InfinityPart.Application  │
│       Casos de uso / DTOs    │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│      InfinityPart.Domain     │
│ Entidades / Regras / Contratos│
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│ InfinityPart.Infrastructure  │
│      EF Core / Database      │
└──────────────┬───────────────┘
               │
               ▼
          SQL Server
```

A principal característica dessa arquitetura é a separação das responsabilidades.

A aplicação Desktop é responsável pela interface do usuário, enquanto a API controla a comunicação HTTP e as camadas internas cuidam das regras de negócio e persistência.

---

# Estrutura do projeto

```text
InfinityPart
│
├── InfinityPart.Desktop
│   ├── Controls
│   ├── Forms
│   ├── Models
│   ├── Services
│   ├── Theme
│   ├── Program.cs
│   ├── UIHelpers.cs
│   └── Validacoes.cs
│
├── InfinityPart.API
│   └── Controllers
│
├── InfinityPart.Application
│   ├── DTOs
│   └── Services
│
├── InfinittyPart.domain
│   ├── Entidades
│   ├── Enum
│   ├── Excecoes
│   ├── Interfaces
│   ├── Services
│   └── ValueObjects
│
└── InfinityPart.Infrastructure
    ├── Context
    ├── Repositories
    └── Services
```

---

# Domain

A camada Domain representa o núcleo do negócio.

Ela não deve depender de elementos específicos da interface gráfica, banco de dados ou HTTP.

## Entidades

As principais entidades são:

### Administrador

Representa os usuários administrativos do sistema.

Principais informações:

* ID
* Nome
* E-mail
* CPF
* Telefone
* Data de cadastro
* Hash da senha
* Status de atividade
* Último acesso

### Cliente

Representa os clientes da loja.

Possui informações como:

* Nome
* CPF
* E-mail
* Telefone
* Data de nascimento
* Senha
* Endereço
* Pedidos realizados

### Produto

Representa os produtos comercializados.

Possui:

* Nome
* Código
* Descrição
* Preço
* Quantidade em estoque
* Marca
* Categoria
* Itens de pedidos

### Marca

Representa as marcas dos produtos.

Uma marca pode possuir diversos produtos.

```text
Marca
 ├── Produto
 ├── Produto
 └── Produto
```

### Categoria

Representa a categoria de um produto.

Exemplo:

```text
Placas de Vídeo
 ├── RTX 4060
 ├── RTX 4070
 └── RX 7600
```

### Pedido

Representa uma compra realizada por um cliente.

Um pedido possui:

* Cliente
* Status
* Data
* Valor total
* Itens

Estrutura:

```text
Pedido
│
├── Cliente
│
├── Status
│
└── Itens
    ├── ItemPedido
    ├── ItemPedido
    └── ItemPedido
```

### ItemPedido

Representa um produto dentro de um pedido.

Possui:

* Produto
* Quantidade
* Preço unitário
* Subtotal

O subtotal é calculado através de:

```text
Subtotal = Quantidade × PreçoUnitário
```

O preço unitário é armazenado no item para preservar o preço praticado no momento da compra.

### StatusPedido

Representa o estado atual de um pedido.

Exemplos:

```text
Pendente
Pago
Enviado
Entregue
Cancelado
```

### Auditoria

Registra ações realizadas no sistema.

Pode armazenar informações como:

* Usuário responsável
* Ação realizada
* Tabela afetada
* Data e hora
* Valores anteriores
* Novos valores

Isso permite manter um histórico das alterações realizadas.

---

# Value Objects

A Domain também possui Value Objects.

## CPF

Responsável por representar um CPF e realizar validações relacionadas ao seu formato.

Exemplo:

```csharp
new Cpf("123.456.789-00");
```

## Endereço

Representa um endereço completo.

Possui informações como:

```text
Logradouro
Número
Bairro
Cidade
Estado
CEP
```

---

# Services da Domain

## PedidoDomainService

Concentra regras relacionadas aos pedidos.

### Desconto PIX

O sistema possui uma regra de desconto de 10% para pagamento via PIX.

Exemplo:

```text
Valor original: R$ 1.000,00

Desconto: 10%
Valor do desconto: R$ 100,00

Valor final: R$ 900,00
```

### Validação de estoque

Antes de realizar uma venda, o estoque disponível é validado.

Exemplo:

```text
Estoque disponível: 10
Quantidade solicitada: 4

Resultado: venda permitida
```

Caso:

```text
Estoque disponível: 2
Quantidade solicitada: 5
```

o sistema gera uma:

```text
EstoqueInsuficienteException
```

---

# Interfaces

A Domain define contratos para os repositórios.

Entre eles:

```text
IAdministradorRepository
IAuditoriaRepository
ICategoriaRepository
IClienteRepository
IItemPedidoRepository
IMarcaRepository
IPedidoRepository
IProdutoRepository
IStatusPedidoRepository
```

Essas interfaces permitem que a Domain conheça o que precisa ser feito sem conhecer os detalhes de implementação do banco de dados.

Exemplo:

```text
IProdutoRepository
        │
        │ contrato
        ▼
ProdutoRepository
        │
        ▼
Entity Framework Core
        │
        ▼
SQL Server
```

---

# Segurança

O projeto possui uma interface específica para tratamento de senhas:

```text
ISenhaHasher
```

Ela permite:

* Gerar hash de senha
* Verificar uma senha informada
* Evitar armazenamento direto de senhas em texto puro

O projeto utiliza uma abordagem baseada em hash para armazenamento das credenciais.

---

# Desktop

O projeto Desktop é desenvolvido utilizando:

* C#
* .NET 9
* Windows Forms

Ele é responsável pela interação direta com o usuário.

---

# Login

A aplicação inicia pela tela:

```text
FrmLogin
```

O usuário informa suas credenciais e o Desktop realiza a autenticação através da API.

Fluxo:

```text
Usuário
   ↓
FrmLogin
   ↓
AutenticacaoApiService
   ↓
POST /api/autenticacao/login
   ↓
API
   ↓
Resultado da autenticação
   ↓
SessaoAtual
```

Após uma autenticação válida, o usuário é direcionado para:

```text
FrmPrincipal
```

---

# Sessão atual

A classe:

```text
SessaoAtual
```

mantém as informações do administrador atualmente autenticado.

Isso permite que diferentes telas saibam qual usuário está utilizando o sistema.

Ao sair da aplicação, a sessão pode ser encerrada.

---

# Tela principal

A tela:

```text
FrmPrincipal
```

funciona como o painel principal do sistema.

Ela possui:

* Menu lateral
* Cabeçalho
* Área de conteúdo
* Dashboard
* Atalhos para os principais módulos

Estrutura visual:

```text
┌───────────────────────────────────────────────┐
│                  HEADER                       │
├───────────────┬───────────────────────────────┤
│               │                               │
│   SIDEBAR     │          DASHBOARD            │
│               │                               │
│ Produtos      │                               │
│ Categorias    │       Cards / Dados           │
│ Marcas        │                               │
│ Clientes      │                               │
│ Pedidos       │                               │
│ Administrador │                               │
│ Configurações │                               │
│               │                               │
└───────────────┴───────────────────────────────┘
```

---

# Produtos

O módulo de produtos permite:

* Listar produtos
* Pesquisar produtos
* Cadastrar produtos
* Editar produtos
* Excluir produtos
* Informar preço
* Controlar estoque
* Relacionar marcas
* Relacionar categorias

Principais formulários:

```text
FrmProdutos
FrmProdutoCadastro
```

---

# Clientes

Permite gerenciar os clientes cadastrados.

Funcionalidades:

* Listagem
* Cadastro
* Edição
* Exclusão
* Dados pessoais
* Endereço
* Informações de contato

Formulários:

```text
FrmClientes
FrmClienteCadastro
```

---

# Marcas

Permite:

* Listar marcas
* Cadastrar marcas
* Editar marcas
* Excluir marcas

Formulários:

```text
FrmMarcas
FrmMarcaCadastro
```

---

# Categorias

Permite:

* Listar categorias
* Criar categorias
* Editar categorias
* Excluir categorias

Formulários:

```text
FrmCategorias
FrmCategoriaCadastro
```

---

# Pedidos

O módulo de pedidos permite visualizar e administrar as compras realizadas.

Cada pedido pode possuir:

```text
Cliente
Produtos
Quantidade
Preço
Valor total
Status
Data
```

Formulários:

```text
FrmPedidos
FrmPedidoCadastro
```

---

# Administradores

O sistema também possui gerenciamento dos administradores.

Permite:

* Listar administradores
* Cadastrar administradores
* Editar administradores
* Controlar dados de acesso

Formulários:

```text
FrmAdministradores
FrmAdministradorCadastro
```

---

# Configurações

A tela:

```text
FrmConfiguracoes
```

concentra opções relacionadas às configurações do sistema.

---

# Comunicação com a API

O Desktop não acessa diretamente o banco de dados.

A comunicação acontece através do:

```text
ApiClient
```

A API possui uma URL base configurada como:

```text
http://localhost:5022/api
```

O `ApiClient` possui métodos para operações HTTP, como:

```text
GET
POST
PUT
DELETE
```

Exemplo:

```text
GET /api/produto
```

retorna uma lista de produtos.

---

# Fluxo de uma requisição

Quando o usuário abre a tela de produtos:

```text
1. Usuário abre Produtos
        ↓
2. FrmProdutos
        ↓
3. ApiClient
        ↓
4. GET /api/produto
        ↓
5. ProdutoController
        ↓
6. Application
        ↓
7. Domain
        ↓
8. Repository
        ↓
9. SQL Server
```

A resposta percorre o caminho inverso:

```text
SQL Server
    ↓
Repository
    ↓
Application
    ↓
API
    ↓
JSON
    ↓
ApiClient
    ↓
ProdutoModel
    ↓
DataGridView
```

---

# Models

O Desktop possui modelos próprios para representar os dados recebidos da API.

Entre eles:

```text
ProdutoModel
ClienteModel
AutenticacaoModels
OutrosModels
```

Esses modelos não devem ser confundidos com as entidades da Domain.

A entidade:

```text
Domain → Produto
```

representa o conceito de negócio.

Enquanto:

```text
Desktop → ProdutoModel
```

representa os dados utilizados pela aplicação Desktop.

---

# LocalStore

O Desktop também possui um:

```text
LocalStore
```

que mantém alguns dados em memória durante a execução da aplicação.

Ele pode manter coleções como:

```text
Produtos
Clientes
Marcas
Pedidos
StatusPedidos
```

O LocalStore não substitui o banco de dados.

Os dados armazenados nele existem apenas enquanto a aplicação estiver em execução.

---

# Interface e tema

O projeto possui um sistema próprio de tema através de:

```text
Theme/AppTheme.cs
```

O tema centraliza:

* Cores
* Fontes
* Fundos
* Bordas
* Botões
* Estados de sucesso
* Estados de alerta
* Estados de erro

Isso permite manter uma identidade visual consistente em todo o sistema.

---

# Componentes reutilizáveis

O projeto possui controles personalizados:

```text
AppButton
DashboardCard
SidebarButton
```

Eles permitem reutilizar componentes visuais em diferentes telas.

---

# UIHelpers

O arquivo:

```text
UIHelpers.cs
```

centraliza funções relacionadas à interface.

Exemplos:

```text
StyleGrid()
StyleTextBox()
StyleComboBox()
StyleDateTimePicker()
StyleButton()
RoundControl()
```

Assim, diferentes formulários podem utilizar os mesmos padrões visuais.

---

# Validações

O Desktop possui uma classe:

```text
Validacoes.cs
```

que contém validações relacionadas à entrada de dados.

Exemplos:

```text
SomenteNumeros()
SomenteLetras()
SomenteDecimal()
EmailValido()
DataValida()
DecimalValido()
InteiroValido()
```

Essas validações são principalmente relacionadas à interface.

As regras de negócio continuam sendo responsabilidade da Domain.

---

# Separação de responsabilidades

Uma das características mais importantes do projeto é a separação das responsabilidades.

## Desktop

Responsável por:

* Interface
* Formulários
* Eventos
* Validação de entrada
* Exibição de informações
* Comunicação com a API

## API

Responsável por:

* Receber requisições HTTP
* Validar requests
* Encaminhar operações
* Retornar respostas HTTP

## Application

Responsável por:

* Casos de uso
* DTOs
* Orquestração das operações
* Comunicação entre API e Domain

## Domain

Responsável por:

* Entidades
* Regras de negócio
* Value Objects
* Exceções
* Contratos
* Serviços de domínio

## Infrastructure

Responsável por:

* Banco de dados
* Entity Framework Core
* Implementação dos repositories
* Persistência dos dados
* Serviços externos

---

# Tecnologias utilizadas

| Tecnologia            | Utilização              |
| --------------------- | ----------------------- |
| C#                    | Linguagem principal     |
| .NET 9                | Plataforma              |
| Windows Forms         | Aplicação Desktop       |
| ASP.NET Core          | API                     |
| Entity Framework Core | Persistência            |
| SQL Server            | Banco de dados          |
| HTTP/JSON             | Comunicação Desktop/API |
| Git                   | Controle de versão      |

---

# Requisitos

Para executar o projeto, é necessário ter instalado:

* .NET 9 SDK
* Visual Studio 2022 ou superior
* SQL Server
* Git

Para desenvolvimento do Desktop, é necessário utilizar Windows devido ao uso do Windows Forms.

---

# Configuração

Primeiramente, clone o repositório:

```bash
git clone <URL_DO_REPOSITORIO>
```

Entre na pasta do projeto:

```bash
cd InfinityPart
```

Restaure as dependências:

```bash
dotnet restore
```

Compile a solução:

```bash
dotnet build
```

---

# Banco de dados

O projeto utiliza SQL Server para persistência dos dados.

Antes de iniciar a aplicação, configure a connection string da API de acordo com o ambiente utilizado.

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InfinityPart;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

A connection string deve ser ajustada de acordo com a instalação do SQL Server.

---

# Executando a API

Acesse a pasta da API:

```bash
cd InfinityPart.API
```

Execute:

```bash
dotnet run
```

A API estará disponível na porta configurada pelo projeto.

O Desktop utiliza atualmente:

```text
http://localhost:5022/api
```

Portanto, certifique-se de que a API esteja disponível nesse endereço ou ajuste a URL utilizada pelo `ApiClient`.

---

# Executando o Desktop

Depois de iniciar a API, execute o projeto Desktop:

```bash
dotnet run --project InfinityPart.Desktop
```

Ou abra a solução no Visual Studio e execute o projeto:

```text
InfinityPart.Desktop
```

O sistema deverá abrir inicialmente na tela de login.

---

# Fluxo de inicialização

O fluxo básico da aplicação é:

```text
Program.cs
    ↓
FrmLogin
    ↓
Autenticação
    ↓
SessaoAtual
    ↓
FrmPrincipal
    ↓
Módulos do sistema
```

---

# CRUD

Os principais módulos utilizam operações CRUD.

CRUD significa:

```text
Create  → Criar
Read    → Consultar
Update  → Atualizar
Delete  → Excluir
```

Por exemplo, para produtos:

```text
GET     /api/produto
POST    /api/produto
PUT     /api/produto/{id}
DELETE  /api/produto/{id}
```

O Desktop utiliza essas operações através do `ApiClient`.

---

# Exemplo de fluxo de cadastro

Para cadastrar um produto:

```text
Usuário
   ↓
FrmProdutoCadastro
   ↓
Preenchimento dos campos
   ↓
Validações
   ↓
ApiClient
   ↓
POST /api/produto
   ↓
Controller
   ↓
Application
   ↓
Domain
   ↓
Repository
   ↓
SQL Server
```

Após a persistência, a API retorna o resultado para o Desktop.

---

# Regras de negócio

Algumas regras implementadas no projeto incluem:

* Validação de estoque
* Cálculo de subtotal de itens
* Cálculo do valor do pedido
* Desconto de 10% para PIX
* Validação de CPF
* Validação de dados
* Tratamento de preços inválidos
* Controle de sessão
* Registro de auditoria
* Hash de senhas

---

# Tratamento de exceções

A Domain possui exceções específicas para regras de negócio.

```text
RegraNegocioException
EstoqueInsuficienteException
PrecoInvalidoException
```

Isso permite diferenciar erros de negócio de erros técnicos.

Exemplo:

```text
Quantidade solicitada > estoque
            ↓
EstoqueInsuficienteException
```

---

# Auditoria

O sistema possui uma entidade de auditoria para registrar alterações realizadas.

Um registro pode conter:

```text
Usuário
Ação
Tabela
Data/Hora
Valores anteriores
Novos valores
```

Exemplo:

```text
Usuário: Administrador
Ação: Atualização
Tabela: Produto

Antes:
Preço = 100

Depois:
Preço = 120
```

---

# Segurança

O projeto utiliza hash para armazenamento de senhas.

As senhas não devem ser armazenadas diretamente em texto puro.

A responsabilidade de gerar e validar hashes é abstraída através de:

```text
ISenhaHasher
```

Essa abstração permite trocar a implementação sem alterar as regras principais do sistema.

---

# Comunicação entre camadas

A comunicação segue o princípio de que cada camada possui uma responsabilidade específica.

```text
Desktop
   │
   │ HTTP
   ▼
API
   │
   │ DTOs / casos de uso
   ▼
Application
   │
   │ Entidades / regras
   ▼
Domain
   │
   │ Interfaces
   ▼
Infrastructure
   │
   ▼
SQL Server
```

Essa organização facilita:

* Manutenção
* Testes
* Evolução do sistema
* Reutilização
* Separação de responsabilidades
* Substituição de tecnologias

---

# Princípios utilizados

O projeto segue conceitos comuns de arquitetura de software, como:

* Separation of Concerns
* Dependency Inversion
* Repository Pattern
* Domain-Driven Design em nível estrutural
* DTOs
* Value Objects
* Services
* Injeção de dependência
* Separação entre UI e regras de negócio

---

# Possíveis melhorias futuras

Algumas evoluções que podem ser consideradas:

* Adicionar testes unitários
* Adicionar testes de integração
* Melhorar a validação de CPF
* Implementar autenticação baseada em JWT
* Adicionar controle de permissões por administrador
* Implementar paginação nas listagens
* Adicionar filtros avançados
* Melhorar tratamento global de erros
* Adicionar documentação Swagger mais completa
* Adicionar logs estruturados
* Utilizar variáveis de ambiente para configurações
* Melhorar o sistema de auditoria
* Adicionar confirmação em operações destrutivas
* Criar pipeline de CI/CD

---

# Contribuição

Para contribuir com o projeto:

1. Faça um fork do repositório.
2. Crie uma branch para sua alteração.

```bash
git checkout -b minha-feature
```

3. Faça as alterações necessárias.
4. Execute os testes e verifique se o projeto continua compilando.

```bash
dotnet build
```

5. Faça o commit.

```bash
git add .
git commit -m "feat: adiciona nova funcionalidade"
```

6. Envie a branch:

```bash
git push origin minha-feature
```

7. Abra um Pull Request.

---

# Licença

Este projeto está sujeito à licença definida pelo proprietário do repositório.

Caso nenhuma licença tenha sido definida, todos os direitos sobre o código permanecem reservados aos respectivos autores.

---

# Resumo da arquitetura

O InfinityPart pode ser entendido através do seguinte fluxo:

```text
                     USUÁRIO
                        │
                        ▼
              ┌──────────────────┐
              │     DESKTOP       │
              │   Windows Forms   │
              └────────┬─────────┘
                       │
                    HTTP/JSON
                       │
                       ▼
              ┌──────────────────┐
              │       API        │
              │   Controllers    │
              └────────┬─────────┘
                       │
                       ▼
              ┌──────────────────┐
              │   APPLICATION    │
              │  Casos de uso    │
              │      DTOs        │
              └────────┬─────────┘
                       │
                       ▼
              ┌──────────────────┐
              │      DOMAIN      │
              │ Entidades        │
              │ Regras           │
              │ Value Objects    │
              │ Interfaces       │
              └────────┬─────────┘
                       │
                       ▼
              ┌──────────────────┐
              │ INFRASTRUCTURE   │
              │ Repositories     │
              │ Entity Framework │
              └────────┬─────────┘
                       │
                       ▼
                 SQL SERVER
```

O Desktop funciona como a camada de apresentação, a API como porta de entrada para as operações, a Application como camada de aplicação, a Domain como núcleo das regras de negócio e a Infrastructure como responsável pelos detalhes de persistência.

Essa separação permite que cada parte do sistema tenha uma responsabilidade bem definida e reduz o acoplamento entre interface, regras de negócio e banco de dados.
