<Query Kind="Program">
  <NuGetReference>itext7</NuGetReference>
</Query>

string pdfFileLocation = @"C:\aquaDRAW\Image";
void Main()
{
	DeletePlansetFolders();
	//CreatePDFs();
	CombinePDF();
}

void CombinePDF()
{
	string[] plansetDirectories = Directory.GetDirectories(pdfFileLocation);
	foreach (var directory in plansetDirectories)
	{
		bool directoryExists = Directory.Exists(directory);
		if (directoryExists)
		{
			string[] files = Directory.GetFiles(directory);
			foreach (var file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
			}
		}
	}
}
	public void DeletePlansetFolders()
{
	string[] plansetDirectories = Directory.GetDirectories(pdfFileLocation);
	foreach(var directory in plansetDirectories)
	{		
	bool directoryExists = Directory.Exists(directory);
	if(directoryExists){
		string[] files = Directory.GetFiles(directory);
		foreach (var file in files)
		{
			File.SetAttributes(file, FileAttributes.Normal);
			File.Delete(file);
		}				
		Directory.Delete(directory);
	}
	directoryExists = false;
	}	
}



// Define other methods and classes here
