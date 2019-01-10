using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace BD
{
    class Program
    {
        static readonly char[] Columns = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
        static void Main(string[] args)
        {
            List<string[]> db = new List<string[]>();
            try
            {
                using (StreamReader sr = new StreamReader("db.tsv"))
                {
                    string line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        var tokens = line.Split('\t').Skip(1).ToArray();
                        db.Add(tokens);
                    }
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
            bool work = true;
            while (work)
            {
                string[] command = Console.ReadLine().Split(' ');
                if (command.Length == 0)
                {
                    Console.WriteLine("Wrong Command");
                    continue;
                }
                command[0].ToLower();
                try
                {
                    switch (command[0])
                    {
                        case "create":
                            var data = Console.ReadLine().Split();
                            if (data.Length != 7)
                            {
                                Console.WriteLine("Wrong Data");
                                continue;
                            }
                            if (command.Length == 1)
                            {
                                db.Add(data);
                                continue;
                            }
                            int insertIndex;
                            if (command.Length == 2 && int.TryParse(command[1], out insertIndex))
                            {
                                db.Insert(insertIndex, data);
                                continue;
                            }
                            Console.WriteLine("Wrong Command");
                            break;
                        case "read":
                            if (command.Length == 1)
                            {
                                foreach (var tmp in db)
                                {
                                    foreach (var word in tmp) Console.Write(word + " ");
                                    Console.Write("\n");

                                }
                                continue;
                            }
                            int readIndex;
                            if (command.Length == 2 && int.TryParse(command[1], out readIndex))
                            {
                                foreach (var word in db[readIndex]) Console.Write(word + " ");
                                Console.Write("\n");
                                continue;
                            }
                            Console.WriteLine("Wrong Command");
                            break;
                        case "update":
                            int updateIndex;
                            if (command.Length == 3 && int.TryParse(command[1], out updateIndex) && Columns.Contains(command[3][0]))
                            {
                                db[updateIndex][command[3][0] - 'A'] = Console.ReadLine();
                                continue;
                            }
                            Console.WriteLine("Wrong Command");
                            break;
                        case "delete":
                            int deleteIndex;
                            if (command.Length == 2 && int.TryParse(command[1], out deleteIndex))
                            {
                                db.RemoveAt(deleteIndex);
                                continue;
                            }
                            break;
                        case "exit":
                            work = false;
                            break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Index out of range");
                }
            }
            using (var )
        }
    }
}
