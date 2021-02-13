using System;

namespace apiserver
{
    public class Job
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Started { get; set; }
        public DateTime Completed { get; set; }
    }
}