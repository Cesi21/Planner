using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Planner.Models;

namespace Planner.Services
{
    public class RoutineService
    {
        private readonly string _filePath;
        private Dictionary<string, List<Routine>> _routinesByDate = new();

        public RoutineService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "routines.json");
        }

        private async Task LoadAsync()
        {
            if (File.Exists(_filePath))
            {
                var json = await File.ReadAllTextAsync(_filePath);
                _routinesByDate = JsonSerializer.Deserialize<Dictionary<string, List<Routine>>>(json) ?? new();
            }
        }

        private async Task SaveAsync()
        {
            var json = JsonSerializer.Serialize(_routinesByDate, new JsonSerializerOptions { WriteIndented = true });
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory!);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<List<Routine>> GetRoutinesByDate(DateTime date)
        {
            await LoadAsync();
            var key = date.ToString("yyyy-MM-dd");
            if (_routinesByDate.TryGetValue(key, out var routines))
            {
                return routines;
            }
            return new List<Routine>();
        }

        public async Task SaveRoutineForDate(Routine routine)
        {
            await LoadAsync();
            var key = routine.Date.ToString("yyyy-MM-dd");
            if (!_routinesByDate.TryGetValue(key, out var routines))
            {
                routines = new List<Routine>();
                _routinesByDate[key] = routines;
            }
            var index = routines.FindIndex(r => r.Id == routine.Id);
            if (index >= 0)
                routines[index] = routine;
            else
                routines.Add(routine);

            await SaveAsync();
        }

        public async Task DeleteRoutineAsync(Guid id, DateTime date)
        {
            await LoadAsync();
            var key = date.ToString("yyyy-MM-dd");
            if (_routinesByDate.TryGetValue(key, out var routines))
            {
                var r = routines.FirstOrDefault(x => x.Id == id);
                if (r != null)
                {
                    routines.Remove(r);
                    if (routines.Count == 0)
                        _routinesByDate.Remove(key);
                    await SaveAsync();
                }
            }
        }

        public async Task<Dictionary<DateTime, List<Routine>>> GetRoutinesForDates(IEnumerable<DateTime> dates)
        {
            await LoadAsync();
            var result = new Dictionary<DateTime, List<Routine>>();
            foreach (var date in dates)
            {
                var key = date.ToString("yyyy-MM-dd");
                if (_routinesByDate.TryGetValue(key, out var routines))
                    result[date] = new List<Routine>(routines);
            }
            return result;
        }

        public async Task<Dictionary<DateTime, List<Routine>>> GetRoutinesForRange(DateTime start, DateTime end)
        {
            await LoadAsync();
            var result = new Dictionary<DateTime, List<Routine>>();
            foreach (var kvp in _routinesByDate)
            {
                if (DateTime.TryParse(kvp.Key, out var date))
                {
                    if (date >= start && date <= end)
                        result[date] = new List<Routine>(kvp.Value);
                }
            }
            return result;
        }

        public async Task<int> GetTotalCompletedInRange(DateTime start, DateTime end)
        {
            var routines = await GetRoutinesForRange(start, end);
            return routines.Values.Sum(list => list.Count(r => r.IsCompleted));
        }

        public async Task<int> GetTotalPlannedInRange(DateTime start, DateTime end)
        {
            var routines = await GetRoutinesForRange(start, end);
            return routines.Values.Sum(list => list.Count);
        }

        public async Task<int> GetFullCompletionDaysInRange(DateTime start, DateTime end)
        {
            var routines = await GetRoutinesForRange(start, end);
            int count = 0;
            foreach (var list in routines.Values)
            {
                if (list.Count > 0 && list.All(r => r.IsCompleted))
                    count++;
            }
            return count;
        }
    }
}
