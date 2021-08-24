<Query Kind="Program">
  <NuGetReference>ExcelDataReader</NuGetReference>
  <NuGetReference>ExcelDataReader.DataSet</NuGetReference>
  <NuGetReference>Microsoft.Office.Interop.Excel</NuGetReference>
  <Namespace>ExcelDataReader</Namespace>
  <Namespace>ExcelDataReader.Core.NumberFormat</Namespace>
  <Namespace>ExcelDataReader.Exceptions</Namespace>
  <Namespace>ExcelDataReader.Log</Namespace>
  <Namespace>ExcelDataReader.Log.Logger</Namespace>
  <Namespace>Microsoft.Office.Interop.Excel</Namespace>
  <Namespace>System.Data.OleDb</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

void Main()
{
	string advsmelt = @"C:\Users\ankit.shah\Desktop\Test DBF\Advsmelt_variable_20082021.dbf";
	string destDBF = @"C:\Users\ankit.shah\Desktop\Test DBF\variable_Advsmelt_Done.dbf";
	string furnace = @"C:\Users\ankit.shah\Desktop\Test DBF\Furnace_variable_20082021.dbf";
	string kns = @"C:\Users\ankit.shah\Desktop\Test DBF\KNS_variable_20082021.dbf";
	string flux = @"C:\Users\ankit.shah\Desktop\Test DBF\Flux_variable_23082021.dbf";
	string filePath = @"C:\Users\ankit.shah\Desktop\Stage 2\75655-REP-036_A FF2 PLC1 Conversion & Standardisation Report.xlsx";
	string plcIOTableName = "PLC5 Conv Map_PLC1";
	string ff2plc1 = "FF2PLC1_CLX1";
	string ff2plc2 = "FF2PLC2A_CLX1";
	string oldff2plc1 = "FF2PLC1_DE1";
	
	// Read Citect Tags and PLC Addresses	
	//var dbfTags = ReadDBF(flux,ff2plc1);
	//dbfTags.Dump();
	ChangeUnitDescription(destDBF,oldff2plc1,ff2plc1);
	//var processedTags = ProcessTags(dbfTags);
	//processedTags.Dump();
		
	// ProcessTagsWithPLCIOMapping(filePath,plcIOTableName,processedTags);
		
	//	WriteTags(processedTags,flux);
}

public void ChangeUnitDescription(string filePath, string oldUnitName, string newUnitName)
{
	string fileName = Path.GetFileNameWithoutExtension(filePath);
	using (OleDbConnection conn = new OleDbConnection(GetConnection(filePath)))
	{
		string updateTable = $"UPDATE {fileName} SET UNIT = '{newUnitName}' WHERE UNIT == '{oldUnitName}'";
			OleDbCommand cmd = new OleDbCommand(updateTable, conn);
			try
			{
				conn.Open();
				cmd.ExecuteNonQuery();
				conn.Close();
				Console.WriteLine("Successfully changed unit name from old unit name {0} to new unit name {1}", oldUnitName, newUnitName);
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			finally
			{
				conn.Close();
			}
	}
}

public List<Tag> ReadDBF(string path, string unit)
{
	string fileName = Path.GetFileName(path);
	using (OleDbConnection conn = new OleDbConnection(GetConnection(path)))
	{
		var mapping = new List<Tag>();
		string query = $"select * from {fileName} where UNIT == '{unit}'";
		OleDbCommand cmd = new OleDbCommand(query);
		cmd.Connection = conn;
		conn.Open();
		IDataReader reader = cmd.ExecuteReader();
		while (reader.Read())
		{
			if(reader["addr"].ToString().Trim().Contains("{"))
			{
				mapping.Add(new Tag() { new_address = reader["addr"].ToString().Trim(), tagname = reader["name"].ToString().Trim() });
			}
			else
			{
				mapping.Add(new Tag() { old_address = reader["addr"].ToString().Trim(), tagname = reader["name"].ToString().Trim() });
			}
		}
		conn.Close();
		return mapping;
	}
}

public void ProcessTagsWithPLCIOMapping(string filePath, string plcIOTableName, List<Tag> processedTags)
{
	var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
	var reader = ExcelReaderFactory.CreateReader(stream);
	DataSet workSheets = reader.AsDataSet();
	System.Data.DataTable table = reader.AsDataSet().Tables[plcIOTableName];

	foreach (DataRow row in table.Rows)
	{
		//if (row.ItemArray[2].ToString() == "FF2PLC1_DE1")
		//{
			foreach (var tag in processedTags)
			{
				if (tag.tagname == row.ItemArray[1]?.ToString())
				{
					//row["Column3"] = tag.new_address;
					Console.Write("Processed {0}, {1}", tag.tagname, tag.new_address);
					Console.Write(row.ItemArray[2].ToString());
				}
			}
		//}
	}
	//table.Dump();
}

public void CopyToExcel(string filePath, List<Tag> processedTags){
	
		var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
		var reader = ExcelReaderFactory.CreateReader(stream);
		DataSet workSheets = reader.AsDataSet();
		System.Data.DataTable table = reader.AsDataSet().Tables["SCADA_IOs"];
	
		foreach(DataRow row in table.Rows)
		{
			if(row.ItemArray[2].ToString() == "FF2PLC1_DE1")
			{
				foreach (var tag in processedTags)
				{
					if (tag.tagname == row.ItemArray[0].ToString())
					{
						row["Column3"] = tag.new_address;
						Console.Write("Processed {0}, {1}",tag.tagname, tag.new_address);
					}
				}
			}		
		}	
		table.Dump();	
	}

public class Tag
{
	public string tagname { get; set; }
	public string old_address { get; set; }
	public string new_address { get; set; }
}

private static string GetConnection(string path)
{
	return $"Provider=vfpoledb;Data Source={path};Collating Sequence=machine;";
}

public void WriteTags(List<Tag> dbfTags, string path)
{
	string fileName = Path.GetFileName(path);
	using (OleDbConnection conn = new OleDbConnection(GetConnection(path)))
	{

		foreach (var tag in dbfTags)
		{
			string updateTable = $"UPDATE {fileName} SET ADDR = ? WHERE NAME = ?";
			OleDbCommand cmd = new OleDbCommand(updateTable, conn);
			cmd.Parameters.AddWithValue("ADDR", tag.new_address);
			cmd.Parameters.AddWithValue("NAME", tag.tagname);
			try
			{
				conn.Open();
				cmd.ExecuteNonQuery();
				conn.Close();
				Console.WriteLine("Successfully changed tag name {0} old address {1} to new tag address {2}", tag.tagname, tag.old_address, tag.new_address);
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			finally	
			{
				conn.Close();
			}
		}
	}
}

public List<Tag> ReadDBFDest(OleDbConnection connection)
{
	var mapping = new List<Tag>();
	OleDbCommand cmd = new OleDbCommand("select * from variable_Jonathan where UNIT == 'FF2PLC1_DE1'");
	cmd.Connection = connection;
	connection.Open();
	IDataReader reader = cmd.ExecuteReader();
	while (reader.Read())
	{
		mapping.Add(new Tag() { old_address = reader["addr"].ToString().Trim(), tagname = reader["name"].ToString().Trim() });
	}
	connection.Close();
	return mapping;
}

public List<Tag> ProcessTags(List<Tag> dbfTags)
{
	foreach (var item in dbfTags)
	{
		if (item.old_address != null)
		{
			if (item.old_address.Contains(":") && item.old_address.Contains("/") && !item.old_address.Contains("."))
			{
				item.new_address = Method2(item.old_address);
			}

			else if (item.old_address.Contains(":") && !item.old_address.Contains("/") && !item.old_address.Contains("."))
			{
				item.new_address = Method3(item.old_address);
			}

			else if (!item.old_address.Contains(":") && item.old_address.Contains("/") && !item.old_address.Contains("."))
			{
				string[] data = item.old_address.Split('/');
				string firstChar = Cleanup(data[0]);
				string secondChar = int.Parse(data[1]).ToString();
				//item.old_address = string.Concat(firstChar,"/",secondChar);
				string modified_old_address = string.Concat(firstChar, "/", secondChar);

				item.new_address = Method1(modified_old_address);
			}
			else if (item.old_address.Contains(":") && item.old_address.Contains(".") && !item.old_address.Contains("/"))
			{
				item.new_address = Method4(item.old_address);
			}
		}
		else
		{
			Console.WriteLine("No method exists to convert this Tag {0}", item);
		}
	}
	return dbfTags;
}

public string Cleanup(string old_tag_Char)
{
	string b = string.Empty;
	string c = string.Empty;
	int val = 0;
	string result = string.Empty;

	for (int i = 0; i < old_tag_Char.Length; i++)
	{
		if (Char.IsDigit(old_tag_Char[i]))
		{
			b += old_tag_Char[i];
		}
		else if (Char.IsLetter(old_tag_Char[i]))
		{
			c += old_tag_Char[i];
		}
	}

	if (b.Length > 0 && c.Length > 0)
	{
		val = int.Parse(b);
		result = string.Concat(c, val.ToString());
	}

	else if (b.Length > 0)
	{
		val = int.Parse(b);
		result = val.ToString();
	}

	else
	{
		result = c;
	}

	return result;
}

// Define other methods and classes here
// Pattern B12/1672 --->  B12{104}/8 

public string Method1(string text)
{
	string[] txt = text.Split('/');
	int lastChr = Convert.ToInt32(txt[1]) % 16;
	decimal middleChr = Math.Floor(Convert.ToDecimal(txt[1]) / 16);
	string newString = string.Concat(txt[0], "{", middleChr, "}", "/", lastChr);
	return newString;
}

// Pattern O:030/12 --->  O{30}/10 (Inputs and Outputs)

public string Method2(string text)
{
	char[] splitConditions = { ':', '/' };
	string[] txt = text.Split(splitConditions);
	int numMiddle = Convert.ToInt32(txt[1]);
	int numEnd = Convert.ToInt32(txt[2]);
	if (numEnd > 7 && !txt[0].Contains("B"))
	{
		numEnd = numEnd - 2;
	}
	string newString = string.Concat(Cleanup(txt[0]), "{", numMiddle, "}", "/", numEnd);
	return newString;
}

// Define other methods and classes here
// Pattern N24:243 --->  N24{243}
// Integer Array of N24

public string Method3(string text)
{

	string[] txt = text.Split(':');
	string newString = string.Concat(Cleanup(txt[0]), "{", Cleanup(txt[1]), "}");
	return newString;
}

// Define other methods and classes here
// Pattern T4:0.PRE --->  T4{0}.PRE
// Timers

public string Method4(string text)
{
	char[] splitConditions = { ':', '.' };
	string[] txt = text.Split(splitConditions);
	string newString = string.Concat(Cleanup(txt[0]), "{", Cleanup(txt[1]), "}", ".", txt[2]);
	return newString;
}