using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HospitalClassLibrary
{
    [Serializable]
    
    public class Nurse : Employee
    {
        //przeładowany konstruktor dla pielęgniarki
        public Nurse(string name, string surname, int[] pesel, string username, string password) : base(name, surname, pesel, username, password)
        {
        }
        //funkcja info po polimorfizacji dla pielęgniarki
        public override string Info()
        {
            return base.Info() + "Pielęgniarka";

        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
