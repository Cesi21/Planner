using System.IO;
using System.Text.Json;
using System.Linq;
using Microsoft.Maui.Storage;
using Planner.Models;

namespace Planner.Services
{
    public class DataService
    {
        private readonly string _filePath;
        private DataStore _data = new();

        public DataService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "data.json");
        }

        private async Task LoadAsync()
        {
            if (File.Exists(_filePath))
            {
                var json = await File.ReadAllTextAsync(_filePath);
                _data = JsonSerializer.Deserialize<DataStore>(json) ?? new DataStore();
            }
        }

        private async Task SaveAsync()
        {
            var json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            await File.WriteAllTextAsync(_filePath, json);
        }

        private string TodayKey => DateTime.Today.ToString("yyyy-MM-dd");

        public async Task<List<Goal>> GetGoalsAsync()
        {
            await LoadAsync();
            return _data.Goals;
        }

        public async Task AddGoalAsync(Goal goal)
        {
            await LoadAsync();
            _data.Goals.Add(goal);
            await SaveAsync();
        }

        public async Task<List<Routine>> GetRoutinesAsync()
        {
            await LoadAsync();
            return _data.Routines;
        }

        public async Task UpdateRoutineAsync(Routine routine)
        {
            await LoadAsync();
            var index = _data.Routines.FindIndex(r => r.Id == routine.Id);
            if (index >= 0)
            {
                _data.Routines[index] = routine;
                await SaveAsync();
            }
        }

        public async Task AddRoutineAsync(Routine routine)
        {
            await LoadAsync();
            _data.Routines.Add(routine);
            await SaveAsync();
        }

        public async Task<List<Routine>> GetTodayRoutinesAsync()
        {
            await LoadAsync();

            if (!_data.RoutineProgress.TryGetValue(TodayKey, out var routines))
            {
                routines = _data.Routines.Select(r => new Routine
                {
                    Id = r.Id,
                    Name = r.Name,
                    RepeatInterval = r.RepeatInterval,
                    LastCompletedDate = r.LastCompletedDate,
                    StreakCount = r.StreakCount,
                    Date = DateTime.Today,
                    IsCompleted = false
                }).ToList();
                _data.RoutineProgress[TodayKey] = routines;
                await SaveAsync();
            }

            return routines;
        }

        public async Task UpdateTodayRoutineAsync(Routine routine)
        {
            await LoadAsync();

            if (_data.RoutineProgress.TryGetValue(TodayKey, out var routines))
            {
                var index = routines.FindIndex(r => r.Id == routine.Id);
                if (index >= 0)
                {
                    routines[index] = routine;
                    await SaveAsync();
                }
            }
        }
    }
}
