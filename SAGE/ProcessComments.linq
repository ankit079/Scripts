<Query Kind="Program">
  <NuGetReference>ExcelDataReader</NuGetReference>
  <NuGetReference>ExcelDataReader.DataSet</NuGetReference>
  <Namespace>ExcelDataReader</Namespace>
  <Namespace>ExcelDataReader.Core.NumberFormat</Namespace>
  <Namespace>ExcelDataReader.Exceptions</Namespace>
  <Namespace>ExcelDataReader.Log</Namespace>
  <Namespace>ExcelDataReader.Log.Logger</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

void Main()
{
	// File Path to the L5X file
	string xmlFile = @"C:\Users\ankit.shah\Desktop\Stage 2\FF2_PLC1AND3_12082021.L5X";
		string filePath = @"C:\Users\ankit.shah\Desktop\Stage 2\75655-REP-036_A FF2 PLC1 Conversion & Standardisation Report.xlsx";

	// Retrieve XML Document
	XDocument root = XDocument.Load(xmlFile);
	
	var tagList = ObtainTagList(root,filePath);
	//tagList.Dump();
	
	foreach(var tag in tagList)
	{
		FindTagAndReturMatch(root,tag);
	}

	
	//foreach (var tag in tagList)
	//{
	// Get Module Name if the tag exists
	//string moduleName = FindTagAndReturnModuleName(root, "FF2_R03:6:I.Data.3");
	
	//if(moduleName != string.Empty)
	//{
	//	// Get XML Tree
	//	var XMLModuleTree = GetModule(root, moduleName).FirstOrDefault();
	//	XMLModuleTree.Dump();
	//	ProcessComment(XMLModuleTree, "Comment");
	//}
	////}
}

public void ProcessComment(XElement ModuleElement, string comment)
{
	var result = from element in ModuleElement.Descendants("Comments")
				 select element;
				 
	if(result.Count() > 0)
	{
		result.Dump();						
	}
	
	else
	{
		var AliasNode = from element in ModuleElement.Descendants("InAliasTag")
					 select element;
		//AliasNode
	}
	
}

// Define other methods and classes here

// Find if the new tag exists

public void FindTagAndReturMatch(XDocument root, Tag tag)
{
	string moduleName = String.Empty;
	IEnumerable<XElement> result =
from el in root.Descendants("Text")
where
el.Value.Contains(string.Concat("(",tag.Name,")"))

select el;

	if (result.Count() > 0)
	{
		tag.Dump();
	}	
}

// Find if the new tag exists

public string FindTagAndReturnModuleName(XDocument root, string newTag)
{
	string moduleName = String.Empty;
	IEnumerable<XElement> result =
from el in root.Descendants("Text")
where
el.Value.Contains(newTag)

select el;

if(result.Count() > 0)
{
	string [] newTagSplit = newTag.Split(':','.').Dump();
	
	if(Convert.ToInt32(newTagSplit[1]) < 9)
	{
		moduleName = string.Concat(newTagSplit[0], "S0", newTagSplit[1]);		
	}
}
return moduleName;
}


// Gets the Module from XML Tree
public IEnumerable<XElement> GetModule(XDocument root, string moduleName)
{
	IEnumerable<XElement> result =
from el in root.Descendants("Module")
where
	el.Attribute("Name").Value == moduleName

select el;

	return result;
}

public List<Tag> ObtainTagList(XDocument root, string filePath)
{
	var tagList = new List<Tag>();

	var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
	var reader = ExcelReaderFactory.CreateReader(stream);
	DataSet workSheets = reader.AsDataSet();
	DataTable table = reader.AsDataSet().Tables["ProcessComment"];

	for (int i = 0; i < table.Rows.Count; i++)
	{
		Tag singleEntry = new Tag();
		singleEntry.Name = Convert.ToString(table.Rows[i]["Column0"]);
		singleEntry.Comment = Convert.ToString(table.Rows[i]["Column1"]);
		tagList.Add(singleEntry);
	}
	return tagList;
}

public class Tag{
	public string Name { get; set; }
	public string Comment { get; set; }	
}


