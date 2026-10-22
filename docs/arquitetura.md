# Arquitetura da API

O fluxo principal será `Controller → Service → DbContext → PostgreSQL`.

- Controllers cuidam das rotas HTTP, dos DTOs e dos códigos de resposta.
- Services aplicam regras de negócio e coordenam operações.
- `TaskManagerDbContext` e configurações EF Core cuidam do acesso e mapeamento do banco.
- DTOs definem o contrato HTTP; entidades do EF Core não são expostas diretamente.

Manter a solução simples no MVP: sem repository genérico, MediatR ou CQRS.
