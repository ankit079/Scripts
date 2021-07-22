<Query Kind="Program">
  <NuGetReference>PdfSharp</NuGetReference>
  <Namespace>PdfSharp</Namespace>
  <Namespace>PdfSharp.Charting</Namespace>
  <Namespace>PdfSharp.Drawing</Namespace>
  <Namespace>PdfSharp.Drawing.BarCodes</Namespace>
  <Namespace>PdfSharp.Drawing.Layout</Namespace>
  <Namespace>PdfSharp.Fonts</Namespace>
  <Namespace>PdfSharp.Internal</Namespace>
  <Namespace>PdfSharp.Pdf</Namespace>
  <Namespace>PdfSharp.Pdf.AcroForms</Namespace>
  <Namespace>PdfSharp.Pdf.Actions</Namespace>
  <Namespace>PdfSharp.Pdf.Advanced</Namespace>
  <Namespace>PdfSharp.Pdf.Annotations</Namespace>
  <Namespace>PdfSharp.Pdf.Content</Namespace>
  <Namespace>PdfSharp.Pdf.Content.Objects</Namespace>
  <Namespace>PdfSharp.Pdf.Filters</Namespace>
  <Namespace>PdfSharp.Pdf.Internal</Namespace>
  <Namespace>PdfSharp.Pdf.IO</Namespace>
  <Namespace>PdfSharp.Pdf.Security</Namespace>
  <Namespace>PdfSharp.SharpZipLib.Zip</Namespace>
</Query>

// Usage Note: No folders in the end directory only files
// After pdf conversion, the source file tif will be deleted
// Add process locking (Pending)

string dirPath = @"C:\Users\ankit.shah\Desktop";
void Main()
{
	var sourceFilePaths = getFilePaths();	
	foreach(var sourcePath in sourceFilePaths)
	{
		string extension = sourcePath.Substring(sourcePath.Length - 3,3);
		var destPath =   sourcePath.Replace(extension,"pdf");
		convertToPDF(sourcePath,destPath);
	}	
	//removeTIFFiles(sourceFilePaths);	
}

private void getAllPDFSizes()
{
	PageSize[] pageSizes = (PageSize[])Enum.GetValues(typeof(PageSize));	
}

// Associated methods are listed below

//private void removeTIFFiles(string[] sourceFilePaths)
//{
//	foreach (var sourcePath in sourceFilePaths)
//	{
//		File.Delete(sourcePath);
//	}
//}

private void convertToPDF(string sourcePath, string destPath)
{
	PdfDocument s_document = new PdfDocument();

	PdfPage page = s_document.AddPage();

	XGraphics gfx = XGraphics.FromPdfPage(page);

	XImage image = XImage.FromFile(sourcePath);
	page.Orientation = PageOrientation.Landscape;
	page.Width = image.PointWidth;
	page.Height = image.PointHeight;	
	page.Size = PageSize.A1;
	//page.Size = PageSize.A3;
	XRect box = new XRect(0,0,page.Width,page.Height);
	gfx.DrawImage(image,box);
	s_document.Save(destPath);
}

private string[] getFilePaths()
{
	string[] dwgDrawings = null;
	try
	{
		string[] MultipleFilters = {"*dwg"};
		// Only get files that ending with the letter "tif" or "TIF".
		foreach(var filter in MultipleFilters)
		{
			dwgDrawings = Directory.GetFiles(dirPath, filter);	
		}
		
		Console.WriteLine("The number of files ending with dwg is {0}.", dwgDrawings.Length);
		foreach (string dir in dwgDrawings)
		{
			Console.WriteLine(dir);
		}
	}
	catch (Exception e)
	{
		Console.WriteLine("The process failed: {0}", e.ToString());
	}
	return dwgDrawings;
}