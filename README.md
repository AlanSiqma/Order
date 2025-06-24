# Sistema de Gestão de Pedidos

Este projeto é um Sistema de Gestão de Pedidos desenvolvido em ASP.NET Core MVC que permite o gerenciamento completo de produtos e pedidos em um estabelecimento comercial.

## Funcionalidades Principais

- Cadastro e gerenciamento de produtos
- Criação de pedidos com múltiplos produtos
- Acompanhamento do status dos pedidos (Em Andamento, Pronto, Entregue)
- Painel visual para visualização em tempo real dos pedidos
- Cálculo automático de valores e tempo de espera

## Pré-requisitos

Para executar este projeto, você precisará ter instalado:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) (versão 8.0 ou superior)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) ou [Visual Studio Code](https://code.visualstudio.com/download)

## Configuração do Banco de Dados

### Opção 1: Instalação Local do MySQL

1. Instale o MySQL Server seguindo as instruções do [site oficial](https://dev.mysql.com/doc/mysql-installation-excerpt/8.0/en/)
2. Crie um banco de dados chamado `sgp`
3. Configure a string de conexão no arquivo `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;user=SEU_USUARIO;password=SUA_SENHA;database=sgp;"
}
```

### Opção 2: Usando Docker

1. [Instale o Docker](https://docs.docker.com/get-docker/)
2. Execute o seguinte comando para iniciar um container MySQL:

```bash
docker run --name mysql-sgp -e MYSQL_ROOT_PASSWORD=123456 -e MYSQL_DATABASE=sgp -p 3306:3306 -d mysql:latest
```

3. Verifique se a string de conexão no `appsettings.json` está correta:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;user=root;password=123456;database=sgp;"
}
```

## Executando o Projeto

### Usando Visual Studio

1. Abra a solução `Order.sln` no Visual Studio
2. Restaure os pacotes NuGet
3. Execute as migrações do banco de dados:
   - Abra o Console do Gerenciador de Pacotes
   - Execute: `Update-Database`
4. Execute o projeto pressionando F5 ou clicando em "Iniciar"

### Usando Linha de Comando

1. Navegue até a pasta do projeto `Order.MVC`
2. Execute os seguintes comandos:

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

3. Abra um navegador e acesse `https://localhost:7000` ou `http://localhost:5038`

## Tecnologias Utilizadas

- [ASP.NET Core MVC 9](https://learn.microsoft.com/aspnet/core)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [MySQL](https://www.mysql.com/) com [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
- [Bootstrap 5](https://getbootstrap.com/)
- [jQuery](https://jquery.com/)

## Licença

Este projeto está licenciado sob a GNU General Public License v3.0.