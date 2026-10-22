# Task Manager API

API REST para gerenciamento de tarefas, construída com ASP.NET Core 10, Entity Framework Core 10 e PostgreSQL 17.

## Pré-requisitos

- .NET SDK 10
- Docker Compose

## Início rápido

1. Copie `.env.example` para `.env` e defina `POSTGRES_PASSWORD`.
2. Inicie o banco: `docker compose up -d db`.
3. Restaure: `dotnet restore`.
4. Execute a API com a connection string no ambiente:

   ```bash
   ConnectionStrings__DefaultConnection='Host=localhost;Port=5433;Database=task_manager_db;Username=task_manager;Password=sua_senha_local' dotnet run --project src/TaskManager.Api
   ```

Veja [`docs/arquitetura.md`](docs/arquitetura.md), [`docs/workflow-dotnet.md`](docs/workflow-dotnet.md) e [`docs/ci-releases.md`](docs/ci-releases.md) para arquitetura, desenvolvimento e automação.

## Solução

- `src/TaskManager.Api`: aplicação ASP.NET Core.
- `tests/TaskManager.UnitTests`: testes unitários.
- `tests/TaskManager.IntegrationTests`: testes de integração.
- `docs/`: documentação técnica.

## Automação

A CI valida formatação, build e as duas suítes de testes em PRs para `main` e em pushes para `main`. Releases são manuais por `workflow_dispatch`; veja [`docs/ci-releases.md`](docs/ci-releases.md). A branch protection e os checks obrigatórios precisam ser configurados nas configurações do GitHub.
