namespace QuickOrder.Common.Entities;

public class Store
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Adress { get; set; }
    public int Postcode { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    public Customer Customer { get; set; }
}