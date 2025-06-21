using System;
using System.Collections.Generic;

namespace Planner.Models
{
    public class Routine
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string RepeatInterval { get; set; } = "Daily"; // Daily/Weekly/etc.
        public DateTime? LastCompletedDate { get; set; }
        public int StreakCount { get; set; }

        // Marks a routine as a reusable template
        public bool IsTemplate { get; set; }

        // Days of week this routine should repeat on when used as a template
        public List<DayOfWeek> RepeatOnDays { get; set; } = new();

        // Date this routine entry applies to
        public DateTime Date { get; set; } = DateTime.Today;

        // Indicates if the routine has been completed for the given date
        public bool IsCompleted { get; set; }
    }
}
