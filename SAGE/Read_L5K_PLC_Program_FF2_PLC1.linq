<Query Kind="Program">
  <Connection>
    <ID>d49b1367-135a-4276-ae42-a67c56f45126</ID>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <Database>master</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>ExcelDataReader</NuGetReference>
  <NuGetReference>ExcelDataReader.DataSet</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>CsvHelper.Configuration.Attributes</Namespace>
  <Namespace>CsvHelper.Expressions</Namespace>
  <Namespace>CsvHelper.TypeConversion</Namespace>
  <Namespace>ExcelDataReader</Namespace>
  <Namespace>ExcelDataReader.Core.NumberFormat</Namespace>
  <Namespace>ExcelDataReader.Exceptions</Namespace>
  <Namespace>ExcelDataReader.Log</Namespace>
  <Namespace>ExcelDataReader.Log.Logger</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
	// File Path to the L5X file
	string xmlFile = @"C:\Users\ankit.shah\Desktop\Stage 2\FF2_PLC1_03082021.L5X";	
	string filePath = @"C:\Users\ankit.shah\Desktop\Stage 2\75655-REP-XXX_A FF2 PLC1 Conversion & Standardisation Report.xlsx";
	
	// Retrieve XML Document
	XDocument root = XDocument.Load(xmlFile);
		
	// Requires a text based mapped data
	var MappingList = CreatedMapping(root, filePath);
	//MappingList.Dump();
	
	foreach (var singleMapping in MappingList)
	{
	 	// Processing Logic
		//UpdateAllAssociations(singleMapping, root);
		//FindAndUpdateOtherIOReferences(singleMapping,root, xmlFile);
		//root.Save(xmlFile);
	}

	// Read existing comments
	string[] tagName = { "I", "O"};
	string dataType = "INT";

	foreach (var name in tagName)
	{	
		var ioTag = ReadDigitalAlias(name, dataType, root);			
		//ioTag.Dump();		
		foreach (var data in ioTag.Comments.Comment)
		{
			data.Operand = String.Concat(name,data.Operand);
		}
		
		//ioTag.Dump();
		
		foreach (var data in ioTag.Comments.Comment)
		{
			var tagToCompare = data.Operand.Split(new Char[] {'.'})[0];
			
			foreach (var singleMapping in MappingList)
			{
				if (tagToCompare == singleMapping.oldAliasArray)
				{
					var newString = data.Operand.Replace(tagToCompare,singleMapping.baseArray);
					data.Operand = newString;
				}
			}
		}
		
		ioTag.Dump();
		
		foreach(var mappedData in ioTag.Comments.Comment)
		{
			IEnumerable<XElement> result =
					from el in root.Descendants("Tag")
					where
					(string)el.Attribute("AliasFor")?.Value == mappedData?.Operand
			select el;
			//result.Dump();
		}
		// TO DO in the future
		//ProcessComments(root,ioTag,ioMapping);
	}	
	
	// Process Comments for Digital IO Cards
	

	//	New data capture and display
	//foreach (var association in associations)
	//{
	//	var newResult = CaptureNewTagXMLData(association, root);
	//	newResult.Dump();
	//}		
	
}

public void FindAndUpdateOtherIOReferences(ModuleArrayData singleMapping,XDocument root, string xmlFile)
{
	// For each rung there is a text child with a check required on the text and if the I[27] found, change to the equivalent card number.

	IEnumerable<XElement> result =
	from el in root.Descendants("Text")
	where
			el.Value.Contains(singleMapping.oldAliasArray)			
	
	select el;
	
	result.Dump();
	
	foreach (var data in result)
	{
		string newText = data.Value.Replace(singleMapping.oldAliasArray,singleMapping.baseArray);
		data.Value = newText;
		root.Save(xmlFile);
	}	
}

public List<ModuleArrayData> CreatedMapping(XDocument root, string filePath)
{
	var mapping = new List<ModuleArrayData>();
	
	var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);	
	var reader = ExcelReaderFactory.CreateReader(stream);
	DataSet workSheets= reader.AsDataSet();
	DataTable table = reader.AsDataSet().Tables["Mappings"];

	for (int i = 0; i < table.Rows.Count; i++)
	{
		ModuleArrayData singleEntry = new ModuleArrayData();
		singleEntry.oldAliasArray = Convert.ToString(table.Rows[i]["Column0"]);
		singleEntry.baseArray = Convert.ToString(table.Rows[i]["Column1"]);
		singleEntry.module = Convert.ToString(table.Rows[i]["Column2"]);		
		mapping.Add(singleEntry);
	}
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
	//data.Dump();
	var deserializedTagObject = Deserializer.FromXElement<Tag>(data.FirstOrDefault());
	return deserializedTagObject;
}

public void UpdateAllAssociations(ModuleArrayData singleMapping, XDocument root)
{
		//	Old data capture and display
		
		Console.Write("Old Tag Data");
		var oldResult = CaptureOldTagXMLData(singleMapping, root);
		oldResult.Dump();
		
		// Update the data
		
		UpdateXMLData(singleMapping, oldResult);	
}

public void UpdateXMLData(ModuleArrayData singleMapping, IEnumerable<XElement> oldResult)
{
	foreach (var data in oldResult)
	{
		data.Attribute("AliasFor").Value = data.Attribute("AliasFor").Value.Replace(singleMapping.oldAliasArray, singleMapping.baseArray);
		data.Dump();
	}	
}

public IEnumerable<XElement> CaptureOldTagXMLData(ModuleArrayData singleMapping, XDocument root)
{
	IEnumerable<XElement> result =
	from el in root.Descendants("Tag")
	where
	(string)el.Attribute("TagType").Value == "Alias" && el.Attribute("AliasFor").Value.Contains(singleMapping.oldAliasArray)
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

public List<ModuleArrayData> ObtainInformationOfDigitalIOMapping(XDocument root)
{
	var moduleInfo = new List<ModuleArrayData>();
	string [] data = null;
	List<string[]> dataList = new List<string[]>();
	IEnumerable<XElement> result =
	from el in root.Descendants("Routine")
	where
			(string)el.Attribute("Name") == "_004_DigitalIOMapping"
	select el;
	data.Dump();
	
	var texts = (
	from c in result.DescendantNodes().OfType<XElement>()
	where c.Name == "Text"
	select c);

	foreach (var text in texts)
	{
		data = Regex.Split(text.ToString(), "[(-,]+");
		dataList.Add(data);
	}

	foreach (var item in dataList)
	{
		if (item[5].Contains("I.Data"))
		{
			var modArray = new ModuleArrayData();
			modArray.oldAliasArray = item[7];
			modArray.baseArray = item[5];
			moduleInfo.Add((modArray));
		}

		else
		{
			var modArray = new ModuleArrayData();
			modArray.oldAliasArray = item[5];
			modArray.baseArray = item[7];
			moduleInfo.Add((modArray));
		}
	}

	moduleInfo.Dump();
	return moduleInfo;
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
//public XElement getRoutine(IEnumerable<XElement> result,  string routineName)
//{
//	XElement routineNode = null;
//	foreach (XElement e in result)
//	{
//		if (e.FirstAttribute.Value == "MainProgram")
//		{
//			var selectedProgram = e;
//			var routines = (
//		from c in selectedProgram.DescendantNodes().OfType<XElement>()
//		where c.Name == "Routine"
//		select c
//			);
//			foreach (var routine in routines)
//			{
//				if (routine.FirstAttribute.Value == routineName)
//				{
//					routineNode = routine;
//				}
//			}
//		}
//	}
//	return routineNode;
//}
//

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