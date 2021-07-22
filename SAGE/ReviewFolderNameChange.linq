<Query Kind="Program">
  <NuGetReference>Microsoft.Office.Word.Server</NuGetReference>
  <NuGetReference>Microsoft.SharePoint2019.CSOM</NuGetReference>
  <Namespace>Microsoft.Office.Interop.Word</Namespace>
  <Namespace>Microsoft.SharePoint.Client</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Microsoft.Office.Word.Server.Conversions</Namespace>
</Query>

void Main()
{
	bool fcd = false;
	pdfESR("60");
}

public void pdfESR(string siteNo)
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
						string path = dstUrl + "Engineering/" + folderRelativeUrl;
						path.Dump();
						Folder reviewFolder = web.GetFolderByServerRelativeUrl(dstUrl + "Engineering/Metro Engineering Sites/" + folderRelativeUrl);
						srcContext.Load(reviewFolder);
						srcContext.Load(reviewFolder.Files);
						srcContext.ExecuteQuery();		
						try
						{
							foreach (var file in reviewFolder.Files)
							{
								if (file.Name.Contains("ESR"))
								{
									string input = path + "/" + file.Name;
									string fileName = file.Name;
									string output = "C/Test/" + fileName;
									//ConvertSharepointWordToPDF(input, output.Replace("docx", "pdf"));
								}
							}
						}
						catch (Exception ex)
						{
							ex.Message.Dump();
						}
					}
				}
			}
		}	
	}
}

//void ConvertSharepointWordToPDF(string input, string output, Web web)
//{
//	Folder inputFolder = web.GetFolderByServerRelativeUrl();
//	SPFolder outputFolder = web.GetFolder("Shared Documents/PDF Documents");
//	ConversionJobSettings jobSettings=new ConversionJobSettings();
//	 jobSettings.OutputFormat = SaveFormat.PDF;
//	 ConversionJob pdfConversion = new ConversionJob("Word Automation Services", jobSettings);
//	   pdfConversion.UserToken = web.CurrentUser;
//	 pdfConversion.AddFolder(
//	       pdfConversion.Start();
//}