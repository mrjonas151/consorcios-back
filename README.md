# Gerenciador de cotas de consórcios ✅

**O Gerenciador de cotas de consórcios** é uma aplicação web desenvolvida com .NET 8 no backend e React no frontend, além do uso de BFF. A aplicação permite que as cotas de consórcio sejam criadas, visualizadas, deletadas e atualizadas, um CRUD completo de algumas entidades extras também foram implementadas para fazer relação com a tabela de cotas.

## 🚀 Começando

Estas instruções ajudarão você a colocar uma cópia do projeto em funcionamento em sua máquina local para fins de desenvolvimento e teste.

### 📋 Pré-Requisitos

Certifique-se de que seu ambiente de desenvolvimento esteja configurado corretamente com os seguintes requisitos:

### 🔧 Instalação

Siga os passos abaixo para configurar o ambiente de desenvolvimento e rodar a aplicação:

## 🚢 Rodando BD com Docker

Para utilizar o projeto corretamente, é **necessário** rodar o SQL Server via Docker. Siga os passos abaixo:

### Pré-requisitos Docker

Certifique-se de que o Docker está instalado em sua máquina.  
Para verificar se o Docker está instalado corretamente, execute o seguinte comando no terminal:

```bash
docker --version
```

### Configuração do SQL Server

O arquivo `docker-compose.yml` na raiz do projeto já está configurado para criar um container do SQL Server:

```yaml
version: "3.8"
services:
    sqlserver:
        image: mcr.microsoft.com/mssql/server:2022-latest
        environment:
            - ACCEPT_EULA=Y
            - SA_PASSWORD=SuaSenha
        ports:
            - "1433:1433"
        volumes:
            - sqlserver_data:/var/opt/mssql

volumes:
    sqlserver_data:
```

### Iniciando o SQL Server

Execute o seguinte comando para iniciar o container do SQL Server:

```bash
docker-compose up -d sqlserver
```

O parâmetro `-d` executa o container em modo detached (em segundo plano).

### Verificando se o SQL Server está rodando

Para confirmar que o container está em execução:

```bash
docker ps
```

Você deverá ver um container com o nome do projeto e SQL Server em execução.

### Configuração da Conexão

Após iniciar o SQL Server no Docker, configure a string de conexão no arquivo `appsettings.json` do projeto backend:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost,1433;Database=ConsorciosDB;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
    }
}
```

**Backend (.NET 8)**
Para rodar o backend, verifique se você tem o .NET 8 SDK instalado. Para verificar se o .NET está instalado corretamente, execute o seguinte comando no terminal:

```
dotnet --version
```

O resultado deve mostrar pelo menos a versão 8.0.0 ou superior.

**Frontend (React)**
Para rodar o frontend em React, verifique se o Node.js está instalado. Para isso, execute o seguinte comando no terminal:

```
node -v
```

Recomendo que o Node.js seja da versão 18.x ou superior.

**Banco de Dados**
O projeto utiliza SQL Server como banco de dados. Certifique-se de ter:

-   SQL Server instalado ou acesso a uma instância
-   Uma conexão válida configurada no arquivo de configuração

**Ferramentas Recomendadas**

-   Visual Studio Code para desenvolvimento do backend
-   VS Code com extensões para React/JavaScript para o frontend
-   Insomnia ou similar para testar os endpoints da API

#### **Clone os repositórios**

```bash
git clone https://jonasaquino@bitbucket.org/newm-dev1/jonas-backend.git
```

```bash
git clone https://jonasaquino@bitbucket.org/newm-dev1/jonas-frontend.git
```

```bash
git clone https://jonasaquino@bitbucket.org/newm-dev1/jonas-bff.git
```

#### **Configuração do Backend (.NET 8)**

```bash
cd ConsorcioBackend
```

**Configurando o Banco de Dados:**

-   Certifique-se de seguir os passos de configuração do Banco de Dados na seção de Docker.

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=ConsorciosDB;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
    }
}
```

**Executando as migrações do banco de dados:**

```bash
cd ConsorcioBackend
dotnet ef database update
```

**Rodando o backend:**

```bash
dotnet run
```

O servidor backend estará acessível em `https://localhost:PORTA` (as portas podem variar, verifique a saída do console).

#### **3. Configuração do Frontend (React)**

Navegue até a pasta do frontend:

```bash
cd ConsorcioFrontend
```

Instale as dependências do projeto:

```bash
npm install
```

Inicie o servidor de desenvolvimento:

```bash
npm run dev
```

O frontend estará disponível em `http://localhost:5173` (ou a porta indicada no console).

#### **4. Variáveis de Ambiente**

Para configurar variáveis de ambiente específicas, crie um arquivo `.env` na raiz do projeto backend:

```
DB_HOST=localhost
DB_PORT=PORT
DB_NAME=name
DB_USER=user
DB_PASSWORD=db_password
JWT_KEY=jwt_key
JWT_ISSUER=jwt_issuer
JWT_AUDIENCE=jwt_audience
```

#### **5. Verificando a instalação**

-   Backend: Acesse `https://localhost:PORTA/swagger` para visualizar a documentação da API pelo Swagger.
-   Frontend: Acesse a URL fornecida pelo terminal do frontend (geralmente `http://localhost:5173`).

Com o backend e o frontend configurados e em execução, você poderá acessar a aplicação completa. Certifique-se de que o backend está ativo antes de testar as funcionalidades da aplicação.

## 📚 API Endpoints

(SWagger)

## 🧪 Rodando os Testes

O projeto contém testes unitários e de integração que validam o comportamento dos serviços, controladores e outras funcionalidades chave da aplicação. Estes testes são construídos usando xUnit e Moq para mocking de dependências.

### Visualizando detalhes dos testes

Para obter mais detalhes sobre a execução dos testes unitários:

```bash
cd tests
cd ConsorcioBackend.UnitTests
```

```bash
dotnet test --verbosity normal --no-build
```

## 🛠️ Construído com

-   [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - Framework moderno e de alto desempenho para construção de APIs e aplicações web
-   [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) - ORM (Object-Relational Mapping) para acesso a dados
-   [SQL Server](https://www.microsoft.com/en-us/sql-server) - Banco de dados relacional utilizado para armazenar as informações
-   [JWT Authentication](https://jwt.io/) - Sistema de autenticação baseado em tokens
-   [Swagger/OpenAPI](https://swagger.io/) - Documentação interativa da API
-   [Docker](https://www.docker.com/) - Plataforma de containerização para facilitar o desenvolvimento e implantação

## 🧪 Ferramentas de Teste

-   [xUnit](https://xunit.net/) - Framework para testes unitários em .NET
-   [Moq](https://github.com/moq/moq4) - Framework de mocking para testes unitários

## ✒️ Author

Aplicação desenvolvida por um autor

<div align="center">
  <table>
    <tr>
      <td align="center">
        <a href="https://github.com/mrjonas151">
          <img src="https://avatars.githubusercontent.com/u/89425034?v=4" width="100px;" alt="Jonas's Photo"/><br>
          <sub>
            <b>Jonas Tomaz de Aquinos</b>
          </sub>
        </a>
      </td>
    </tr>
  </table>
</div>

## 🎁 Agradecimentos

Agradeço a leitura deste documento e a equipe da NewM pelo apoio ao desenvolvimento de um projeto bem completo desde a criação dessa API com testes e docker.

---

⌨️ com ❤️ por Jonas Tomaz 😉
