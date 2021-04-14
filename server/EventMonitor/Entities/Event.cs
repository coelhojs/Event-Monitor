using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventMonitor.Entities
{
    [Table("EVENT")]
    public class Event
    {
        [Required]
        [Column(name: "ID")]
        public long Id { get; set; }

        [Required]
        [Column(name: "REGION")]
        public string Region { get; set; }

        [Required]
        [Column(name: "SENSOR")]
        public string Sensor { get; set; }

        [Required]
        [Column(name: "TIMESTAMP")]
        public DateTime Timestamp { get; set; }

        [Required]
        [Column(name: "VALUE")]
        public string Value { get; set; }
    }
}