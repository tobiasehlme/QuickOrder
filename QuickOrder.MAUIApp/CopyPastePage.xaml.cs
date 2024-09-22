using OrderGenius;
using QuickOrder.Common.Entities;
using QuickOrder.Common.Services;

namespace QuickOrder.MAUIApp;

public partial class CopyPastePage : ContentPage
{
    public event Action<string> OnCompleted;
    private Store _selectedStore = new Store();
    private string _user = string.Empty;
    public CopyPastePage(Store newStore, string user)
    {
        _selectedStore = newStore;
        _user = user;
        InitializeComponent();
    }


    private async void KBKButton_OnClicked(object? sender, EventArgs e)
    {
        var newJob = new JobService(_user, _selectedStore, _selectedStore.Customer);
        newJob.CopyPaste(copyPasteTextBox.Text);

        OnCompleted.Invoke("Allt är klart!");
        await Navigation.PopModalAsync();
        CustomerManager.OpenPdf();
    }
}