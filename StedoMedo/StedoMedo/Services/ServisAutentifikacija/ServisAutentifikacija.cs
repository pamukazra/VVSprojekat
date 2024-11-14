using StedoMedo.Models;
using StedoMedo.Data;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StedoMedo.Services.ServisAutentifikacija
{
    public class ServisAutentifikacija : IServisAutentifikacija
    {
        private readonly DbClass _db;

        public ServisAutentifikacija(DbClass db)
        {
            _db = db;
        }

        public bool KreirajKorisnika(string username, string ime, string prezime, string telefon, string email, string sifra)
        {
            if (_db.Korisnici.Any(k => k.Username == username))
            {
                Console.WriteLine("Korisnik sa tim korisničkim imenom već postoji");
                return false;
            }

            string sifraHash = Hash(sifra);

            int noviId = _db.Korisnici.Any() ? _db.Korisnici.Max(k => k.Id) + 1 : 1;
            Korisnik noviKorisnik = new Korisnik(
                noviId,
                username,
                ime,
                prezime,
                telefon,
                email,
                sifraHash
            );

            _db.AddKorisnik(noviKorisnik);
            return true;
        }

        public Korisnik PrijaviKorisnika(string username, string sifra)
        {
            string sifraHash = Hash(sifra);

            return _db.Korisnici.FirstOrDefault(k => k.Username == username && k.SifraHash == sifraHash);
        }

        public bool OdjaviKorisnika(Korisnik user)
        {
            return _db.Korisnici.Contains(user);
        }



        private static readonly uint[] k = {
        0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
        0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
        0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
        0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
        0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
        0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
        0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
        0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
        0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
        0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
        0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
        0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
        0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
        0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
        0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
        0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
    };

        private static uint RightRotate(uint x, int n) => (x >> n) | (x << (32 - n));

        public string Hash(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            uint[] paddedData = PreProcessData(data);

            uint[] hashValues = {
            0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
            0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
        };

            for (int i = 0; i < paddedData.Length / 16; i++)
            {
                uint[] w = new uint[64];
                Array.Copy(paddedData, i * 16, w, 0, 16);
                for (int t = 16; t < 64; t++)
                {
                    uint s0 = RightRotate(w[t - 15], 7) ^ RightRotate(w[t - 15], 18) ^ (w[t - 15] >> 3);
                    uint s1 = RightRotate(w[t - 2], 17) ^ RightRotate(w[t - 2], 19) ^ (w[t - 2] >> 10);
                    w[t] = w[t - 16] + s0 + w[t - 7] + s1;
                }

                uint a = hashValues[0], b = hashValues[1], c = hashValues[2], d = hashValues[3];
                uint e = hashValues[4], f = hashValues[5], g = hashValues[6], h = hashValues[7];

                for (int t = 0; t < 64; t++)
                {
                    uint S1 = RightRotate(e, 6) ^ RightRotate(e, 11) ^ RightRotate(e, 25);
                    uint ch = (e & f) ^ (~e & g);
                    uint temp1 = h + S1 + ch + k[t] + w[t];
                    uint S0 = RightRotate(a, 2) ^ RightRotate(a, 13) ^ RightRotate(a, 22);
                    uint maj = (a & b) ^ (a & c) ^ (b & c);
                    uint temp2 = S0 + maj;

                    h = g;
                    g = f;
                    f = e;
                    e = d + temp1;
                    d = c;
                    c = b;
                    b = a;
                    a = temp1 + temp2;
                }

                hashValues[0] += a;
                hashValues[1] += b;
                hashValues[2] += c;
                hashValues[3] += d;
                hashValues[4] += e;
                hashValues[5] += f;
                hashValues[6] += g;
                hashValues[7] += h;
            }

            StringBuilder hashString = new StringBuilder();
            foreach (uint value in hashValues)
            {
                hashString.Append(value.ToString("x8"));
            }
            return hashString.ToString();
        }

        private uint[] PreProcessData(byte[] data)
        {
            int originalLength = data.Length;
            int paddedLength = ((originalLength + 8) / 64 + 1) * 64;
            byte[] padded = new byte[paddedLength];
            Array.Copy(data, padded, originalLength);
            padded[originalLength] = 0x80;
            ulong bitLength = (ulong)originalLength * 8;

            for (int i = 0; i < 8; i++)
            {
                padded[paddedLength - 8 + i] = (byte)(bitLength >> (8 * (7 - i)));
            }

            uint[] result = new uint[paddedLength / 4];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (uint)(padded[i * 4] << 24 | padded[i * 4 + 1] << 16 | padded[i * 4 + 2] << 8 | padded[i * 4 + 3]);
            }
            return result;
        }
        public bool ObrisiKorisnika(Korisnik user)
        {
            try
            {
                while (true)
                {
                    var trosak = _db.Troskovi.FirstOrDefault(t => t.Korisnik == user);
                    if (trosak == null) break;

                    _db.Troskovi.Remove(trosak);
                }

                while (true)
                {
                    var budzet = _db.Budzeti.FirstOrDefault(b => b.Korisnik == user);
                    if (budzet == null) break;

                    _db.Budzeti.Remove(budzet);
                }
                _db.Korisnici.Remove(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska prilikom brisanja troska!");
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }

}

