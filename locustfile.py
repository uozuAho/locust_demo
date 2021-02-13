import time
from datetime import datetime, timedelta
from locust import HttpUser, task, between, SequentialTaskSet

class CreateJobAndWaitUntilCompleted(HttpUser):
    @task
    def doit(self):
        job_id = self.start_job()
        start_time = datetime.now()

        self.wait_for_job_to_complete(job_id)
        finish_time = datetime.now()

        self.notify_work_completed(start_time, finish_time)

    def start_job(self):
        with self.client.post("/work?message=yo") as create_response:
            job_id = int(create_response.content)
            return job_id

    def wait_for_job_to_complete(self, job_id):
        work_complete = False
        while not work_complete:
            with self.client.get(f"/work/{job_id}", name="/work/[id]") as poll_response:
                work_complete = poll_response.content == b"done"
            time.sleep(.25)

    def notify_work_completed(self, start_time, finish_time):
        work_time_ms = (finish_time - start_time).total_seconds() * 1000

        self.client.request_success.fire(
            request_type="",
            name="work completed",
            response_time=work_time_ms,
            response_length=0)
