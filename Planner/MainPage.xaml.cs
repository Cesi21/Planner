namespace Planner
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnRoutines(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Views.RoutineListPage));
        }

        private async void OnGoals(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Views.GoalListPage));
        }
    }
}
