using System;
using System.Text.Json.Serialization;

namespace Planner.Models
{
    public class Goal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TargetValue { get; set; }
        public int CurrentValue { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }

        [JsonIgnore]
        public double Progress => TargetValue == 0 ? 0 : (double)CurrentValue / TargetValue;
    }
}
