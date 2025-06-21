# ConsultaAtivos ![CI](https://github.com/dougasilva/ConsultaAtivos/actions/workflows/ci.yml/badge.svg)

API para consulta de cotações e dados financeiros de ativos da bolsa utilizando a Brapi.  
Arquitetura baseada em DDD, com integração via Swagger, testes com TDD e persistência via EF Core.

---

## ⚙️ Tecnologias

- .NET 8
- C#
- Brapi API
- Entity Framework Core
- AutoMapper
- xUnit
- Swagger

---

## 🧱 Arquitetura

```
/src
  ├── ConsultaAtivos.API          ← Camada de apresentação (controllers, Swagger)
  ├── ConsultaAtivos.Application  ← Serviços de aplicação
  ├── ConsultaAtivos.Domain       ← Entidades e contratos do domínio
  ├── ConsultaAtivos.Infra        ← Persistência, serviços externos (Brapi)
  └── ConsultaAtivos.Tests        ← Testes unitários e de integração
```

---

## 🚀 Como executar

### Pré-requisitos

- .NET 8 SDK
- VS Code ou Visual Studio

### Rodando localmente

```bash
git clone https://github.com/dougasilva/ConsultaAtivos.git
cd ConsultaAtivos
dotnet build
dotnet run --project src/ConsultaAtivos.API
```

Acesse a documentação via Swagger em:  
[http://localhost:5000/swagger](http://localhost:5000/swagger)

---

## ✅ Testes

```bash
dotnet test
```

---

## 🔄 Fluxo de desenvolvimento

Este projeto segue um fluxo de Git baseado em branches:

- `desenvolvimento` – desenvolvimento principal
- `*` – novas features
- `homologacao` – ambiente de homologação
- `master` – código em produção (com tags)

➡️ Veja o fluxo completo: [`docs/fluxo-git.md`](docs/fluxo-git.md)

---

## 🛠 Integração Contínua (CI)

Pull Requests para `desenvolvimento`, `homologacao` e `master` são validados automaticamente com build + testes usando GitHub Actions.

➡️ Detalhes aqui: [`docs/github-actions-ci.md`](docs/github-actions-ci.md)

---

## 📦 Integração com Brapi

A API se comunica com a [Brapi](https://brapi.dev/) para retornar:

- Cotações históricas
- Dados completos de ativos
- Proventos e indicadores

---

## 📋 To-do

- [x] Estrutura inicial DDD
- [x] Swagger configurado
- [x] Suporte a histórico de cotações
- [ ] Agendador de consultas
- [ ] Persistência local de resultados
- [ ] Interface frontend (separado)

---

## 👨‍💻 Autor

**Douglas Silva**  
[github.com/dougasilva](https://github.com/dougasilva)

---
