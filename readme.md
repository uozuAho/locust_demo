# Locust demo

Runs locust against a dotnet server, using docker-compose.


# Getting started

- install docker

> docker-compose build  # needed after any changes to the server
> LOCUST_FILE=load_tests/locustfiles/create_job_and_wait_until_completed.py docker-compose up

- browse to localhost:8089
- select any number of users & spawn rate
- set the host to http://server
- click start swarming


# The server

The server has two endpoints:

POST /work?message=x          returns a job id
GET /work/job_id              returns the status of the job (new or done)

Jobs are processed by a background worker at a rate of 1 job per second.


# Running different load tests

Choose the locustfile to use by setting the LOCUST_FILE environment variable.
