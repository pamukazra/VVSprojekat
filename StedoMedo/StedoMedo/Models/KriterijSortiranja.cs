using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.Models
{
    public enum SmjerSortiranja
    {
        Opadajuci,
        Rastuci
    }
    public static class MetodeSortiranja
    {
        public static int SortirajPoId(Trosak t1, Trosak t2, SmjerSortiranja smjer)
        {
            if (t1.Id == t2.Id)
                return 0;
            if (smjer == SmjerSortiranja.Rastuci && t1.Id < t2.Id)
                return 1;
            else if (smjer == SmjerSortiranja.Opadajuci && t1.Id > t2.Id)
                return 1;
            return -1;
        }
        public static int SortirajPoDatumu(Trosak t1, Trosak t2, SmjerSortiranja smjer)
        {
            if (t1.DatumIVrijeme == t2.DatumIVrijeme)
                return 0;
            if (smjer == SmjerSortiranja.Rastuci && t1.DatumIVrijeme < t2.DatumIVrijeme)
                return 1;
            else if (smjer == SmjerSortiranja.Opadajuci && t1.DatumIVrijeme > t2.DatumIVrijeme)
                return 1;
            return -1;
        }
        public static int SortirajPoIznosu(Trosak t1, Trosak t2, SmjerSortiranja smjer)
        {
            if (t1.Iznos == t2.Iznos)
                return 0;
            if (smjer == SmjerSortiranja.Rastuci && t1.Iznos < t2.Iznos)
                return 1;
            else if (smjer == SmjerSortiranja.Opadajuci && t1.Iznos > t2.Iznos)
                return 1;
            return -1;
        }
        public static int SortirajPoKategoriji(Trosak t1, Trosak t2, SmjerSortiranja smjer)
        {
            if (t1.KategorijaTroska == t2.KategorijaTroska)
                return 0;
            if (smjer == SmjerSortiranja.Rastuci && t1.KategorijaTroska < t2.KategorijaTroska)
                return 1;
            else if (smjer == SmjerSortiranja.Opadajuci && t1.KategorijaTroska > t2.KategorijaTroska)
                return 1;
            return -1;
        }
    }
    public class KriterijSortiranja
    {
        public Func<Trosak,Trosak,SmjerSortiranja,int> KriterijPoredjenja { get; set; }
        public SmjerSortiranja SmjerSortiranja { get; set; }
        public KriterijSortiranja(Func<Trosak, Trosak, SmjerSortiranja, int> kriterijPoredjenja, SmjerSortiranja smjerSortiranja = SmjerSortiranja.Rastuci) {
            KriterijPoredjenja = kriterijPoredjenja;
            SmjerSortiranja = smjerSortiranja;
        }
    }
}
