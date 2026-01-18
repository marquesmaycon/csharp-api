# CSharpApi

API RESTful de gerenciamento de usuários construída com .NET 9.0, utilizando PostgreSQL como banco de dados e autenticação JWT.

## 📋 Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker e Docker Compose](https://www.docker.com/get-started)

## 🚀 Executando o Sistema Localmente

### Opção 1: Usando Docker (Recomendado) ⭐

**A forma mais simples de rodar tudo!**

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/marquesmaycon/csharp-api
   cd csharp-api
   ```

2. **Crie o arquivo `.env` na raiz do projeto:**
   ```env
   POSTGRES_USER=postgres
   POSTGRES_PASSWORD=password123
   POSTGRES_DB=csharpapi_db
   ASPNETCORE_ENVIRONMENT=Development

   AppSettings__Token=5d6f0aa47ec2c0195214ce5c2a8f4f7d8779c162584c3c05ee490877e2682079f6e2b511126c5b1a87fcbcf81eef7a1773d561d4f7f73fe41c37b86bf0b3ab0f
   AppSettings__Issuer=https://csharpapi.example.com/auth
   AppSettings__Audience=https://csharpapi.example.com/api
   ConnectionStrings__DefaultConnection=Server=postgres;Port=5432;Database=csharpapi_db;User Id=postgres;Password=password123;
   ```

3. **Inicie os containers:**
   ```bash
   docker-compose up -d
   ```
   
   O Docker irá automaticamente:
   - ✅ Criar e iniciar o container PostgreSQL
   - ✅ Fazer build e iniciar o container da API
   - ✅ Executar as migrations do banco de dados
   - ✅ A aplicação estará pronta em ~30 segundos

4. **Acesse a aplicação:**
   - API: http://localhost:8080
   - Swagger: http://localhost:8080/swagger

5. **Ver logs da aplicação:**
   ```bash
   docker-compose logs -f api
   ```

6. **Para parar os containers:**
   ```bash
   docker-compose down
   ```

7. **Para parar e remover volumes (limpar dados):**
   ```bash
   docker-compose down -v
   ```

---

## 📚 Testando os Endpoints via Swagger

### 1. Acessando o Swagger

Abra seu navegador e acesse: **http://localhost:8080/swagger**

### 2. Endpoints Disponíveis

#### 🔓 **Autenticação** (Endpoints Públicos)

##### **POST /api/Auth/register**
Registra um novo usuário no sistema.

**Exemplo de Request Body:**
```json
{
  "name": "João Silva",
  "email": "joao@example.com",
  "password": "Senha@123"
}
```

##### **POST /api/Auth/login**
Realiza o login e retorna um token JWT.

**Exemplo de Request Body:**
```json
{
  "email": "joao@example.com",
  "password": "Senha@123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "João Silva",
    "email": "joao@example.com",
    "role": "User"
  }
}
```

### 3. Autenticando Requisições no Swagger

Para testar endpoints protegidos:

1. **Registre um usuário** usando o endpoint `/api/Auth/register`
2. **Faça login** usando o endpoint `/api/Auth/login` e copie o `token` retornado
3. **Clique no botão "Authorize" 🔒** no topo da página do Swagger
4. **Cole o token** no campo que aparecerá (formato: `Bearer seu_token_aqui`)
   - O Swagger já adiciona o prefixo "Bearer" automaticamente, cole apenas o token
5. **Clique em "Authorize"** e depois em "Close"

Agora você pode testar os endpoints protegidos!

#### 🔒 **Usuários** (Endpoints Protegidos - Requerem Autenticação)

##### **GET /api/Users**
Lista todos os usuários (requer autenticação).

##### **GET /api/Users/{id}**
Busca um usuário específico por ID.

##### **GET /api/Users/name/{name}**
Busca usuários por nome.

##### **POST /api/Users**
Cria um novo usuário (apenas Admin).

**Exemplo de Request Body:**
```json
{
  "name": "Maria Santos",
  "email": "maria@example.com",
  "password": "Senha@456"
}
```

##### **PUT /api/Users/{id}**
Atualiza os dados de um usuário.

**Exemplo de Request Body:**
```json
{
  "name": "João Silva Santos",
  "email": "joao.santos@example.com"
}
```

##### **DELETE /api/Users/{id}**
Remove um usuário do sistema.

#### 🌐 **JsonPlaceholder** (Endpoints Públicos)

Endpoints de exemplo que consomem a API externa JSONPlaceholder:

##### **GET /api/JsonPlaceholder/posts**
Lista todos os posts da API externa.

##### **GET /api/JsonPlaceholder/posts/{id}**
Busca um post específico por ID.

##### **GET /api/JsonPlaceholder/users/{userId}/posts**
Lista todos os posts de um usuário específico.

##### **POST /api/JsonPlaceholder/posts**
Cria um novo post na API externa.

**Exemplo de Request Body:**
```json
{
  "userId": 1,
  "title": "Título do Post",
  "body": "Conteúdo do post"
}
```

## 🏗️ Estrutura do Projeto

```
CSharpApi/
├── Constants/          # Constantes da aplicação (ex: roles)
├── Context/           # Contexto do Entity Framework
├── Controllers/       # Controllers da API
├── Helpers/           # Classes auxiliares
├── Migrations/        # Migrations do banco de dados
├── Models/           # Modelos de dados e DTOs
│   ├── DTOs/         # Data Transfer Objects
│   └── JsonPlaceholder/  # Modelos da API externa
└── Services/         # Lógica de negócio
```

## 🔐 Roles e Permissões

O sistema possui dois tipos de usuários:

- **User** (padrão): Pode visualizar e editar seu próprio perfil
- **Admin**: Pode visualizar, editar e deletar qualquer usuário

O primeiro usuário registrado pode ser promovido a Admin manualmente no banco de dados, se necessário.

## 🛠️ Tecnologias Utilizadas

- **.NET 9.0** - Framework principal
- **ASP.NET Core** - API REST
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados
- **JWT** - Autenticação
- **Swagger/OpenAPI** - Documentação da API
- **Docker** - Containerização