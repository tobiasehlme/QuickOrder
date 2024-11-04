using Windows.Storage.Pickers;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;


namespace QuickOrder.MAUIApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void MenuItem_OnClicked(object? sender, EventArgs e)
        {
            var folderPicker = new FolderPicker();
            var hwnd = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                double fontSize = 20;

                var toast = Toast.Make("Mappen har sparats och kommer läsas in varje gång.", ToastDuration.Short, fontSize);
                await toast.Show(cts.Token);

                // Spara mappens path nånstans, appdata eller liknande.
            }
            else
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                double fontSize = 20;

                var toast = Toast.Make("Välj mapp tjomme!", ToastDuration.Short, fontSize);
                await toast.Show(cts.Token);
            }
        }
    }
}
