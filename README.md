# InfinityPart

O **InfinityPart** é um sistema de gerenciamento para uma loja de produtos, desenvolvido para facilitar o controle de produtos, clientes, marcas, categorias, pedidos e administradores.

O sistema possui uma aplicação Desktop para gerenciamento das informações e uma API responsável pela comunicação entre a interface, as regras de negócio e o banco de dados.

## Funcionalidades

* Autenticação de administradores
* Dashboard administrativo
* Cadastro e gerenciamento de produtos
* Controle de estoque
* Cadastro e gerenciamento de clientes
* Cadastro de marcas e categorias
* Gerenciamento de pedidos
* Controle de status dos pedidos
* Cálculo de valores e descontos
* Validação de dados
* Registro de auditoria
* Gerenciamento de administradores

## Arquitetura

O projeto utiliza uma arquitetura em camadas, separando as responsabilidades do sistema:

```text
Desktop
   ↓
API
   ↓
Application
   ↓
Domain
   ↓
Infrastructure
   ↓
SQL Server
```

O **Desktop** é responsável pela interface e interação com o usuário.

A **API** recebe e processa as requisições do Desktop.

A **Application** organiza os casos de uso do sistema.

A **Domain** concentra as entidades e regras de negócio.

A **Infrastructure** é responsável pela persistência e comunicação com o banco de dados.

## Tecnologias

* C#
* .NET 9
* Windows Forms
* ASP.NET Core
* Entity Framework Core
* SQL Server
* HTTP/JSON
* Git

## Principais módulos

```text
Login
Dashboard
Produtos
Clientes
Marcas
Categorias
Pedidos
Administradores
Configurações
```

## Fluxo do sistema

```text
Usuário
   ↓
Desktop / Windows Forms
   ↓
API
   ↓
Regras de negócio
   ↓
Banco de dados
```

O sistema foi desenvolvido com foco em organização, separação de responsabilidades e facilidade de manutenção.
