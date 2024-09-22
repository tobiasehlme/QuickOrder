using CommunityToolkit.Maui.Animations;
using OrderGenius;
using QuickOrder.Common.Entities;

namespace QuickOrder.MAUIApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            List<string> uppdrag = new() { "Accesspunkt", "Cradle", "Incheckning", "Kortläsare" };
            uppdragsPicker.ItemsSource = uppdrag;
           CustomerManager.CustomersChanged += CustomerManager_CustomersChanged;
        }

        private async void CustomerManager_CustomersChanged()
        {
        

            if (kundPicker is not null)
            {
                foreach (var customer in CustomerManager.Customers)
                {
                    kundPicker.Items.Add($"{customer.Id}|{customer.Name}");
                }
            }

            

        }

        private async void CreatePdfButton_OnClicked(object? sender, EventArgs e)
        {
            if (kundPicker.SelectedItem == null)
            {
                await DisplayAlert("Döh!", "Glömt att välja kund!", "OK");
                return;
            }
            if(string.IsNullOrEmpty(storeNameBox.Text))
            {
                await DisplayAlert("Döh!","Glömt att fylla i namnet!", "OK");
                return;
            }
            else if (string.IsNullOrEmpty(storeNumberBox.Text))
            {
                await DisplayAlert("Döh!", "Glömt att fylla i butiksnummer!", "OK");
                return;
            }
            else if(string.IsNullOrEmpty(storeAdressBox.Text))
            {
                await DisplayAlert("Döh!", "Glömt att fylla i adress!", "OK");
                return;

            }
            else if (string.IsNullOrEmpty(storePostBox.Text))
            {
                await DisplayAlert("Döh!", "Glömt att fylla i postnummer!", "OK");
                return;

            }
            else if (string.IsNullOrEmpty(storeCountryBox.Text))
            {
                await DisplayAlert("Döh!", "Glömt att fylla i land!", "OK");
                return;

            }
            else if (uppdragsPicker.SelectedItem == null)
            {
                if (!copyCheckBox.IsChecked)
                {
                    await DisplayAlert("Döh!", "Välj uppdrag!", "OK");
                    return;
                }
                
            }
            var username = CustomerManager.GetUsername();
            Store newStore = MethodMan(storePostBox.Text);

            if (copyCheckBox.IsChecked)
            {
                CopyPastePage copyPasteWindow = new(newStore, username);
                copyPasteWindow.OnCompleted += async (string text) =>
                {
                    await DisplayAlert("Lägg en swish till mig \ud83d\ude18", text, "OK");
                };
                await Navigation.PushModalAsync(copyPasteWindow);

                return;
            }
            else
            {
                var job = uppdragsPicker.SelectedItem.ToString();
                JobTemplatePage jobTemplatePage = new(job, newStore, username);
                jobTemplatePage.OnCompleted += async (string text) =>
                {
                    await DisplayAlert("Lägg en swish till mig \ud83d\ude18", text, "OK");
                };
                await Navigation.PushModalAsync(jobTemplatePage);
            }


        }

        private Store MethodMan(string postcodeBox)
        {
            string postcode = string.Empty;
            string province = string.Empty;
            string kundId = string.Empty;
            string kundNamn = string.Empty;

            foreach (var c in postcodeBox)
            {
                if (Char.IsDigit(c))
                {
                    postcode += c.ToString();
                }
                else if (c.ToString() != " ")
                {
                    province += c.ToString();
                }
            }

            foreach (var c in kundPicker.SelectedItem.ToString())
            {
                if (Char.IsDigit(c))
                {
                    kundId += c.ToString();
                }
                else if (c.ToString() != ' '.ToString() || c.ToString() != '|'.ToString()) // TODO : Behövs se över!
                {
                    kundNamn += c;
                }
            }
            if (string.IsNullOrEmpty(postcode))
            {
                postcode = " ";
            }
            var store = new Store() { Postcode = postcode, Province = province, Adress = storeAdressBox.Text, Country = storeCountryBox.Text, Name = storeNameBox.Text, Id = storeNumberBox.Text, Customer = new Customer() { Id = int.Parse(kundId), Name = kundNamn } };
            return store;
        }


    }
    class SampleScaleToAnimation : BaseAnimation
    {
        public double Scale { get; set; }

        public override Task Animate(VisualElement view, CancellationToken token = default)
        {
            return view.ScaleTo(Scale, Length, Easing);
        }
    }
    class ButtonAnimation : BaseAnimation
    {
        public override async Task Animate(VisualElement view, CancellationToken token)
        {
            await view.ScaleTo(1.2, Length, Easing).WaitAsync(token);
            await view.ScaleTo(1, Length, Easing).WaitAsync(token);
        }
    }

}
