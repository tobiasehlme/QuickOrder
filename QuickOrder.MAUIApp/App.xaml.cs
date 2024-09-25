using CommunityToolkit.Maui.Alerts;
using OrderGenius;
using QuickOrder.Common.Entities;
using QuickOrder.Common.Services;


namespace QuickOrder.MAUIApp
{
    public partial class App : Application
    {
        private bool _isCompanyExist;
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {

            var window = base.CreateWindow(activationState);
            window.MaximumWidth = 600;
            window.MinimumWidth = 600;
            window.MaximumHeight = 680;
            window.MinimumHeight = 680;
            return window;  
        }

        protected override async void OnStart()
        {


            await CustomerManager.LoadCustomersFromFile();
            _isCompanyExist = await CustomerManager.LoadCompanyFromFile();
            await CheckRequiredFiles();
            base.OnStart();
        }

        private async Task CheckRequiredFiles()
        {
            string appDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "bobbo");
            string mainSrc = System.IO.Path.Combine(appDir, "templatePdf.pdf");
            string dest = System.IO.Path.Combine(appDir, "editedPdf.pdf");


            if (!Directory.Exists(appDir))
            {
                Directory.CreateDirectory(appDir);
            }
            if (!File.Exists(mainSrc) || !File.Exists(dest))
            {
                var toast = Toast.Make("Hittar inte pdf filerna i bobbo mappen! Skapar nya...", CommunityToolkit.Maui.Core.ToastDuration.Long, 18);
                await toast.Show();
                await File.WriteAllTextAsync(mainSrc, "");
                await File.WriteAllTextAsync(dest, "");
                Application.Current.Quit();
            }

            if (!_isCompanyExist)
            {
                var toast = Toast.Make("company.json filen är tom!", CommunityToolkit.Maui.Core.ToastDuration.Short, 18); // Fyll i company.json filen med rätt information!
                await toast.Show();
                Application.Current.Quit();
            }
        }

    }

}
