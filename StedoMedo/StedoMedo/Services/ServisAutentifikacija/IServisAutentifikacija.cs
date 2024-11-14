using StedoMedo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.Services.ServisAutentifikacija
{
    public interface IServisAutentifikacija
    {
        public bool KreirajKorisnika(string username, string name, string surname, string phone, string email, string password);

        public Korisnik PrijaviKorisnika(string username, string password);

        public bool OdjaviKorisnika(Korisnik user);

        public bool ObrisiKorisnika(Korisnik user);

        public string Hash(string password);
    }
}
