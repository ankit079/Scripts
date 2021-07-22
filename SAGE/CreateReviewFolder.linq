<Query Kind="Program">
  <NuGetReference>Microsoft.SharePoint2019.CSOM</NuGetReference>
  <Namespace>Microsoft.SharePoint.Client</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	string planSet = "";
	string siteNo = "81";
	createDrawingReviewFolder(siteNo, planSet);
}

public void createDrawingReviewFolder(string siteNo, string planSet)
{
	string dstUrl = "https://projects.sageautomation.com/p/68230/";
	using (ClientContext srcContext = new ClientContext(dstUrl))
	{
		Microsoft.SharePoint.Client.List list = srcContext.Web.Lists.GetByTitle("Engineering");
		Web web = srcContext.Web;
		srcContext.Load(web);
		srcContext.Load(list);
		srcContext.Load(list.RootFolder);
		srcContext.Load(list.RootFolder.Folders);
		srcContext.Load(list.RootFolder.Files);
		srcContext.ExecuteQuery();
		FolderCollection fcol = list.RootFolder.Folders;
		List<string> lstFile = new List<string>();
		foreach (Folder f in fcol)
		{
			if (f.Name == "Metro Engineering Sites")
			{
				srcContext.Load(f);
				srcContext.Load(f.Folders);
				srcContext.ExecuteQuery();
				FolderCollection folders = f.Folders;
				foreach (Microsoft.SharePoint.Client.Folder fol in folders)
				{
					if(fol.Name.Contains(siteNo))
					{
						string folderRelativeUrl = fol.Name; 
						string path = dstUrl + "Engineering/Metro Engineering Sites/" + folderRelativeUrl;
						Folder reviewFolder = web.GetFolderByServerRelativeUrl(dstUrl + "Engineering/Metro Engineering Sites/" + folderRelativeUrl + "/Drawings");			
						srcContext.Load(reviewFolder);
						srcContext.Load(reviewFolder.Folders);
						srcContext.ExecuteQuery();		
						reviewFolder.ServerRelativeUrl.Dump();		
						// Step 1: Create Review Folder
						CreateReviewFolder(reviewFolder,srcContext);
						// Step 2: Get File URL
						GetFileUrl(reviewFolder, srcContext);						
						// Step 3: Change the file naming convention		
						//string fileUrl = 
						//RenameDrawingFile(planSet, srcContext, "Test");						
					}
				}
			}
		}	
	}
}

public void GetFileUrl(Folder reviewFolder, ClientContext srcContext)
{
	//string url;
	try
	{
		foreach (var folder in reviewFolder.Folders)
		{
			if (folder.Name.Contains("Review"))
			{
				srcContext.Load(folder);
				srcContext.Load(folder.Files);
				srcContext.ExecuteQuery();


				foreach (var file in folder.Files)
				{
					file.ServerRelativeUrl.Dump();
				}
				break;
			}
		}
	}
	catch (Exception ex)
	{
		ex.Message.Dump();
	}
}

public void RenameDrawingFile(Microsoft.SharePoint.Client.File file, ClientContext ctx)
{
	ctx.Load(file.ListItemAllFields);
	ctx.ExecuteQuery();
	file.MoveTo(file.ListItemAllFields["FileDirRef"] + "/" + "Test", MoveOperations.Overwrite);
	ctx.ExecuteQuery();
}

public void CreateReviewFolder(Folder reviewFolder, ClientContext srcContext )
{
	try
	{		
		foreach (var folder in reviewFolder.Folders)
		{
			if (folder.Name.Contains("Review"))
			{				
				Console.WriteLine("Review folder exists");
			}	
			
			else if(folder.Name.Contains("Original"))
			{
			
				srcContext.Load(folder);
				srcContext.Load(folder.Files);
				srcContext.ExecuteQuery();
				MoveFile(folder);
					//RenameDrawingFile(file, srcContext);
				
			}
			
			else
			{
				var loadReviewFolder = reviewFolder.Folders.Add("Review");	
				srcContext.Load(loadReviewFolder);
				srcContext.ExecuteQuery();
				break;
			}
		}
	}
	catch (Exception ex)
	{
		ex.Message.Dump();
	}
}

public void MoveFile(Microsoft.SharePoint.Client.Folder folder) {

	foreach (var file in folder.Files)
	{
		file.Name.Dump();
		file.ServerRelativeUrl.Dump();
		var targetFileUrl = file.ServerRelativeUrl.Replace("Original", "Review");
		targetFileUrl.Dump();
		file.CopyTo(targetFileUrl, true);
	}
}