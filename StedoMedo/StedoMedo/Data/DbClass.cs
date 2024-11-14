using StedoMedo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.Data
{
    public class DbClass
    {
        public List<Korisnik> Korisnici { get; set; }
        public List<Trosak> Troskovi { get; set; }
        public List<Budzet> Budzeti { get; set; }
        public DbClass() {
            Korisnici = new List<Korisnik>();
            Troskovi = new List<Trosak>();
            Budzeti = new List<Budzet>();
        }
        public void AddKorisnik(Korisnik korisnik) {
            Korisnici.Add(korisnik);
        }
        public void AddTrosak(Trosak trosak) {
            Troskovi.Add(trosak);
        }
        public void AddBudzet(Budzet budzet) {
            Budzeti.Add(budzet);
        }
        public int GetNextKorisnikId()
        {
            if (Korisnici.Count == 0)
                return 1;

            int maxId = Korisnici.Max(k => k.Id);
            return maxId + 1;
        }
    }
}
