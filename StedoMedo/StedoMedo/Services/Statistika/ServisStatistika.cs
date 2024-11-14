using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StedoMedo.Data;
using StedoMedo.Models;

namespace StedoMedo.Services.Statistika
{
    public class ServisStatistika : IServisStatistika
    {
        public DbClass db;
        public ServisStatistika(DbClass db)
        {
            this.db = db;
        }


        public double NajveciTrosak(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma, List<KategorijaTroska> poKategorijama)
        {
            double najveciTrosak = 0;
            try
            {
                foreach (Trosak trosak in db.Troskovi)
                {
                    if (trosak != null && trosak.Korisnik == korisnik && trosak.DatumIVrijeme >= odDatuma && trosak.DatumIVrijeme <= doDatuma && poKategorijama.Contains(trosak.KategorijaTroska))
                    {
                        if (trosak.Iznos > najveciTrosak)
                        {
                            najveciTrosak = trosak.Iznos;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return najveciTrosak;
        }
        public double ProsjecnaPotrosnja(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma, List<KategorijaTroska> poKategorijama)
        {
            double ukupniTrosak = 0;
            double brojTroskova = 0;
            foreach (Trosak trosak in db.Troskovi)
            {
                if (trosak != null && trosak.Korisnik == korisnik && trosak.DatumIVrijeme >= odDatuma && trosak.DatumIVrijeme <= doDatuma && poKategorijama.Contains(trosak.KategorijaTroska))
                {
                    ukupniTrosak += trosak.Iznos;
                    brojTroskova++;
                }
            }
            return ukupniTrosak / brojTroskova;
        }

        public Dictionary<KategorijaTroska, double> RaspodjelaPoKategorijama(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma)
        {
            Dictionary<KategorijaTroska, double> mapaTroskova = new Dictionary<KategorijaTroska, double>();
            foreach (Trosak trosak in db.Troskovi)
            {

                if (trosak != null && trosak.Korisnik == korisnik && trosak.DatumIVrijeme >= odDatuma && trosak.DatumIVrijeme <= doDatuma)
                {
                    try
                    {
                        if (mapaTroskova.ContainsKey(trosak.KategorijaTroska))
                        {
                            mapaTroskova[trosak.KategorijaTroska] += trosak.Iznos;
                        }
                        else
                        {
                            mapaTroskova[trosak.KategorijaTroska] = trosak.Iznos;
                        }
                    }
                    catch (KeyNotFoundException ex)
                    {
                        Console.WriteLine("Greska prilikom pronalaska ključa");
                        Console.WriteLine(ex.ToString());
                    }

                }

            }
            return mapaTroskova;
        }

        public double UkupniTrosak(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma, List<KategorijaTroska> poKategorijama)
        {
            double ukupniTrosak = 0;
            foreach (Trosak trosak in db.Troskovi)
            {
                if (trosak != null && trosak.Korisnik == korisnik && trosak.DatumIVrijeme >= odDatuma && trosak.DatumIVrijeme <= doDatuma && poKategorijama.Contains(trosak.KategorijaTroska))
                {
                    ukupniTrosak += trosak.Iznos;
                }
            }
            return ukupniTrosak;
        }



        public double VarijansaTroskova(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma)
        {
            double ukupanTrosak = 0;
            double brojTroskova = 0;
            foreach (Trosak trosak in db.Troskovi)
            {
                if(trosak!=null && trosak.Korisnik==korisnik && trosak.DatumIVrijeme>=odDatuma && trosak.DatumIVrijeme <= doDatuma)
                {
                    ukupanTrosak += trosak.Iznos;
                    brojTroskova++;
                }
            }
            double prosjcanTrosak=ukupanTrosak/brojTroskova;
            double sumaVarijacija = 0;
            foreach (Trosak trosak in db.Troskovi)
            {
                if (trosak != null && trosak.Korisnik == korisnik && trosak.DatumIVrijeme >= odDatuma && trosak.DatumIVrijeme <= doDatuma)
                {
                   sumaVarijacija +=Math.Pow(trosak.Iznos-prosjcanTrosak,2);
                }
            }
            return sumaVarijacija / brojTroskova;

        }
    }
}
