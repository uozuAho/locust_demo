import time
from locust import HttpUser, task, between

class QuickstartUser(HttpUser):
    wait_time = between(1, 2.5)

    @task
    def view_items(self):
        self.client.get("/WeatherForecast")
