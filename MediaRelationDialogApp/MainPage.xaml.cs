using MediaRelationDialogApp.ViewModels;

namespace MediaRelationDialogApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.Title = "Fuse til rækkefølge";

            InitializeComponent();
            BindingContext = new MainPageViewModel(new Services.ApiService()); 
        }
    }
}
