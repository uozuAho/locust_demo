import time
from datetime import datetime, timedelta
from locust import HttpUser, task, between, SequentialTaskSet

class CreateOneJobPerSecond(HttpUser):
    wait_time = between(1, 1)

    @task
    def doit(self):
        self.start_job()

    def start_job(self):
        with self.client.post("/work?message=yo") as create_response:
            job_id = int(create_response.content)
            return job_id
