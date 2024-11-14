using StedoMedo.Models;
using StedoMedo.Services.UpravljanjeTroskovima;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.UI
{
    public class KonzolaUpravljanjeTroskovima
    {
        private readonly IServisUpravljanjeTroskovima _servis;
        private readonly Korisnik Korisnik;
        public KonzolaUpravljanjeTroskovima(IServisUpravljanjeTroskovima servis, Korisnik korisnik)
        {
            _servis = servis;
            Korisnik = korisnik;
        }

        public void PokreniInterakcijuKonzole()
        {
            while (true)
            {
                Console.WriteLine("\n--- Upravljanje Troškovima ---");
                Console.WriteLine("1. Dodaj trošak");
                Console.WriteLine("2. Prikaži troškove");
                Console.WriteLine("3. Izmijeni trošak");
                Console.WriteLine("4. Obriši trošak");
                Console.WriteLine("5. Pocetni meni");
                Console.Write("Izaberite opciju: ");

                string izbor = Console.ReadLine() ?? "";
                switch (izbor)
                {
                    case "1":
                        DodajTrosakKonzola();
                        break;
                    case "2":
                        PrikaziTroskoveKonzola();
                        break;
                    case "3":
                        IzmijeniTrosakKonzola();
                        break;
                    case "4":
                        ObrisiTrosakKonzola();
                        break;
                    case "5":
                        Console.WriteLine("Povratak na pocetni meni.");
                        return;
                    default:
                        Console.WriteLine("Nevažeća opcija. Molimo pokušajte ponovo.");
                        break;
                }
            }
        }

        private void DodajTrosakKonzola()
        {
            try
            {
                Console.Write("Unesite iznos troška: ");
                double iznos = double.Parse(Console.ReadLine() ?? "0");

                Console.Write("Izaberite kategoriju troška (Hrana, Rezije, Prijevoz, Izlasci, Odjeca, Ostalo): ");
                KategorijaTroska kategorija = Enum.Parse<KategorijaTroska>(Console.ReadLine() ?? "Ostalo");

                Console.Write("Unesite opis troška: ");
                string opis = Console.ReadLine() ?? "";

                Console.Write("Unesite datum troška (dd/MM/yyyy) ili pritisnite Enter za današnji datum: ");
                DateTime datum = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate) ? parsedDate : DateTime.Now;

                bool uspjelo = _servis.DodajTrosak(Korisnik, iznos, kategorija, opis, datum);
                Console.WriteLine(uspjelo ? "Trošak uspješno dodan." : "Neuspješno dodavanje troška.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }
        }

        private void PrikaziTroskoveKonzola()
        {
            Console.Write("Unesite početni datum (dd/MM/yyyy) ili pritisnite Enter za početak vremenskog opsega: ");
            DateTime? odDatuma = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedOdDatum) ? parsedOdDatum : (DateTime?)null;

            Console.Write("Unesite krajnji datum (dd/MM/yyyy) ili pritisnite Enter za današnji datum: ");
            DateTime? doDatuma = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDoDatum) ? parsedDoDatum : DateTime.Now;


            Console.WriteLine("Unesite kategorije troškova (odvojene zarezima, npr. Hrana, Izlasci). Pritisnite Enter za sve kategorije:");
            string kategorijeInput = Console.ReadLine();
            List<KategorijaTroska> kategorijeTroskova = new List<KategorijaTroska>();

            if (!string.IsNullOrWhiteSpace(kategorijeInput))
            {
                var kategorije = kategorijeInput.Split(',').Select(k => k.Trim()).ToList();
                foreach (var kategorija in kategorije)
                {
                    if (Enum.TryParse(kategorija, true, out KategorijaTroska parsedKategorija))
                    {
                        kategorijeTroskova.Add(parsedKategorija);
                    }
                    else
                    {
                        Console.WriteLine($"Nepoznata kategorija: {kategorija}. Ignoriše se.");
                    }
                }
            }
            else
            {
                // Ako je korisnik pritisnuo Enter, uzimaju se sve kategorije
                kategorijeTroskova = Enum.GetValues(typeof(KategorijaTroska)).Cast<KategorijaTroska>().ToList();
            }




            List<Func<Trosak, Trosak, SmjerSortiranja, int>> kriteriji = new List<Func<Trosak, Trosak, SmjerSortiranja, int>>()
            {
                MetodeSortiranja.SortirajPoId,
                MetodeSortiranja.SortirajPoDatumu,
                MetodeSortiranja.SortirajPoIznosu,
                MetodeSortiranja.SortirajPoKategoriji
            };

            List<string> kriterijImena = new List<string>()
            {
                "Id",
                "Datum",
                "Iznos",
                "Kategorija"
            };


            Console.WriteLine("Unesite kriterije za sortiranje (u redoslijedu prioriteta). Redoslijed je bitan!");
            Console.WriteLine("Opcije: Id, Datum, Iznos, Kategorija");
            Console.WriteLine("Primjer unosa: 'Id, Datum, Iznos'");

            string unos = Console.ReadLine();
            var kriterijiUnos = unos.Split(',').Select(k => k.Trim()).ToList();

            List<KriterijSortiranja> kriterijSortiranja = [];

            // Iteracija kroz korisnički unos i postavljanje sortiranja
            foreach (var kriterij in kriterijiUnos)
            {
                if (kriterijImena.Contains(kriterij))
                {
                    int index = kriterijImena.IndexOf(kriterij);
                    var smjerSortiranja = SmjerSortiranja.Rastuci;
                    
                    Console.WriteLine($"Želite li sortirati po {kriterij} rastuće (R) ili opadajuće (O)?");
                    string smjer = Console.ReadLine().Trim().ToUpper();

                    if (smjer == "O")
                    {
                        smjerSortiranja = SmjerSortiranja.Opadajuci;
                    }
                    else if (smjer != "R")
                    {
                        Console.WriteLine("Pogrešan unos, koristi se rastući poredak.");
                    }
                    kriterijSortiranja.Add(new KriterijSortiranja(kriteriji[index], smjerSortiranja));
                }
                else if(kriterij.Any())
                {
                    Console.WriteLine($"Kriterij '{kriterij}' nije prepoznat. Preskače se.");
                }
            }


            bool prikazano = _servis.PrikaziTroskove(Korisnik, odDatuma, doDatuma, kategorijeTroskova, kriterijSortiranja);
            if (!prikazano) Console.WriteLine("Prikaz troškova nije uspio.");
        }

        private void IzmijeniTrosakKonzola()
        {
            Console.Write("Unesite ID troška za izmjenu: ");
            int idTroska = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Unesite novi iznos: ");
            double iznos = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("Izaberite novu kategoriju (Hrana, Rezije, Prijevoz, Izlasci, Odjeca, Ostalo): ");
            KategorijaTroska kategorija = Enum.Parse<KategorijaTroska>(Console.ReadLine() ?? "Ostalo");

            bool izmijenjeno = _servis.IzmijeniTrosak(Korisnik, idTroska, iznos, kategorija);
            Console.WriteLine(izmijenjeno ? "Trošak uspješno izmijenjen." : "Neuspješno izmijenjen trošak.");
        }

        private void ObrisiTrosakKonzola()
        {
            Console.Write("Unesite ID troška za brisanje: ");
            int idTroska = int.Parse(Console.ReadLine() ?? "0");

            bool obrisano = _servis.ObrisiTrosak(Korisnik, idTroska);
            Console.WriteLine(obrisano ? "Trošak uspješno obrisan." : "Neuspješno brisanje troška.");
        }

    }
}
