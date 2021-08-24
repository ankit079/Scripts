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
		string path = @"C:\Users\ankit.shah\Desktop\dbf\variable.dbf";
		string filePath = @"C:\Users\ankit.shah\Desktop\Stage 2\75655-REP-037_A FF2 PLC1 SCADA Standardisation Report.xlsx";

		OleDbConnection connection = new OleDbConnection("Provider=VFPOLEDB.1;Data Source=" + path + ";");
		// Read Citect Tags and PLC Addresses	
		var dbfTags = ReadDBF(connection);
		// dbfTags.Dump();
		// Process the Tag changes for ABCLX compatibility
		var processedTags = ProcessTags(dbfTags);
		processedTags.Dump();

		// Copy to Excel

		//CopyToExcel(filePath,processedTags);	

		// Write back to the DBF file the change of PLC Tags - NOT WORKING

		WriteTags(processedTags, connection);
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
						row["Column3"] = tag.old_address;
						Console.Write("Processed {0}, {1}",tag.tagname, tag.old_address);
					}
				}
			}		
		}	
		table.Dump();	
	}
	
	public class Tag{
		public string tagname { get; set; }
		public string old_address { get; set; }
		public string new_address { get; set; }
	}
	
	public void WriteTags(List<Tag> dbfTags,OleDbConnection connection)
	{
		connection.Open();
			
		foreach(var tag in dbfTags){
	
					OleDbCommand cmd = new OleDbCommand("UPDATE [variable] SET [ADDR] = @addr WHERE [NAME] = @name", connection);			
					OleDbTransaction transaction = null;
					transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
					cmd.Connection = connection;
					cmd.Transaction = transaction;
					cmd.Parameters.AddWithValue("@addr", tag.old_address);
					cmd.Parameters.AddWithValue("@name", tag.tagname);
					try
					{
						cmd.ExecuteNonQuery();
						// Commit the transaction.
						transaction.Commit();
						Console.WriteLine("Successfully changed tag name {0} to tag address {1}",tag.tagname,tag.old_address);
					}
					catch (Exception ex)
					{
						try
						{
						// Attempt to roll back the transaction.
						transaction.Rollback();
						}
						catch
						{						
							Console.Write("Transaction is not active.");
							Console.Write(ex.Message);
						}
					}					
						
		}
		connection.Close();
	}
	
	public List<Tag> ReadDBF(OleDbConnection connection)
	{
		var mapping = new List<Tag>();
		OleDbCommand cmd = new OleDbCommand("select * from variable where UNIT == 'FF2PLC1_DE1'");
		cmd.Connection = connection;
		connection.Open();
		IDataReader reader = cmd.ExecuteReader();
		while (reader.Read())
		{
			mapping.Add(new Tag(){old_address = reader["addr"].ToString().Trim(),tagname = reader["name"].ToString().Trim()});		
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
						string [] data = item.old_address.Split('/');
						string firstChar = Cleanup(data[0]);
						string secondChar = int.Parse(data[1]).ToString();
						//item.old_address = string.Concat(firstChar,"/",secondChar);
						string modified_old_address = string.Concat(firstChar,"/",secondChar);
						
						item.new_address = Method1(modified_old_address);
					}
					else if (item.old_address.Contains(":") && item.old_address.Contains(".") && !item.old_address.Contains("/"))
					{
						item.new_address = Method4(item.old_address);
					}
				}
				else{
					Console.WriteLine("No method exists to convert this Tag {0}",item);
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

public void ToBeCleaned()
{
	//	DataTable tables = connection.GetSchema(OleDbMetaDataCollectionNames.Tables);
	//	//tables.Dump();
	//	foreach (DataRow rowTables in tables.Rows)
	//	{
	//		if (rowTables["table_name"].ToString() != null)
	//		{
	//			DataTable columns = connection.GetSchema(OleDbMetaDataCollectionNames.Columns,
	//				new String[] { null, null, rowTables["table_name"].ToString(), null });
	//			//columns.Dump();
	//
	//			foreach (System.Data.DataRow rowColumns in columns.Rows)
	//			{
	//				if (rowColumns["column_name"].ToString() == "name")
	//				{
	//					//rowColumns.Dump();
	//				}
	//			}
	//			break;
	//		}
	//	}
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
		string newString = string.Concat(Cleanup(txt[0]), "{",Cleanup(txt[1]), "}",".",txt[2]);
		return newString;
	}


// Define other methods and classes here
