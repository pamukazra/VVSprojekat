using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StedoMedo.Data;
using StedoMedo.Models;
using StedoMedo.Services;
using BudzetModel = StedoMedo.Models.Budzet;


namespace StedoMedo.Services
{
    public class BudzetService: IBudzetService
    {
        private readonly DbClass _db;

        public BudzetService(DbClass db)
        {
            _db = db;
        }

        public double PreostaloStanjeBudzeta(Korisnik korisnik, DateOnly doDana)
        {
            if (korisnik == null)
            {
                throw new ArgumentNullException(nameof(korisnik), "Korisnik ne postoji.");
            }

            bool imaBudzet = false;
            foreach (var budzet in _db.Budzeti)
            {
                if (budzet.Korisnik.Id == korisnik.Id)
                {
                    imaBudzet = true;
                    break;
                }
            }

            if (!imaBudzet)
            {
                throw new InvalidOperationException("Za ovog korisnika ne postoji definisan budzet.");
            }

            /*bool imaTrosak = false;
            foreach (var trosak in _db.Troskovi)
            {
                if (trosak.Korisnik.Id == korisnik.Id)
                {
                    imaTrosak = true;
                    break;
                }
            }

            if (!imaTrosak)
            {
                Console.WriteLine("Za ovog korisnika ne postoje definisani troskovi.");
            }*/

            double ukupnoPotroseno = 0;
            DateTime doDanaDateTime = doDana.ToDateTime(new TimeOnly(0, 0));

            bool datumJeValidan = false;
            foreach (var budzet in _db.Budzeti)
            {
                if (budzet.Korisnik.Id == korisnik.Id && budzet.KrajPerioda >= doDana)
                {
                    datumJeValidan = true;
                    break;
                }
            }

            if (!datumJeValidan)
            {
                throw new ArgumentOutOfRangeException(nameof(doDana), "Datum za izračunavanje je van perioda definisanih budžeta.");
            }

            foreach (var trosak in _db.Troskovi)
            {
                if (trosak.Korisnik.Id == korisnik.Id && trosak.DatumIVrijeme.Date <= doDanaDateTime.Date)
                {
                    ukupnoPotroseno += trosak.Iznos;
                }
            }

            double ukupanBudzet = 0;
            foreach (var budzet in _db.Budzeti)
            {
                if (budzet.Korisnik.Id == korisnik.Id && budzet.KrajPerioda >= doDana)
                {
                    ukupanBudzet += budzet.Iznos;
                }
            }

            return ukupanBudzet - ukupnoPotroseno;
        }

        public Korisnik DodajBudzet(Korisnik korisnik, DateOnly pocetak, DateOnly kraj, double iznos) 
        {
            if (korisnik == null) throw new ArgumentNullException(nameof(korisnik), "Korisnik ne postoji.");
            if (iznos < 0) throw new ArgumentException(nameof(iznos),"Iznos budzeta mora biti nenegativan");
            var noviBudzet = new Budzet(_db.Budzeti.Count + 1, korisnik, pocetak, kraj, iznos);
            _db.AddBudzet(noviBudzet);
            return korisnik;
        }
        public bool ObrisiBudzet(Korisnik korisnik, int idBudzeta)
        {
            if (korisnik == null) throw new ArgumentNullException(nameof(korisnik),"Korisnik ne postoji.");
            var budzet = _db.Budzeti.FirstOrDefault(b => b.Id == idBudzeta && b.Korisnik.Id == korisnik.Id);
            if (budzet != null)
            {
                _db.Budzeti.Remove(budzet);
                return true;
            }
            return false;
        }
        public bool IzmjeniBudzet(Korisnik korisnik, int idBudzeta, DateOnly noviPocetak, DateOnly noviKraj, double noviIznos) 
        {
            if (korisnik == null) throw new ArgumentNullException(nameof(korisnik), "Korisnik ne postoji.");
            if (noviIznos < 0) throw new ArgumentException(nameof(noviIznos),"Iznos budzeta mora biti nenegativan.");
            var budzet = _db.Budzeti.FirstOrDefault(b => b.Id == idBudzeta && b.Korisnik.Id == korisnik.Id);
            if (budzet != null)
            {
                budzet.Id = idBudzeta;
                budzet.PocetakPerioda = noviPocetak;
                budzet.KrajPerioda = noviKraj;
                budzet.Iznos = noviIznos;
                _db.AddBudzet(budzet);
                return true;
            }
            return false;
        }
        public bool ProvjeraPostojanja(Korisnik korisnik, int idBudzeta)
        {
            return _db.Budzeti.Any(b => b.Id == idBudzeta && b.Korisnik.Id == korisnik.Id);
        }

    }
}
