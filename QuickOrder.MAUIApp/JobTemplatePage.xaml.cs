using CommunityToolkit.Maui.Animations;
using OrderGenius;
using System;
using System.Diagnostics;
using QuickOrder.Common.Entities;
using QuickOrder.Common.Services;

namespace QuickOrder.MAUIApp;

public partial class JobTemplatePage : ContentPage
{
    public event Action<string> OnCompleted;
    private string _job = string.Empty;
    private Store _store = new Store();
    private string _user = string.Empty;
    public JobTemplatePage(string job, Store newStore, string user)
	{
        InitializeComponent();
        _job = job;
        _store = newStore;
        _user = user;
        Setup();
    }
    public void Setup()
    {
        if (_job == "Kortläsare")
        {
            info1.IsVisible = false;
            info2.IsVisible = false;
        }
        else if (_job == "Accesspunkt")
        {
            
            info1.Placeholder = "AP Nr?";
            info2.Placeholder = "MAC Adress?";
        }
        else if (_job == "Cradle")
        {
            info1.Placeholder = "Cradle Nr?";
            info2.IsVisible = false;
        }
        else if (_job == "Incheckning")
        {
            info1.Placeholder = "IP-adress";
            info2.IsVisible = false;
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        List<Entry> entries = new List<Entry> {quantity, ticket, telefonNr, info1, info2 };
        bool allFilled = entries
            .Where(entry=>entry.IsVisible)
            .All(entry => !string.IsNullOrEmpty(entry.Text));

        if (!allFilled)
        {
            await DisplayAlert("Döh!", "Glömt att fylla i ett fält!", "OK");
            return;
        }

        if (_job == "Kortläsare")
        {
            _store.Phone = telefonNr.Text;
            var newJob = new JobService(_user, _store, _store.Customer);
            await newJob.GetCompany();
            await newJob.CardReader(int.Parse(quantity.Text), ticket.Text);
           

        }
        else if (_job == "Accesspunkt")
        {
        
            _store.Phone = telefonNr.Text;

            var newJob = new JobService(_user, _store, _store.Customer);
            await newJob.GetCompany();
            await newJob.Accesspoint(int.Parse(quantity.Text), ticket.Text, info1.Text, info2.Text);
           

        }
        else if (_job == "Cradle")
        {
            _store.Phone = telefonNr.Text;
            var newJob = new JobService(_user, _store, _store.Customer);
            await newJob.GetCompany();
            await newJob.Cradle(info1.Text, int.Parse(quantity.Text), ticket.Text);
            
        }
        else if (_job == "Incheckning")
        {
            _store.Phone = telefonNr.Text;
            var newJob = new JobService(_user, _store, _store.Customer);
            await newJob.GetCompany();

            await newJob.CheckIn(info1.Text, int.Parse(quantity.Text), ticket.Text);
        
        }
        OnCompleted.Invoke("Allt är klart!");
        await Navigation.PopModalAsync();
        CustomerManager.OpenPdf();

    }
}