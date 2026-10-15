# CQRS - Produtos e Categorias

API ASP.NET Core 10 que usa MySQL como armazenamento de escrita e MongoDB como
modelo de leitura. O projeto segue CQRS com MediatR; categorias e produtos são
gravados no MySQL e suas projeções de leitura são publicadas no MongoDB.

## Pré-requisitos

- .NET SDK 10
- Docker Desktop com Docker Compose

## Iniciar os bancos de dados

Na raiz do repositório, execute:

```powershell
docker compose up -d
docker compose ps
```

O Compose disponibiliza:

| Serviço | Endereço local | Banco |
| --- | --- | --- |
| MySQL 8.4 | `localhost:3306` | `cqrs_escrita` |
| MongoDB 8.0 | `localhost:27017` | `cqrs_leitura` |

Os dados são mantidos nos volumes Docker `mysql_data` e `mongo_data`, mesmo
após parar e recriar os containers. As portas ficam vinculadas ao localhost.

As credenciais padrão são destinadas apenas ao desenvolvimento local:

- MySQL: usuário `cqrs_app`, senha `cqrs_dev_password`
- MySQL root: senha `cqrs_dev_root`
- MongoDB: sem autenticação, acessível somente localmente

É possível personalizar usuário, senhas e portas antes de iniciar o Compose,
definindo `MYSQL_USER`, `MYSQL_PASSWORD`, `MYSQL_ROOT_PASSWORD`, `MYSQL_PORT` e
`MONGO_PORT` no ambiente ou em um arquivo `.env` local. Não use os valores
padrão em ambientes compartilhados ou de produção.

Para parar os containers sem remover os dados:

```powershell
docker compose down
```

Para remover também os bancos persistidos, use `docker compose down -v`. Essa
operação apaga os dados dos volumes.

## Configurar e executar a API

Com os containers iniciados e saudáveis, configure as strings de conexão no
PowerShell a partir da raiz do projeto:

```powershell
$env:ConnectionStrings__MySqlConnectionString = "Server=localhost;Port=3306;Database=cqrs_escrita;Uid=cqrs_app;Pwd=cqrs_dev_password;"
$env:ConnectionStrings__MongoConnectionString = "mongodb://localhost:27017"
$env:MongoDatabaseName = "cqrs_leitura"
```

Se alterou as credenciais ou portas no Compose, atualize os valores
correspondentes nas strings de conexão.

As migrations do Entity Framework criam ou atualizam o schema do MySQL:

```powershell
dotnet ef database update --project .\MeuProjeto.Api\MeuProjeto.Api.csproj --startup-project .\MeuProjeto.Api\MeuProjeto.Api.csproj
```

Execute a API:

```powershell
dotnet run --project .\MeuProjeto.Api\MeuProjeto.Api.csproj --launch-profile http
```

Com o perfil HTTP padrão, a API fica em `http://localhost:5241`. Em
desenvolvimento, a documentação OpenAPI está em
`http://localhost:5241/openapi/v1.json` e a interface Scalar em
`http://localhost:5241/scalar/v1`.

## Endpoints principais

### Criar categoria

```http
POST /api/categorias
Content-Type: application/json

{
  "descricao": "Eletrônicos"
}
```

### Listar categorias

```http
GET /api/categorias
```

A listagem é servida pelo modelo de leitura do MongoDB.

### Criar produto

Use o `id` retornado ao criar uma categoria como `categoriaId`:

```http
POST /api/produtos
Content-Type: application/json

{
  "nome": "Teclado",
  "preco": 199.90,
  "categoriaId": "GUID-DA-CATEGORIA"
}
```

### Consultar produto

```http
GET /api/produtos/{id}
```

## Parar a API e os bancos

Encerre a API com `Ctrl+C` e, na raiz do projeto, execute:

```powershell
docker compose down
```
