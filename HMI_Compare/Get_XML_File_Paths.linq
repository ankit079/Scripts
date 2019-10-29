<Query Kind="Program" />

static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
void Main()
{
	string logpath = @"c:\temp\filepath.txt";
	List<ProjectInfo> proj = new List<ProjectInfo>();
	// Get all drivers from this PC
	// string[] drives = System.Environment.GetLogicalDrives();
	if (File.Exists(logpath))
	{
		File.Delete(logpath);
	}

	using (FileStream fs = File.Create(logpath))
	{
		string[] dir = new string[1];
		int count = 0;
		dir[0] = @"C:\Users\shaha\Desktop\TLA_HMI_S1_20191027_SiteBackup\Page xml exports";

		foreach (var dr in dir)
		{
			if (Directory.Exists(dr))
			{
				ProcessDirectory(dr, fs, count, proj);
			}
		}

		// Write out all the files that could not be processed.
		Console.WriteLine("Files with restricted access:");
		foreach (string s in log)
		{
			Console.WriteLine(s);
		}
		// Keep the console window open in debug mode.

		Console.Write($"Total Files found {count.ToString()}");
		//Console.WriteLine("Press any key");
	}

	foreach (var item in proj)
	{
		item.Dump();
	}
}


public static void ProcessDirectory(string targetDirectory, FileStream fs, int count, List<ProjectInfo> proj)
{
	System.IO.DriveInfo di = new System.IO.DriveInfo(targetDirectory);
	try
	{
		// Process the list of files found in the directory.
		string[] fileEntries = Directory.GetFiles(targetDirectory, "*.xml*");

		foreach (string fileName in fileEntries)
		{
			//ProcessPhase2Files(fileName, fs, count, proj);
		}
			

		// Recurse into subdirectories of this directory.
		string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
		foreach (string subdirectory in subdirectoryEntries)
			ProcessDirectory(subdirectory, fs, count, proj);

		//System.IO.DirectoryInfo dir = di.RootDirectory;
		//WalkDirectoryTree(dir);
	}
	catch (Exception ex)
	{
		Console.Write(ex.Message);
		Console.Write(ex.StackTrace);
	}
}

public static void ProcessPhase2Files(string path, FileStream fs, int count, List<ProjectInfo> proj)
{
	//Create the file.
/*

	if (path.Contains("11251-QUO") && !path.Contains("Itemised Breakdown") && !path.Contains("Superseded") && !path.Contains("Superceded") && !path.Contains("Obsolete") && !path.Contains("Out of Scope"))
	{
		bool found = false;
		int length = path.Length - 8;
		string removeProjectNumberFromPath = path.Remove(0, length);
		//string projectNo = removeProjectNumberFromPath.Remove(3);
		var proj_String = Generate_Project_String();
		//		for (int i = 1; i <= 126; i++)
		//		{
		// using String.Contains() Method 

		foreach (var no in proj_String)
		{
			found = (removeProjectNumberFromPath.Contains(no)) || (removeProjectNumberFromPath.Equals(" Option2"));
			if (found)
			{
				AddText(fs, path);
				AddText(fs, "\r\n");
				Console.WriteLine("Processed file '{0}'.", path);
				found = false;
				count++;
				proj.Add(new ProjectInfo(path, no));
				//break;
			}
		}

		//Console.WriteLine($"{(found == true ? "Found" : "Did not find")} {sites}");

		//		}	
	}
	*/
}

// Define other methods and classes here

public class ProjectInfo
{
	public ProjectInfo(string Filepath, string ProjectNo)
	{
		this.filepath = Filepath;
		this.projectNo = ProjectNo;
	}

	public string filepath { get; set; }
	public string projectNo { get; set; }
}
