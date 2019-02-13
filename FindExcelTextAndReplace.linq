<Query Kind="Program">
  <NuGetReference>ExcelDataReader</NuGetReference>
  <NuGetReference>ExcelDataReader.DataSet</NuGetReference>
  <Namespace>ExcelDataReader</Namespace>
</Query>

void Main()
{
	string[] filePaths = System.IO.File.ReadAllLines(@"C:\temp\filepath.txt");
		List<FinalCosting> excellData = new List<FinalCosting>();
	foreach (string path in filePaths)
	{
		using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
		{

			// Auto-detect format, supports:
			//  - Binary Excel files (2.0-2003 format; *.xls)
			//  - OpenXml Excel files (2007 format; *.xlsx)
			using (var reader = ExcelReaderFactory.CreateReader(stream))
			{
								
//				string [] data = new string[100];
				
				// Choose one of either 1 or 2:

				// 1. Use the reader methods
//				do
//				{
//					while (reader.Read())
//				{
					DataSet workSheets = reader.AsDataSet();

					DataTable specificWorkSheet = reader.AsDataSet().Tables[1];

				foreach (var row in specificWorkSheet.Rows)
				{
					excellData.Add(new FinalCosting((((DataRow)row)[3]).ToString(),(((DataRow)row)[0]).ToString()));
				}
					// reader.GetDouble(0);
//				}
//				} while (reader.NextResult());
//
//				// 2. Use the AsDataSet extension method
//				var result = reader.AsDataSet();

				// The result of each spreadsheet is in result.Tables
			}
			
			foreach(var data in excellData)
			{
				if(data.partNumber.Contains("PB1H"))
				{
					Console.WriteLine(data.info,data.partNumber);
					Console.WriteLine(path);
				}
			}
		}
	}
}

public class FinalCosting
{
	public FinalCosting(string partNumber, string info)
	{
		this.partNumber = partNumber;
		this.info = info;
	}

	public string partNumber {get;set;}
	public string info {get;set;}
}

// Define other methods and classes here