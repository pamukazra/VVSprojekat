using StedoMedo.Data;
using StedoMedo.Models;
using StedoMedo.Services;
using StedoMedo.Services.ServisAutentifikacija;
using StedoMedo.Services.Statistika;
using StedoMedo.Services.UpravljanjeTroskovima;
using StedoMedo.UI;

DbClass db = new DbClass();


IServisAutentifikacija servisAutentifikacija = new ServisAutentifikacija(db);
KonzolaAutentifikacija konzolaAutentifikacija = new KonzolaAutentifikacija(servisAutentifikacija);

while (true)
{
    Korisnik korisnik = konzolaAutentifikacija.StartConsole();
    
    bool petlja = true;

    while (petlja)
    {
        Console.WriteLine();
        Console.WriteLine("Odaberite željenu kategoriju:");
        Console.WriteLine("1. Upravljanje troškovima");
        Console.WriteLine("2. Upravljanje budžetom");
        Console.WriteLine("3. Statistika troškova");
        Console.WriteLine("4. Procjena troškova");
        Console.WriteLine("5. Odjava");
        Console.WriteLine("6. Brisanje profila");
        string izbor = Console.ReadLine() ?? "";
        switch (izbor)
        {
            case "1":
                IServisUpravljanjeTroskovima servisUpravljanjeTroskovima = new ServisUpravljanjeTroskovima(db);
                KonzolaUpravljanjeTroskovima konzolaUpravljanjeTroskovima = new KonzolaUpravljanjeTroskovima(servisUpravljanjeTroskovima, korisnik);
                konzolaUpravljanjeTroskovima.PokreniInterakcijuKonzole();
                break;
            case "2":
                IBudzetService budzetService = new BudzetService(db);
                KonzolaBudzet konzolaBudzet = new KonzolaBudzet(budzetService, korisnik);
                konzolaBudzet.PokreniInterakcijuKonzole();
                break;
            case "3":
                IServisStatistika servisStatistika = new ServisStatistika(db);
                KonzolaStatistika konzolaStatistika = new KonzolaStatistika(servisStatistika, korisnik);
                konzolaStatistika.PokreniInterakcijuKonzole();
                break;
            case "4":
                try
                {
                    Console.Write("Unesite brojem za koji dan u odnosu na današnji vas interesuje procjena: ");
                    int dan = int.Parse(Console.ReadLine());
                    IServisPredvidjanjeTroskova servisPredvidjanjeTroskova = new ServisPredvidjanjeTroskova(db);
                    double procjena = servisPredvidjanjeTroskova.ProcijeniTroskove(korisnik, dan);
                    if (procjena > 0)
                        Console.WriteLine($"Procjena troška je: {procjena}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Greška!");
                }
                break;
            case "5":
                Console.WriteLine("Izlaz iz programa.");
                petlja = false;
                break;
            case "6":
                petlja = !konzolaAutentifikacija.BrisanjeKorisnika(korisnik);
                break;
            default:
                Console.WriteLine("Nevažeća opcija. Molimo pokušajte ponovo.");
                break;
        }
    }
}