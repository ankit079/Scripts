<Query Kind="Program">
  <Output>DataGrids</Output>
  <NuGetReference>Microsoft.Office.Interop.Excel</NuGetReference>
  <Namespace>Microsoft.Office.Interop.Excel</Namespace>
</Query>

void Main()
{
	string sourceDir = @"c:\current";
	string backupDir = @"c:\archives\2008";

	try
	{
		string[] picList = Directory.GetFiles(sourceDir, "*.jpg");
		string[] txtList = Directory.GetFiles(sourceDir, "*.txt");

		// Copy picture files.
		foreach (string f in picList)
		{
			// Remove path from the file name.
			string fName = f.Substring(sourceDir.Length + 1);

			// Use the Path.Combine method to safely append the file name to the path.
			// Will overwrite if the destination file already exists.
			File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName), true);
		}

		// Copy text files.
		foreach (string f in txtList)
		{

			// Remove path from the file name.
			string fName = f.Substring(sourceDir.Length + 1);

			try
			{
				// Will not overwrite if the destination file already exists.
				File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName));
			}

			// Catch exception if the file was already copied.
			catch (IOException copyError)
			{
				Console.WriteLine(copyError.Message);
			}
		}

		// Delete source files that were copied.
		foreach (string f in txtList)
		{
			File.Delete(f);
		}
		foreach (string f in picList)
		{
			File.Delete(f);
		}
	}

	catch (DirectoryNotFoundException dirNotFound)
	{
		Console.WriteLine(dirNotFound.Message);
	}

}
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
}
// Define other methods and classes here

