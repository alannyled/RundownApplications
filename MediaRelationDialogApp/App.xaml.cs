
namespace MediaRelationDialogApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current.UserAppTheme = AppTheme.Dark;
        }
        protected override Window CreateWindow(IActivationState activationState) =>
            new(new AppShell())
            {
                Width = 1000,
                Height = 800,
                X = 200,
                Y = 100
            };
    }
}
