using CommunityToolkit.Maui.Alerts;
using Microsoft.UI.Xaml;
using OrderGenius;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuickOrder.MAUIApp.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            
            CheckRequiredFiles();
        }

        public async Task CheckRequiredFiles()
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
            }
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

}
