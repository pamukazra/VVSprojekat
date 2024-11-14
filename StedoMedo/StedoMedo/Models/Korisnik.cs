using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.Models
{
    public class Korisnik
    {
        [Required(ErrorMessage = "Id je obavezan.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username je obavezan.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username mora imati između 3 i 50 karaktera.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Ime je obavezno.")]
        [StringLength(30, ErrorMessage = "Ime može imati najviše 30 karaktera.")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [StringLength(30, ErrorMessage = "Prezime može imati najviše 30 karaktera.")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Telefon je obavezan.")]
        [RegularExpression(@"^\+387\d{8,9}$", ErrorMessage = "Telefon mora biti u formatu +387xxxxxxxx ili +387xxxxxxxxx.")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Email nije validan.")]
        public string Email { get; set; }
        public string SifraHash { get; set; }

        public Korisnik(int id, string username, string ime, string prezime, string telefon, string email, string sifraHash)
        {
            Id = id;
            Username = username;
            Ime = ime;
            Prezime = prezime;
            Telefon = telefon;
            Email = email;
            SifraHash = sifraHash;
        }
        public override string ToString()
        {
            return Id.ToString() + " " + Username + " " + Ime + " " + Prezime + " " + Telefon + " " + Email;
        }
    }
}
