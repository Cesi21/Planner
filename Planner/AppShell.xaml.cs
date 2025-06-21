namespace Planner
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.RoutineListPage), typeof(Views.RoutineListPage));
            Routing.RegisterRoute(nameof(Views.GoalListPage), typeof(Views.GoalListPage));
        }
    }
}
