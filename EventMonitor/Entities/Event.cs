using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventMonitor.Entities
{
    [Table("EVENT")]
    public class Event
    {
        [Required]
        [Column(name: "TIMESTAMP")]
        public DateTime Timestamp { get; set; }

        [Required]
        [Column(name: "TAG")]
        public string Tag { get; set; }

        [Required]
        [Column(name: "VALUE")]
        public string Value { get; set; }
    }
}