# 🧪 Ambev Developer Evaluation – Sales API

Este repositório contém a solução do desafio técnico da Ambev, estruturada com arquitetura limpa (Clean Architecture), utilizando DDD, CQRS, e boas práticas modernas de desenvolvimento.

## Funcionalidades

- CRUD completo de vendas
- Cálculo de descontos por quantidade
- Cancelamento de vendas e itens
- Listagem paginada, com ordenação e filtros
- Eventos de domínio logados (VendaCriada, VendaModificada, etc.)
- Validação com FluentValidation
- Retorno padronizado para erros e respostas
- Testes automatizados (unitários, integração, funcionais)

---

## Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- PostgreSQL
- AutoMapper
- MediatR (CQRS)
- FluentValidation
- xUnit + NSubstitute
- Swagger/OpenAPI

---

## 📁 Estrutura do Projeto

```
Ambev.DeveloperEvaluation/
│
├── Adapters/
│   ├── Driven/
│   │   └── Infrastructure/
│   │       └── Ambev.DeveloperEvaluation.ORM
│   └── Drivers/
│       └── WebApi/
│           └── Ambev.DeveloperEvaluation.WebApi
│
├── Core/
│   ├── Application/
│   │   └── Ambev.DeveloperEvaluation.Application
│   └── Domain/
│       └── Ambev.DeveloperEvaluation.Domain
│
├── Crosscutting/
│   ├── Ambev.DeveloperEvaluation.Common
│   └── Ambev.DeveloperEvaluation.IoC
│
├── Tests/
│   ├── Ambev.DeveloperEvaluation.Unit
│   ├── Ambev.DeveloperEvaluation.Integration
│   └── Ambev.DeveloperEvaluation.Functional
│
└── docker-compose.yml
```

---

## Como Executar o Projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/)
- Docker (opcional, se for usar docker-compose)

---

### Configuração do Banco de Dados

Crie um banco chamado `ambev_sales` no PostgreSQL local ou utilize Docker:

```bash
docker-compose up -d
```

> O `docker-compose.yml` já sobe um container PostgreSQL com as credenciais corretas.

---

### 🔄 Migrations

Aplique as migrations com:

```bash
dotnet ef database update \
  --project src/Adapters/Driven/Infrastructure/Ambev.DeveloperEvaluation.ORM \
  --startup-project src/Adapters/Drivers/WebApi/Ambev.DeveloperEvaluation.WebApi
```

---

### ▶️ Executando a API

```bash
cd src/Adapters/Drivers/WebApi/Ambev.DeveloperEvaluation.WebApi
dotnet run
```

Acesse: [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## 🧪 Executando Testes

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit
dotnet test tests/Ambev.DeveloperEvaluation.Integration
dotnet test tests/Ambev.DeveloperEvaluation.Functional
```

---

## Regras de Negócio

| Quantidade de Itens | Desconto Aplicado |
|---------------------|-------------------|
| < 4                 | Nenhum         |
| 4 a 9               | 10%            |
| 10 a 20             | 20%            |
| > 20                | Não permitido  |

---

## Diferenciais Implementados

- [x] Arquitetura limpa (Clean Architecture)
- [x] Separação clara entre camadas
- [x] Inversão de dependência com IoC
- [x] Testes organizados por escopo
- [x] Documentação Swagger automática
- [x] Retorno padronizado via `ApiResponse<T>`

---

## Autor

**Luis Otavio da Silva Braga**  
📧 [oluisotavio2@hotmail.com](mailto:oluisotavio2@hotmail.com)  
🔗 [LinkedIn](https://www.linkedin.com/in/luis-silva-63832417a)