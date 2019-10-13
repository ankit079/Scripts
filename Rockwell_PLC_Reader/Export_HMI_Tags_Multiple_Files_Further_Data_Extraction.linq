<Query Kind="Program">
  <Connection>
    <ID>e0e1a2f9-3e46-474d-8a95-a03f59555e6e</ID>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <Database>master</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	string folderPath = @"C:\Users\shaha\Desktop\LoadXML\4140_HMI File";
	foreach (var file in Directory.EnumerateFiles(folderPath, "*.xml"))
	{
		Console.WriteLine("*****************************************************");
		Console.WriteLine(file);
		Console.WriteLine("*****************************************************");
		XElement config = XElement.Load(file);
		var result = (
				from c in config.DescendantNodes().OfType<XElement>()
				where c.Name == "parameter"
				select c
					);
		//result.Dump();

		foreach (XElement e in result)
		{
			if(e.FirstAttribute.Value == "#102")
			{
				e.LastAttribute.Value.Dump();
			}
			if(e.FirstAttribute.Value == "#150")
			{
				e.LastAttribute.Value.Dump();
			}
		}			
	}
}
