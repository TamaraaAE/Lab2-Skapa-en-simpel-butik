

using System;
using System.Net.Http.Headers; // Importerar System-namnrymden för att använda Console
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Xml.Linq;

class Program
{

    // Lista för registrerade kunder
    static List<Customer> registreraKunder = new List<Customer>();

    // Variabel för att hålla den inloggade kunden
    static Customer inloggadKund = null;


    // Variabler för att spara registrerade kunders namn och lösenord
    static string sparatNamn = "";
    static string sparatLosenord = "";
    static List<Product> kundvagn = new List<Product>(); // Kundvagn för att lagra valda produkter

    // Förbestämda kunder för att logga in ( dessa är två arrayer som innehåller namn och lösenord
    static string[] fordefinieradeKunder = { "Knatte", "Fnatte", "Tjatte" }; // Lista med namn
    static string[] fordefinieradeLosenord = { "123", "456", "789" };

    


    // Huvudmetod där programmet startar
    static void Main(string[] args)
    {
        Butik(); // Startar butikmenyn
    }

    public static void Butik()
    {
        // Huvudmeny där använadren får välja att registrera sig eller logga in
        Console.WriteLine("Välkommen till butiken!");
        Console.WriteLine("1. Registrera ny kund");
        Console.WriteLine("2. Logga in");
        Console.WriteLine("3. Avsluta");



        Console.WriteLine("Välj ett alternativ (1, 2 eller 3):");

        // Läser användarens val
        string val = Console.ReadLine();

        // Switch för att hantera användarens val
        switch (val)
        {
            case "1":
                RegistreraKund(); //Anropa metoden för att reg en ny kund
                break;
            case "2":
                LoggaIn(); // Anropar inloggningsmetoden
                break;
            case "3":
                System.Environment.Exit(0); // Avslutar programmet
                break;
            default:
                Console.WriteLine("Ogiltligt val. Försök igen");
                break;
        }

    }




    // Funktion för att registrera en ny kund
    static void RegistreraKund()
    {
        Console.WriteLine("Ange ditt namn:"); // Ber använadren om namn
        sparatNamn = Console.ReadLine(); // Sparar namnet i variabeln

        Console.WriteLine("Ange ditt lösenord:"); // Ber användaren om lösenord
        sparatLosenord = Console.ReadLine(); // Sparar lösenordet i variablen

        registreraKunder.Add(new Customer(sparatNamn, sparatLosenord));
        

        Console.WriteLine($"Kund {sparatNamn} har registrerats!"); // Bekräftar registrering


        ButikMeny(); // Går vidare till butiksmenyn
    }

    static bool LoggaIn()
    {
        Console.WriteLine("Ange ditt namn");
        string namn = Console.ReadLine(); // Läser in namnet

        Console.WriteLine("Ange ditt lösenord:");
        string losenord = Console.ReadLine();

        // Kontrollerar om användaren finns i listan av registrerade kunder
        foreach (var kund in registreraKunder)
        {
            if (kund.Name == namn && kund.Password == losenord)
            {
                Console.WriteLine($"Inloggning lyckades! Välkommen, {namn}!");

                // Sätt inloggadKund till den hittade kunden
                inloggadKund = kund;

                // Om inloggning lyckas, visa butiksmenyn
                ButikMeny();
                return true; // Inloggningen lyckades
            }
        }

        // Om ingen matchning hittades
        Console.WriteLine("Inloggning misslyckades. Vill du registrera dig? (j/n)");
        string svar = Console.ReadLine();

        if (svar.ToLower() == "j")
        {
            RegistreraKund();
        }
        return false; // Inloggningen misslyckades
    }







    // En klass som representerar en produkt
    public class Product
    {
        public string Namn { get; set; }
        public int Pris { get; set; }
        public int Antal { get; set; }


        // Omdefinierar ToString för att visa produktinfo
        public override string ToString()
        {
            return ($"{Namn}, {Pris} kr , {Antal} st");
        }

    }

    // Klass som innehåller en lista med produkter
    public class Lista
    {
        // Fördefinierad lista med produkter
        public List<Product> Produkter = new List<Product>
        {
            new Product{ Namn = "Kött", Pris = 100 },
            new Product{ Namn = "Tomat", Pris = 20 },
            new Product{ Namn = "Bröd", Pris = 12 },
            new Product {Namn = "Cola", Pris = 22}
        };
    }



    // Metod för att hantera shopping
    static void Handla(List<Product> Produkter)
    {
        // Lägg till en loop för att kunna fortsätta handla tills användaren vill gå tillbaka till menyn
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Välj en produkt att lägga till i din kundvagn:");

            // Visar alla produkter
            for (int i = 0; i < Produkter.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Produkter[i].Namn} - {Produkter[i].Pris} kr");
            }

            Console.Write("Skriv numret på produkten du vill köpa: ");
            int produktVal = int.Parse(Console.ReadLine()) - 1;

            // Kolla om produkten redan finns i kundvagnen
            var produktIKundvagn = Produkter[produktVal];
            if (produktIKundvagn != null)
            {
                produktIKundvagn.Antal++; // Öka antal om den redan finns
            }
            else
            {
                // Om produkten inte finns, lägg till den med antal 1
                Console.WriteLine("Ogiltligt val.");

            }
            Console.WriteLine("Vill du forstätta handla? (j/n)");
            string fortsattaHandla = Console.ReadLine().ToLower();
            if (fortsattaHandla != "j")
            {
                break; // Avsluta loopen
            }

            /*Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
            Console.ReadKey(); // Väntar på att användaren trycker på en tangent innan den går tillbaka till menyn.*/
        }
        ButikMeny(); // Går tillbaka till butiksmenyn
    }

    public class Kundvagn { public List<Product> Varor { get; set; } public Kundvagn() { Varor = new List<Product>(); } }

    // Metod för att visa kundvagnen
    public void VisaKundVagn()
    {
        if (kundvagn.Count == 0)
        {
            Console.WriteLine("Kundvagnen är tom.");
        }
        else
        {
            Console.WriteLine("Din kundvagn innehåller:");
            int totalPris = 0;
            foreach (var product in kundvagn)
            {
                Console.WriteLine($"{product.Namn} - {product.Pris} kr");
                totalPris += product.Pris; // Räknar ihop totalpris
            }
            Console.WriteLine($"Totalt pris: {totalPris} kr");

        }
    }

    //  Metod för att gå till kassan
    public void GoTillKassan()
    {
        Console.Clear();
        Console.WriteLine("Du har gått till kassan.");
        VisaKundVagn(); // Visar innehållet i kundvagnen

        if (kundvagn.Count > 0)
        {
            Console.WriteLine("\nBekräftar betalning...");
            Console.WriteLine("Betalning genomförd. Kundvagnen är nu tom.");
            kundvagn.Clear(); // Tömmer kundvagnen efter betalning

        }
        else
        {
            Console.WriteLine("Din kundvagn är tom. Ingen betalning genomfördes.");
        }

        // Väntar en kort stund innan programmet avslutas
        Console.WriteLine("\nProgrammet avslutas... Tack för att du handlade hos oss!");

        System.Threading.Thread.Sleep(3000); // Vänatr 3 sekunder för att användaren ska hinna se meddelandet

        Environment.Exit(0); // Avslutar programmet





    }
    // Klass som representerar en kund
    public class Customer
    {
        public string Name { get; private set; } // Namn som är privat men går att hämta

        public string Password { get; set; } // Lösenord är privat

        public Kundvagn Kundvagn { get; set; }



        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            Kundvagn = new Kundvagn(); // Varje kund har en egen kundvagn

        }
    }
    // Butiksmeny efter inloggning
    static void ButikMeny()
    {
        while (true)
        {
            Console.Clear(); // Rensar skärmen innan menyn visas
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("1. Handla");
            Console.WriteLine("2. Se kundvagn");
            Console.WriteLine("3. Gå till kassan");
            Console.WriteLine("4. Logga ut");


            string val = Console.ReadLine();

            switch (val)
            {
                case "1":
                    Console.Clear();
                    Lista produktLista = new Lista(); // Skapa en instans av Lista-klassen för att få produkterna
                    Handla(produktLista.Produkter); // Skicka produkterna som argument till Handla-metoden
                    break;
                case "2":
                    Console.Clear();
                    Program butik = new Program();
                    butik.VisaKundVagn(); // Anropar metoden via instansen
                    break;
                case "3":
                    Console.Clear();
                    Program butikKassa = new Program();
                    butikKassa.GoTillKassan();
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Du loggades ut.");
                    Environment.Exit(0); // Avslutar programmet.
                    return; // Gå ur ButikMeny loopen och tbx till huvudmeny
                default:
                    Console.WriteLine("Ogiltligt val. Försök igen");
                    break;

            }
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey(); // Väntar på att användaren trycker på valfri tangent innan skärmen rensas igen.

        }

    }
}




