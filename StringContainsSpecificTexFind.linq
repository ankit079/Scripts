<Query Kind="Program" />

void Main()
{
	// string type 
	String[] str = { @"C:\Dev\#28_ABC.xlsx", @"C:\Dev\#29_ABC.xlsx"};
	String[] phase2Sites = {"28","29","30","31"};
	bool found = false;
	
	// using String.Contains() Method 
	foreach(var filePath in str)
	{
		foreach (var sites in phase2Sites)
		{
			found = (filePath.Contains(sites));
			Console.WriteLine($"{(found == true ? "Found" : "Did not find")} {sites}");
			found = false;
		}
	}
	int i = 29;
	string stri = i.ToString();
	Console.Write(stri);
}