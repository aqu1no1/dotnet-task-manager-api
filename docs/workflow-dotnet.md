# Workflow de desenvolvimento .NET

## Executar localmente

1. Copie `.env.example` para `.env` e defina uma senha local para o Compose.
2. Suba o PostgreSQL com `docker compose up -d db`.
3. Restaure pacotes com `dotnet restore`.
4. A API lê `ConnectionStrings__DefaultConnection` do ambiente; ASP.NET Core não carrega `.env` automaticamente. Para executar localmente:

   ```bash
   ConnectionStrings__DefaultConnection='Host=localhost;Port=5433;Database=task_manager_db;Username=task_manager;Password=sua_senha_local' dotnet run --project src/TaskManager.Api
   ```

5. Consulte o OpenAPI em `/openapi/v1.json` no ambiente de desenvolvimento.

## Ciclo de uma mudança

1. Altere o código e/ou os testes.
2. Se o modelo de dados mudar, crie uma migration com `dotnet ef migrations add NomeDaMudanca --project src/TaskManager.Api`.
3. Revise a migration e aplique localmente com `dotnet ef database update --project src/TaskManager.Api`.
4. Verifique com `dotnet format TaskManager.slnx --verify-no-changes`, `dotnet build TaskManager.slnx -c Release` e `dotnet test TaskManager.slnx -c Release`.
5. Abra um PR; a CI repetirá as verificações antes do merge.

## Atalhos com `make`

O `Makefile` na raiz agrupa os comandos acima. Execute `make help` para listar todos.

| Comando | O que faz |
|---|---|
| `make restore` | Restaura pacotes e ferramentas locais (`dotnet-ef`) |
| `make check` | Verifica formatação, faz build e roda os testes unitários, na mesma ordem da CI |
| `make format` | Corrige formatação e estilo |
| `make format-check` | Apenas verifica a formatação |
| `make build` | Build em Release |
| `make test-unit` | Testes unitários (sem banco) |
| `make test` | Todos os testes (precisa do PostgreSQL de teste) |
| `make run` | Roda a API (precisa de `ConnectionStrings__DefaultConnection`) |
| `make db-up` / `make db-down` | Sobe e para o PostgreSQL local |
| `make migration name=NomeDaMudanca` | Cria uma migration |
| `make db-update` | Aplica as migrations no banco local |

Como a API não lê o `.env`, exporte a connection string antes de `make run` ou `make db-update`:

```bash
export ConnectionStrings__DefaultConnection='Host=localhost;Port=5433;Database=task_manager_db;Username=task_manager;Password=sua_senha_local'
make run
```

O `make` vem instalado na maioria das distribuições Linux e no macOS. No Windows, use WSL ou execute os comandos `dotnet` diretamente.

Os testes de integração usam um banco PostgreSQL dedicado. Enquanto não houver migrations, a suíte cria o schema diretamente a partir do modelo EF Core. Configure `ConnectionStrings__DefaultConnection` apontando apenas para o banco de teste antes de executá-la. Com o Compose local, crie um banco separado:

```bash
docker compose exec db createdb -U task_manager task_manager_test
ConnectionStrings__DefaultConnection='Host=localhost;Port=5433;Database=task_manager_test;Username=task_manager;Password=sua_senha_local' dotnet test tests/TaskManager.IntegrationTests/TaskManager.IntegrationTests.csproj -c Release
```
