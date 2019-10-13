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
		dir[0] = @"J:\J11251_GAR1_PRELIMINARY_DESIGN_-GAR_(Water_Corporation)\Drawings and Documents\E80 Docs";
		
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
	
	foreach(var item in proj)
	{
		item.Dump();
	}
}

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

public static void ProcessDirectory(string targetDirectory, FileStream fs, int count, List<ProjectInfo> proj)
{
	System.IO.DriveInfo di = new System.IO.DriveInfo(targetDirectory);
	try
	{
		// Process the list of files found in the directory.
		string[] fileEntries = Directory.GetFiles(targetDirectory, "*.xlsx*");

		foreach (string fileName in fileEntries)
		ProcessPhase2Files(fileName, fs, count, proj);

		// Recurse into subdirectories of this directory.
		string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
		foreach (string subdirectory in subdirectoryEntries)
		ProcessDirectory(subdirectory, fs, count,proj);

		//System.IO.DirectoryInfo dir = di.RootDirectory;
		//WalkDirectoryTree(dir);
	}
	catch (Exception ex)
	{
		Console.Write(ex.Message);
		Console.Write(ex.StackTrace);
	}
}

// Insert logic for processing found files here.
public static void ProcessPhase2Files(string path, FileStream fs, int count, List<ProjectInfo> proj)
{
	//Create the file.

	
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

			foreach(var no in proj_String)
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
}

public static string[] Generate_Project_String()
{
	int[] arr = Enumerable.Range(1, 126).ToArray();
	string[] arrstring = new string[126];
	//arr.Dump();

	for (int i = 0; i < 126; i++)
	{
		if (arr[i] < 10)
		{
			arrstring[i] = arr[i].ToString();
			arrstring.SetValue(string.Concat("00", arrstring[i]), i);
		}

		else if (arr[i] > 9 && arr[i] < 100)
		{
			arrstring[i] = arr[i].ToString();
			arrstring.SetValue(string.Concat("0", arrstring[i]), i);
		}

		else if (arr[i] > 99 && arr[i] < 127)
		{
			arrstring[i] = arr[i].ToString();
		}
	}
	return arrstring;
}

private static void AddText(FileStream fs, string value)
{
	byte[] info = new UTF8Encoding(true).GetBytes(value);
	fs.Write(info, 0, info.Length);
}

static void WalkDirectoryTree(System.IO.DirectoryInfo root)
{
	System.IO.FileInfo[] files = null;
	System.IO.DirectoryInfo[] subDirs = null;

	// First, process all the files directly under this folder
	try
	{
		files = root.GetFiles("*.xlsx*");
	}
	// This is thrown if even one of the files requires permissions greater
	// than the application provides.
	catch (UnauthorizedAccessException e)
	{
		// This code just writes out the message and continues to recurse.
		// You may decide to do something different here. For example, you
		// can try to elevate your privileges and access the file again.
		log.Add(e.Message);
	}

	catch (System.IO.DirectoryNotFoundException e)
	{
		Console.WriteLine(e.Message);
	}

	if (files != null)
	{
		foreach (System.IO.FileInfo fi in files)
		{
			// In this example, we only access the existing FileInfo object. If we
			// want to open, delete or modify the file, then
			// a try-catch block is required here to handle the case
			// where the file has been deleted since the call to TraverseTree().
			Console.WriteLine(fi.FullName);
		}

		// Now find all the subdirectories under this directory.
		subDirs = root.GetDirectories();

		foreach (System.IO.DirectoryInfo dirInfo in subDirs)
		{
			// Resursive call for each subdirectory.
			WalkDirectoryTree(dirInfo);
		}
	}
}