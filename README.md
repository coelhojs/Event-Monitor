# Event Monitor

## Sensor Monitoring System

This system processes and monitors real-time data sent by sensors located in the northeast, north, southeast, and south regions. The proposed solution is based on a Microservices architecture, utilizing Docker Compose for automated deployment.
Microservices that make up the solution:
- Event-Monitor (API in .NET Core 3.1):
  - EventProcessor: A service that maintains and controls the queue of new events to be processed and persisted in the database. Requests for new events sent by sensors are added to this queue, and the controller returns a success message to the sensor if the event has been successfully queued. Event processing is done asynchronously and in parallel. Unit and integration tests have been implemented using the xUnit and Moq libraries.
  - EventAggregator: This service is responsible for keeping the statistical data of all registered sensors up to date. Additionally, it updates the data in the front-end application.
  - EventHub: Hub is a communication interface between applications implemented using the SignalR library. With this interface, it's possible to maintain an open channel for sending and receiving messages without the need for new requests.
- Front-End (Web Application in Angular 11): This client application displays statistical data for events sent by the sensors. Currently, it displays three items: a table with the totals for each sensor and totals by region. Additionally, the table indicates the current state of the sensor, which can be "error" or "processed."
- Postgres: A relational database where events are being persisted. The current data model stores events in the following data columns: "Region" (region where the sensor is located), "Sensor" (sensor identifier), "Timestamp" - date and time of the event occurrence, "Value" - value of the tag recorded by the sensor, or empty if an error occurred.
- Event-Simulator (Worker Service in .NET Core 3.1): Application for testing purposes of the implemented solution.

### Execution Instructions:
- The system can be fully initialized with the `docker-compose up` command, which should be executed in the root directory of the solution, or the solution components can be initialized individually with the `docker-compose up <service-name>` command. For example, to start the solution without the simulator, you can use the `docker-compose up frontend` command.

### Technologies Used:
- .NET Core 3.1
- Entity Framework Core
- xUnit + Moq
- Postgres 9.6
- Angular 11 with Angular Material
- Docker and Docker Compose
- Gitlab CI

### Improvement Points and Possible Innovations:
- To reduce intensive database usage, objects such as lists and dictionaries could be used to aggregate data as it is received, without the need to query the database. This way, database queries would only be necessary in case of failures and system interruptions.
- To ensure additional quality attributes such as scalability and fault tolerance, it would be important to maintain service replicas and use a container orchestrator like Kubernetes.
- More unit and integration tests and load tests.
- Since this is a real-time data processing solution, it would be advantageous to use a streaming and data integration platform like Apache Kafka.
- The charts used in the front-end are histograms, where columns only show the quantities of events for each sensor. It would be more informative to present a chart where each line represents a sensor, the y-axis is the tag value, and the x-axis is the date/time of occurrence. I attempted to implement this chart, as can be seen in the GetChartData method and the Angular statsChart component, but due to a stronger focus on the back-end and limited time, it was not possible.
- I would have liked to include a tool for monitoring code coverage in unit tests.

### Notes to the Evaluator:
The Event-Monitor was implemented using the native Dependency Injection system of .NET Core 3.1, with the use of the platform's standard logger. The design pattern used was MVC with a Business layer for implementing business rules and a Service layer where autonomous services are located. I also used logs as a way to document the implementation in areas that may generate more questions. Additionally, I am available to demonstrate the system's operation or provide more details about my implementation decisions.
