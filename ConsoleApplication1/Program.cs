using Raml.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            var file = @"C:\Users\Jeroen\Documents\zivver-api\src\api.raml";

            Directory.SetCurrentDirectory(Path.GetDirectoryName(file));

            var parser = new RamlParser().LoadAsync(file).Result;

            Console.WriteLine("The end...");

        }
    }
}
