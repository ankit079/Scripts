<Query Kind="Program" />


void Main()
{
	string logpath = @"c:\temp\filepath.txt";
	List<FilePath> proj = new List<FilePath>();
	// Get all drivers from this PC
	// string[] drives = System.Environment.GetLogicalDrives();
	if (File.Exists(logpath))
	{
		File.Delete(logpath);
	}

	using (FileStream fs = File.Create(logpath))
	{
		string[] dir = new string[1];
		dir[0] = @"C:\Users\shaha\Desktop\TLA_HMI_S1_20191027_SiteBackup\Page xml exports\Gfx";

		foreach (var dr in dir)
		{
			if (Directory.Exists(dr))
			{
				ProcessDirectory(dr, fs, proj);
			}
		}
	}
}

public static void ProcessDirectory(string targetDirectory, FileStream fs, List<FilePath> proj)
{
	System.IO.DriveInfo di = new System.IO.DriveInfo(targetDirectory);
	try
	{
		// Process the list of files found in the directory.
		string[] fileEntries = Directory.GetFiles(targetDirectory, "*.xml*");

		foreach (string fileName in fileEntries)
		{
			Console.WriteLine(fileName);
		}
			

		// Recurse into subdirectories of this directory.
		string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
		foreach (string subdirectory in subdirectoryEntries)
			ProcessDirectory(subdirectory, fs, proj);

		//System.IO.DirectoryInfo dir = di.RootDirectory;
		//WalkDirectoryTree(dir);
	}
	catch (Exception ex)
	{
		Console.Write(ex.Message);
		Console.Write(ex.StackTrace);
	}
}

// Define other methods and classes here

public class FilePath
{
	public FilePath(string Filepath)
	{
		this.filepath = Filepath;
	}

	public string filepath { get; set; }
}