using StedoMedo.Data;
using StedoMedo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StedoMedo.Services.UpravljanjeTroskovima
{
    public class ServisUpravljanjeTroskovima : IServisUpravljanjeTroskovima
    {
        public DbClass db;
        public ServisUpravljanjeTroskovima(DbClass db)
        {
            this.db = db;
        }

        public bool DodajTrosak(Korisnik korisnik, double iznos, KategorijaTroska kategorijaTroska = KategorijaTroska.Ostalo, string opis = "", DateTime? dateTime = null)
        {
            try
            {
                DateTime datumIVrijeme = DateTime.Now;
                if (dateTime != null) datumIVrijeme = dateTime.Value;
                int id = (db.Troskovi.Count == 0)? 0 : db.Troskovi.Last().Id + 1;
                db.AddTrosak(new Trosak(id, korisnik, datumIVrijeme, iznos, kategorijaTroska, opis));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska prilikom dodavanja troska!");
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
        public bool ObrisiTrosak(Korisnik korisnik, int idTroska)
        {
            try
            {
                var trosak = db.Troskovi.FirstOrDefault(t => t.Id == idTroska && t.Korisnik == korisnik);
                if (trosak == null) return false;

                db.Troskovi.Remove(trosak);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska prilikom brisanja troska!");
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
        public bool IzmijeniTrosak(Korisnik korisnik, int idTroska, double iznos, KategorijaTroska kategorijaTroska)
        {
            try
            {
                var trosak = db.Troskovi.FirstOrDefault(t => t.Id == idTroska && t.Korisnik == korisnik);
                if (trosak == null) return false;

                trosak.Iznos = iznos;
                trosak.KategorijaTroska = kategorijaTroska;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska prilikom izmjene troska!");
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
        public bool PrikaziTroskove(Korisnik korisnik, DateTime? odDatuma = null, DateTime? doDatuma = null,
            List<KategorijaTroska>? kategorijeTroskova = null, List<KriterijSortiranja>? kriterijiSortiranja = null)
        {
            try
            {
                var troskovi = db.Troskovi.Where(t => t.Korisnik == korisnik).ToList();
                troskovi = FiltrirajTroskove(troskovi, kategorijeTroskova, odDatuma, doDatuma);

                if (kriterijiSortiranja != null && kriterijiSortiranja.Any())
                    troskovi = SortirajTroskove(troskovi, kriterijiSortiranja);

                Console.WriteLine($"{"Id",15}|{"Iznos",10:F2}|{"Kategorija troska",25}|{"Datum",15}|Opis");
                for(int i = 0; i <70; i++) {
                    Console.Write("-");
                }
                Console.WriteLine();
                foreach (var trosak in troskovi) {
                    Console.WriteLine($"{trosak.Id,15}|{trosak.Iznos,10:F2}|{trosak.KategorijaTroska,25}|{trosak.DatumIVrijeme.ToShortDateString(),15}|{trosak.Opis}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska prilikom prikazivanja troskova!");
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        private List<Trosak> SortirajTroskove(List<Trosak> troskovi, List<KriterijSortiranja> kriterijiSortiranja)
        {
            troskovi = QuickSort(troskovi, kriterijiSortiranja, 0, troskovi.Count - 1);
            return troskovi;
        }

        private List<Trosak> QuickSort(List<Trosak> troskovi, List<KriterijSortiranja> kriterij, int low, int high)
        {
            if (low < high) {
                int pivotIndex = Partition(troskovi, kriterij, low, high);
                QuickSort(troskovi, kriterij, low, pivotIndex - 1);
                QuickSort(troskovi, kriterij, pivotIndex + 1, high);
            }
            return troskovi;
        }

        private int Partition(List<Trosak> troskovi, List<KriterijSortiranja> kriterij, int low, int high) {
            var pivot = troskovi[high];
            int i = low - 1;

            for (int j = low; j < high; j++) {
                bool condition = false;

                foreach(KriterijSortiranja kriterij1 in kriterij) {
                    int krit = kriterij1.KriterijPoredjenja(troskovi[j], pivot, kriterij1.SmjerSortiranja);
                    if (krit == 1) condition = true;
                    if (condition || krit == -1) break;
                }

                if (condition) {
                    i++;
                    (troskovi[i], troskovi[j]) = (troskovi[j], troskovi[i]);
                }
            }
            (troskovi[i + 1], troskovi[high]) = (troskovi[high], troskovi[i + 1]);
            return i + 1;
        }

        private List<Trosak> FiltrirajTroskove(List<Trosak> troskovi, List<KategorijaTroska>? kategorijeTroskova, DateTime? odDatuma, DateTime? doDatuma)
        {
            if (kategorijeTroskova == null || !kategorijeTroskova.Any())
                kategorijeTroskova = [
                    KategorijaTroska.Hrana,
                    KategorijaTroska.Rezije,
                    KategorijaTroska.Prijevoz,
                    KategorijaTroska.Izlasci,
                    KategorijaTroska.Odjeca,
                    KategorijaTroska.Ostalo
                ];
            if(odDatuma == null) odDatuma = DateTime.MinValue;
            if(doDatuma == null) doDatuma = DateTime.Now;
            List<Trosak> filtriraniTroskovi = [];
            foreach(var trosak in troskovi) {
                if (kategorijeTroskova.Contains(trosak.KategorijaTroska) && odDatuma <= trosak.DatumIVrijeme && doDatuma >= trosak.DatumIVrijeme)
                    filtriraniTroskovi.Add(trosak);
            }
            return filtriraniTroskovi;
        }
    }
}
