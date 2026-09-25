# 🛒 InfinityPart
 
Sistema de e-commerce desenvolvido para comercialização de peças, componentes e acessórios de informática.
 
O InfinityPart foi desenvolvido como um projeto acadêmico utilizando uma arquitetura em camadas, separando as responsabilidades do sistema entre domínio, aplicação, infraestrutura, API e interfaces.
 
---
 
# 📋 Sumário
 
- [Sobre o projeto](#-sobre-o-projeto)
- [Objetivo](#-objetivo)
- [Funcionalidades](#-funcionalidades)
- [Arquitetura](#-arquitetura)
- [Estrutura do projeto](#-estrutura-do-projeto)
- [Tecnologias](#-tecnologias)
- [Entidades](#-entidades)
- [Relacionamentos](#-relacionamentos)
- [Backend](#-backend)
- [Application](#-application)
- [Infrastructure](#-infrastructure)
- [API](#-api)
- [Autenticação](#-autenticação)
- [Banco de dados](#-banco-de-dados)
- [Migrations](#-migrations)
- [Produtos](#-produtos)
- [Marcas](#-marcas)
- [Pedidos](#-pedidos)
- [Itens do pedido](#-itens-do-pedido)
- [Status dos pedidos](#-status-dos-pedidos)
- [Auditoria](#-auditoria)
- [Frontend](#-frontend)
- [Imagens dos produtos](#-imagens-dos-produtos)
- [Carrinho](#-carrinho)
- [Checkout](#-checkout)
- [CORS](#-cors)
- [Swagger](#-swagger)
- [Como executar](#-como-executar)
- [Git e GitHub](#-git-e-github)
- [Fluxo do sistema](#-fluxo-do-sistema)
- [Testes realizados](#-testes-realizados)
- [Próximos passos](#-próximos-passos)
- [Equipe](#-equipe)
- [Licença](#-licença)
 
---
 
# 📌 Sobre o projeto
 
O **InfinityPart** é um sistema de e-commerce voltado para produtos de informática.
 
A aplicação permite o gerenciamento de produtos, marcas, clientes e pedidos, além de fornecer uma interface para que o cliente possa visualizar produtos, adicionar itens ao carrinho e realizar uma compra.
 
O sistema foi desenvolvido utilizando uma arquitetura separada em camadas, permitindo organizar melhor as responsabilidades e facilitar a manutenção e evolução do projeto.
 
---
 
# 🎯 Objetivo
 
O principal objetivo do InfinityPart é desenvolver uma aplicação completa de comércio eletrônico, aplicando conhecimentos de:
 
- Programação orientada a objetos
- C#
- .NET
- ASP.NET Core
- APIs REST
- Entity Framework Core
- SQL Server
- HTML
- CSS
- JavaScript
- Git
- GitHub
- Arquitetura em camadas
- DTOs
- Services
- Repositories
- Autenticação
- Controle de estoque
- Gerenciamento de pedidos
 
---
 
# ⚙️ Funcionalidades
 
## 👤 Clientes
 
- Cadastro de clientes
- Consulta de clientes
- Atualização de clientes
- Login de clientes
- Autenticação através de e-mail ou CPF
- Senha protegida através de hash
- Associação de pedidos ao cliente
 
## 👨‍💼 Administradores
 
- Estrutura preparada para gerenciamento administrativo
- Autenticação de administrador
- Gerenciamento dos dados do sistema
- Controle dos produtos
- Controle dos pedidos
 
## 📦 Produtos
 
- Cadastro de produtos
- Consulta de produtos
- Atualização de produtos
- Controle de estoque
- Código do produto
- Preço
- Descrição
- Marca
- Imagem do produto
 
## 🏷️ Marcas
 
- Cadastro de marcas
- Consulta de marcas
- Associação entre marcas e produtos
 
## 🛍️ Pedidos
 
- Criação de pedidos
- Associação do pedido ao cliente
- Controle do valor total
- Controle do status
- Associação dos itens comprados
 
## 📋 Itens do pedido
 
- Associação entre pedido e produto
- Quantidade comprada
- Preço unitário
- Cálculo do subtotal
 
## 🔐 Autenticação
 
- Login de administrador
- Login de cliente
- Definição de senha
- Validação de senha
- Armazenamento seguro através de hash
 
## 🛒 Loja
 
- Catálogo de produtos
- Filtros
- Página de detalhes
- Carrinho de compras
- Checkout
- Integração com a API
 
---
 
# 🏗️ Arquitetura
 
O InfinityPart utiliza uma arquitetura dividida em camadas.
 
```text
                    ┌──────────────────────┐
                    │      FRONTEND        │
                    │ HTML / CSS / JS      │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │         API          │
                    │    ASP.NET Core      │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │     APPLICATION      │
                    │ DTOs / Services      │
                    │ Interfaces           │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │    INFRASTRUCTURE    │
                    │ EF Core / Repository │
                    │ Banco de dados       │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │      SQL SERVER      │
                    └──────────────────────┘****
