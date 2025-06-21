using System;
using Plugin.LocalNotification;
using Planner.Models;

namespace Planner.Services
{
    public class ReminderService
    {
        private readonly RoutineService _routineService;

        public ReminderService(RoutineService routineService)
        {
            _routineService = routineService;
        }

        private int GetNotificationId(Guid id) => Math.Abs(id.GetHashCode());

        public async Task ScheduleReminderAsync(Routine routine)
        {
            if (!routine.IsReminderEnabled || !routine.ReminderTime.HasValue || routine.IsCompleted)
                return;

            var notifyTime = routine.Date.Date + routine.ReminderTime.Value;
            if (notifyTime <= DateTime.Now)
                return;

            var request = new NotificationRequest
            {
                NotificationId = GetNotificationId(routine.Id),
                Title = "Routine Reminder",
                Description = routine.Name,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime,
                    RepeatType = NotificationRepeat.No
                }
            };
            NotificationCenter.Current.Show(request);
        }

        public void CancelReminder(Guid id)
        {
            NotificationCenter.Current.Cancel(GetNotificationId(id));
        }

        public async Task RescheduleTodayRemindersAsync()
        {
            NotificationCenter.Current.CancelAll();
            var routines = await _routineService.GetRoutinesByDate(DateTime.Today);
            foreach (var r in routines)
            {
                await ScheduleReminderAsync(r);
            }
        }
    }
}
