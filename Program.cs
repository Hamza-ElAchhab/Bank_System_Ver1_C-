using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.Net.Sockets;

namespace ConsoleApp3
{
    internal class Program
    {

        public const string FileName = "Clients.txt";
        struct st_ClientInfo
        {
            public string AcountNumber;
            public string PinCode;
            public string Name;
            public string Phone;
            public int Balance;
            public bool isMark;
        }

        enum en_MainMenu_Options
        {
            E_ClientsList = 1, E_Add = 2, E_Delete = 3, E_UpDate = 4, E_Find = 5, E_Transations_Menu = 6, E_Exit = 7
        }
        static en_MainMenu_Options Read_Main_Menu_Option()
        {
            int num = 0;
            Console.Write("\n\tEnter Your Choice [1 -> 7] : ");
            num = Convert.ToInt16(Console.ReadLine());

            return (en_MainMenu_Options)num;
        }
        static st_ClientInfo Fill_Struct_Data_From_Record(string record)
        {
            st_ClientInfo info = new st_ClientInfo();

            string[] arr = new string[5];
            arr = record.Split('#');

            info.AcountNumber = arr[0];
            info.PinCode = arr[1];
            info.Name = arr[2];
            info.Phone = arr[3];
            info.Balance = Convert.ToInt32(arr[4]);

            return info;
        }
        static List<st_ClientInfo> LoadData_FromFile_ToList()
        {
            List<st_ClientInfo> lData = new List<st_ClientInfo>();

            if (File.Exists(FileName))
            {
                StreamReader MyFile = new StreamReader(FileName);
                st_ClientInfo info;

                string Buffer;
                while ((Buffer = MyFile.ReadLine()) != null)
                {
                    info = Fill_Struct_Data_From_Record(Buffer);
                    info.isMark = false;
                    lData.Add(info);
                }

                MyFile.Close();
                return lData;
            }
            else
            {
                Console.WriteLine("\n");
            }
            return lData;
        }
        static void Print_One_Record_Info(st_ClientInfo info)
        {
            Console.WriteLine($"\t\t{info.AcountNumber}    | {info.PinCode}         | {info.Name}  |       {info.Phone}  |     {info.Balance}");
        }
        static void ClientsList()
        {
            Console.Clear();

            List<st_ClientInfo> lData = LoadData_FromFile_ToList();

            Console.WriteLine($"\n\t\t\t\t\t   {lData.Count} Client(s).");
            Console.WriteLine("\t---------------------------------------------------------------------------------");
            Console.WriteLine("\t Acount Number :|  PinCode :   |  Name :          |     Phone :      |  Balance ");
            Console.WriteLine("\t---------------------------------------------------------------------------------");

            foreach (st_ClientInfo info in lData)
            {
                Print_One_Record_Info(info);
            }

            Console.WriteLine("\t---------------------------------------------------------------------------------");
        }
        static void system_pause()
        {
            Console.Write("\t\n\nPress Any Key To Continue...");
            Console.ReadKey();
        }
        static string Join_Struct_Client_Info(st_ClientInfo info, char sep = '#')
        {
            string str = info.AcountNumber + sep;

            str += info.PinCode + sep;
            str += info.Name + sep;
            str += info.Phone + sep;
            str += (info.Balance).ToString();

            return str;
        }
        static bool isCleintExist_GetIt(string AcountNumber, st_ClientInfo[] info)
        {
            List<st_ClientInfo> lData = LoadData_FromFile_ToList();

            for (int i = 0; i < lData.Count; i++)
            {
                if (lData[i].AcountNumber == AcountNumber)
                {
                    info[0] = lData[i];
                    return true;
                }
            }
            return false;
        }
        static void SaveData_FromList_ToFile(List<st_ClientInfo> lData)
        {
            StreamWriter MyFile = new StreamWriter(FileName);
            string Line;

            for (int i = 0; i < lData.Count; i++)
            {
                if (lData[i].isMark == false)
                {
                    Line = Join_Struct_Client_Info(lData[i]);
                    MyFile.WriteLine(Line);
                }
            }

            MyFile.Close();
        }
        static void Add_RecordLine_ToFile(string RecLine)
        {
            List<st_ClientInfo> lData = LoadData_FromFile_ToList();

            lData.Add((Fill_Struct_Data_From_Record(RecLine)));

            SaveData_FromList_ToFile(lData);
        }
        static st_ClientInfo Read_StructCleint_Info(string AcountNumber)
        {
            st_ClientInfo info = new st_ClientInfo();

            Console.WriteLine("\n\n\t________ Read Info __________");

            info.AcountNumber = AcountNumber;
            Console.Write("\t  Enter Pin Code      : ");
            info.PinCode = Console.ReadLine();
            Console.Write("\t  Enter Your Name     : ");
            info.Name = Console.ReadLine();
            Console.Write("\t  Enter Your Phone    : ");
            info.Phone = Console.ReadLine();
            Console.Write("\t  Enter Your Balance  : ");
            info.Balance = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\t______________________________");

            return info;
        }
        static string Read_AcountNumber()
        {
            string AcountN;
            Console.Write("\n\n\tEnter an Acount Number : ");
            AcountN = Console.ReadLine();

            return AcountN;
        }
        static void AddClient()
        {
            char ans = 'n';
            do
            {
                Console.Clear();
                Console.WriteLine("\t________________________");
                Console.WriteLine("\t   Add Client Screen");
                Console.WriteLine("\t________________________");

                string acountnumber = Read_AcountNumber();
                st_ClientInfo[] info = new st_ClientInfo[1];

                while (isCleintExist_GetIt(acountnumber, info))
                {
                    Console.Write("\nAcount Number Already Exists, Enter Another one : ");
                    acountnumber = Console.ReadLine();
                }

                Add_RecordLine_ToFile(Join_Struct_Client_Info(Read_StructCleint_Info(acountnumber)));
                Console.WriteLine("\nAdded Succesfully.\n");

                Console.Write("\nDo You Want To Add More [Y/N] : ");
                ans = Convert.ToChar(Console.ReadLine());

            } while (char.ToUpper(ans) == 'Y');
        }
        static void EndProject()
        {
            Console.Clear();
            Console.WriteLine("\n\t_________________________");
            Console.WriteLine("\t   End Program");
            Console.WriteLine("\t-------------------------\n");
        }
        static void Print_Client_Card(st_ClientInfo info)
        {
            Console.WriteLine("\n\n________ Client :________");
            Console.WriteLine("Acount Num : {0}", info.AcountNumber);
            Console.WriteLine("Pin Code   : {0}", info.PinCode);
            Console.WriteLine("Name       : {0}", info.Name);
            Console.WriteLine("Phone      : {0}", info.Phone);
            Console.WriteLine("Balance    : {0}", info.Balance);
            Console.WriteLine("_________________________");
        }
        static st_ClientInfo get_mark_true(st_ClientInfo info)
        {
            info.isMark = true;
            return info;
        }
        static List<st_ClientInfo> Mark_Client_For_Delete(string acountnumber)
        {
            List<st_ClientInfo> lData = LoadData_FromFile_ToList();

            for (int i = 0; i < lData.Count; i++)
            {
                if (lData[i].AcountNumber == acountnumber)
                {
                    lData[i] = get_mark_true(lData[i]);
                    break;
                }
            }
            return lData;
        }
        static void DeleteClient()
        {
            Console.Clear();
            Console.WriteLine("\t________________________");
            Console.WriteLine("\t Delete Client Screen");
            Console.WriteLine("\t________________________");

            string AcountNum = Read_AcountNumber();
            st_ClientInfo[] info = new st_ClientInfo[1];
            char ans = 'n';

            if (isCleintExist_GetIt(AcountNum, info))
            {
                Print_Client_Card(info[0]);

                Console.Write("\nAre You Sure [Y/N] : ");
                ans = Convert.ToChar(Console.ReadLine());

                if(char.ToUpper(ans) == 'Y')
                {
                    List<st_ClientInfo> lData =  Mark_Client_For_Delete(AcountNum);
                    SaveData_FromList_ToFile(lData);
                    Console.WriteLine("\n\nDeleted Succesfully.\n");
                }
            }
            else
            {
                Console.WriteLine($"\nClient With {AcountNum} Does Not Exists.\n");
            }
        }
        static void UpDate_ByAcountNumber(string AcountNumber)
        {
            List<st_ClientInfo> lData = LoadData_FromFile_ToList();

            for (int i = 0; i < lData.Count; i++)
            {
                if (lData[i].AcountNumber == AcountNumber)
                {
                    lData[i] = Read_StructCleint_Info(AcountNumber);
                    break;
                }
            }
            SaveData_FromList_ToFile(lData);
        }
        static void UpDateClient()
        {
            Console.Clear();
            Console.WriteLine("\t________________________");
            Console.WriteLine("\t UpDate Client Screen");
            Console.WriteLine("\t________________________");

            string AcountNum = Read_AcountNumber();
            st_ClientInfo[] info = new st_ClientInfo[1];
            char ans = 'n';

            if (isCleintExist_GetIt(AcountNum, info))
            {
                Print_Client_Card(info[0]);

                Console.Write("\nAre You Sure [Y/N] : ");
                ans = Convert.ToChar(Console.ReadLine());

                if (char.ToUpper(ans) == 'Y')
                {
                    UpDate_ByAcountNumber(AcountNum);
                    Console.WriteLine("\nUpDated Successfully.\n");
                }
            }
            else
            {
                Console.WriteLine($"\nClient With {AcountNum} Does Not Exists.\n");
            }
        }
        static void FindClient()
        {
            Console.Clear();
            Console.WriteLine("\t________________________");
            Console.WriteLine("\t Find Client Screen");
            Console.WriteLine("\t________________________");

            string AcountNum = Read_AcountNumber();
            st_ClientInfo[] info = new st_ClientInfo[1];

            if (isCleintExist_GetIt(AcountNum, info))
            {
                Print_Client_Card(info[0]);
            }
            else
            {
                Console.WriteLine($"\nClient With {AcountNum} Does Not Exists.\n");
            }
        }
        static void Woking_Main_Menu(en_MainMenu_Options option)
        {
            switch (option)
            {
                case en_MainMenu_Options.E_ClientsList:
                    ClientsList();
                    system_pause();
                    MainMenu();
                    break;

                case en_MainMenu_Options.E_Add:
                    AddClient();
                    system_pause();
                    MainMenu();
                    break;

                case en_MainMenu_Options.E_Exit:
                    EndProject();
                    break;

                case en_MainMenu_Options.E_Delete:
                    DeleteClient();
                    system_pause();
                    MainMenu();
                    break;

                case en_MainMenu_Options.E_UpDate:
                    UpDateClient();
                    system_pause();
                    MainMenu();
                    break;

                case en_MainMenu_Options.E_Find:
                    FindClient();
                    system_pause();
                    MainMenu();
                    break;

                case en_MainMenu_Options.E_Transations_Menu:
                    Transactions_Menu();
                    break;


                default:
                    Console.WriteLine("\nWrong Choice....\n");
                    break;
            }
        }
        static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t  Version 1 C#\n\n");
            Console.WriteLine("\t=================================================");
            Console.WriteLine("\t\t\tMain Menu Screen :");
            Console.WriteLine("\t=================================================");
            Console.WriteLine("\t\t      [1] : Show Clients List.");
            Console.WriteLine("\t\t      [2] : Add New Client.");
            Console.WriteLine("\t\t      [3] : Delete Client.");
            Console.WriteLine("\t\t      [4] : UpDate Client.");
            Console.WriteLine("\t\t      [5] : Find Client.");
            Console.WriteLine("\t\t      [6] : Transations Menu.");
            Console.WriteLine("\t\t      [7] : Exit.");
            Console.WriteLine("\t=================================================");

            Woking_Main_Menu(Read_Main_Menu_Option());
        }


        /////
        enum en_TransationsMenu_Options
        {
            e_Deposite = 1, e_WithDraw = 2, e_TotalBalances = 3, e_GoBack = 4
        }
        static en_TransationsMenu_Options Read_TransationsMenu_Option()
        {
            int num = 0;
            Console.Write("\n\t\tEnter Your Choice [1 -> 4] : ");
            num = Convert.ToInt32(Console.ReadLine());
            return (en_TransationsMenu_Options)num;
        }

        static st_ClientInfo Increase_Decrease_By_StructInfo(st_ClientInfo info, int amount)
        {
            info.Balance += amount;
            return info;
        }
        static void Increase_And_Decrease_BalanceOfClient(String Acount, int Amount)
        {
            List<st_ClientInfo> lData = LoadData_FromFile_ToList();
            int NewBalance = 0;

            for (int i = 0; i < lData.Count; i++)
            {
                if (lData[i].AcountNumber == Acount)
                {
                    lData[i] = Increase_Decrease_By_StructInfo(lData[i], Amount);
                    NewBalance = lData[i].Balance;
                    break;
                }
            }

            Console.WriteLine($"\nNew Balance is {NewBalance}");
            SaveData_FromList_ToFile(lData);
        }
        static void Deposite()
        {
            Console.Clear();
            Console.WriteLine("\t-------------------------------");
            Console.WriteLine("\t\tDeposite Screen : ");
            Console.WriteLine("\t-------------------------------\n");

            string AcountNum;
            Console.Write("\n\tEnter An Acount Number : ");
            AcountNum = Console.ReadLine();
            st_ClientInfo[] info = new st_ClientInfo[1]; 

            while (!isCleintExist_GetIt(AcountNum, info))
            {
                Console.Write($"\nAcount With {AcountNum} Does not Exists\nEnter An Acount Number Again : ");
                AcountNum = Console.ReadLine();
            }

            Print_Client_Card(info[0]);

            int Amount;
            Console.Write("\n\n\tEnter positive Deposite Amount : ");
            Amount = Convert.ToInt32(Console.ReadLine());

            while(Amount < 0)
            {
                Console.Write("\n\tEnter positive Deposite Amount : ");
                Amount = Convert.ToInt32(Console.ReadLine());
            }

            char ans = 'n';
            Console.Write("\nAre You Sure [Y/N] : ");
            ans = Convert.ToChar(Console.ReadLine());

            if (char.ToUpper(ans) == 'Y')
            {
                Increase_And_Decrease_BalanceOfClient(AcountNum, Amount);
                Console.WriteLine("Done.");
            }
        }
        static void WithDraw()
        {
            Console.Clear();
            Console.WriteLine("\t-------------------------------");
            Console.WriteLine("\t\tWithDraw Screen : ");
            Console.WriteLine("\t-------------------------------\n");

            string AcountNum;
            Console.Write("\n\tEnter An Acount Number : ");
            AcountNum = Console.ReadLine();
            st_ClientInfo[] info = new st_ClientInfo[1];

            while (!isCleintExist_GetIt(AcountNum, info))
            {
                Console.Write($"\nAcount With {AcountNum} Does not Exists\nEnter An Acount Number Again : ");
                AcountNum = Console.ReadLine();
            }
            Print_Client_Card(info[0]);

            int Amount;
            Console.Write("\n\n\tEnter positive WithDraw Amount : ");
            Amount = Convert.ToInt32(Console.ReadLine());

            while (Amount < 0)
            {
                Console.Write("\n\tEnter positive WithDraw Amount : ");
                Amount = Convert.ToInt32(Console.ReadLine());
            }

            while (Amount > info[0].Balance || Amount < 0)
            {
                Console.Write("\n\tAcount Balance is Less Than Amount,\n\tEnter Again positive WithDraw : ");
                Amount = Convert.ToInt32(Console.ReadLine());
            }

            char ans = 'n';
            Console.Write("\nAre You Sure [Y/N] : ");
            ans = Convert.ToChar(Console.ReadLine());

            if (char.ToUpper(ans) == 'Y')
            {
                Increase_And_Decrease_BalanceOfClient(AcountNum, Amount*(-1));
                Console.WriteLine("Done.");
            }
        }

        static void Print_One_Record_Info_TotalBalances(st_ClientInfo info)
        {
            Console.WriteLine($"\t\t\t{info.AcountNumber}           | {info.Name}            |        {info.Balance}");
        }
        static void TotalBalancesList()
        {
            Console.Clear();

            List<st_ClientInfo> lData = LoadData_FromFile_ToList();
            int TotalBalances = 0;
            for (int i = 0; i<lData.Count; i++)
            {
                TotalBalances += lData[i].Balance;
            }

            Console.WriteLine($"\n\t\t\t\t\t   {lData.Count} Client(s).");
            Console.WriteLine("\t---------------------------------------------------------------------------------");
            Console.WriteLine("\t\tAcount Number :        |  Name :                    |  Balance : ");
            Console.WriteLine("\t---------------------------------------------------------------------------------");

            foreach (st_ClientInfo info in lData)
            {
                Print_One_Record_Info_TotalBalances(info);
            }

            Console.WriteLine("\t---------------------------------------------------------------------------------");
            Console.WriteLine($"\n\t\t\t\tTotal Balances : {TotalBalances}");
            Console.WriteLine("\t---------------------------------------------------------------------------------");
        }

        static void Working_Transactions_Menu(en_TransationsMenu_Options option)
        {
            switch(option)
            {

                case en_TransationsMenu_Options.e_GoBack:
                    MainMenu();
                    break;


                case en_TransationsMenu_Options.e_Deposite:
                    Deposite();
                    system_pause();
                    Transactions_Menu();
                    break;


                case en_TransationsMenu_Options.e_WithDraw:
                    WithDraw();
                    system_pause();
                    Transactions_Menu();
                    break;


                case en_TransationsMenu_Options.e_TotalBalances:
                    TotalBalancesList();
                    system_pause();
                    Transactions_Menu();
                    break;


                default:
                    Console.WriteLine("\nWrong Choice.\n");
                    break;
            }
        }

        static void Transactions_Menu()
        {
            Console.Clear();
            Console.WriteLine("\t\t\t  Version 1 C#\n\n");
            Console.WriteLine("\t=================================================");
            Console.WriteLine("\t\t   Transactions Menu Screen :");
            Console.WriteLine("\t=================================================");
            Console.WriteLine("\t\t      [1] : Deposite.");
            Console.WriteLine("\t\t      [2] : WithDarw.");
            Console.WriteLine("\t\t      [3] : Total Balances.");
            Console.WriteLine("\t\t      [4] : Go Back To Main Menu.");
            Console.WriteLine("\t=================================================");

            Working_Transactions_Menu(Read_TransationsMenu_Option());
        }
        /////



        static void Main(string[] args)
        {

            MainMenu();



            Console.ReadKey();
        }
    }
}
