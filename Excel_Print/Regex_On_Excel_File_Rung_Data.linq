<Query Kind="Program">
  <Reference>&lt;ProgramFilesX86&gt;\Microsoft Visual Studio\Shared\Visual Studio Tools for Office\PIA\Office15\Microsoft.Office.Interop.Excel.dll</Reference>
  <Namespace>Microsoft.Office.Interop.Excel</Namespace>
</Query>

void Main()
{	
	string input = "XIC(CRYS11A_Reset.Out_Reset)OTE(CRYS11A_Data.DINT[0].0);";
	var data_Seperation = new Dictionary<string,List<string>>();
	var result = ExtractDataFromRung(input);
	data_Seperation.Add(input,result);
	data_Seperation.Dump();
}

// Method to extract the data from a string

public List<string> ExtractDataFromRung(string input)
{
	var extracted_List = new List<String>();		
	var matches = Regex.Matches(input, @"\(.*?\)", RegexOptions.None);
	var match = matches.Cast<Match>();
	foreach (var data in match)
	{
		if (data.Value != "()")
		{
			var value1 = data.Value.Replace("(", "");
			var value2 = value1.Replace(")", "");	
			extracted_List.Add(value2);
		}		
	}
	return extracted_List;
}




