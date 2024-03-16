namespace DiagramApp.Client
{
    public partial class MainPage : ContentPage
    {
        private int _fileCounter = 0;
        private readonly Dictionary<string, ContentView> _tabs = new();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnNewFileClicked(object sender, EventArgs e)
        {
            string tabName = $"Файл {++_fileCounter}";

            var newTab = new ContentView
            {
                Content = new AbsoluteLayout()
                {
                    Children =
                    {
                        new Label { Text = $"Это {tabName}" }
                    }
                }
            };

            _tabs[tabName] = newTab;

            var newTabButton = new Button
            {
                Text = tabName,
                Style = Application.Current!.Resources.MergedDictionaries.FirstOrDefault(c => c.TryGetValue("Tab", out var _))!["Tab"] as Style
            };
            newTabButton.Clicked += (s, e) => SwitchTab(tabName);
            TabHeaders.Children.Add(newTabButton);

            SwitchTab(tabName);
        }

        private void SwitchTab(string tabName)
        {
            if (_tabs.TryGetValue(tabName, out var tab))
            {
                MainContent.Content = tab;
            }
        }
    }

}
