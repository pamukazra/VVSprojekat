using StedoMedo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StedoMedo.Data;
using StedoMedo.Models;
using StedoMedo.Services;

namespace StedoMedo.Services
{
    public class ServisPredvidjanjeTroskova:IServisPredvidjanjeTroskova
    {
        private readonly DbClass _db;

        public ServisPredvidjanjeTroskova(DbClass db)
        {
            _db = db;
        }
        private List<Trosak> troskoviKorisnika(Korisnik korisnik)
        {
            if (korisnik == null)
            {
                throw new ArgumentNullException(nameof(korisnik), "Korisnik ne postoji.");
            }
            var troskoviKorisnika = new List<Trosak> { };
            foreach (var trosak in _db.Troskovi)
            {
                if(trosak.Korisnik.Id==korisnik.Id)
                troskoviKorisnika.Add(trosak);
            }
            if (!troskoviKorisnika.Any())
            {
                throw new ArgumentNullException(nameof(korisnik), "Korisnik nema troškova.");
            }
            return troskoviKorisnika;
        }
        private List<Trosak> sortirajPoDanu(List<Trosak> troskovi)
        {
            int n = troskovi.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (troskovi[j].DatumIVrijeme.Date > troskovi[j + 1].DatumIVrijeme.Date)
                    {
                        Trosak temp = troskovi[j];
                        troskovi[j] = troskovi[j + 1];
                        troskovi[j + 1] = temp;
                    }
                }
            }
            return troskovi;
        }
        private List<double> troskoviPoDanu(List<Trosak> troskovi)
        {
            if (!troskovi.Any())
            {
                throw new ArgumentNullException("Nema troškova za grupisanje.");
            }
            var troskoviPoDanu = new List<double> { };
            troskoviPoDanu.Add(0);
            var trenutniDan = troskovi[0].DatumIVrijeme;   
            int n = troskovi.Count;
            troskovi = sortirajPoDanu(troskovi);
            foreach (var trosak in troskovi)
            {
                if (trenutniDan.Date == trosak.DatumIVrijeme.Date)
                {
                    troskoviPoDanu[^1] += trosak.Iznos;
                }
                else
                {
                    troskoviPoDanu.Add(trosak.Iznos);
                    trenutniDan = trosak.DatumIVrijeme;
                }
            }
            return troskoviPoDanu;
        }
        private List<double> generisiKoeficijente(List<double> troskoviPoDanu)
        {
            if (!troskoviPoDanu.Any())
            {
                throw new ArgumentNullException("Nema dovoljno vrijednosti za generisanje koeficijenata.");
            }
            var listaKoeficijenata = new List<double>();
            foreach(var trosak in troskoviPoDanu)
            {
                listaKoeficijenata.Add(trosak);
            }
            var n = listaKoeficijenata.Count-1;
            for(int i = 0; i < n;i++)
            {
                for (int j = n; j >= i + 1; j--)
                    listaKoeficijenata[j] = (listaKoeficijenata[j] - listaKoeficijenata[j - 1]) / (i + 1);
            }
            return listaKoeficijenata;
        }
        private double ocekivanaVrijednost(List<double> koeficijenti,int danZaProcjenu)
        {
            if (!koeficijenti.Any())
            {
                throw new ArgumentNullException("Nema koeficijenata za proračun.");
            }
            if (danZaProcjenu<=0)
            {
                throw new ArgumentOutOfRangeException("Dana za procjenu ne smije biti negativan.");
            }
            if(danZaProcjenu>2)
            {
                Console.WriteLine("Uneseni broj dana je veoma veliki, pa procjena možda neće biti precizna.");
            }
            double ocekivanaVrijednost = koeficijenti[^1];
            var n= koeficijenti.Count-1;
            for( int i = n-1; i >= 0;i--)
            {
                ocekivanaVrijednost = ocekivanaVrijednost * (koeficijenti.Count+danZaProcjenu - i-1) + koeficijenti[i];
            }
            if (ocekivanaVrijednost<0)
            {
                Console.WriteLine("Uneseni podaci previše variraju. Nije moguće izračunati tačnu predikciju.");
                return -1;
            }
            return ocekivanaVrijednost;
        }
        public double ProcijeniTroskove(Korisnik korisnik, int brojDanaZaProcjenu) {
            var _troskoviKorisnika = troskoviKorisnika(korisnik);
            var _troskoviPoDanu = troskoviPoDanu(_troskoviKorisnika);
            var _koeficijenti = generisiKoeficijente(_troskoviPoDanu);
            return Math.Round(ocekivanaVrijednost(_koeficijenti, brojDanaZaProcjenu),2);
            ///return ocekivanaVrijednost(generisiKoeficijente(troskoviPoDanu(troskoviKorisnika(korisnik))),brojDanaZaProcjenu);
        }

    }
}
