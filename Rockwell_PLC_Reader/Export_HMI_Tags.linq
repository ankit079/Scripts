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
	string xmlFile = @"C:\Users\shaha\Desktop\LoadXML\1620_Hydro_Glauber_Salt_Chillers.xml";
	XElement config = XElement.Load(xmlFile);
	
	var result = (
			from c in config.DescendantNodes().OfType<XElement>()
			where c.Name == "parameter"
			select c
				);
	
	foreach(XElement e in result)
	{
		if(e.FirstAttribute.Value == "#102")
		{
			e.LastAttribute.Value.Dump();			
		}
	}
}