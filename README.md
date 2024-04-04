# DesafioAlura
Este repositório terá como principal objetivo armazenar e auxiliar o projeto do Desafio Alura

# Tecnologias utilizadas
Este projeto foi desenvolvido na plataforma .NET, utilizando a versão 8 do framework. Para a manipulação de objetos de páginas web, foi empregada a tecnologia Selenium, com foco no Selenium Chromedriver. Optei por utilizar o navegador Chrome devido à minha familiaridade com ele.


O desenvolvimento foi realizado na IDE Visual Studio 2022, na versão 17.8.6.

Tecnologias Principais Utilizadas:

Plataforma .NET 8

Selenium (Selenium Chromedriver)

Npgsql

Visual Studio 2022 (Versão 17.8.6)

# Banco de dados

Utilizei o banco de dados PostgreSQL para este projeto, realizando a instalação da versão 
mais recente, a 16, por meio do site https://www.postgresql.org/download/. Estabeleci 
uma conexão local, configurando o usuário como "postgres" e a senha como "root".
Contudo, é possível optar por uma configuração diferente ao criar uma conexão para o 
projeto, sendo necessário lembrar de ajustar a connection string no código-fonte 
(localizada como um atributo no arquivo worker) caso seja adotada uma abordagem 
alternativa.

Abaixo os scripts necessários para o banco de dados:

--primeiro passo pra criação do banco
CREATE DATABASE desafioalura;

-- Após criado o banco de dados, conectar no mesmo e criar as tabelas do projeto

CREATE TABLE item_result (

  id SERIAL PRIMARY KEY,
  
  titulo VARCHAR(400) NOT NULL,
  
  professor VARCHAR(255) NOT NULL,
  
  carga_horaria integer NOT NULL,
  
  descricao VARCHAR(2000) NOT NULL,	
  
  date TIMESTAMP
  
);


CREATE TABLE log (

  id SERIAL PRIMARY KEY,
  
  text VARCHAR(1000) NOT NULL,
  
  date TIMESTAMP
  
);

-- Após fazer testes no projeto, podemos consultar os dados através das consultas abaixo.

SELECT * FROM item_result;

SELECT * FROM log;


# Sobre o Código
Busquei desenvolver o projeto de forma simples para solucionar o problema, enquanto ainda seguia alguns princípios do Domain-Driven Design (DDD) e garantia sua escalabilidade. Com o passar do tempo, pude aprimorar diversos aspectos do projeto, o que só foi possível devido a uma implementação correta desde o início.

No que diz respeito à manipulação de threads, julguei desnecessário aplicá-las no 
processo de coleta e escrita de dados, uma vez que se trata de um fluxo singular. Optei, 
então, por empregar uma thread para cada aba do navegador, distribuindo as tarefas de
forma eficiente.

A estrutura do projeto foi concebida de maneira simplificada em poucas camadas, 
considerando a natureza do projeto, que não demandava grande complexidade. A 
camada "application" é responsável pela execução do console, com a classe principal 
sendo o "Worker", que contém o método "ExecuteAsync". Este método estabelece a 
estrutura para a execução com threads, chamando o método "DoWork" do serviço.

O método "DoWork" representa essencialmente o fluxo principal do serviço. 
Cada etapa desse fluxo foi separada em métodos no serviço utilizado pelo Worker". 

# Abordagem DDD
Desenvolver um projeto em C# utilizando Selenium e respeitando os princípios do Domain-Driven Design (DDD) é uma boa prática para garantir a organização, manutenibilidade e escalabilidade do código.

Organização do Projeto:

Domain: Contém as entidades, serviços e objetos de valor do domínio do seu aplicativo.

Infrastructure: Responsável por implementar os detalhes técnicos, como a interação com o Selenium e o acesso a dados.

Application: Contém os casos de uso e serviços de aplicação que orquestram as operações entre o domínio e a infraestrutura.

Persistence: Camada de acesso a dados que contém as implementações dos repositórios usando o Entity Framework.

No contexto do Domain-Driven Design (DDD), a separação em camadas é fundamental para manter o princípio de Single Responsibility, que busca garantir que cada classe ou componente tenha apenas uma razão para mudar. A separação em camadas ajuda a isolar o domínio das outras implementações, como infraestrutura e apresentação, permitindo que cada camada se concentre em suas responsabilidades específicas.

# Injeção de dependência
A implementação específica pode variar dependendo das necessidades e complexidade do seu projeto. No entanto, seguindo esses passos básicos, você pode criar uma aplicação usando a abordagem DDD com injeção de dependência em C#.


Um exemplo prático de injeção de dependência envolve a configuração de um serviço de escopo, como o AluraService, na classe Program. Em seguida, essa dependência é injetada no worker. Além disso, parâmetros específicos para a execução do projeto podem ser passados como argumentos (args) que também são injetados para serem utilizados no projeto.



# Tratamento dos Erros
Foram implementados tratamentos extensivos, sendo o foco direcionado principalmente à 
geração de logs. A presença de logs de erro é crucial para a manutenção do robô, 
facilitando a detecção do local e motivo de possíveis falhas.

