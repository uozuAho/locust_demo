import time
from locust import HttpUser, task, between, SequentialTaskSet

class WorkUser(HttpUser):
    wait_time = between(0.5, 0.5)

    @task
    def create_work(self):
        with self.client.post("/work?message=yo") as create_response:
            job_id = int(create_response.content)

        work_complete = False
        while not work_complete:
            with self.client.get(f"/work/{job_id}", name="/work/[id]") as poll_response:
                work_complete = poll_response.content == b"done"
            time.sleep(1)
