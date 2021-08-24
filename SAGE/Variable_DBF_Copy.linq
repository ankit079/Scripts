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

string sourcePath = @"C:\Users\ankit.shah\Desktop\Test DBF\Advsmelt_variable_20082021.dbf";
string destinationPath = @"C:\Users\ankit.shah\Desktop\Test DBF\variable.dbf";

void Main()
	{
		OleDbConnection connection_Ankit = new OleDbConnection("Provider=VFPOLEDB.1;Data Source=" + sourcePath + ";");
		OleDbConnection connection_dest = new OleDbConnection("Provider=VFPOLEDB.1;Data Source=" + destinationPath + ";");
		// Read Citect Tags and PLC Addresses	
		 var dbfTags = ReadDBF(connection_Ankit);
		 dbfTags.Dump();
		 // WriteTags(dbfTags);
		//var dbfTagsDest = ReadDBFDest(connection_Jonathan);
		//dbfTagsDest.Dump();
		// ProcessTransfer();
	}
	
//	public void ProcessTransfer()
//	{
//
//	try
//	{
//		connection_Jonathan.Open();
//		WriteTags(dbfTags, connection_Jonathan);
//		connection_Jonathan.Close();
//	}
//	catch (Exception ex)
//	{
//		connection_Jonathan.Close();
//		ex.Message.Dump();
//	}
//}

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
						row["Column3"] = tag.address;
						Console.Write("Processed {0}, {1}",tag.tagname, tag.address);
					}
				}
			}		
		}	
		table.Dump();	
	}
	
	public class Tag{
		public string tagname { get; set; }
		public string address { get; set; }
	}

private static string GetConnection(string path)
{
	return $"Provider=vfpoledb;Data Source={path};Collating Sequence=machine;";
}


public void WriteTags(List<Tag> dbfTags)
{
	using (OleDbConnection conn = new OleDbConnection(GetConnection(destinationPath)))
	{

		foreach (var tag in dbfTags)
		{
			string updateTable = $"UPDATE variable SET ADDR = ? WHERE NAME = ?";
			OleDbCommand cmd = new OleDbCommand(updateTable, conn);
			cmd.Parameters.AddWithValue("ADDR", tag.address);
			cmd.Parameters.AddWithValue("NAME", tag.tagname);
			try
			{
				conn.Open();
				cmd.ExecuteNonQuery();
				conn.Close();
				Console.WriteLine("Successfully changed tag name {0} to tag address {1}", tag.tagname, tag.address);
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			finally	{
				conn.Close();
			}
		}
	}
}

public List<Tag> ReadDBF(OleDbConnection connection)
	{
		var mapping = new List<Tag>();
		OleDbCommand cmd = new OleDbCommand("select * from variable where UNIT == 'FF2PLC1_CLX1'");
		cmd.Connection = connection;
		connection.Open();
		IDataReader reader = cmd.ExecuteReader();
		while (reader.Read())
		{
			mapping.Add(new Tag(){address = reader["addr"].ToString().Trim(),tagname = reader["name"].ToString().Trim()});		
		}
		connection.Close();
		return mapping;
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
		mapping.Add(new Tag() { address = reader["addr"].ToString().Trim(), tagname = reader["name"].ToString().Trim() });
	}
	connection.Close();
	return mapping;
}