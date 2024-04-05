# Desafio AeC Alura
Este repositório tem como principal objetivo armazenar e auxiliar no desenvolvimento do projeto do Desafio AeC Alura.

## Tecnologias utilizadas
Este projeto foi desenvolvido na plataforma .NET, utilizando a versão 8 do framework. Para a manipulação de elementos em páginas web, foi empregada a tecnologia Selenium, sendo o WebDriver utilizado o ChromeDriver. A escolha pelo navegador Chrome se deu devido à minha familiaridade com ele.

O desenvolvimento foi realizado na IDE Visual Studio 2022, na versão 17.8.6.

#### Principais tecnologias utilizadas:

- Plataforma .NET 8

- Selenium (Selenium Chromedriver)

- Npgsql

- Visual Studio 2022 (Versão 17.8.6)

## Banco de dados

Utilizei o banco de dados PostgreSQL para este projeto, realizando a instalação da versão mais recente, a 16, através do site PostgreSQL. Estabeleci uma conexão local, configurando o usuário como "postgres" e a senha como "root". No entanto, é possível optar por uma configuração diferente ao criar uma conexão para o projeto, sendo necessário lembrar de ajustar a connection string no código-fonte (localizada como um atributo no arquivo worker) caso seja adotada uma abordagem alternativa.

Abaixo os scripts necessários para o banco de dados:

```sql
-- Primeiro passo para a criação do banco
CREATE DATABASE desafioalura;

-- Após a criação do banco de dados, conectar-se ao mesmo e criar as tabelas do projeto
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

-- Após realizar testes no projeto, podemos consultar os dados através das consultas abaixo.
SELECT * FROM item_result;
SELECT * FROM log;
```

## Sobre o Código
Busquei desenvolver o projeto de forma simples para solucionar o problema, seguindo alguns princípios do Domain-Driven Design (DDD) para garantir sua organização, manutenibilidade e escalabilidade. Com o passar do tempo, pude aprimorar diversos aspectos do projeto, o que só foi possível devido a uma implementação correta desde o início.

No que diz respeito à manipulação de threads, julguei desnecessário aplicá-las no processo de coleta e escrita de dados, uma vez que se trata de um fluxo singular. Optei, então, por empregar uma thread para cada aba do navegador, distribuindo as tarefas de forma eficiente.

A estrutura do projeto foi concebida de maneira simplificada em poucas camadas, considerando a natureza do projeto, que não demandava grande complexidade. A camada "application" é responsável pela execução do console, com a classe principal sendo o "Worker", que contém o método "ExecuteAsync". Este método estabelece a estrutura para a execução com threads, chamando o método "DoWork" do serviço.

O método "DoWork" representa essencialmente o fluxo principal do serviço. Cada etapa desse fluxo foi separada em métodos no serviço utilizado pelo Worker.

## Abordagem DDD
Desenvolver um projeto em C# utilizando Selenium e respeitando os princípios do Domain-Driven Design (DDD) é uma boa prática para garantir a organização, manutenibilidade e escalabilidade do código.

Organização do Projeto:

- Domain: Contém as entidades, serviços e objetos de valor do domínio do seu aplicativo.

- Infrastructure: Responsável por implementar os detalhes técnicos, como a interação com o Selenium e o acesso a dados.

- Application: Contém os casos de uso e serviços de aplicação que orquestram as operações entre o domínio e a infraestrutura.

- Persistence: Camada de acesso a dados que contém as implementações dos repositórios usando o Entity Framework.

No contexto do Domain-Driven Design (DDD), a separação em camadas é fundamental para manter o princípio de Single Responsibility, que busca garantir que cada classe ou componente tenha apenas uma razão para mudar. A separação em camadas ajuda a isolar o domínio das outras implementações, como infraestrutura e apresentação, permitindo que cada camada se concentre em suas responsabilidades específicas.

## Injeção de dependência
A implementação específica pode variar dependendo das necessidades e complexidade do seu projeto. No entanto, seguindo esses passos básicos, você pode criar uma aplicação usando a abordagem DDD com injeção de dependência em C#.

Um exemplo prático de injeção de dependência envolve a configuração de um serviço de escopo, como o AluraService, na classe Program. Em seguida, essa dependência é injetada no worker. Além disso, parâmetros específicos para a execução do projeto podem ser passados como argumentos (args) que também são injetados para serem utilizados no projeto.



## Tratamento dos Erros
Foram implementados tratamentos extensivos, sendo o foco direcionado principalmente à geração de logs. A presença de logs de erro é crucial para a manutenção do robô, facilitando a detecção do local e motivo de possíveis falhas. Em quase toda a aplicação, empregamos blocos try-catch para tratar cada etapa com mensagens específicas, simplificando a identificação de erros. Um ponto relevante é que esse processo direciona o tratamento até o método principal, onde os erros são registrados como logs no banco de dados.

## Assincronicidade na Execução de Tarefas Paralelas
No projeto, optei por utilizar threads para a execução eficiente de tarefas. Distribuindo o trabalho entre threads, possibilitamos a execução simultânea de múltiplas tarefas, o que pode resultar em melhorias significativas no desempenho e na eficiência do processo como um todo. Essa abordagem de execução paralela exemplifica a assincronicidade, onde várias tarefas podem ser realizadas simultaneamente, sem bloquear o fluxo principal do programa.

## Descrevendo o fluxo da aplicação

#### Inicialização do Programa:


O arquivo da solução está localizado na pasta Application, onde o programa inicia sua execução no projeto Application. Ele é iniciado no arquivo Program, onde algumas dependências são injetadas, e então o Worker é chamado para iniciar a execução.

#### Método ExecuteAsync:

Ao ser invocado, o método ExecuteAsync do Worker realiza o seguinte procedimento:

- Estabelece um ambiente assíncrono para a execução das tarefas.
- Cria um conjunto de threads para processar as tarefas de maneira paralela.
- Utiliza o serviço AluraService, injetado por dependência, para invocar o método DoWork. Este serviço é implementado pela classe base SeleniumService, que por sua vez implementa a interface ISeleniumService.

#### Método DoWork:

No método DoWork, são executadas as principais operações do programa. Cada etapa do fluxo principal é separada em métodos no serviço utilizado pelo Worker. Para o serviço Selenium, foram criados os seguintes métodos:

- <b>CreateInput</b>: Responsável por criar o objeto de entrada a partir do parâmetro string passado como argumento.

- <b>NavigateToInitialPage</b>: Inicia a página inicial da URL que está na classe passada por injeção de dependência (no caso, AluraService).

- <b>GettingData</b>: Responsável por coletar os dados necessários no site.

- <b>MakePersistence</b>: Os objetos obtidos são persistidos nesta etapa.

- <b>FinishWork</b>: Encerra o WebDriver e, possivelmente, outras tarefas caso necessário no futuro do projeto.

#### Inicialização do WebDriver:

Uma instância do WebDriver (ChromeDriver) é criada para interagir com o navegador Chrome.
Threads individuais são atribuídas para abrir abas do navegador e acessar páginas web.

#### Coleta de Dados:

Durante esta etapa, cada thread acessa uma página específica da plataforma de cursos online. O Selenium WebDriver é empregado para navegar nas páginas, encontrar os elementos relevantes (como título do curso, nome do professor, carga horária, etc.) e coletar essas informações.

Essa etapa é de suma importância para o desafio, como mencionado anteriormente. Ela é executada pelo método GettingData, que é sobrescrito no AluraService e contém a dinâmica principal do objetivo do projeto.

Primeiramente, o método SearchResults é chamado, onde é passado o objeto de entrada (itemInput), e são realizadas todas as ações necessárias para pesquisar os resultados. Nesta etapa, o tipo de conteúdo "cursos" é selecionado, pois é o que contém todos os campos necessários para a estrutura proposta no desafio.

Após a realização da busca, é hora de obter as informações nos cards por meio do método GetCardsInformation. Nesta etapa, são coletados o link, o título do curso e a descrição.

Com o link para a página do curso, as informações sobre o professor e a carga horária são obtidas no método CatchResult. Finalmente, os objetos contendo as informações necessárias são gerados.

#### Armazenamento no Banco de Dados:

Após a coleta de dados de cada curso, as informações são armazenadas em uma estrutura de dados temporária.
Uma conexão com o banco de dados PostgreSQL é estabelecida.
Os dados são inseridos nas tabelas correspondentes do banco de dados.

#### Tratamento de Erros:

Durante todo o processo, são implementados tratamentos extensivos de erros.
Bloco try-catch é empregado para capturar exceções e fornecer mensagens específicas para facilitar a identificação de erros.
Em caso de erro, os detalhes são registrados como logs no banco de dados.

#### Finalização do Programa:

Após concluir todas as tarefas, o programa é encerrado.


