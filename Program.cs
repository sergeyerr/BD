using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace BD
{
    class DB
    {
        private readonly char[] columns = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

        private List<string[]> db;

        private readonly bool[] isInt;

        private bool IndexCheck(int i)
        {
            return i >= 0 && i < db.Count;
        }

        private bool Create(string[] command)
        {
            Console.WriteLine("Enter Data");
            string[] data = Console.ReadLine().Split();
            if (data.Length != 7)
            {
                Console.WriteLine("Wrong Data Size");
                return false;
            }
            for (int i = 0; i < 7; i++)
            {
                if (isInt[i])
                {
                    int tmp;
                    if (int.TryParse(data[i].ToString(), out tmp))
                    {
                        continue;
                    }
                    Console.WriteLine("Wrong Data Format");
                    return false;
                }
            }
            if (command.Length == 1)
            {
                db.Add(data);
                return true;
            }
            int insertIndex;
            if (command.Length == 2 && int.TryParse(command[1], out insertIndex) && IndexCheck(insertIndex + 1))
            {
                db.Insert(insertIndex + 1, data);
                return true;
            }
            return false;
        }

        private bool Read(string[] command)
        {
            if (command.Length == 1)
            {
                for (int i = 0; i < db.Count;i++)
                {
                    Console.Write(i + ") ");
                    foreach (var word in db[i]) Console.Write(word + " ");
                    Console.Write("\n");

                }
                return true;
            }
            int readIndex;
            if (command.Length == 2 && int.TryParse(command[1], out readIndex) && IndexCheck(readIndex))
            {
                foreach (var word in db[readIndex]) Console.Write(word + " ");
                Console.Write("\n");
                return true;
            }
            return false;
        }

        private bool Update(string[] command)
        {
            int updateRowIndex;
            if (command.Length == 3 && int.TryParse(command[1], out updateRowIndex) && columns.Contains(command[2][0]) && IndexCheck(updateRowIndex))
            {
                if (!isInt[command[2][0] - 'A']) return false;
                Console.WriteLine("Enter Data");
                int updateColumnIndex = command[2][0] - 'A';
                string tmp = Console.ReadLine();
                if (isInt[updateColumnIndex])
                {
                    int tmpInt;
                    if (!int.TryParse(tmp, out tmpInt))
                    {
                        Console.WriteLine("Wrong Format");
                        return false;
                    }
                }
                db[updateRowIndex][command[2][0] - 'A'] = tmp;
                return true;
            }
            return false;
        }

        private bool Delete(string[] command)
        {
            int deleteIndex;
            if (command.Length == 2 && int.TryParse(command[1], out deleteIndex) && IndexCheck(deleteIndex))
            {
                db.RemoveAt(deleteIndex);
                return true;
            }
            return false;

        }

        private bool Stats(string[] command)
        {
            bool isThereSmthToPrint = false;
            for (int j = 0; j < 7; j++)
            {
                int sum = 0;
                if (isInt[j])
                {
                    isThereSmthToPrint = true;
                    for (int i = 0; i < db.Count; i++)
                    {
                        sum += int.Parse(db[i][j]);  
                    }
                    Console.WriteLine((char)('A' + j) + ") sum: " + sum + " average: " + ((double)sum) / db.Count);
                }
            }
            if (!isThereSmthToPrint) Console.WriteLine("Nothing to print  :((");
            return true;
        } 

        public DB()
        {
            db = new List<string[]>();
            isInt = new bool[7];
        }

        public DB(string fileName)
        {
            db = new List<string[]>();
            isInt = new bool[7];
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File not found");
                return;
            }
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var tokens = line.Split('\t').Skip(1).ToArray();
                    db.Add(tokens);
                }
            }

            for (int j = 0; j < 7; j++)
            {
                bool flag = true;
                for (int i = 0; i < db.Count; i++)
                {
                    int tmp;
                    if (!(int.TryParse(db[i][j], out tmp)))
                    {
                        flag = false;
                        break;
                    }
                }
                isInt[j] = flag;
            }
        }

        public void OpyatRabotat()
        {
            Console.WriteLine("Welcome! Command List:\ncreate\ncreate (id)\nread\nread (id)\nupdate (id) (column letter from A to G)\ndelete (id)\nstats\nexit");
            Dictionary<string, Func<string[], bool>> commandDictionary = new Dictionary<string, Func<string[], bool>>()
            {
                {"create", Create }, {"read", Read}, {"update", Update}, {"delete", Delete }, {"stats", Stats}
            };
            while (true)
            {
                Console.WriteLine("Enter Command");
                string[] command = Console.ReadLine().Split(' ');
                if (command.Length == 0)
                {
                    Console.WriteLine("Wrong Command");
                    continue;
                }
                command[0] = command[0].ToLower();
                bool res = false;
                if (commandDictionary.ContainsKey(command[0])) res = commandDictionary[command[0]](command);
                if (command[0] == "exit")
                {
                    SaveToFile("default.tsv");
                    break;
                }
                if (!res) Console.WriteLine("Wrong Command");
            }
        }

        public void SaveToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < db.Count; i++)
                {
                    writer.Write(i.ToString() + '\t');
                    foreach (var word in db[i])
                        writer.Write(word.ToString() + '\t');
                    writer.Write('\n');
                }
            }
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            var test = new DB("default.tsv");
            test.OpyatRabotat();
        }
    }
}
