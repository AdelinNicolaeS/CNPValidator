using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CNPValidator
{
    public class CNPChecker : IChecker
    {
        
        public string CNP { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Code { get; set; }
        public string Sex { get; set; }
        public bool noYearCase = false;

        private readonly StringBuilder output = new StringBuilder();
        public StringBuilder GetOutput()
        {
            return output;
        }

        public CNPChecker(string CNP)
        {
            this.CNP = CNP;
        }
        
        public int CalculateChecksum()
        {
            int[] array1 = new int[] { 2,7,9,1,4,6,3,5,8,2,7,9 };
            int sum = 0;
            for (int i = 0; i < array1.Length; i++)
            {
                int ch = CNP[i] - '0';
                sum += ch * array1[i];
            }
           
            sum %= 11;
            
            if (sum == 10)
            {
                sum = 1;
            }
            return sum;

        }
        public bool Check13()
        {
            int sum = CalculateChecksum();
            if (sum.ToString().Equals(CNP[^1].ToString()))
            {
                
                return true;
            }
            output.Clear();
            output.Append("Cifra de control gresita\n");
            return false;
        }
        public bool Check10and11and12()
        {
            string cut = CNP.Substring(9, 3);
            if (!Int32.TryParse(cut, out int j))
            {
                output.Clear();
                output.Append("Codul de 3 cifre gresit\n");
                return false;
            }
            if (j >= 1 && j <= 999)
            {
                return true;
            }
            output.Clear();
            output.Append("Codul de 3 cifre gresit\n");
            return false;
        }

        public bool Check2and3()
        {
            // anul
            string cut = CNP.Substring(1, 2);
            if (!Int32.TryParse(cut, out int j))
            {
                output.Clear();
                output.Append("An gresit\n");
                return false;
            }
            Year += cut;
            return true;
        }

        public bool Check4and5()
        {
            // luna
            string cut = CNP.Substring(3, 2);
            
            if (cut.Equals("01") || cut.Equals("02") || cut.Equals("03") || cut.Equals("04") || cut.Equals("05")
                || cut.Equals("06") || cut.Equals("07") || cut.Equals("08") || cut.Equals("09") || cut.Equals("10")
                || cut.Equals("11") || cut.Equals("12"))
            {
                Month = cut;
                return true;
            }
            output.Clear();
            output.Append("Luna gresita\n");
            return false;
        }

        public bool Check6and7()
        {
            // ziua
            string cut = CNP.Substring(5, 2);
            if (!Int32.TryParse(cut, out int j))
            {
                output.Clear();
                output.Append("Ziua gresita\n");
                return false;
            }
            if (j > 30)
            {
                output.Clear();
                output.Append("Ziua gresita\n");
                return false;
            }
            Day = j.ToString();
            if (!noYearCase) // sa nu fie rezidenti (lor nu le stim intervalul nasterii)
            {
                string completeDay = Month + "/" + Day + "/" + Year;
                bool newCheck = Program.IsDate(completeDay);
                if (!newCheck)
                {
                    output.Clear();
                    output.Append("Data nasterii e invalida\n");
                }
            }
            
            return true;
        }
        public string TakeCounty(string code)
        {
            StreamReader reader = File.OpenText("judete.txt");
            string line;
            // pe prima linie avem codul, pe a doua judetul
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                if (items[0].Equals(code))
                {
                    return items[1];
                }
            }
            return "fail";
        }
        public bool Check8and9()
        {
            string cut = CNP.Substring(7, 2);
            string aux = TakeCounty(cut);
            if (aux.Equals("fail"))
            {
                output.Clear();
                output.Append("Cod de judet gresit\n");
                return false;
            }
            Code = aux;
            return true;
        }

        public bool CheckFirstDigit()
        {
            char c1 = CNP[0];
            if (c1.Equals('1'))
            {
                Year = "19";
                Sex = "M";
            } else if (c1.Equals('2'))
            {
                Year = "19";
                Sex = "F";
            } else if (c1.Equals('3'))
            {
                Year = "18";
                Sex = "M";
            } else if (c1.Equals('4'))
            {
                Year = "18";
                Sex = "F";
            } else if (c1.Equals('5'))
            {
                Year = "20";
                Sex = "M";
            } else if (c1.Equals('6'))
            {
                Year = "20";
                Sex = "F";
            } else if (c1.Equals('7'))
            {
                noYearCase = true;
                Sex = "M";
            } else if (c1.Equals('8'))
            {
                noYearCase = true;
                Sex = "F";
            } else
            {
                output.Clear();
                output.Append("Nu corespunde primul caracter\n");
                return false;
            }
            return true;
        }

        public bool CheckLength()
        {
            if (CNP.Length == 13)
            {
                return true;
            }
            output.Clear();
            output.Append("Lungime incorecta");
            return false;
        }

        public bool CheckAll()
        {
            bool ok = true;
            ok = ok && CheckLength();
            if (!ok)
            {
                return ok;
            }
            ok = ok && CheckFirstDigit();
            if (!ok)
            {
                return ok;
            }
            ok = ok && Check2and3();
            if (!ok)
            {
                return ok;
            }
            ok = ok && Check4and5();
            if (!ok)
            {
                return ok;
            }
            ok = ok && Check6and7();
            if (!ok)
            {
                return ok;
            }
            ok = ok && Check8and9();
            if (!ok)
            {
                return ok;
            }
            ok = ok && Check10and11and12();
            if (!ok)
            {
                return ok;
            }
            ok = ok && Check13();
            if (ok)
            {
                output.Append("Sex: " + Sex + "\n");
                output.Append("Data nasterii: " + Day + "/" + Month + "/" + Year + "\n");
                output.Append("Judet: " + Code);
            }
            return ok;
        }
    }
}
