using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalClassLibrary
{
    [Serializable]
    //klasa administratora
    public class Admin : Employee
    {
        public Admin(string imie, string nazwisko, int[] pesel, string nazwaUzytkownika, string haslo) : base(imie, nazwisko, pesel, nazwaUzytkownika, haslo)
        {

        }
        //funkcja info po polimorfizacji dla Admin
        public override string Info()
        {
            return base.Info() + "Admin";

        }

    }
}
