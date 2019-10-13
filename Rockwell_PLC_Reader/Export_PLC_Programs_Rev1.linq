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
	string xmlFile = @"C:\Users\shaha\Desktop\LoadXML\PLC File\CRYS12B_AZ_20191010.L5X";
	XElement config = XElement.Load(xmlFile);

	var result = (
			from c in config.DescendantNodes().OfType<XElement>()
			where c.Name == "Program"
			select c
				);

	foreach (XElement e in result)
	{
		if (e.FirstAttribute.Value == "AIn_1000ms_167x")
		{
			var selectedProgram = e;
			var routines = (
		from c in selectedProgram.DescendantNodes().OfType<XElement>()
		where c.Name == "Routine"
		select c
			);
			foreach (var routine in routines)
			{
				if (routine.FirstAttribute.Value == "PI167100")
				{
					routine.Dump();
					var deserializedObject = Deserializer.FromXElement<Routine>(routine);
					deserializedObject.Dump();					
				}

			}
		}
	}
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
/* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0
    */

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