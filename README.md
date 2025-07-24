# Ambev – Avaliação Técnica | Sales API (.NET 8 + Clean Architecture)

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()
[![License](https://img.shields.io/badge/license-MIT-green)]()

Este repositório apresenta a solução do desafio técnico proposto pela Ambev, com foco em boas práticas de desenvolvimento backend utilizando .NET 8.  
A aplicação simula uma API de vendas, aplicando conceitos como Clean Architecture, DDD, CQRS e validações de negócio.

---

##  Funcionalidades

- CRUD completo de vendas
- Cálculo automático de descontos por quantidade
- Cancelamento de vendas e de itens individualmente
- Listagem com paginação, filtros e ordenação
- Log de eventos de domínio (VendaCriada, VendaModificada, etc.)
- Validação com FluentValidation
- Padrão consistente para erros e respostas (`ApiResponse<T>`)
- Testes automatizados: unitários, integração e funcionais

---

##  Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- PostgreSQL
- AutoMapper
- MediatR (CQRS)
- FluentValidation
- xUnit + NSubstitute
- Swagger/OpenAPI

---

##  Estrutura do Projeto

```
Ambev.DeveloperEvaluation/
├── Adapters/
│   ├── Driven/Infrastructure/Ambev.DeveloperEvaluation.ORM
│   └── Drivers/WebApi/Ambev.DeveloperEvaluation.WebApi
├── Core/
│   ├── Application/Ambev.DeveloperEvaluation.Application
│   └── Domain/Ambev.DeveloperEvaluation.Domain
├── Crosscutting/
│   ├── Ambev.DeveloperEvaluation.Common
│   └── Ambev.DeveloperEvaluation.IoC
├── Tests/
│   ├── Ambev.DeveloperEvaluation.Unit
│   ├── Ambev.DeveloperEvaluation.Integration
│   └── Ambev.DeveloperEvaluation.Functional
└── docker-compose.yml
```

---

##  Como Executar

###  Opção 1: Rodar localmente com banco PostgreSQL instalado

#### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- PostgreSQL rodando localmente

#### 1. Crie um banco de dados chamado `ambev_sales`

#### 2. Configure a connection string no `appsettings.json`

#### 3. Aplique as migrations

```bash
dotnet ef database update \
  --project src/Adapters/Driven/Infrastructure/Ambev.DeveloperEvaluation.ORM \
  --startup-project src/Adapters/Drivers/WebApi/Ambev.DeveloperEvaluation.WebApi
```

#### 4. Rode a API

```bash
cd src/Adapters/Drivers/WebApi/Ambev.DeveloperEvaluation.WebApi
dotnet run
```

Acesse: [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

###  Opção 2: Rodar com Docker Compose

```bash
docker-compose up --build
```

> O container `webapi` será iniciado junto com o banco PostgreSQL com as credenciais esperadas pela aplicação.

---

##  Regras de Negócio – Descontos por Quantidade

| Quantidade de Itens | Desconto        |
|---------------------|-----------------|
| 1 a 3               | 0%              |
| 4 a 9               | 10%             |
| 10 a 20             | 20%             |
| > 20                | **Não permitido** 🚫

---

##  Executando os Testes

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit
dotnet test tests/Ambev.DeveloperEvaluation.Integration
dotnet test tests/Ambev.DeveloperEvaluation.Functional
```

---

##  Diferenciais

- [x] Clean Architecture
- [x] DDD + CQRS
- [x] IoC com separação clara entre camadas
- [x] Eventos de domínio publicados
- [x] Testes organizados por escopo
- [x] Documentação Swagger
- [x] Respostas padronizadas com `ApiResponse<T>`

---

## Autor

**Luis Otavio da Silva Braga**  
  [oluisotavio2@hotmail.com](mailto:oluisotavio2@hotmail.com)  
  [LinkedIn](https://www.linkedin.com/in/luis-silva-63832417a)
