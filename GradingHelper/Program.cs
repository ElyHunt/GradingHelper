using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
//using System.Windows.Forms;



namespace GradingHelper
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello! Please enter the directory which you'd like to unzip.\n\n");
            string directoryName = Console.ReadLine();

            bool DSA = isDSA();
            //DSA stands for Data Structures and Algorithms and is a class I commonly grade for.
            //If the class is not DSA, then a more generically formatted grading file is generated later.




            Console.WriteLine("\n\nUnzipping Files...\n\n");

            string[] files = Directory.GetFiles(directoryName, "*.zip", SearchOption.AllDirectories);
            if (files.Length <= 0)
            {
                Console.WriteLine("No files detected! Sorry! :)");
                Environment.Exit(0); 
            }




           
            List<string> zips = new List<string>();



            int formerZipCount = 0;
            do
            {
                formerZipCount = zips.Count;

                foreach (string file in files)
                {
                    if ((file.EndsWith(".zip") || file.EndsWith(".Zip") || file.EndsWith(".ZIP")) && !zips.Contains(file))
                    {
                        zips.Add(file);
                        Directory.CreateDirectory(file.Remove(file.Length - 4));
                        ZipFile.ExtractToDirectory(file, file.Remove(file.Length - 4));
                    }
                }
                files = Directory.GetFiles(directoryName, "*.zip", SearchOption.AllDirectories);

            } while (formerZipCount < zips.Count);
            
            

            Console.WriteLine("...Done!\n\n");

            Console.WriteLine("Creating grades.txt...");


            string gradesDotText = directoryName + "\\grades.txt";
            FileStream fs = File.Create(gradesDotText);

            

            StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);

            foreach(string zip in zips)
            {
                sw.WriteLine(zip + " : --/100");
                sw.WriteLine(GenerateGradingBody(DSA));
                sw.WriteLine();
                sw.WriteLine("|_*~-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-~*_|\n\n\n");
                sw.Flush();
            }
            sw.Close();


            Console.WriteLine("\n\n Thanks! Everything was created and should be ready for grading! :) Let me just move all the .zips to one place for you...");

            Directory.CreateDirectory(directoryName + "\\zips");

            int nameCollisions = 0;
            foreach (string zip in zips)
            {
                try
                {
                    File.Move(zip, directoryName + "\\zips\\" + Path.GetFileName(zip));
                }
                catch (IOException e)
                {
                    nameCollisions++;
                    Console.WriteLine("Error Found: " + e.ToString() + "\nProbably there were two files named the same thing... Attempting to rename.");
                    Console.WriteLine("This has happened " + nameCollisions + " times.");
                    File.Move(zip, directoryName + "\\zips\\(" + nameCollisions + ")" + Path.GetFileName(zip));
                }
            }


            // Directory.CreateDirectory(directoryName + "\\Headers"); uncomment if broken0

            if (DSA)
            {
                Console.WriteLine("\n\n Printing .h file paths...\n\n");
                string[] headerFiles = Directory.GetFiles(directoryName, "*.h", SearchOption.AllDirectories);
                Console.WriteLine();
                foreach(string headerFile in headerFiles)
                {
                    headerFile.Substring(directoryName.Length + 1);
                    Console.WriteLine(headerFile.Replace('\\', 'x'));
                    //File.Copy(headerFile, directoryName + "\\Headers\\" + headerFile.Replace('\\', 'x')); uncomment if broken
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n\n All done! Thanks again! :D\n\n");


        }

        public static bool isDSA()
        {//Asks user whether or not the class they're grading for is DSA.
            ConsoleKey key;
            do
            {
                Console.WriteLine("Is this for DSA? (This affects the grades.txt generation.) Y or N. :) Q to Quit.");
                key = Console.ReadKey().Key;
            }

            while (key!= ConsoleKey.Q && key != ConsoleKey.Y && key != ConsoleKey.N);

            if (key == ConsoleKey.Q) Environment.Exit(0);
            if (key == ConsoleKey.Y) return true;
            return false;
        }

        public static string GenerateGradingBody(bool dsa)
        {

            string gradingBody = "";
            if (dsa)
                gradingBody = "\nCompiler Warnings? \n Memory Leaks? \n Code Passes All Tests? \n Coding Style/Comments: \n\n\n Grade: --/100\n";
            else
                gradingBody = "\n\nGrading Rubric: \n\n\n\n\n Grade: --/100\n";
            return gradingBody;
        }


    }

}


