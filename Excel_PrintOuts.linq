<Query Kind="Program">
  <NuGetReference>EntityFramework</NuGetReference>
  <NuGetReference>ExcelDataReader</NuGetReference>
  <NuGetReference>ExcelDataReader.DataSet</NuGetReference>
  <NuGetReference>Microsoft.Office.Interop.Excel</NuGetReference>
  <Namespace>ExcelDataReader</Namespace>
  <Namespace>ExcelDataReader.Core.NumberFormat</Namespace>
  <Namespace>ExcelDataReader.Exceptions</Namespace>
  <Namespace>ExcelDataReader.Log</Namespace>
  <Namespace>ExcelDataReader.Log.Logger</Namespace>
  <Namespace>Microsoft.Office.Interop.Excel</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
  <Namespace>System.ComponentModel.DataAnnotations.Schema</Namespace>
  <Namespace>System.Data.Entity</Namespace>
  <Namespace>System.Data.Entity.Core</Namespace>
  <Namespace>System.Data.Entity.Core.Common</Namespace>
  <Namespace>System.Data.Entity.Core.Common.CommandTrees</Namespace>
  <Namespace>System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder</Namespace>
  <Namespace>System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder.Spatial</Namespace>
  <Namespace>System.Data.Entity.Core.Common.EntitySql</Namespace>
  <Namespace>System.Data.Entity.Core.EntityClient</Namespace>
  <Namespace>System.Data.Entity.Core.Mapping</Namespace>
  <Namespace>System.Data.Entity.Core.Metadata.Edm</Namespace>
  <Namespace>System.Data.Entity.Core.Objects</Namespace>
  <Namespace>System.Data.Entity.Core.Objects.DataClasses</Namespace>
  <Namespace>System.Data.Entity.Infrastructure</Namespace>
  <Namespace>System.Data.Entity.Infrastructure.Annotations</Namespace>
  <Namespace>System.Data.Entity.Infrastructure.DependencyResolution</Namespace>
  <Namespace>System.Data.Entity.Infrastructure.Design</Namespace>
  <Namespace>System.Data.Entity.Infrastructure.Interception</Namespace>
  <Namespace>System.Data.Entity.Infrastructure.MappingViews</Namespace>
  <Namespace>System.Data.Entity.Infrastructure.Pluralization</Namespace>
  <Namespace>System.Data.Entity.Migrations</Namespace>
  <Namespace>System.Data.Entity.Migrations.Builders</Namespace>
  <Namespace>System.Data.Entity.Migrations.Design</Namespace>
  <Namespace>System.Data.Entity.Migrations.History</Namespace>
  <Namespace>System.Data.Entity.Migrations.Infrastructure</Namespace>
  <Namespace>System.Data.Entity.Migrations.Model</Namespace>
  <Namespace>System.Data.Entity.Migrations.Sql</Namespace>
  <Namespace>System.Data.Entity.Migrations.Utilities</Namespace>
  <Namespace>System.Data.Entity.ModelConfiguration</Namespace>
  <Namespace>System.Data.Entity.ModelConfiguration.Configuration</Namespace>
  <Namespace>System.Data.Entity.ModelConfiguration.Conventions</Namespace>
  <Namespace>System.Data.Entity.Spatial</Namespace>
  <Namespace>System.Data.Entity.SqlServer</Namespace>
  <Namespace>System.Data.Entity.SqlServer.Utilities</Namespace>
  <Namespace>System.Data.Entity.Utilities</Namespace>
  <Namespace>System.Data.Entity.Validation</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

void Main()
{
	var ExcelFormater = new ExcelFormater();
	string path = @"J:\J11251_GAR1_PRELIMINARY_DESIGN_-GAR_(Water_Corporation)\Drawings and Documents\E80 Docs\#47 George Rd Tank East\11251-QUO-047.xlsx";
//	string[] filePaths = System.IO.File.ReadAllLines(@"C:\temp\filepath.txt");
//	foreach (string path in filePaths)
//	{
		ExcelFormater.FormatExcelDoc(path);
		Console.Write("Processed the filepath: {0}", path);
//	}

}

// Class that stores Application

public class ExcelFormater
{
	Application _excelApp;
	Workbook workBook;

	public ExcelFormater()
	{
		_excelApp = new Application();
		// Make the object visible.
		//_excelApp.Visible = true;
	}

	public void FormatExcelDoc(string fileName)
	{
		try
		{
			// Opens an Excel Workbook

			workBook = _excelApp.Workbooks.Open(fileName);

			// Pass the workbook to a separate function that formats the workbook

		//	FormatExcelSheet(fileName);

			// Save the workbook

			_excelApp.DisplayAlerts = false;
			//workBook.SaveAs(fileName);
			printOut();
			workBook.Close(true, fileName, null);
			_excelApp.Quit();

		}
		catch (Exception ex)
		{
			Console.Write(ex.Message);
			Console.Write(ex.StackTrace);
		}

		finally
		{
			KillExcel();
			Console.Write("Excel Application is closed" + "\r\n");
			System.Threading.Thread.Sleep(100);
		}
	}

	[DllImport("User32.dll")]
	public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);
	private void KillExcel()
	{
		int id = 0;
		IntPtr intptr = new IntPtr(_excelApp.Hwnd);
		System.Diagnostics.Process p = null;
		try
		{
			GetWindowThreadProcessId(intptr, out id);
			p = System.Diagnostics.Process.GetProcessById(id);
			if (p != null)
			{
				p.Kill();
				p.Dispose();
			}
		}
		catch (Exception ex)
		{
			Console.Write("KillExcel:" + ex.Message);
		}
	}

	private void deleteExistingRevisionSheet(string[] worksheetNames)
	{
		foreach (var name in worksheetNames)
		{
			if (name == "Revision")
			{
				RemoveRevisionSheet();
				Console.Write("The previous Revision Sheet has been deleted" + "\r\n");
				Thread.Sleep(100);
			}
		}
	}
	
	private void FormatExcelSheet(string fileName)
	{
		var worksheetNames = getWorksheetNames();
		//worksheetNames.Dump();
		deleteExistingRevisionSheet(worksheetNames);

		var workSheet = CreateRevisionSheet();
		FormatRevisionSheet(workSheet);		
		AddStudyReportSheetRevision();
		Console.Write("The new Revision Sheet has been generated" + "\r\n");
	}

	private string[] getWorksheetNames()
	{
		// Get sheet count and store the number of sheets

		int numSheets = workBook.Sheets.Count;
		string[] worksheetName = new string[numSheets];

		// Iterate through the sheets. They are indexed starting at 1

		for (int i = 1; i < numSheets + 1; i++)
		{
			Worksheet worksheet = (Worksheet)workBook.Sheets[i];
			worksheetName[i - 1] = worksheet.Name;
		}
		return worksheetName;
	}

	private Worksheet CreateRevisionSheet()
	{

		var newWorkSheet = (Worksheet)workBook.Sheets.Add();
		newWorkSheet.Name = "Revision";
		return newWorkSheet;
	}

	private void FormatRevisionSheet(Worksheet revisionSheet)
	{
		var nameofSite = getSiteName();
		revisionSheet.Cells[1, "A"] = "     " + nameofSite + " - Revision History" + "     ";
		revisionSheet.get_Range("A1", "C1").Merge();
		//revisionSheet.get_Range("A1","C1").EntireColumn.AutoFit();						
		revisionSheet.Cells[2, "A"] = "Current Revision";
		//var range = _excelApp.get_Range(revisionSheet.Cells[2, "C"]);
		//range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
		revisionSheet.Cells[3, "A"] = "Revision Information";
		revisionSheet.Cells[4, "A"] = "Revision";
		revisionSheet.Cells[4, "B"] = "Description";
		revisionSheet.Cells[4, "C"] = "Date";
		var columnHeadingsRange = revisionSheet.get_Range("A4", "C4");
		columnHeadingsRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LawnGreen);
		//columnHeadingsRange.EntireColumn.AutoFit();
		revisionSheet.Cells[5, "A"] = "B";
		revisionSheet.Cells[5, "B"] = "Incorporated client comments";
		var date = new DateTime(2019,01,02);	
		revisionSheet.Cells[5, "C"] = date.ToString("dd/MM/yyyy");
		//revisionSheet.Cells[5, "C"] = DateTime.Today.Date.ToString("dd/M/yyyy",CultureInfo.InvariantCulture);		
		revisionSheet.Cells[6, "A"] = "A";
		revisionSheet.Cells[6, "B"] = "Issued to client for review";
		revisionSheet.Cells[6, "C"] = "21/12/2018";
		revisionSheet.Cells[2, "C"] = "=A5";
		//revisionSheet.Cells[5, "C"] = DateTime.Today.Date.ToString("dd/MM/yyyy");
		//((Microsoft.Office.Interop.Excel.Range)revisionSheet.Rows[1]).AutoFit();
		revisionSheet.Columns.AutoFit();
		revisionSheet.Rows.AutoFit();			
		//AddStudyReportSheetRevision();
		AddCostingSheetRevision();
		Console.Write("Formating Adjusted" + "\r\n");	
		
	}

	private void AddCostingSheetRevision()
	{

		foreach (Worksheet sheet in workBook.Sheets)
		{
			if (sheet.Name == "Final Costing")
			{
				Range rng1 = sheet.Range["U2"];
				rng1.Formula = "=Revision!C2";
				Range rng2 = sheet.Range["U3"];
				rng2.Formula = "=Revision!C5";
				Range rng3 = sheet.Range["U1"];
				rng3.Formula = "=MID(CELL(\"filename\",A1),FIND(\"[\",CELL(\"filename\",A1))+1,FIND(\"]\", CELL(\"filename\",A1))-FIND(\"[\",CELL(\"filename\",A1))-1)";
			}
		}
	}

	private void RemoveRevisionSheet()
	{
		foreach (Worksheet sheet in workBook.Sheets)
		{
			if (sheet.Name == "Revision")
			{
				_excelApp.DisplayAlerts = false;
				sheet.Delete();
				_excelApp.DisplayAlerts = true;
			}
		}
	}
	
	private string getSiteName()
	{
		string siteName = string.Empty;
		foreach (Worksheet sheet in workBook.Sheets)
		{
			if (sheet.Name == "Study Report")
			{
				siteName = (string)(sheet.Cells[3, "B"] as Microsoft.Office.Interop.Excel.Range).Value;
			}
		}
		return siteName;
	}
	
	private void AddStudyReportSheetRevision()
	{

		foreach (Worksheet sheet in workBook.Sheets)
		{
			if (sheet.Name == "Study Report")
			{
				sheet.Activate();
				//Range rng = sheet.Range["C1"];
				//rng.Formula = "=CONCATENATE(\"REV\",\" \",Revision!C2)";
				var line = (Range)sheet.Rows[2];
				line.Insert();				
				sheet.Cells[2, "B"] = "Revision History";
				sheet.Range["C2"].Formula = "=CONCATENATE(\"REV\", \" \", Revision!C2)";
				var currentCell = (Range)this._excelApp.ActiveCell[2,3];
				currentCell.EntireRow.Font.Bold = true;
			}
		}
	}
	
	public void printOut()
	{
		foreach (Worksheet sheet in workBook.Sheets)
		{
			//if ((sheet.Name == "Study Report") || (sheet.Name == "Final Costing") || (sheet.Name == "Revision"))
			if(sheet.Name != "Rates")
			{
				sheet.Activate();
				Console.Write("The sheet {0} is about to be printed\n", sheet.Name);
				//_excelApp.ActivePrinter = "Adobe PDF";
				((Worksheet)_excelApp.ActiveSheet).PrintOut();
				//sheet.Application.ActivePrinter = "Adobe PDF";
				//sheet.Activate();
				Console.Write("The sheet {0} has been printed\n", sheet.Name);
			}
		}
	}

	public void getExcelCellValue()
	{
		FileStream stream = File.Open(@"C:\\Dev\\Scripts\\#028 PS North Cleary Tank 5049 (Mollerin WPS)\\11251-QUO-028A.xlsx", FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet result = excelReader.AsDataSet();

		foreach (System.Data.DataTable table in result.Tables)
		{
			foreach (DataRow row in table.Rows)
			{
				var quoteNo = (string)row["Quote No."];
				if (String.Equals(quoteNo, "Quote No."))
				{
					var year = (int)(double)row["Year"];
					Console.WriteLine($"Janet's year is {year}");
				}
			}
		}
	}
}