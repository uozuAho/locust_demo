# Locust demo

Runs locust against a dotnet server, using docker-compose.


# Getting started

- install docker

> docker-compose build  # needed after any changes to the server
> docker-compose up

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

Change the `command` setting in docker-compose.yml


# Experiments

For all of these experiments, I'm interested in the time it takes to complete a
job. You'll notice in the locust reports that the endpoint response times are
great - a few milliseconds. These are independent of the load on the system.
However, the 'work completed' metric is affected by the load on the system.

## 1 virtual user (VU) creates one job per second
With one user, the worker can keep up, so jobs don't wait for long in the queue.
With any more than one user, the job queue gets longer and longer. The job
completion time is unbounded, and the queue will only shrink once the job
creation rate drops below 1 job per second.

@ 5 VUs
- 5 RPS to POST /work
- time to complete job: unbounded, proportional to length of load test


## 1 VU creates jobs, waiting for each one to complete
The time each user waits for a job to complete tends to the number of users.
This is only an accurate load test if your users are blocked from performing
actions by a time that is proportional to system load.

@ 5 VUs
- 1 RPS to POST /work
- time to complete job: 5s

Note that RPS is constrained to the rate that the worker completes jobs.
