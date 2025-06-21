using System;

namespace Planner.Models
{
    public class Routine
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string RepeatInterval { get; set; } = "Daily"; // Daily/Weekly/etc.
        public DateTime? LastCompletedDate { get; set; }
        public int StreakCount { get; set; }
    }
}
