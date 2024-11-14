using StedoMedo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StedoMedo.Services
{
    internal interface IServisPredvidjanjeTroskova
    {
        double ProcijeniTroskove(Korisnik korisnik, int brojDanaZaProcjenu);
    }
}
