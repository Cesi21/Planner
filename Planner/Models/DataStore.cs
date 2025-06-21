using System.Collections.Generic;

namespace Planner.Models
{
    public class DataStore
    {
        public List<Goal> Goals { get; set; } = new();
        public List<Routine> Routines { get; set; } = new();

        // Tracks routine completion status by date
        public Dictionary<string, List<Routine>> RoutineProgress { get; set; } = new();
    }
}
