using CommunityToolkit.Maui.Alerts;
using OrderGenius;
using QuickOrder.Common.Entities;
using QuickOrder.Common.Services;


namespace QuickOrder.MAUIApp
{
    public partial class App : Application
    {
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
            

            await Task.WhenAll(CheckRequiredFiles(), CustomerManager.LoadCustomersFromFile());
            base.OnStart();
        }

        private async Task CheckRequiredFiles()
        {
            string appDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "bobbo");
            string mainSrc = System.IO.Path.Combine(appDir, "templatePdf.pdf");
            string dest = System.IO.Path.Combine(appDir, "editedPdf.pdf");
            string company = System.IO.Path.Combine(appDir, "company.json");


            if (!Directory.Exists(appDir))
            {
                Directory.CreateDirectory(appDir);
            }
            if (!File.Exists(mainSrc) || !File.Exists(dest))
            {
                var toast = Toast.Make("Hittar inte pdf filerna i bobbo mappen!", CommunityToolkit.Maui.Core.ToastDuration.Short, 12);
                await toast.Show();
                base.Quit();
            }
            if (!File.Exists(company))
            {
                var toast = Toast.Make("Hittar inte company.json filen i bobbo mappen!", CommunityToolkit.Maui.Core.ToastDuration.Short, 12);
                await toast.Show();
                base.Quit();
            }
            string? json = await File.ReadAllTextAsync(company);
            if (string.IsNullOrEmpty(json))
            {
                var toast = Toast.Make("company.json filen är tom!", CommunityToolkit.Maui.Core.ToastDuration.Short, 12); // Fyll i company.json filen med rätt information!
                await toast.Show();
                base.Quit();
            }
        }
    }
    
}
