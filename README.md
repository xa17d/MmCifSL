# MmCifSL
C# Library for reading PDBx/mmCIF files (http://mmcif.wwpdb.org/)

The MmCifSL Library can be used to read CIF files.
The PdbXReader Library (based on MmCifSL) can be used to read AtomSites (atoms and their elements, positions, etc.), Entities and Asymmetric Units out of .cif files.

Example usage: read all atom symbol and position from .cif file:
```cs
using System;
using System.IO;
using PdbXReader;

...

string file = File.ReadAllText("1hho.cif"); // downloaded from http://www.rcsb.org/pdb/explore.do?structureId=1HHO (PDBx/mmCIF Format)

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
```