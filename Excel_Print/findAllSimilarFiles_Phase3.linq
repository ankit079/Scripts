<Query Kind="Program" />

static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();

void Main()
{
	string logpath = @"c:\temp\filepath.txt";
	int count = 0;
	// Get all drivers from this PC
	// string[] drives = System.Environment.GetLogicalDrives();
	if (File.Exists(logpath))
	{
		File.Delete(logpath);
	}
	using (FileStream fs = File.Create(logpath))
	{
		string[] dir = new string[1];
		
		dir[0] = @"J:\J11251_GAR1_PRELIMINARY_DESIGN_-GAR_(Water_Corporation)\Drawings and Documents\E80 Docs";
		foreach (var dr in dir)
		{
			if (Directory.Exists(dr))
			{
				ProcessDirectory(dr, fs, count);
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
}

public static void ProcessDirectory(string targetDirectory, FileStream fs, int count)
{
	System.IO.DriveInfo di = new System.IO.DriveInfo(targetDirectory);
	try
	{
		// Process the list of files found in the directory.
		string[] fileEntries = Directory.GetFiles(targetDirectory, "*.xlsx*");
		foreach (string fileName in fileEntries)
		ProcessPhase3Files(fileName,fs, count);

		// Recurse into subdirectories of this directory.
		string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
		foreach (string subdirectory in subdirectoryEntries)
		ProcessDirectory(subdirectory,fs, count);

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
public static int ProcessPhase3Files(string path, FileStream fs, int count)
{
	//Create the file.
	
	if(path.Contains("11251-QUO") && !path.Contains("Itemised Breakdown") && !path.Contains("Superseded") && !path.Contains("Superceded") && !path.Contains("Obsolete") && !path.Contains("Out of Scope"))
	{
		bool found = false;
		int length = path.Length - 8;
		string removeProjectNumberFromPath = path.Remove(0,length);
		for (int i = 71; i <= 126; i++)
		{
			// using String.Contains() Method 
			
			found = (removeProjectNumberFromPath.Contains(i.ToString()));
			//Console.WriteLine($"{(found == true ? "Found" : "Did not find")} {sites}");
			if(found)
			{
				AddText(fs, path);
				AddText(fs, "\r\n");
				Console.WriteLine("Processed file '{0}'.", path);
				found = false;
				count++;
				break;
			}		
		}
	}
	return count;
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