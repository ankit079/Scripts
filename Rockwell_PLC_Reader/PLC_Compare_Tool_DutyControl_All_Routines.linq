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
	// Compare Logic between two files
	
	var difference1 = RetrieveLatestFileData().Except(RetrievePreviousFileData()).ToList();
	var difference2 = RetrievePreviousFileData().Except(RetrieveLatestFileData()).ToList();
	Console.WriteLine("Comparing the latest file with the previous file for difference");
	difference1.Dump();
	Console.WriteLine("Comparing the previous file with the latest file for difference");
	difference2.Dump();
}

public List<String> RetrieveLatestFileData()
{	var latestData = new List<String>();	
	// Latest File Data
	string filename = @"LoadXML\PLC_Compare_Tool\HYDR1A_SB_20191028.L5X";
	string xmlFile = FilePathFromDesktop(filename);
	XElement config = XElement.Load(xmlFile);

	var result = (
			from c in config.DescendantNodes().OfType<XElement>()
			where c.Name == "Program"
			select c
				);

	foreach (XElement e in result)
	{
		if (e.FirstAttribute.Value == "DutyControl")
		{
			var selectedProgram = e;
			var routines = (
		from c in selectedProgram.DescendantNodes().OfType<XElement>()
		where c.Name == "Routine"
		select c
			);

			foreach (var routine in routines)
			{
				Console.WriteLine("Completed routine checks for {0}",routine.FirstAttribute.Value);

			if (routine.Attribute("Type").Value == "FBD")
				{
					var deserializedObject = Deserializer.FromXElement<Routine>(routine);

					// Retrieve IREF data
					foreach (var iref in deserializedObject.FBDContent.Sheet.IRef)
					{
						latestData.Add(iref.Operand);
					}
					// Retrieve Block data
					foreach (var block in deserializedObject.FBDContent.Sheet.Block)
					{
						latestData.Add(block.Operand);
					}
				}
			}
		}
		
	}
	return latestData;
}

public List<String> RetrievePreviousFileData()
{
	var previousData = new List<String>();
	// Latest File Data
	string filename = @"LoadXML\PLC_Compare_Tool\HYDR1A_BHG_20191003.L5X";
	string xmlFile = FilePathFromDesktop(filename);
	XElement config = XElement.Load(xmlFile);

	var result = (
			from c in config.DescendantNodes().OfType<XElement>()
			where c.Name == "Program"
			select c
				);

	foreach (XElement e in result)
	{
		if (e.FirstAttribute.Value == "DutyControl")
		{
			var selectedProgram = e;
			var routines = (
		from c in selectedProgram.DescendantNodes().OfType<XElement>()
		where c.Name == "Routine"
		select c
			);

			foreach (var routine in routines)
			{

			if (routine.Attribute("Type").Value == "FBD")
				{
					var deserializedObject = Deserializer.FromXElement<Routine>(routine);

					// Retrieve IREF data
					foreach (var iref in deserializedObject.FBDContent.Sheet.IRef)
					{
						previousData.Add(iref.Operand);
					}
					// Retrieve Block data
					foreach (var block in deserializedObject.FBDContent.Sheet.Block)
					{
						previousData.Add(block.Operand);
					}
				}
			}
		}
	}
	return previousData;
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

[XmlRoot(ElementName = "IRef")]
public class IRef
{
	[XmlAttribute(AttributeName = "ID")]
	public string ID { get; set; }
	[XmlAttribute(AttributeName = "X")]
	public string X { get; set; }
	[XmlAttribute(AttributeName = "Y")]
	public string Y { get; set; }
	[XmlAttribute(AttributeName = "Operand")]
	public string Operand { get; set; }
	[XmlAttribute(AttributeName = "HideDesc")]
	public string HideDesc { get; set; }
}

[XmlRoot(ElementName = "ICon")]
public class ICon
{
	[XmlAttribute(AttributeName = "ID")]
	public string ID { get; set; }
	[XmlAttribute(AttributeName = "X")]
	public string X { get; set; }
	[XmlAttribute(AttributeName = "Y")]
	public string Y { get; set; }
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
}

[XmlRoot(ElementName = "OCon")]
public class OCon
{
	[XmlAttribute(AttributeName = "ID")]
	public string ID { get; set; }
	[XmlAttribute(AttributeName = "X")]
	public string X { get; set; }
	[XmlAttribute(AttributeName = "Y")]
	public string Y { get; set; }
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
}

[XmlRoot(ElementName = "Block")]
public class Block
{
	[XmlAttribute(AttributeName = "Type")]
	public string Type { get; set; }
	[XmlAttribute(AttributeName = "ID")]
	public string ID { get; set; }
	[XmlAttribute(AttributeName = "X")]
	public string X { get; set; }
	[XmlAttribute(AttributeName = "Y")]
	public string Y { get; set; }
	[XmlAttribute(AttributeName = "Operand")]
	public string Operand { get; set; }
	[XmlAttribute(AttributeName = "VisiblePins")]
	public string VisiblePins { get; set; }
	[XmlAttribute(AttributeName = "HideDesc")]
	public string HideDesc { get; set; }
}

[XmlRoot(ElementName = "AddOnInstruction")]
public class AddOnInstruction
{
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
	[XmlAttribute(AttributeName = "ID")]
	public string ID { get; set; }
	[XmlAttribute(AttributeName = "X")]
	public string X { get; set; }
	[XmlAttribute(AttributeName = "Y")]
	public string Y { get; set; }
	[XmlAttribute(AttributeName = "Operand")]
	public string Operand { get; set; }
	[XmlAttribute(AttributeName = "VisiblePins")]
	public string VisiblePins { get; set; }
}

[XmlRoot(ElementName = "Wire")]
public class Wire
{
	[XmlAttribute(AttributeName = "FromID")]
	public string FromID { get; set; }
	[XmlAttribute(AttributeName = "ToID")]
	public string ToID { get; set; }
	[XmlAttribute(AttributeName = "ToParam")]
	public string ToParam { get; set; }
	[XmlAttribute(AttributeName = "FromParam")]
	public string FromParam { get; set; }
}

[XmlRoot(ElementName = "TextBox")]
public class TextBox
{
	[XmlElement(ElementName = "Text")]
	public string Text { get; set; }
	[XmlAttribute(AttributeName = "ID")]
	public string ID { get; set; }
	[XmlAttribute(AttributeName = "X")]
	public string X { get; set; }
	[XmlAttribute(AttributeName = "Y")]
	public string Y { get; set; }
	[XmlAttribute(AttributeName = "Width")]
	public string Width { get; set; }
}

[XmlRoot(ElementName = "Sheet")]
public class Sheet
{
	[XmlElement(ElementName = "Description")]
	public string Description { get; set; }
	[XmlElement(ElementName = "IRef")]
	public List<IRef> IRef { get; set; }
	[XmlElement(ElementName = "ICon")]
	public List<ICon> ICon { get; set; }
	[XmlElement(ElementName = "OCon")]
	public List<OCon> OCon { get; set; }
	[XmlElement(ElementName = "Block")]
	public List<Block> Block { get; set; }
	[XmlElement(ElementName = "AddOnInstruction")]
	public List<AddOnInstruction> AddOnInstruction { get; set; }
	[XmlElement(ElementName = "Wire")]
	public List<Wire> Wire { get; set; }
	[XmlElement(ElementName = "TextBox")]
	public List<TextBox> TextBox { get; set; }
	[XmlAttribute(AttributeName = "Number")]
	public string Number { get; set; }
}

[XmlRoot(ElementName = "FBDContent")]
public class FBDContent
{
	[XmlElement(ElementName = "Sheet")]
	public Sheet Sheet { get; set; }
	[XmlAttribute(AttributeName = "SheetSize")]
	public string SheetSize { get; set; }
	[XmlAttribute(AttributeName = "SheetOrientation")]
	public string SheetOrientation { get; set; }
}

[XmlRoot(ElementName = "Routine")]
public class Routine
{
	[XmlElement(ElementName = "Description")]
	public string Description { get; set; }
	[XmlElement(ElementName = "FBDContent")]
	public FBDContent FBDContent { get; set; }
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
	[XmlAttribute(AttributeName = "Type")]
	public string Type { get; set; }

}

public class Compare
{
	public string PreviousData {get;set;}
	public string LatestData {get;set;}
}