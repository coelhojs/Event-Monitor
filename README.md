# Event Monitor

## Sistema de monitoramento de sensores

O presente sistema processa e monitora em tempo real os dados enviados por sensores localizados nas regiões nordeste, norte, sudeste e sul. A solução proposta foi baseada na arquitetura de Microsserviços, com o uso do Docker Compose para a sua implantação automatizada.
Microsserviços que compõem a solução:
- Event-Monitor (API em .NET Core 3.1:)
  - EventProcessor: Serviço que mantém e controla a fila de novos eventos a serem processados e persistidos no banco de dados. As requisições de novos eventos enviadas pelos sensores são adicionadas a essa fila e o Controller retorna uma mensagem de sucesso ao sensor caso o evento tenha sido enfileirado com sucesso. O processamento dos eventos é feito de forma assíncrona e paralela. Os testes unitários e de integração foram implementados com a biblioteca xUnit e Moq.
  - EventAggregator: Este serviço é responsável por manter atualizados os dados estatísticos de todos os sensores registrados. Além disso, a atualização dos dados na aplicação front-end é realizada por esse serviço.
  - EventHub: Hub é uma interface de comunicação entre aplicações implementada pela biblioteca SignalR. Com essa interface, é possível manter aberto um canal de envio e recebimento de mensagens, sem que seja necessário realizar novas requisições.
- Front-End (Aplicação Web em Angular 11): Essa aplicação cliente exibe os dados estatísticos dos eventos enviados pelos sensores. Atualmente, ela exibe três itens: Uma tabela com os totalizadores de cada sensor e os totalizadores por região. Além disso, a tabela também informa o estado atual do sensor, podendo ser "erro" ou "processado".
- Postgres: Banco de dados relacional em que estão sendo persistidos os eventos. A modelagem atual persiste os eventos nas seguintes colunas de dados: "Region" (região onde está localizado o sensor), "Sensor" (identificador do sensor), "Timestamp" - data e hora da ocorrência do evento, "Value" - valor da tag registrado pelo sensor, ou vazio caso tenha ocorrido um erro.
- Event-Simulator (Worker Service em .NET Core 3.1): Aplicação somente para fins de teste da solução implementada.

### Instruções de execução:
- O sistema pode ser inicializado por completo a partir do comando `docker-compose up`, que deve ser executado no diretório raiz da solução, ou os componentes da solução podem ser inicializados individualmente com o comando `docker-compose up <nome-do-serviço>`. Para iniciar a solução sem o simulador, por exemplo, pode ser usado o comando `docker-compose up frontend`.

### Tecnologias utilizadas
- .NET Core 3.1
- Entity Framework Core
- xUnit + Moq
- Postgres 9.6
- Angular 11 com Angular Material
- Docker e Docker Compose
- Gitlab CI

### Pontos de melhoria e possíveis inovações:
- Para evitar um uso tão intenso do banco de dados, poderiam ser utilizados objetos, como listas e dicionários para agregar os dados à medida em que eles são recebidos, sem a necessidade de consultar o banco de dados. Desse modo, a consulta no banco somente seria feita em caso de falhas e interrupções no funcionamento do sistema.
- Visando garantir mais alguns atributos de qualidade, como escalabilidade e tolerância a falhas, seria importante manter réplicas do serviço e o uso de um orquestrador de containers, como o Kubernetes.
- Mais testes unitários e de integração e testes de carga.
- Por se tratar de uma solução de tratamento de dados em tempo-real, seria bastante vantajoso utilizar uma plataforma de streaming e integração de dados, como o Apache Kafka.
- Os gráficos utilizados no front-end são do tipo histograma, cujas colunas só informam as quantidades de eventos de cada sensor. Seria mais informativo apresentar um gráfico em que cada linha representaria um sensor, o eixo y seria o valor da tag, e o eixo x seria a data/hora da ocorrência. Eu tentei implementar esse gráfico, como pode ser visto nos método GetChartData e no componente do Angular statsChart, mas, devido ao enfoque maior no back-end e ao tempo limitado, não foi possível.
- Eu gostaria de ter incluído uma ferramenta para monitorar a cobertura de código nos testes unitários.

### Notas ao avaliador:
O Event-Monitor foi implementado utilizando o sistema de Injeção de dependências nativo do .NET Core 3.1, com o uso do logger padrão da plataforma. O padrão de projetos utilizado foi o MVC com uma camada Business, para a implementação das regras de negócio e uma camada Service, onde ficam as instâncias de serviços autônomos. Utilizei os logs também como forma de documentar a implementação nos pontos que possam gerar mais dúvidas. Além disso, coloco-me à disposição para demonstrar o funcionamento do sistema ou dar mais detalhes sobre as minhas decisões de implementação.