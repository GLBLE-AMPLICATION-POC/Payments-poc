# payments-service
Repository to proof-of-concept the ability to manage and auto update template and services with Amplication.com

Definition:
A .NET Core Web API service for a job runner that uses Redis for data persistence,
provides RESTful endpoints to execute jobs, get job status, and view job history.
The service can be deployed using Docker Compose.


To test the service:
PreRequisite:
 - Redis in container: use docker-compose file to run Redis in container on the predefined port.

Running service locally with debug:
 - Run payments-service locally in debug mode
 - In Broswer go to `HTTP/1.1 GET http://localhost:5071/` and swagger page would be shown
 - Perform HTTP Post request to create and execute job  [`POST http://localhost:5071/api/v1/Jobs`]
 - Perform HTTP Get request to het job execution status [`GET http://localhost:5071/api/v1/Jobs/e6269b7a-7621-4e73-87b4-a26303e64c26`]

Running service in docker container:
 - Execute `podman compose up --build -d` or `docker-compose up --build -d` to build the image and run it in container
 - Navigate to `http://localhost:5071` to see swagger page
 - Perform requests similar to previous section
