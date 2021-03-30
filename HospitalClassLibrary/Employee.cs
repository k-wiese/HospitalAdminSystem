using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalClassLibrary
{
    [Serializable]
    //klasa abstrakcyjna, forma dla lekarza, pielęgniarki i admina
    abstract public class Employee
    {
        //członkowie klasy po hermetyzacji
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string surname;
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        int[] pesel = new int[11];

        string username;
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public List<DateTime> shifts = new List<DateTime>();
        //konstruktor bazowy 
        public Employee(string name, string surname, int[] pesel, string username, string password)
        {
            this.name = name;
            this.surname = surname;
            this.pesel = pesel;
            this.username = username;
            this.password = password;
        }
        //bazowa funkcja info
        public virtual string Info()
        {
            string userData;
            return userData = name + " " + surname + " ";
        }
        //bazowa funkcja zmien dane
        public virtual void ChangeUserData(string name, string surname, int[] pesel, string username, string password)
        {
            this.name = name;
            this.surname = surname;
            this.pesel = pesel;
            this.username = username;
            this.password = password;
        }
    }
}
