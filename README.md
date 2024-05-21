Research Files Storage

Main ideea of this project is to allow entities to add some notes/information that will eventually be stored in a database and  'on a local drive'. Size of the content is very limited but by no means saving it to database should not be acceptable in a production environment. Saving to local drive is not actually happening but is out of the scope of this project, since the idea here was to perform some data manipulation on the client side.

HttpRequestCounter.cs is the singleton thread safe implementation that will count all the http requests coming to api
Note: HttpRequestCounterController allows to check the number of requests since api is running ( calling this controller will not increase counter of the http calls )

FileBuilder.cs is the builder pattern implementation.

How to run:
Inside project are 2 docker compose files
'docker-compose-partial.yml' will start mongodb and rabbitmq
'docker-compose.yml' will start all services which mean: mongodb, rabbitmq, api, client

To run docker-compose use the following command: docker compose -f docker-compose.yml up --build -d 
Note: You must be on the same folder of the docker compose file
After testing apply the following command to stop the services: docker compose down

To start Api or Client from Visual Studio ( or other ide ) use the preset configuration. ( Note that mongodb and rabbitmq have to be started via docker-compose-partial.yml file ).
There are two environments: 
    - Development, used when you start project from Visual Studio
    - Docker, used when you start via docker compose