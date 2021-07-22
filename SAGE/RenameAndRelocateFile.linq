<Query Kind="Program">
  <NuGetReference>Microsoft.SharePoint2019.CSOM</NuGetReference>
  <Namespace>Microsoft.SharePoint.Client</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	string planSet = "GY35";
	string siteNo = "55";
	getFiletobeMoved(siteNo, planSet);
}

public void getFiletobeMoved(string siteNo, string planSet)
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
						Folder drawingFolder = web.GetFolderByServerRelativeUrl(dstUrl + "Engineering/Metro Engineering Sites/" + folderRelativeUrl + "/Drawings");			
						srcContext.Load(drawingFolder);
						srcContext.Load(drawingFolder.Folders);
						srcContext.ExecuteQuery();		
						drawingFolder.ServerRelativeUrl.Dump();	
						
						// Step 0: Create IFC Folder
						Folder submissionFolder =  web.GetFolderByServerRelativeUrl(dstUrl + "Engineering/Metro Engineering Sites/" + folderRelativeUrl + "/Design Submission");
						CreateIFCFolder(web,srcContext,submissionFolder);
						
						// Step 1: Get File URL
						var file = GetFileUrl(drawingFolder, srcContext, planSet);

						// Step 2: Move the file to Design Submission Folder
						var destFile = CopyDrawingFile(file, srcContext);
						
						// Step 3: Rename the file
						RenameFile(destFile);
					}
				}
			}
		}	
	}
}

public void RenameFile(string file){
	
	
}

public Microsoft.SharePoint.Client.File GetFileUrl(Folder reviewFolder, ClientContext srcContext, string planSet)
{
	Microsoft.SharePoint.Client.File latestFile = null;
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
					if(file.Name.Contains(planSet) && file.TimeLastModified.Date.Date == DateTime.Today.Date && !file.Name.Contains("Review"))
					{
						latestFile = file;
					}	
				}				
				break;
			}

		}

	}
	catch (Exception ex)
	{
		ex.Message.Dump();
	}
	return latestFile;
}

public string CopyDrawingFile(Microsoft.SharePoint.Client.File file, ClientContext ctx)
{
	ctx.Load(file);
	ctx.ExecuteQuery();
	file.ServerRelativeUrl.Dump();
	var targetFileUrl = file.ServerRelativeUrl.Replace("Drawings/Review", "Design Submission");
	file.CopyTo(targetFileUrl, true);
	ctx.ExecuteQuery();
	return targetFileUrl;
}

public void CreateIFCFolder(Web web, ClientContext srcContext,Folder submissionFolder)
{
	bool ifcFolderExists = false;
	try
	{
		foreach (var folder in submissionFolder.Folders)
		{
			if (folder.Name.Contains("IFC"))
			{
				ifcFolderExists = true;
			}
		}

		if (!ifcFolderExists)
		{
			var loadReviewFolder = submissionFolder.Folders.Add("RevA3IFC");
			srcContext.Load(loadReviewFolder);
			srcContext.ExecuteQuery();			
		}
	}
	catch (Exception ex)
	{
		ex.Message.Dump();
	}
}
