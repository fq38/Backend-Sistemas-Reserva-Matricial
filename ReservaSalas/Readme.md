# Sistema de Reserva de Salas - API Back-end (Banana Ltda.)

Esta é a API RESTful desenvolvida para o Sistema de Reserva de Salas. Construída com .NET 8 e C#, esta API serve como o cérebro da aplicação, responsável por toda a lógica de negócio, validação de dados e comunicação com o banco de dados SQL Server.

A API fornece endpoints seguros e eficientes para que a aplicação front-end possa realizar operações de CRUD (Create, Read, Update, Delete) para as reservas de salas.

## ✨ Funcionalidades

*   **Endpoints RESTful:** Uma API bem definida seguindo os padrões REST para manipulação de recursos (`Reservations`, `Locations`, `Rooms`).
*   **Validação de Conflito de Horários:** Lógica de negócio implementada no servidor para impedir que duas reservas sejam criadas para a mesma sala no mesmo período de tempo.
*   **Persistência de Dados:** Utiliza o Entity Framework Core para mapear objetos C# para um banco de dados relacional SQL Server, garantindo a persistência e a integridade dos dados.
*   **Serviço de Dados Auxiliares:** Fornece endpoints para listar locais e salas, permitindo que o front-end popule seus formulários dinamicamente.
*   **Configuração de CORS:** Política de CORS configurada para permitir requisições do front-end em ambiente de desenvolvimento.

## 🚀 Tecnologias e Padrões Utilizados

A escolha das tecnologias foi focada em robustez, performance e produtividade, utilizando ferramentas amplamente consolidadas no mercado corporativo.

| Tecnologia / Padrão | Justificativa |
| :--- | :--- |
| **.NET 8 e C#** | A versão mais recente da plataforma .NET, escolhida por sua alta performance, segurança e ecossistema robusto. É ideal para a construção de APIs escaláveis e de alta velocidade. |
| **ASP.NET Core Web API** | Framework para a construção de APIs HTTP. Foi utilizado para criar os endpoints RESTful de forma rápida e estruturada, com suporte nativo a injeção de dependência e configuração. |
| **Entity Framework Core 8** | ORM (Object-Relational Mapper) moderno e flexível. Foi a escolha para abstrair a comunicação com o banco de dados, permitindo trabalhar com objetos C# em vez de escrever SQL manualmente, o que aumenta a produtividade e a segurança. |
| **SQL Server** | Sistema de gerenciamento de banco de dados relacional robusto e confiável, amplamente utilizado em ambientes corporativos, garantindo a integridade dos dados através de constraints e chaves estrangeiras. |
| **Padrão Repository/Unit of Work (implícito via DbContext)** | O `DbContext` do Entity Framework Core já implementa esses padrões, centralizando a lógica de acesso a dados e gerenciando as transações, o que mantém os controllers limpos e focados na lógica de negócio. |
| **DTO (Data Transfer Object)** | Foi utilizado o padrão DTO (`CreateReservationDto`) para desacoplar o modelo da API do modelo do banco de dados. Isso aumenta a segurança (expondo apenas os dados necessários) e simplifica a validação dos dados que chegam do cliente. |

## ⚙️ Como Rodar a Aplicação

Para executar esta API em seu ambiente local, siga os passos abaixo.

### Pré-requisitos

*   **.NET 8 SDK**
*   **SQL Server** (Express, Developer ou qualquer outra edição)
*   Uma ferramenta de gerenciamento de banco de dados, como **SQL Server Management Studio (SSMS)** ou **Azure Data Studio**.
*   Front-end da aplicação Acesse o [github do frontend](https://github.com/fq38/Frontend-Sistemas-Reserva-Matricial.git).

### 1. Configuração do Banco de Dados

1.  **Crie o Banco de Dados:**
    No SQL Server, crie um novo banco de dados com o nome exatamente igual a: `ReservaSalasDb`.

2.  **Configure a String de Conexão:**
    Abra o arquivo `appsettings.json` na raiz do projeto. Encontre a seção `ConnectionStrings` e ajuste o valor de `DefaultConnection` para que corresponda à sua instância local do SQL Server.

    *   **Para autenticação do Windows (geralmente padrão no SQL Server Express):**
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ReservaSalasDb;Trusted_Connection=True;TrustServerCertificate=True;"
        }
        ```
    *   **Se você usa um login e senha específicos:**
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=localhost;Database=ReservaSalasDb;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;"
        }
        ```

### 2. Execução da Aplicação

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/fq38/Backend-Sistemas-Reserva-Matricial.git
    cd sistema-reserva-salas-backend
    ```

2.  **Restaure as dependências:**
    Abra um terminal na pasta raiz do projeto e execute:
    ```bash
    dotnet restore
    ```

3.  **Aplique as Migrações do Entity Framework:**
    Este comando irá criar todas as tabelas (`Locations`, `Rooms`, `Reservations`) no banco de dados `ReservaSalasDb` que você criou.
    ```bash
    dotnet ef database update
    ```

4.  **(MUITO IMPORTANTE) Popule os Dados Iniciais:**
    Para que a aplicação funcione corretamente, as tabelas `Locations` e `Rooms` precisam de dados. Execute os seguintes comandos SQL no seu banco `ReservaSalasDb` usando SSMS ou outra ferramenta:

    ```sql
    -- Inserir alguns locais/filiais
    INSERT INTO dbo.Locations (Name) VALUES ('São Paulo - Paulista');
    INSERT INTO dbo.Locations (Name) VALUES ('Rio de Janeiro - Centro');

    -- Se 'São Paulo' ficou com ID 1 e 'Rio' com ID 2, insira as salas:
    INSERT INTO dbo.Rooms (Name, LocationId) VALUES ('Sala de Reunião 101', 1); -- Sala para SP
    INSERT INTO dbo.Rooms (Name, LocationId) VALUES ('Sala de Foco 102', 1);     -- Outra sala para SP
    INSERT INTO dbo.Rooms (Name, LocationId) VALUES ('Auditório', 2);             -- Sala para o RJ
    ```

5.  **Inicie a API:**
    ```bash
    dotnet run
    ```

A API estará em execução e pronta para receber requisições, geralmente em `https://localhost:7036` (verifique a porta exata no seu terminal).

## 📄 Endpoints da API

A API expõe os seguintes endpoints principais (a lista completa pode ser vista na interface do Swagger, geralmente em `/swagger`):

*   `GET /api/reservations`: Lista todas as reservas.
*   `POST /api/reservations`: Cria uma nova reserva.
*   `PUT /api/reservations/{id}`: Atualiza uma reserva existente.
*   `DELETE /api/reservations/{id}`: Exclui uma reserva.
*   `GET /api/locations`: Lista todos os locais/filiais.
*   `GET /api/rooms`: Lista todas as salas.