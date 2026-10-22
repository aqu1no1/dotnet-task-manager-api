# Pull Request

## Descrição

<!-- Descreva objetivamente o que esta PR altera. -->

## Tipo de PR

- [ ] Bugfix
- [ ] Feature
- [ ] Refactor
- [ ] Performance
- [ ] Testes
- [ ] Documentação
- [ ] Chore
- [ ] Hotfix

## Links relacionados

- Task no Linear:
- Issue:

## Contexto e regras de negócio

<!-- Qual problema motivou a alteração? Quais comportamentos ou contratos mudaram? -->

**Comportamento anterior:**

**Comportamento esperado depois da PR:**

**Regras afetadas (se aplicável):**

## Plano de testes e evidências

<!-- Para alterações de código, descreva cenários e riscos relevantes. Não crie testes apenas para aumentar cobertura. -->

| Cenário relevante | Nível (unit/integration) | Resultado esperado |
|---|---|---|
|  |  |  |

**Falha ou regressão plausível que os testes conseguem detectar:**

**Se testes não foram adicionados/alterados, justifique:**

**Comandos realmente executados e resultado:**

```text
<!-- Ex.: dotnet test tests/TaskManager.UnitTests -c Release -->
```

## Checklist

### Código e arquitetura

- [ ] A mudança respeita as responsabilidades entre Controller, Service, DTOs, Models e DbContext.
- [ ] Os nomes seguem as convenções de C#.
- [ ] Não ficaram logs de depuração ou código comentado desnecessários.
- [ ] Erros HTTP usam respostas adequadas, como ProblemDetails quando aplicável.
- [ ] Não foram introduzidas abstrações ou dependências desnecessárias.

### Qualidade e testes

- [ ] Executei `dotnet restore TaskManager.slnx`.
- [ ] Executei `dotnet format TaskManager.slnx --verify-no-changes`.
- [ ] Executei `dotnet build TaskManager.slnx -c Release`.
- [ ] Executei testes unitários pertinentes, quando existentes.
- [ ] Executei testes de integração pertinentes, quando existentes e aplicáveis.
- [ ] Não marquei verificações que não executei; registrei impedimentos nas evidências.

### Banco de dados / Entity Framework Core

- [ ] Alterações no modelo foram acompanhadas das migrations necessárias.
- [ ] Revisei operações potencialmente destrutivas nas migrations.
- [ ] Verifiquei persistência e regras relevantes usando PostgreSQL de testes.
- [ ] Os modelos e DTOs mantêm contratos coerentes.

### Documentação e versionamento

- [ ] Atualizei `CHANGELOG.md` em `[Unreleased]` quando a mudança é relevante para consumidores da API.
- [ ] Atualizei README, documentação e OpenAPI quando necessário.
- [ ] A mudança respeita as convenções de commit e versionamento.

### Segurança e configuração

- [ ] Não há segredos, credenciais reais ou dados pessoais desnecessários no código, logs ou evidências.
- [ ] Documentei novas configurações sem adicionar valores sensíveis.
- [ ] Considerei o mapeamento de variáveis de ambiente do ASP.NET Core.
- [ ] Atualizei Docker Compose e ambiente de testes quando necessário.

## Variáveis de ambiente (se aplicável, sem valores sensíveis)

**Adicionadas:**

**Removidas:**

## Evidências (se aplicável)

## Observações adicionais

<!-- Limitações conhecidas e pontos para revisão. -->
