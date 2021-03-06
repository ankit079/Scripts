<Query Kind="Program">
  <Connection>
    <ID>e0e1a2f9-3e46-474d-8a95-a03f59555e6e</ID>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <Database>master</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	string filename = @"LoadXML\PLC File\CRYS12B_AZ_20191010.L5X";
	string xmlFile = FilePathFromDesktop(filename);
	XElement config = XElement.Load(xmlFile);

	var result = (
			from c in config.DescendantNodes().OfType<XElement>()
			where c.Name == "Program"
			select c
				);

	foreach (XElement e in result)
	{
		if (e.FirstAttribute.Value == "COM_Ethernet_PLC")
		{
			var selectedProgram = e;
			var routines = (
		from c in selectedProgram.DescendantNodes().OfType<XElement>()
		where c.Name == "Routine"
		select c
			);
			foreach (var routine in routines)
			{				
				var deserializedObject = Deserializer.FromXElement<Routine>(routine);
				var rung = deserializedObject.RLLContent.Rung;
				
				foreach(var text in rung)
				{
					text.Text.Dump();
				}			
	
			}
		}
	}
}

// Read from desktop a perticular file
private string FilePathFromDesktop(string fileName)
{
	string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	string fullName = System.IO.Path.Combine(desktopPath, fileName);
	return fullName;
}

// https://stackoverflow.com/questions/8373552/serialize-an-object-to-xelement-and-deserialize-it-in-memory 
public static class Deserializer
{
	public static T FromXElement<T>(this XElement xElement)
	{
		var xmlSerializer = new XmlSerializer(typeof(T));
		return (T)xmlSerializer.Deserialize(xElement.CreateReader());
	}
}

// Serialize to XElement - NOT USED --
public static class Serializer
{
	public static XElement ToXElement<T>(this object obj)
	{
		using (var memoryStream = new MemoryStream())
		{
			using (TextWriter streamWriter = new StreamWriter(memoryStream))
			{
				var xmlSerializer = new XmlSerializer(typeof(T));
				xmlSerializer.Serialize(streamWriter, obj);
				return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
			}
		}
	}
}

// DTO Object Parent Object is Routine

[XmlRoot(ElementName = "Rung")]
public class Rung
{
	[XmlElement(ElementName = "Comment")]
	public string Comment { get; set; }
	[XmlElement(ElementName = "Text")]
	public string Text { get; set; }
	[XmlAttribute(AttributeName = "Number")]
	public string Number { get; set; }
	[XmlAttribute(AttributeName = "Type")]
	public string Type { get; set; }
}

[XmlRoot(ElementName = "RLLContent")]
public class RLLContent
{
	[XmlElement(ElementName = "Rung")]
	public List<Rung> Rung { get; set; }
}

[XmlRoot(ElementName = "Routine")]
public class Routine
{
	[XmlElement(ElementName = "Description")]
	public string Description { get; set; }
	[XmlElement(ElementName = "RLLContent")]
	public RLLContent RLLContent { get; set; }
	[XmlAttribute(AttributeName = "Use")]
	public string Use { get; set; }
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
	[XmlAttribute(AttributeName = "Type")]
	public string Type { get; set; }
}

[XmlRoot(ElementName = "Routines")]
public class Routines
{
	[XmlElement(ElementName = "Routine")]
	public Routine Routine { get; set; }
	[XmlAttribute(AttributeName = "Use")]
	public string Use { get; set; }
}

[XmlRoot(ElementName = "Program")]
public class Program
{
	[XmlElement(ElementName = "Routines")]
	public Routines Routines { get; set; }
	[XmlAttribute(AttributeName = "Use")]
	public string Use { get; set; }
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
}