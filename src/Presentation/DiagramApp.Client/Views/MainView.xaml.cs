using DiagramApp.Client.ViewModels;

namespace DiagramApp.Client
{
    public partial class MainView : ContentPage
    {
        private Point? mousePos = null;

        public MainView(MainViewModel viewmodel)
        {
            InitializeComponent();

            BindingContext = viewmodel;
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            App.Current!.Quit();
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            mousePos = e.GetPosition(null);
        }

        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            mousePos = null;
        }
    }
}
