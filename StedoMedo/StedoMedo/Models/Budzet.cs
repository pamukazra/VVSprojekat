using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.Models
{
    public class Budzet
    {
        public int Id { get; set; }
        public Korisnik Korisnik { get; set; }
        public DateOnly PocetakPerioda { get; set; }
        public DateOnly KrajPerioda { get; set; }
        public double Iznos {  get; set; }
        public Budzet(int id, Korisnik korisnik, DateOnly pocetakPerioda, DateOnly krajPerioda, double iznos)
        {
            Id = id;
            Korisnik = korisnik;
            PocetakPerioda = pocetakPerioda;
            KrajPerioda = krajPerioda;
            Iznos = iznos;
        }
        public override string ToString()
        {
            return Id.ToString() + " " + Korisnik.Id.ToString() + " " + 
                PocetakPerioda.ToString() + " " + KrajPerioda.ToString() + Iznos.ToString();
        }
    }
}
