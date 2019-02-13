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
	string[] filePaths = System.IO.File.ReadAllLines(@"C:\temp\filepath.txt");
	
	foreach (string path in filePaths)
	{
		var startInfo = new ProcessStartInfo();
		startInfo.CreateNoWindow = true;
		startInfo.UseShellExecute = false;
		startInfo.FileName = path;
		startInfo.WindowStyle = ProcessWindowStyle.Hidden;
		
		try
		{
			using(Process excelProcess = Process.Start(startInfo.FileName))
			{
				ExcelFormater.FormatExcelDoc(path);
				excelProcess.Close();
				excelProcess.Kill();
				Console.Write("Processed the filepath: {0}", path);
				
				if(!excelProcess.HasExited)
				{
					excelProcess.Refresh();
					Console.WriteLine("Physical Memory Usage: " + excelProcess.WorkingSet64.ToString());
					Thread.Sleep(5000);
				}
				else
				{
					break;	
				}			
				excelProcess.CloseMainWindow();
			}			
		}
		catch (Exception ex)
		{		
			Console.WriteLine("The following exception was raised: ");
			Console.Write(ex.Message);
			Console.Write(ex.StackTrace);					
		}
	}
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
		int length = fileName.Length - 8;
		string pathOutput = fileName.Remove(0, length);
		string projectNo = pathOutput.Remove(3);
		try
		{
			// Opens an Excel Workbook

			workBook = _excelApp.Workbooks.Open(fileName);

			// Save the workbook

			_excelApp.DisplayAlerts = false;
			//workBook.SaveAs(fileName);
			printOut(projectNo);

			//Cleanup
			
			workBook.Close(true, fileName, null);
			//Marshal.FinalReleaseComObject(workBook);
			_excelApp.Quit();
		}
		catch (Exception ex)
		{
			Console.Write(ex.Message);
			Console.Write(ex.StackTrace);
		}

		finally
		{
			
			//KillExcel();
			Console.Write("Excel Application is closed" + "\r\n");
			//System.Threading.Thread.Sleep(100);
		}
	}

//	[DllImport("User32.dll")]
//	public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);
//	private void KillExcel()
//	{
//		int id = 0;
//		IntPtr intptr = new IntPtr(_excelApp.Hwnd);
//		System.Diagnostics.Process p = null;
//		try
//		{
//			GetWindowThreadProcessId(intptr, out id);
//			p = System.Diagnostics.Process.GetProcessById(id);
//			if (p != null)
//			{
//				p.Kill();
//				p.Dispose();
//			}
//		}
//		catch (Exception ex)
//		{
//			Console.Write("KillExcel:" + ex.Message);
//		}
//	}

	public void printOut(string projectNo)
	{
		foreach (Worksheet sheet in workBook.Sheets)
		{
			//if ((sheet.Name == "Study Report") || (sheet.Name == "Final Costing") || (sheet.Name == "Revision"))
			if(sheet.Name != "Rates")
			{
				sheet.Activate();
				Console.Write("The sheet {0} is about to be printed\n", sheet.Name);
				
				//_excelApp.ActivePrinter = "Adobe PDF";
				((Worksheet)_excelApp.ActiveSheet).SaveAs(string.Concat("11251-QUO-",projectNo, " - ", sheet.Name),Type.Missing, Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
				//((Worksheet)_excelApp.ActiveSheet).Visible = XlSheetVisibility.xlSheetHidden;
				((Worksheet)_excelApp.ActiveSheet).PrintOut();
				//((Worksheet)_excelApp.ActiveSheet).SaveAs(string.Concat("11251-QUO-XXX ", sheet.Name),Type.Missing, Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
				//sheet.Application.ActivePrinter = "Adobe PDF";
				//sheet.Activate();
//				GC.Collect();
//				GC.WaitForPendingFinalizers();
//				Marshal.FinalReleaseComObject(((Worksheet)_excelApp.ActiveSheet));

				Console.Write("The sheet {0} has been printed\n", sheet.Name);
			}
		}
	}

}