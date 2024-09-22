namespace QuickOrder.Common.Entities;

public class Customer
{
    public int Id { get; set; } // Kundnummer
    public string Name { get; set; } // Kundnamn
    public List<Store> Stores { get; set; } // Använts inte just nu
}