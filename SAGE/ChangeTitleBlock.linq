<Query Kind="Program">
  <Reference>C:\Dev\UpdateTitleBlock\UpdateTitleBlock\AcCoreMgd.dll</Reference>
  <Reference>C:\Dev\UpdateTitleBlock\UpdateTitleBlock\AcDbMgd.dll</Reference>
  <Reference>C:\Dev\UpdateTitleBlock\UpdateTitleBlock\AcMgd.dll</Reference>
  <Namespace>Autodesk.AutoCAD.DatabaseServices</Namespace>
</Query>


void Main()
{
	string path = @"\\sageautomation\sagedata\WAProjects\Watercorp\Jobs\68230 - Metro SCADA Obsolescence Upgrade\Metro Engineering Sites\[52] Keane St Drainage Cloverdale\Drawings\2. Working";
	
	DirectoryInfo d = new DirectoryInfo(path);
	FileInfo[] files = d.GetFiles("*.dwg");

	foreach (FileInfo file in files.Take(1))
	{
		var filePath = file.FullName;	
		
		using(Database acDb = new Database(false,true))
		{
			acDb.ReadDwgFile(filePath, FileOpenMode.OpenForReadAndAllShare,false,null);

			HostApplicationServices.WorkingDatabase = acDb;
			
		}
	}
}

// Define other methods and classes here
