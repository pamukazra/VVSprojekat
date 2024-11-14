using StedoMedo.Data;
using StedoMedo.Models;
using StedoMedo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.UI
{
    public class KonzolaBudzet
    {
        IBudzetService budzetService;
        Korisnik korisnik;
        public KonzolaBudzet(IBudzetService _budzetService, Korisnik _korisnik) 
        {
            budzetService = _budzetService;
            korisnik = _korisnik;
        }
        public void PokreniInterakcijuKonzole()
        {
            while (true)
            {
                Console.WriteLine("\n--- Upravljanje Budžetom ---");
                Console.WriteLine("1. Dodaj budžet");
                Console.WriteLine("2. Obriši budžet");
                Console.WriteLine("3. Izmijeni budžet");
                Console.WriteLine("4. Prikaži preostalo stanje budžeta");
                Console.WriteLine("5. Pocetni meni");
                Console.Write("Izaberite opciju: ");

                string izbor = Console.ReadLine() ?? "";
                switch (izbor)
                {
                    case "1":
                        DodajBudzet(budzetService, korisnik);
                        break;
                    case "2":
                        ObrisiBudzet(budzetService, korisnik);
                        break;
                    case "3":
                        IzmjeniBudzet(budzetService, korisnik);
                        break;
                    case "4":
                        PrikaziPreostaloStanje(budzetService, korisnik);
                        break;
                    case "5":
                        Console.WriteLine("Povratak na početni meni.");
                        return;
                    default:
                        Console.WriteLine("Nevažeća opcija. Molimo pokušajte ponovo.");
                        break;
                }
            }
        }

        public static void DodajBudzet(IBudzetService budzetService, Korisnik korisnik)
        {
            Console.WriteLine("Dodavanje novog budžeta:");
            Console.Write("Unesite početak budžeta (YYYY-MM-DD): ");
            var pocetak = DateOnly.Parse(Console.ReadLine());
            Console.Write("Unesite kraj budžeta (YYYY-MM-DD): ");
            var kraj = DateOnly.Parse(Console.ReadLine());
            Console.Write("Unesite iznos budžeta: ");
            var iznos = double.Parse(Console.ReadLine());

            budzetService.DodajBudzet(korisnik, pocetak, kraj, iznos);
            Console.WriteLine("Budžet je uspješno dodan.");
        }

        public static void ObrisiBudzet(IBudzetService budzetService, Korisnik korisnik)
        {
            Console.WriteLine("Brisanje budžeta:");
            Console.Write("Unesite ID budžeta za brisanje: ");
            int idBudzeta = int.Parse(Console.ReadLine());

            bool obrisan = budzetService.ObrisiBudzet(korisnik, idBudzeta);
            Console.WriteLine(obrisan ? "Budžet je uspješno obrisan." : "Budžet nije pronađen.");
        }


        public static void IzmjeniBudzet(IBudzetService budzetService, Korisnik korisnik)
        {
            Console.WriteLine("Izmjena budžeta:");

            try
            {
                Console.Write("Unesite ID budžeta za izmjenu: ");
                int idBudzeta = int.Parse(Console.ReadLine());

                var budzetPostoji = budzetService.ProvjeraPostojanja(korisnik, idBudzeta);
                if (!budzetPostoji)
                {
                    Console.WriteLine($"Budžet sa ID-jem {idBudzeta} za navedenog korisnika ne postoji.");
                    return;
                }

                Console.Write("Unesite novi početak budžeta (YYYY-MM-DD): ");
                var noviPocetak = DateOnly.Parse(Console.ReadLine());
                Console.Write("Unesite novi kraj budžeta (YYYY-MM-DD): ");
                var noviKraj = DateOnly.Parse(Console.ReadLine());
                Console.Write("Unesite novi iznos budžeta: ");
                var noviIznos = double.Parse(Console.ReadLine());

                bool izmijenjen = budzetService.IzmjeniBudzet(korisnik, idBudzeta, noviPocetak, noviKraj, noviIznos);
                Console.WriteLine(izmijenjen ? "Budžet je uspješno izmijenjen." : "Budžet nije pronađen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }
        }


        public static void PrikaziPreostaloStanje(IBudzetService budzetService, Korisnik korisnik)
        {
            Console.WriteLine("Prikaz preostalog stanja budžeta:");
            Console.Write("Unesite datum do kojeg želite izračunati stanje (YYYY-MM-DD): ");
            var doDana = DateOnly.Parse(Console.ReadLine());

            try
            {
                double preostaloStanje = budzetService.PreostaloStanjeBudzeta(korisnik, doDana);
                Console.WriteLine($"Preostalo stanje budžeta je: {preostaloStanje}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }
        }
    }
}
