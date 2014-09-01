using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace LPMain
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Args[0] : DestinationXML
                //Args[1] : TaxonomyXML
                //Args[2] : OutputDir
                
                Console.WriteLine(System.DateTime.Now+" Reading arg1: destination xml path, arg2: taxonomy xml path and arg3: output directory path.");
                
                //Temporary fix
                //args = new string[3];
                //args[0] = @"c:\users\ajay\downloads\lonely_planet_coding_exercise\lonely_planet_coding_exercise\destinations.xml";
                //args[1] = @"c:\users\ajay\downloads\lonely_planet_coding_exercise\lonely_planet_coding_exercise\taxonomy.xml";
                //args[2] = @"c:\users\ajay\documents\lonelyplanet\";

                if (args.Length != 0 && args[0] != null && args[1] != null && args[2] != null)
                {

                    if (File.Exists(args[0]) && File.Exists(args[1]) && Directory.Exists(args[2]))
                    {
                        Console.WriteLine("Files found. Reading files...");

                        //Read and Push the xml data to controller
                        FileGenerationModule.FileGenerationController controller = new FileGenerationModule.FileGenerationController(XDocument.Load(args[0]).ToString(), XDocument.Load(args[1]).ToString(), args[2]);
                    }
                    else
                    {
                        Console.WriteLine("There are no files/directory at given location.");
                        
                    }
                }
                else
                {
                    Console.WriteLine("You haven't given all required params.");                    
                }
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: "+ ex.Message);
                Console.ReadLine();
            }
        }
    }
}
