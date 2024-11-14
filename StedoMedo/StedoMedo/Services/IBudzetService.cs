using StedoMedo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.Services
{
    public interface IBudzetService
    {
        double PreostaloStanjeBudzeta(Korisnik korisnik, DateOnly doDana);
        Korisnik DodajBudzet(Korisnik korisnik, DateOnly pocetak, DateOnly kraj, double iznos);
        bool ObrisiBudzet(Korisnik korisnik, int idBudzeta);
        bool IzmjeniBudzet(Korisnik korisnik, int idBudzeta, DateOnly noviPocetak, DateOnly noviKraj, double noviIznos);
        bool ProvjeraPostojanja(Korisnik korisnik, int idBudzeta);

    }
}
