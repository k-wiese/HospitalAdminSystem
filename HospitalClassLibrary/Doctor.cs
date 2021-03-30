using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalClassLibrary
{
    [Serializable]

    public class Doctor : Employee
    {
        //członkowie klasy doctor
        string specialization;
        public string Specialization
        {
            get { return specialization; }
            set { specialization = value; }
        }
        int[] PWZnumber = new int[7];

        //funkcja info po polimorfizacji dla doctor
        public override string Info()
        {
            return base.Info() + "Lekarz " + Specialization;
        }

        //przeładowany konstruktor dla doctor
        public Doctor(string name, string surname, int[] pesel, string username, string password, string specialization, int[] PWZnumber) : base(name, surname, pesel, username, password)
        {
            this.specialization = specialization;
            this.PWZnumber = PWZnumber;
        }
        //przeładowana funkcja zmiany danych
        public void ChangeUserData(string name, string surname, int[] pesel, string username, string password, string specialization, int[] PWZnumber)
        {
            base.ChangeUserData(name, surname, pesel, username, password);
            this.specialization = specialization;
            this.PWZnumber = PWZnumber;

        }

    }
}
