SOLUTION := TaskManager.slnx
API := src/TaskManager.Api
UNIT_TESTS := tests/TaskManager.UnitTests/TaskManager.UnitTests.csproj

.PHONY: help restore format format-check build test-unit test check run db-up db-down migration db-update

help: ## Lista os comandos disponíveis
	@grep -E '^[a-z-]+:.*## ' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*## "} {printf "  %-14s %s\n", $$1, $$2}'

restore: ## Restaura pacotes e ferramentas locais
	dotnet tool restore
	dotnet restore $(SOLUTION)

format: ## Corrige formatação e estilo
	dotnet format $(SOLUTION)

format-check: ## Verifica formatação (igual à CI)
	dotnet format $(SOLUTION) --verify-no-changes

build: ## Build em Release
	dotnet build $(SOLUTION) -c Release

test-unit: ## Testes unitários (sem banco)
	dotnet test $(UNIT_TESTS) -c Release

test: ## Todos os testes (precisa do Postgres de teste)
	dotnet test $(SOLUTION) -c Release

check: format-check build test-unit ## Formatação + build + testes unitários

run: ## Roda a API (precisa de ConnectionStrings__DefaultConnection)
	dotnet run --project $(API)

db-up: ## Sobe o PostgreSQL local
	docker compose up -d db

db-down: ## Para o PostgreSQL local
	docker compose down

migration: ## Cria migration: make migration name=NomeDaMudanca
	@test -n "$(name)" || (echo "Uso: make migration name=NomeDaMudanca" && exit 1)
	dotnet ef migrations add $(name) --project $(API)

db-update: ## Aplica migrations no banco local
	dotnet ef database update --project $(API)
