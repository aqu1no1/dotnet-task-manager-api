# CI e releases

## CI

O workflow `.github/workflows/ci.yml` roda em Pull Requests para `main`, em pushes para `main` e quando é chamado pelo workflow de release. Ele valida formatação e build, depois executa testes unitários e testes de integração com PostgreSQL 17 efêmero. Cada suíte envia seu resultado TRX e cobertura Cobertura como artefatos do workflow. Um script valida que o TRX contém pelo menos um teste descoberto, para impedir que uma suíte vazia apareça como sucesso.

Os testes de integração usam exclusivamente o banco `task_manager_test` criado pelo job; não usam o banco local nem credenciais de produção. Até a primeira migration ser criada, o teste prepara o schema diretamente a partir do modelo EF Core. Depois que migrations forem adicionadas, o teste deverá passar a aplicá-las.

## Release manual

O workflow `.github/workflows/release.yml` aceita somente `workflow_dispatch` na branch `main` e exige uma versão `vMAJOR.MINOR.PATCH`. Antes de publicar, valida a versão do `.csproj`, a seção correspondente do `CHANGELOG.md`, a ausência da tag/release e se o commit ainda é o HEAD de `main`. Depois executa a CI reutilizável e `dotnet publish`.

O workflow não pode publicar enquanto a versão existir somente em `[Unreleased]`. Antes de uma release, atualize `<Version>` no `.csproj` e adicione ao changelog uma seção datada para essa mesma versão.

Somente após as verificações e a geração do ZIP a action cria e envia a tag anotada e cria a GitHub Release com as notas da versão. A release não faz deploy nem aplica migrations em produção.

Se a tag for enviada e a criação da GitHub Release falhar, interrompa e investigue antes de tentar novamente. O processo não deve mover a tag para outro commit.

## Configuração no GitHub

Depois de confirmar que os workflows estão passando em Pull Requests, habilite proteção para `main` e exija os checks `Quality and build`, `Unit tests` e `Integration tests (PostgreSQL)`. Os nomes precisam corresponder aos jobs da CI.

Os workflows de CI têm permissão `contents: read`. Somente o job final de publicação recebe `contents: write`. Não são necessários secrets de produção; o PostgreSQL de integração usa credenciais descartáveis do serviço efêmero.

## Desenvolvimento local

`dotnet test` executa ambas as suítes pela solução. Os testes unitários não precisam de serviços externos. Os testes de integração precisam de um PostgreSQL de teste dedicado e da variável `ConnectionStrings__DefaultConnection`; não os execute contra um banco com dados importantes.
