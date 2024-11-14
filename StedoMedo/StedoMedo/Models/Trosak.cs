using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.Models
{
    public class Trosak
    {
        public int Id { get; set; }
        public Korisnik Korisnik { get; set; }
        public DateTime DatumIVrijeme { get; set; }
        public double Iznos { get; set; }
        public KategorijaTroska KategorijaTroska { get; set; }
        public string Opis {  get; set; }
        public Trosak(int id, Korisnik korisnik, DateTime datumIVrijeme, double iznos, KategorijaTroska kategorijaTroska, string opis)
        {
            Id = id;
            Korisnik = korisnik;
            DatumIVrijeme = datumIVrijeme;
            Iznos = iznos;
            KategorijaTroska = kategorijaTroska;
            Opis = opis;
        }
        public override string ToString()
        {
            return Id.ToString() + " " + Korisnik.Id.ToString() + " " + DatumIVrijeme.ToString() + " " + Iznos.ToString() + " " + KategorijaTroska.ToString() + " " + Opis.ToString();
        }
    }
}
