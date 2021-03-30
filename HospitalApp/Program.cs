using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using HospitalClassLibrary;


namespace HospitalApplication
{
    public struct BasicInfo
    {
        public static string name;
        public static string surname;
        public static int[] pesel;
        public static string password;
        public static string username;
        public struct AdditionalInfo
        {
            public static string specialization;
            public static int[] PWZnumber;
        }
    }

    class Program
    {
        

        static void Main(string[] args)
        {          
            //podstawowa "pusta" lista klasy Employee
            List<HospitalClassLibrary.Employee> usersList = new List<HospitalClassLibrary.Employee>();

            //deserializacja pliku do pustej listy "usersList" klasy Employee       
            usersList = Deserialization(usersList);

            // funkcja login pozwala wpisac dane i porównuje do danych w liście(usersList), która została zdeserializowana sprawdza czy zalogowany uzytkownik jest adminem
            //oddaje string z nazwa uzytkownika
            string username;
            bool Admin = LogIn(usersList,out username);
                      
            //zwykły użytkownik (pielęgniarka/lekarz)
            if (Admin == false)
            {

                int input;
                do
                {
                    Console.Clear();
                    
                    Console.WriteLine("Zalogowano jako: {0}",username+"\n");
                    Console.WriteLine("Wpisz 1, aby sprawdzić dyżury pracowników\nWpisz 0, aby zakończyć działanie programu.");
                    input = int.Parse(Console.ReadLine());
                    switch (input)
                    {
                        case 1: 
                            ShowShift(usersList);
                            break;
                        case 0:
                            Console.WriteLine("Pomyślnie wylogowano");
                            input = 0;
                            break;
                        default:
                            Console.WriteLine("Niepoprawna liczba lub format danych, spróbuj ponownie\n Wciśnij ENTER, aby kontynuować");
                            Console.ReadKey();
                            break;
                    }
                    
            } while (input != 0) ;
        }

            //administrator
            else if (Admin == true)
            {
                int answer;
                do
                {
                    bool validInput = false;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Zalogowano jako: Administator\n");
                        Console.WriteLine("Wpisz liczbę odpowiadającą akcji, którą chcesz wykonać.");
                        Console.WriteLine("1. Dodaj użytkownika");
                        Console.WriteLine("2. Dodaj dyżur pracownikowi");
                        Console.WriteLine("3. Zmień dane użytkownika");
                        Console.WriteLine("4. Wyświetl dyżury pracownika");
                        Console.WriteLine("5. Ręczny zapis pliku");
                        Console.WriteLine("0. Wylogowanie oraz zapis danych");

                        if (!int.TryParse(Console.ReadLine(), out answer))
                        {
                            Console.Clear();
                            Console.WriteLine("Nie podano cyfry !\n Wciśnij ENTER, aby spróbować jeszcze raz.");
                            Console.ReadKey();
                        }
                        else if (answer > 5)
                        {
                            Console.Clear();
                            Console.WriteLine("Wybrano nieprawidłową opcję !\n Wciśnij ENTER, aby spróbować jeszcze raz.");
                            Console.ReadKey();

                        }
                        else
                        {
                            validInput = true;
                        }

                    } while (validInput != true);
                    Console.Clear();

                    switch (answer)
                    {
                        case 1:
                            AddUser(usersList);
                            break;
                        case 2:
                            AddShift(usersList);                        
                            break;
                        case 3:
                            ChangeUserInfo(usersList);                       
                            break;                               
                        case 4:
                            ShowShift(usersList);
                            break;
                        case 5:
                            Serialization(usersList);                           
                            break;
                        case 0:                           
                            Console.WriteLine("Wylogowano prawidłowo.\nNastępuje zamknięcie programu.");
                            break;
                        default:
                            Console.Clear ();
                            Console.WriteLine("Niepoprawna liczba.");
                            break;
                    }

                } while (answer != 0);

                Serialization(usersList);
            }
        }
        public static void ChangeUserInfo(List<HospitalClassLibrary.Employee> List)
        {
            int inputC3;

            for (int i = 0; i <= List.Count - 1; i++)
            {
                Console.WriteLine(i + 1 + ". " + List[i].Info());
            }
            Console.WriteLine("Podaj ID pracownika któremu chcesz zmienić dane");
            if (!int.TryParse(Console.ReadLine(), out inputC3))
            {
                Console.WriteLine("Nie podano liczby");
            }
            else
            {
                
                
                //jeśli user jest doktorem
                if (List[inputC3 - 1] is HospitalClassLibrary.Doctor)
                {
                    
                    string name, surname, password, username;
                    int[] pesel = new int[11];
                    TakeUserInfo(out name, out surname, out pesel, out username, out password);

                    string specialization;
                    int[] PWZnumber = new int[7];
                    TakeDocInfo(out specialization, out PWZnumber);
                   
                    (List[inputC3 - 1] as HospitalClassLibrary.Doctor).ChangeUserData(name, surname, pesel, username, password, specialization, PWZnumber);
                    Console.WriteLine("Zmieniono dane lekarza");
                    
                }
                else//jeśli user nie potrzebuje dodatkowych atrybutów [specjalizacja numer pwz]
                {
                    //jesli pielegniarka
                    if (List[inputC3 - 1] is HospitalClassLibrary.Nurse)
                    {
                        int[] PWZnumber = new int[7];
                        string name, surname, password, username;
                        int[] pesel = new int[11];
                        TakeUserInfo(out name, out surname, out pesel, out username, out password);
                        (List[inputC3 - 1] as HospitalClassLibrary.Nurse).ChangeUserData(name, surname, pesel, username, password);
                        Console.WriteLine("Zmieniono dane Pielęgniarki");
                    }
                    else
                    { //jesli admin
                        int[] PWZnumber = new int[7];
                        string name, surname, password, username;
                        int[] pesel = new int[11];
                        TakeUserInfo(out name, out surname, out pesel, out username, out password);
                       
                        (List[inputC3 - 1] as HospitalClassLibrary.Admin).ChangeUserData(name, surname, pesel, username, password);
                        Console.WriteLine("Zmieniono dane administatora");
                    }
                }
            }
            Console.WriteLine("Wciśnij ENTER, aby kontynuować.");
            Console.ReadKey();
        }
        private static void AddDoctor(List<HospitalClassLibrary.Employee> List)
        {
            
            string name, surname, password, username;
            int[] pesel = new int[11];
            TakeUserInfo(out name, out surname, out pesel, out username, out password);

            string specialization;
            int[] PWZnumber = new int[7];
            TakeDocInfo(out specialization, out PWZnumber);

            HospitalClassLibrary.Employee newDoctor = new HospitalClassLibrary.Doctor(name, surname, pesel, username, password, specialization, PWZnumber);
            List.Add(newDoctor);
            Console.WriteLine("Nowy lekarz został pomyślnie dodany\nWciśnij ENTER, aby kontynuować.");
            Console.ReadKey();
            
           
        }
        private static void AddNurse(List<HospitalClassLibrary.Employee> List)
        {
            string name,surname,password,username;
            int[] pesel = new int[11];
            TakeUserInfo(out name,out surname, out pesel,out username, out password);

            HospitalClassLibrary.Employee newNurse = new HospitalClassLibrary.Nurse(name,surname, pesel,username, password);
            List.Add(newNurse);
            Console.WriteLine("Pomyślnie dodano nową Pielęgniarkę/Pielęgniarza\nWciśnij ENTER, aby kontynuować.");
            Console.ReadKey();
        }
        private static void AddAdmin(List<HospitalClassLibrary.Employee> List)
        {
            //ADMIN
            int[] PWZnumber = new int[7];
            string name, surname, password, username;
            int[] pesel = new int[11];
            TakeUserInfo(out name, out surname, out pesel, out username, out password);
            HospitalClassLibrary.Employee newAdmin = new HospitalClassLibrary.Admin(name, surname, pesel, username, password);
            List.Add(newAdmin);
            Console.WriteLine("Pomyślnie dodano nowego administratora\nWciśnij ENTER, aby kontynuować.");
            Console.ReadKey();
            
            

        }
        public static void AddUser(List<HospitalClassLibrary.Employee> List )
        {
            Console.WriteLine("Podaj rolę nowego użytkownika w systemie \n1. Administrator \n2. Pielegniarka \n3. Lekarz");
            int who = int.Parse(Console.ReadLine());

            switch (who)
            {
                case 1://ADMIN
                    AddAdmin(List);
                    break;

                case 2://NURSE
                    AddNurse(List);
                    break;
                case 3: //DOCTOR 
                    AddDoctor(List);
                    break;
                default:
                    Console.WriteLine("Wybrano nieistniejącą opcję lub wpisano niepoprawny typ danych");
                    break;

            }

        }
        public static void AddShift(List<HospitalClassLibrary.Employee> List)
        {
            int ID;
            bool isOk = true;
            for (int i = 0; i <= List.Count - 1; i++)
            {
                Console.WriteLine(i + 1 + ". " + List[i].Info());
            }
            Console.WriteLine("Podaj ID pracownika, któremu chcesz dodać dyżur");
            if (!int.TryParse(Console.ReadLine(), out ID))
            {
                Console.WriteLine("Nie podano liczby");
            }

            else
            {

                if (List[ID - 1].shifts.Count == 0)
                {
                    
                    DateTime newShift = TakeShiftInfo();

                    if (List[ID - 1] is HospitalClassLibrary.Doctor)
                    {
                        for (int j = 0; j <= List.Count - 1; j++)
                        {
                            if (List[j] != List[ID - 1])
                            {
                                if (List[j] is HospitalClassLibrary.Doctor)
                                {
                                    if ((List[j] as HospitalClassLibrary.Doctor).Specialization == (List[ID - 1] as HospitalClassLibrary.Doctor).Specialization)
                                    {
                                        if (List[j].shifts.Count != 0)
                                        {
                                            for (int z = 0; z <= List[j].shifts.Count - 1; z++)
                                            {
                                                if (List[j].shifts[z] == newShift)
                                                {
                                                    Console.WriteLine("Na jednej zmianie może być tylko jeden specjalista.");
                                                    isOk = false; 
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (isOk == true)
                    {
                        List[ID - 1].shifts.Add(newShift);
                        Console.WriteLine("Dodano dyżur poprawnie\nWciśnij ENTER, aby kontynuować");
                        Console.ReadKey();

                    }
                }
                else
                {

                    Console.Clear();
                    Console.WriteLine("Dyżury użytkownika o nr ID: {0}", ID);
                    for (int i = 0; i <= List[ID - 1].shifts.Count - 1; i++)
                    {
                        Console.WriteLine(List[ID - 1].shifts[i]);
                    }

                    

                    DateTime newShift = TakeShiftInfo();

                    for (int i = 0; i <= List[ID - 1].shifts.Count - 1; i++)
                    {
                        if (newShift != List[ID - 1].shifts[i])
                        {
                            if (newShift.Day != List[ID - 1].shifts[i].Day -1 && newShift.Month != List[ID - 1].shifts[i].Month  && newShift.Year != List[ID - 1].shifts[i].Year )
                            {
                                if (List[ID - 1] is HospitalClassLibrary.Doctor)
                                {
                                    for (int j = 0; j <= List.Count - 1; j++)
                                    {
                                        if (List[j] != List[ID - 1])
                                        {
                                            
                                            if ((List[j] as HospitalClassLibrary.Doctor)?.Specialization == (List[ID - 1] as HospitalClassLibrary.Doctor)?.Specialization)
                                            {
                                                if (List[j].shifts.Count != 0)
                                                {
                                                    for (int z = 0; z <= List[j].shifts.Count - 1; z++)
                                                    {
                                                        if (List[j].shifts[z] == newShift)
                                                        {
                                                            Console.WriteLine("Nie może być dwóch specjalistów na jednej zmianie\nWciśnij ENTER,aby kontynuować.");
                                                            Console.ReadKey();
                                                            isOk = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                isOk = false;
                                Console.WriteLine("Użytkownik nie może mieć dyżuru dzień pod dniu\nWciśnij ENTER,aby kontynuować.");
                                Console.ReadKey();

                            }
                        }
                        else
                        {
                            isOk = false;
                            Console.WriteLine("Użytkownik nie może mieć dyżuru w ten sam dzień\nWciśnij ENTER,aby kontynuować.");
                            Console.ReadKey();
                        }
                    }
                    if (isOk == true)
                    {
                        List[ID - 1].shifts.Add(newShift);
                        Console.WriteLine("Dyżur został dodany\nWciśnij ENTER, aby kontynuować");
                        Console.ReadKey();
                    }

                }

            }

        }
        public static void ShowShift(List<HospitalClassLibrary.Employee> List)
        {

            for (int i = 0; i <= List.Count - 1; i++)
            {
                Console.WriteLine(i + 1 + ". " + List[i].Info());
            }
            int IdC = int.Parse(Console.ReadLine());
            if (IdC > List.Count() || IdC < 0)
            {
                Console.WriteLine("Niepoprawne ID użytkownika");
            }
            else
            {
                if (List[IdC - 1].shifts.Count() == 0)
                {
                    Console.WriteLine("Brak dyżurów do wyświetlenia");
                }
                else
                {
                    for (int i = 0; i <= List[IdC - 1].shifts.Count - 1; i++)
                    {
                        Console.WriteLine(List[IdC - 1].shifts[i]);
                    }
                }

            }

            Console.WriteLine("Wciśnij ENTER, aby kontynuować");

            Console.ReadKey();
        }
        public static void TakeDocInfo(out string specializationOUT,out int[] PWZnumberOUT)
        {
            bool pPesel = false;
            BasicInfo.AdditionalInfo.PWZnumber = new int[7];
            Console.WriteLine("Podaj specjalizację");
            BasicInfo.AdditionalInfo.specialization= Console.ReadLine();

            bool validSpecialiazation = false;
            while (validSpecialiazation != true)
            {


                if (BasicInfo.AdditionalInfo.specialization.ToLower() == "kardiolog" || BasicInfo.AdditionalInfo.specialization.ToLower() == "urolog" || BasicInfo.AdditionalInfo.specialization.ToLower() == "laryngolog" || BasicInfo.AdditionalInfo.specialization.ToLower() == "neurolog")
                {

                    string subspec = BasicInfo.AdditionalInfo.specialization.Substring(1);
                    string subspec2 = BasicInfo.AdditionalInfo.specialization.Substring(0, 1);
                    BasicInfo.AdditionalInfo.specialization = subspec2.ToUpper() + subspec;
                    validSpecialiazation = true;
                }
                else
                {
                    Console.WriteLine("Niepoprawna specjalizacja");
                    Console.WriteLine("Wpisz ponownie specjalizację kardiolog/urolog/laryngolog/neurolog");
                    BasicInfo.AdditionalInfo.specialization = Console.ReadLine();

                    validSpecialiazation = false;
                }
            }
            do
            {

                
                Console.WriteLine("Podaj numer PWZ");
                string checking = Console.ReadLine();
                if (checking.Length < 7)
                {
                    Console.WriteLine("Podano za krótki numer PWZ");
                }
                else
                {
                    if (checking.Length > 7)
                    {
                        Console.WriteLine("Podano za długi numer PWZ");
                    }
                    else
                    {
                        for (int i = 0; i <= checking.Length - 1; i++)
                        {
                            string test = Convert.ToString(checking[i]);
                            if (!int.TryParse(test, out BasicInfo.AdditionalInfo.PWZnumber[i]))
                            {
                                Console.WriteLine("Nie podano liczby (index {0})", i);
                            }
                            else
                            {
                                if (i == 6)
                                {
                                    pPesel = true;
                                }
                            }
                        }
                    }
                }
            } while (pPesel != true);
            specializationOUT = BasicInfo.AdditionalInfo.specialization;
            PWZnumberOUT = BasicInfo.AdditionalInfo.PWZnumber;
        }
        public static DateTime TakeShiftInfo()
        {
            
            int dzien, miesiac, rok;
            
            //ArgumentOutOfRangeException,FormatException

            do
            {
                Console.WriteLine("Podaj dzień ");

                if (!int.TryParse(Console.ReadLine(), out dzien))
                {
                    Console.WriteLine("Nieporawny format");
                    continue;
                }
                Console.WriteLine("Podaj miesiąc ");
                if (!int.TryParse(Console.ReadLine(), out miesiac))
                {
                    Console.WriteLine("Nieporawny format");
                    continue;
                }
                Console.WriteLine("Podaj rok ");
                if (!int.TryParse(Console.ReadLine(), out rok))
                {
                    Console.WriteLine("Nieporawny format");
                    continue;
                }

                try
                {
                    DateTime testShift = new DateTime(rok, miesiac, dzien);

                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Niepoprawna data");

                }


            } while (true);
            DateTime newShift = new DateTime(rok, miesiac, dzien);
            return newShift;
        }
        private static void TakeUserInfo(out string nameOUT, out string surnameOUT, out int[] peselOUT, out string usernameOUT, out string passwordOUT)
        {
            bool pPesel = false;
            BasicInfo.pesel = new int[11];
            Console.WriteLine("Podaj imie");
            BasicInfo.name = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko");
            BasicInfo.surname = Console.ReadLine();
            do
            {

                Console.WriteLine("Podaj pesel");
                string initialPesel = Console.ReadLine();
                if (initialPesel.Length < 11 || initialPesel.Length > 11)
                {
                    Console.WriteLine("Podany pesel jest zbyt krótki lub zbyt długi");
                }
                else
                {

                    for (int i = 0; i <= initialPesel.Length - 1; i++)
                    {
                        //czy jest poprawny
                        string test = Convert.ToString(initialPesel[i]);
                        if (!int.TryParse(test, out BasicInfo.pesel[i]))
                        {
                            Console.WriteLine("Nie podano liczby (index {0})", i);
                            pPesel = false;
                            break;
                        }
                        else
                        {
                            if (i == 10)
                            {
                                pPesel = true;
                            }
                        }
                    }
                }

            } while (pPesel != true);
            Console.WriteLine("Podaj nazwę użytkownika");
            BasicInfo.username = Console.ReadLine();
            Console.WriteLine("Podaj hasło");
            BasicInfo.password = Console.ReadLine();

            nameOUT = BasicInfo.name;
            surnameOUT = BasicInfo.surname;
            peselOUT = BasicInfo.pesel;
            passwordOUT = BasicInfo.password;
            usernameOUT = BasicInfo.username;


        }
        public static bool LogIn(List<HospitalClassLibrary.Employee> List,out string usernameOUT)
        {
            bool isAdmin = false;
            bool isUser = false;
            
            do
            {
                               
                Console.WriteLine("Wpisz nazwę użytkownika");
                BasicInfo.username = Console.ReadLine();

                foreach (var element in List)
                {
                    if (BasicInfo.username == element.Username)
                    {
                        Console.WriteLine("Wpisz hasło");
                        
                        BasicInfo.password = string.Empty;
                        ConsoleKey key;
                        do
                        {
                            var keyInfo = Console.ReadKey(intercept: true);
                            key = keyInfo.Key;

                            if (key == ConsoleKey.Backspace && BasicInfo.password.Length > 0)
                            {
                                Console.Write("\b \b");
                                BasicInfo.password = BasicInfo.password[0..^1];
                            }
                            else if (!char.IsControl(keyInfo.KeyChar))
                            {
                                Console.Write("*");
                                BasicInfo.password += keyInfo.KeyChar;
                            }
                        } while (key != ConsoleKey.Enter);
                        if (BasicInfo.password == element.Password)
                        {
                            if (element is HospitalClassLibrary.Admin)
                            {
                                isAdmin = true;
                                isUser = true;                                                               
                            }
                            else
                            {
                                isAdmin = false;
                                isUser = true;                                
                            }
                        }
                    }
                }
                
                Console.Clear();
                

            } while (isUser != true);
            
            usernameOUT = BasicInfo.username;
            return isAdmin;

        }
        public static void Serialization(List<HospitalClassLibrary.Employee> List)
        {
            FileStream fs = new FileStream("users.dat", FileMode.Create);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, List);
            }
            catch (SerializationException e)
            {
                Console.WriteLine($"Nie udało się ponieważ {e.Message}");
                throw;
            }
            finally
            {
                fs.Close();
                Console.WriteLine("Dane zapisano prawidłowo\nWciśnij ENTER, aby kontynuować");
                Console.ReadKey();
                
            }

        }
        public static List<HospitalClassLibrary.Employee> Deserialization(List<Employee> List)
        {
            
            

            if (!File.Exists("users.dat"))
            {
                int[] tempArray = { 0 };
                HospitalClassLibrary.Employee Admin0 = new HospitalClassLibrary.Admin("Admin", "0", tempArray, "admin", "admin");
                List.Add(Admin0);
                Console.WriteLine("Informacja o pliku: Brak pliku do odczytu. Stworzono nowy plik i admina-0 (admin/admin)");
                
            }
            else
            {
                Stream s = File.OpenRead("users.dat");
                   BinaryFormatter bf = new BinaryFormatter();
                   if (bf.Deserialize(s) is List<HospitalClassLibrary.Employee>)
                   {
                        s.Seek(0, SeekOrigin.Begin);
                        List = (List<HospitalClassLibrary.Employee>)bf.Deserialize(s);
                        s.Close();
                       Console.WriteLine("Informacja o pliku: Wczytano pomyślnie");
                    return List;
                    
                   }
                   else
                   {
                    s.Close();
                    Console.WriteLine("Bledny format wczytywanego pliku");
                    return null;
                    
                   }
            }
            return List;
        }

    }
}