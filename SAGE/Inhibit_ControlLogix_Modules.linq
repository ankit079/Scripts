<Query Kind="Program" />

void Main()
{
	// File Path to the L5X file
	string xmlFile = @"C:\Users\ankit.shah\Desktop\Stage 2\FF2_PLC1AND3_12082021.L5X";

	// Retrieve XML Document
	XDocument root = XDocument.Load(xmlFile);

	IEnumerable<XElement> result =
	from el in root.Descendants("Module")
	//		where
	//		el.Value.Contains(singleMapping.oldAliasArray)
	select el;
	//result.Dump();
	foreach (var module in result)
	{
		//string newText = module.Value.Replace(singleMapping.oldAliasArray, singleMapping.baseArray);
		module.Attribute("Inhibited").Value = "true";
		root.Save(xmlFile);
	}
}

// Define other methods and classes here
