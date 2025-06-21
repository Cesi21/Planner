using Planner.Services;

namespace Planner
{
    public partial class App : Application
    {
        private readonly ReminderService _reminderService;

        public App(ReminderService reminderService)
        {
            InitializeComponent();
            _reminderService = reminderService;
            _ = _reminderService.RescheduleTodayRemindersAsync();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}