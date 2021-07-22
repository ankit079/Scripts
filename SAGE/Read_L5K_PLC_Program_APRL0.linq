<Query Kind="Program">
  <Connection>
    <ID>d49b1367-135a-4276-ae42-a67c56f45126</ID>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <Database>master</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	// File Path to the L5X file
	string xmlFile = @"C:\Users\ankit.shah\Desktop\BHP Kwinana Work\XML\APRL0.L5X";
	
	// Requires a text based mapped data
	var ioMapping = CreatedMapping();
	ioMapping.Dump();
	
	// Retrieve XML Document
	XDocument root = XDocument.Load(xmlFile);
	
	
	// Read existing comments
	string[] tagName = { "I", "O"};
	string dataType = "INT";

	foreach (var name in tagName)
	{		
		var ioTag = ReadDigitalAlias(name, dataType, root);			
		// ioTag.Dump();
		
		// TO DO in the future
		//ProcessComments(root,ioTag,ioMapping);
	}
	
	
	
	// Get correct routine names
	
	// getRoutine(root);
	
	// Process Comments for Digital IO Cards	

	// The below logic ran first so there would be no associattions to run again.
		
	//ObtainInformationOfDigitalIOMapping(root);	
	
	
	foreach (var mapping in ioMapping)
	{
		// Processing Logic
		UpdateAllAssociations(mapping, root);
		FindAndUpdateOtherIOReferences(mapping,root);
	}

	root.Save(xmlFile);

	//	New data capture and display
	
	foreach (var mapping in ioMapping)
	{
		var newResult = CaptureNewTagXMLData(mapping, root);
		// newResult.Dump();
	}
	
}

public void FindAndUpdateOtherIOReferences(ModuleArrayData association, XDocument root)
{
	// For each rung there is a text child with a check required on the text and if the I[27] found, change to the equivalent card number.
	string xmlFile = @"C:\Users\ankit.shah\Desktop\BHP Kwinana Work\XML\APRL0.L5X";

	IEnumerable<XElement> result =
	from el in root.Descendants("Text")
	where
			el.Value.Contains(association.oldAliasArray)

	select el;

	result.Dump();

	foreach (var data in result)
	{
		string newText = data.Value.Replace(association.oldAliasArray, association.baseArray);
		data.Value = newText;
		root.Save(xmlFile);
	}
}

public List<ModuleArrayData> CreatedMapping()
{
	var mapping = new List<ModuleArrayData>();
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[11]", baseArray = "Local:10:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[12]", baseArray = "Local:11:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[20]", baseArray = "APRL0_EN2T_R01:1:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[21]", baseArray = "APRL0_EN2T_R01:2:I.Data", module = null});
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[22]", baseArray = "APRL0_EN2T_R01:3:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[23]", baseArray = "APRL0_EN2T_R01:4:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[24]", baseArray = "APRL0_EN2T_R01:5:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[25]", baseArray = "APRL0_EN2T_R01:6:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[26]", baseArray = "APRL0_EN2T_R01:7:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[27]", baseArray = "APRL0_EN2T_R01:8:I.Data", module = null});
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[30]", baseArray = "APRL0_EN2T_R01:9:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[31]", baseArray = "APRL0_EN2T_R01:10:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[32]", baseArray = "APRL0_EN2T_R01:11:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[33]", baseArray = "APRL0_EN2T_R01:12:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[34]", baseArray = "APRL0_EN2T_R01:13:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[35]", baseArray = "APRL0_EN2T_R01:14:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[36]", baseArray = "APRL0_EN2T_R01:15:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[37]", baseArray = "APRL0_EN2T_R01:16:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "I[40]", baseArray = "APRL0_EN2T_R02:1:I.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "O[46]", baseArray = "APRL0_EN2T_R02:7:O.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "O[47]", baseArray = "APRL0_EN2T_R02:8:O.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "O[50]", baseArray = "APRL0_EN2T_R02:9:O.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "O[51]", baseArray = "APRL0_EN2T_R02:10:O.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "O[52]", baseArray = "APRL0_EN2T_R02:11:O.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "O[53]", baseArray = "APRL0_EN2T_R02:12:O.Data", module = null });
	mapping.Add(new ModuleArrayData() { oldAliasArray = "O[54]", baseArray = "APRL0_EN2T_R02:13:O.Data", module = null });
	
	return mapping;
}

public Tag ReadDigitalAlias(string inputName, string dataType, XDocument root)
{
	IEnumerable<XElement> data =
		from el in root.Descendants("Tag")
		where
				(string)el.Attribute("Name").Value == inputName &&
				(string)el.Attribute("DataType").Value == dataType
				select el;	
	
	var deserializedTagObject = Deserializer.FromXElement<Tag>(data.FirstOrDefault());
	return deserializedTagObject;
}

public void UpdateAllAssociations(ModuleArrayData association, XDocument root)
{
		//	Old data capture and display
		
		Console.Write("Old Tag Data");
		var oldResult = CaptureOldTagXMLData(association, root);
		oldResult.Dump();
		
		// Update the data
		
		UpdateXMLData(association, oldResult);	
}

public void UpdateXMLData(ModuleArrayData association, IEnumerable<XElement> oldResult)
{
	foreach (var data in oldResult)
	{
		data.Attribute("AliasFor").Value = data.Attribute("AliasFor").Value.Replace(association.oldAliasArray, association.baseArray);
		data.Dump();
	}	
}

public IEnumerable<XElement> CaptureOldTagXMLData(ModuleArrayData association, XDocument root)
{
	IEnumerable<XElement> result =
	from el in root.Descendants("Tag")
	where
	(string)el.Attribute("TagType").Value == "Alias" && el.Attribute("AliasFor").Value.Contains(association.oldAliasArray)
	select el;
	return result;	
}

public IEnumerable<XElement> CaptureNewTagXMLData(ModuleArrayData association, XDocument root)
{
	IEnumerable<XElement> result =
	from el in root.Descendants("Tag")
	where
	(string)el.Attribute("TagType").Value == "Alias" && el.Attribute("AliasFor").Value.Contains(association.baseArray)
	select el;
	return result;
}

public void ObtainInformationOfDigitalIOMapping(XDocument root)
{
	var moduleInfo = new List<ModuleArrayData>();
	string [] data = null;
	List<string[]> dataList = new List<string[]>();
	IEnumerable<XElement> result =
	from el in root.Descendants("Routine")
	where
			(string)el.Attribute("Name") == "_004_Digital_Mapping"
	select el;
	data.Dump();
	
	var texts = (
	from c in result.DescendantNodes().OfType<XElement>()
	where c.Name == "Text"
	select c);

	texts.Dump();
	
	foreach (var text in texts)
	{
		data = Regex.Split(text.ToString(), "[(-,]+");
		data.Dump();	
	}
}

//public void getTags(IEnumerable<XElement> result)
//{
//	IEnumerable<XElement> tags = null;
//	foreach (XElement e in result)
//	{
//		tags = (
//	from c in e.DescendantNodes().OfType<XElement>()
//	where c.Name == "Tag"
//	select c);
//	}
//	//tags.Dump();
//}
//
public void getRoutine(XDocument root)
{
	IEnumerable<XElement> result = (
	from c in root.Descendants("Routine")
	select c
		);
	foreach (var routine in result)
	{
		if (routine.FirstAttribute.Value.Contains("Digital"))
		{
			routine.Dump();
		}
	}
}


public void ProcessComments(XDocument root, Tag tag, List<ModuleArrayData> mapping)
{
	IEnumerable<XElement> modules =
	from el in root.Descendants("Module")
	//where
	//		(string)el.Attribute("Name").Value == inputName &&
	//		(string)el.Attribute("DataType").Value == dataType
	select el;
	
	foreach(var data in mapping.Take(1)){
		//data.Attribute("Name").Value.Dump();
		foreach (var module in modules)
		{
			if (module.Attribute("Name").Value == data.module)
			{
				//module.Dump();
				// Add comments to the module				
			}
		}
	}
}

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

public class ModuleArrayData
{
	public string oldAliasArray { get; set; }	
	public string baseArray { get; set; }	
	public string module	{get;set;}
}

public class DigialIOData
{
	public string aliasTag { get; set; }
	public string baseTag { get; set; }
	public string comment { get; set; }
}

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
	[XmlElement(ElementName = "RLLContent")]
	public RLLContent RLLContent { get; set; }
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
	[XmlAttribute(AttributeName = "Type")]
	public string Type { get; set; }
	[XmlElement(ElementName = "Description")]
	public string Description { get; set; }
}

[XmlRoot(ElementName = "Routines")]
public class Routines
{
	[XmlElement(ElementName = "Routine")]
	public List<Routine> Routine { get; set; }
}

[XmlRoot(ElementName = "Program")]
public class Program
{
	[XmlElement(ElementName = "Tags")]
	public Tags Tags { get; set; }
	[XmlElement(ElementName = "Routines")]
	public Routines Routines { get; set; }
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
	[XmlAttribute(AttributeName = "TestEdits")]
	public string TestEdits { get; set; }
	[XmlAttribute(AttributeName = "MainRoutineName")]
	public string MainRoutineName { get; set; }
	[XmlAttribute(AttributeName = "Disabled")]
	public string Disabled { get; set; }
	[XmlAttribute(AttributeName = "UseAsFolder")]
	public string UseAsFolder { get; set; }
}

[XmlRoot(ElementName = "Tags")]
public class Tags
{
	[XmlElement(ElementName = "Tag")]
	public List<Tag> Tag { get; set; }
}

[XmlRoot(ElementName = "Tag")]
public class Tag
{
	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }
	[XmlAttribute(AttributeName = "TagType")]
	public string TagType { get; set; }
	[XmlAttribute(AttributeName = "Radix")]
	public string Radix { get; set; }
	[XmlAttribute(AttributeName = "AliasFor")]
	public string AliasFor { get; set; }
	[XmlAttribute(AttributeName = "ExternalAccess")]
	public string ExternalAccess { get; set; }
	[XmlElement(ElementName = "Description")]
	public string Description { get; set; }
	[XmlElement(ElementName = "Data")]
	public string DataType { get; set; }
	[XmlAttribute(AttributeName = "Constant")]
	public string Constant { get; set; }
	[XmlAttribute(AttributeName = "Dimensions")]
	public string Dimensions { get; set; }
	[XmlElement(ElementName = "Comments")]
	public Comments Comments { get; set; }
}

[XmlRoot(ElementName = "Comments")]
public class Comments
{
	[XmlElement(ElementName = "Comment")]
	public List<Comment> Comment { get; set; }
}

[XmlRoot(ElementName = "Comment")]
public class Comment
{
	[XmlAttribute(AttributeName = "Operand")]
	public string Operand { get; set; }
	[XmlText]
	public string Text { get; set; }
}