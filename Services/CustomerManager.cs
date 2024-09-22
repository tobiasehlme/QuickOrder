using System.Diagnostics;
using System.Text.Json;
using QuickOrder.Common.Entities;


namespace OrderGenius;

public static class CustomerManager
{
    private static string _appDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "bobbo");

    private static string _customerFile = Path.Combine(_appDir, "customers.json");

    public static List<Customer> Customers { get; set; } = new();

    public static event Action CustomersChanged;


    public static async Task<bool> AddCustomer(Customer customer)
    {
        var existingCustomer = Customers.FirstOrDefault(c => c.Id == customer.Id);
        if (existingCustomer != null && Customers.FirstOrDefault(c => c.Name == customer.Name) != null)
        {
            return false;
        }
        Customers.Add(customer);
        await SaveCustomersToFile();
        CustomersChanged?.Invoke();
        return true;
    }

    public static async Task<bool> RemoveCustomer(Customer customer)
    {
        var existingCustomer = Customers.FirstOrDefault(c => c.Id == customer.Id);
        if (existingCustomer == null)
        {
            return false;
        }
        Customers.Remove(existingCustomer);
        await SaveCustomersToFile();
        CustomersChanged?.Invoke();
        return true;
    }

    public static async Task SaveCustomersToFile()
    {
        var json = JsonSerializer.Serialize(Customers);
        await using (StreamWriter sw = new StreamWriter(_customerFile))
        {
            sw.Write(json);
        }
    }

    public static async Task LoadCustomersFromFile()
    {
        if (!Directory.Exists(_appDir))
        {
            Directory.CreateDirectory(_appDir);
        }
        if (!File.Exists(_customerFile))
        {
            File.Create(_customerFile);
            return;
        }
        string? json = await File.ReadAllTextAsync(_customerFile);

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        Customers = JsonSerializer.Deserialize<List<Customer>>(json);

        CustomersChanged?.Invoke();

    }

    

    public static string GetUsername()
    {
        string userName = Environment.UserName;
        var splitedUserName = userName.Split('.');
        string name = "";
        string _name = "";
        if (splitedUserName.Length == 1)
        {
            name = userName;
            _name = name;
            return _name;
        }
        for (int i = 0; i <= 1; i++)
        {
            bool first = true;
            foreach (var letter in splitedUserName[i])
            {
                if (first)
                {

                    first = false;
                    name += letter.ToString().ToUpper();

                }
                else
                {
                    name += letter;
                }
            }
            name += " ";
        }

        _name = name;
        return _name;
    }

    public static void OpenPdf()
    {
        using Process fileopener = new Process();
        var path = Path.Combine(_appDir, "editedPdf.pdf"); ;
        fileopener.StartInfo.FileName = "explorer";
        fileopener.StartInfo.Arguments = "\"" + path + "\"";
        fileopener.Start();
    }
}