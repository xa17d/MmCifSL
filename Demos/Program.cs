using PdbXReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = File.ReadAllText("1hho.cif"); // downloaded from http://www.rcsb.org/pdb/explore.do?structureId=1HHO (PDBx/mmCIF Format9

            using (var reader = new ModelReader())
            {
                var entry = reader.ReadEntry(file);

                foreach (var atomSite in entry.AtomSites)
                {
                    Console.WriteLine(
                        atomSite.Symbol + " Position: " + atomSite.X + "/" + atomSite.Y + "/" + atomSite.Z
                    );
                }
            }

            Console.ReadKey();
        }
    }
}
