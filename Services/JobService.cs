using System.Text.Json;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using QuickOrder.Common.Entities;


namespace QuickOrder.Common.Services;

public class JobService
{
    
    public string User { get; set; }
    public Store Store { get; set; }
    public Customer Customer { get; set; }
    private Company _company;


    private string _quantityText = "en";
    private static string _appDir =
        System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "bobbo");
    private string _mainSrc = System.IO.Path.Combine(_appDir, "templatePdf.pdf");
    private string _dest = System.IO.Path.Combine(_appDir, "editedPdf.pdf");

    public async Task Cradle(string cradleNumber, int qty, string ticket)
    {
        // TODO: Behöver bestämma mig ifall jag ska checka butiksnamnet utanför klassen eller här inne
        var coop = string.Empty;
        if (!Store.Name.ToLower().Contains("coop"))
        {
            coop = "Coop";
        }

        if (qty > 1)
        {
            _quantityText = "flertalet";
        }
        else
        {
            _quantityText = "en";
        }


        var cradleMessage =
            $"{coop} {Store.Name}, {Store.Id}, har {_quantityText} cradles som behöver felsökas och eventuellt bytas ut \r" +
            $"Nummer: {cradleNumber} \r" +
            $"Totalt antal: {qty} \r" +
            $"Kontaktinfo till butik: {Store.Phone} \r" +
            $"Felanmälda enheter skall skickas till \r" +
            $"{_company.Name} \r" +
            $"{_company.Address} \r" +
            $"{_company.PostalCode} \r" +
            $"Vid frågor samt utfört arbete ring {_company.Name} driftsupport: {_company.SupportPhone} och ange ärendenummer: {ticket}  ";

        await InitializePDF(cradleMessage);
    }
    public async Task CheckIn(string ip, int qty, string ticket)
    {
        // TODO: Behöver bestämma mig ifall jag ska checka butiksnamnet utanför klassen eller här inne
        var coop = string.Empty;
        var inch = "incheckningsenhet";

        if (!Store.Name.ToLower().Contains("coop"))
        {
            coop = "Coop";
        }

        if (qty > 1)
        {
            _quantityText = "flertalet";
            inch = "incheckningsenheter";
        }
        else
        {
            _quantityText = "en";
        }
        var checkInDeviceMessage = $"{coop} {Store.Name}, har {_quantityText} {inch} som behöver felsökas och eventuellt bytas ut.\r" +
                            $"IP Nummer: {ip} \r" +
                            $"Totalt antal: {qty} \r" +
                            $"Kontaktinfo till butik: {Store.Phone} \r" +
            $"Felanmälda enheter skall skickas till \r" +
            $"{_company.Name} \r" +
            $"{_company.Address} \r" +
            $"{_company.PostalCode} \r" +
            $"Vid frågor samt utfört arbete ring {_company.Name} driftsupport: {_company.SupportPhone} och ange ärendenummer: {ticket}  ";

        await InitializePDF(checkInDeviceMessage);
    }
    public async Task Accesspoint(int qty, string ticket, string apNumber, string macAdress)
    {
        // TODO: Behöver bestämma mig ifall jag ska checka butiksnamnet utanför klassen eller här inne
        var ap = "accesspunkt";

        if (qty > 1)
        {
            _quantityText = "flertalet";
            ap = "accesspunkter";
        }
        else
        {
            _quantityText = "en";
        }
        var accesspointMessage = $"{Customer.Name} {Store.Name}, {Store.Id}, har {_quantityText} {ap} som behöver felsökas och eventuellt bytas ut. \r" +
                                 $"{apNumber}    \r" +
                                 $"MAC-adress: {macAdress} \r" +
                                 $"Totalt antal: {qty} \r" +
                                 $"Bifogar ritning över acesspunkterna för referens.\r" +
                                 $"Kontaktinfo till butik: {Store.Phone}\r" +
                                 $"En felsökning innefattar själva accesspunkten samt kontrollmätning av patchkabel+lankabel. Om en kabel byts ut måste anledning specificeras!\r" +
                                 $"Ni ansvarar för hyrning av lift om det behövs till felsökning/utbyte\r" +
                                 $"Kontaktinfo till butik: {Store.Phone} \r" +
            $"Felanmälda enheter skall skickas till \r" +
            $"{_company.Name} \r" +
            $"{_company.Address} \r" +
            $"{_company.PostalCode} \r" +
            $"Vid frågor samt utfört arbete ring {_company.Name} driftsupport: {_company.SupportPhone} och ange ärendenummer: {ticket}";


        await InitializePDF(accesspointMessage);


    }

    public async Task CardReader(int qty, string ticket)
    {
        var card = "kortläsare";
        if (qty > 1)
        {
            _quantityText = "flertalet";
            card = "accesspunkter";
        }
        else
        {
            _quantityText = "en";
        }
        var cardMessage = $"{Store.Name}, {Store.Id}, har {_quantityText} {card} som behöver felsökas och eventuellt bytas ut \r" +
                          $"Totalt antal: {qty} \r" +
                          $"Kontaktinfo till butik: {Store.Phone} \r" +
            $"Felanmälda enheter skall skickas till \r" +
            $"{_company.Name} \r" +
            $"{_company.Address} \r" +
            $"{_company.PostalCode} \r" +
            $"Vid frågor samt utfört arbete ring {_company.Name} driftsupport: {_company.SupportPhone} och ange ärendenummer: {ticket}  "; 
        await InitializePDF(cardMessage);
    }
    
    public async Task CopyPaste(string message)
    {
        await InitializePDF(message);
    }

    public async Task InitializePDF(string description)
    {
        Random randomBoy = new Random();
        int randomOrderNumber = randomBoy.Next(10000, 100000);
        PdfDocument pdfDoc = new PdfDocument(new PdfReader(_mainSrc), new PdfWriter(_dest));
        PdfCanvas canvas = new PdfCanvas(pdfDoc.GetPage(1));
        var customerNoWhiteBlock = new Rectangle(445, 739, 100, 10);
        var whiteBlockOrderNoToDate = new Rectangle(445, 729, 100, 33); // White block, from Order No to Order date
        var whiteBlockOurReference = new Rectangle(445, 627, 100, 10); // White block just on "Our reference"

        canvas.SaveState()
            .SetFillColor(DeviceRgb.WHITE)
            .Rectangle(whiteBlockOrderNoToDate)
            .Fill()
            .RestoreState();
        canvas.SaveState()
            .SetFillColor(DeviceRgb.WHITE)
            .Rectangle(whiteBlockOurReference)
            .Fill()
            .RestoreState();
        canvas.SaveState()
            .SetFillColor(DeviceRgb.WHITE)
            .Rectangle(customerNoWhiteBlock)
            .Fill()
            .RestoreState();
        canvas.BeginText() // Replacing Order No
            .SetFontAndSize(PdfFontFactory.CreateFont(), 9)
            .MoveText(446, 754)
            .ShowText($"O{randomOrderNumber}")
            .EndText();
        canvas.BeginText() // Replacing Customer No
            .SetFontAndSize(PdfFontFactory.CreateFont(), 9)
            .MoveText(445, 742)
            .ShowText(Customer.Id.ToString())
            .EndText();
        canvas.BeginText() // Replacing Date
            .SetFontAndSize(PdfFontFactory.CreateFont(), 9)
            .MoveText(445, 731)
            .ShowText(DateTime.Now.Date.ToShortDateString())
            .EndText();

        canvas.BeginText() // Replacing our reference
            .SetFontAndSize(PdfFontFactory.CreateFont(), 10)
            .MoveText(445, 629)
            .ShowText(User)
            .EndText();
        canvas.BeginText()
            .SetFontAndSize(PdfFontFactory.CreateFont(), 9)
            .MoveText(45, 650)
            .ShowText($"{Store.Name}")
            .MoveText(0, -12)
            .ShowText($"{Store.Adress}")
            .MoveText(0, -12)
            .ShowText($"{Store.Postcode} {Store.Province}")
            .MoveText(0, -12)
            .ShowText($"{Store.Country}");
        canvas.EndText();
        canvas.BeginText()
            .SetFontAndSize(PdfFontFactory.CreateFont(), 8)
            .MoveText(45, 400);
            string[] lines = description.Split("\r");
            foreach (var line in lines)
            {
                canvas.ShowText(line);
                canvas.MoveText(0, -12);
            }
        canvas.EndText();
        pdfDoc.Close();
    }

    public async Task GetCompany()
    {
        string companyJson = await File.ReadAllTextAsync(System.IO.Path.Combine(_appDir, "company.json"));
        var company = JsonSerializer.Deserialize<Company>(companyJson);
        _company = company;
    }


    public JobService(string user, Store store, Customer customer)
    {
        User = user;
        Store = store;
        Customer = customer;
    }
}