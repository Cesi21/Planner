using System;

namespace Planner.Models
{
    public class Goal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Type { get; set; } = string.Empty; // Daily/Monthly/Yearly
        public bool IsCompleted { get; set; }
    }
}
