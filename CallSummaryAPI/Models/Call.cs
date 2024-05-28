using System;

namespace CallSummaryAPI.Models
{
    public class Call
    {
        public string? ID { get; set; }
        public DateTime CallDate { get; set; }
        public DateTime CallDateUTC { get; set; }
        public string? CallerID { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public Duration? Duration { get; set; }
        public List<Extension> ConnectedExtensions { get; set; } = new List<Extension>();
    }

    public class Duration
    {
        public long Ticks { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Milliseconds { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public double TotalDays { get; set; }
        public double TotalHours { get; set; }
        public int TotalMilliseconds { get; set; }
        public double TotalMinutes { get; set; }
        public int TotalSeconds { get; set; }
    }

    public class Extension
    {
        public string? Name { get; set; }
        public string? Number { get; set; }
    }
}
