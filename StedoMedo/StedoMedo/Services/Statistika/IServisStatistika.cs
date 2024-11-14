using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StedoMedo.Models;

namespace StedoMedo.Services.Statistika
{
    public interface IServisStatistika
    {
        public double UkupniTrosak(Korisnik korisnik,DateTime odDatuma, DateTime doDatuma, List<KategorijaTroska> poKategorijama);
        public double ProsjecnaPotrosnja(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma, List<KategorijaTroska> poKategorijama);

        public double NajveciTrosak(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma, List<KategorijaTroska> poKategorijama);
        public Dictionary<KategorijaTroska,Double> RaspodjelaPoKategorijama(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma);

        public double VarijansaTroskova(Korisnik korisnik, DateTime odDatuma, DateTime doDatuma);
    }
}
