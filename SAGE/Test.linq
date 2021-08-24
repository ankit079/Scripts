<Query Kind="Program">
  <NuGetReference>Microsoft.Office.Interop.Word</NuGetReference>
  <NuGetReference>Microsoft.SharePoint2019.CSOM</NuGetReference>
  <Namespace>Microsoft.Office.Interop.Word</Namespace>
  <Namespace>Microsoft.SharePoint.Client</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	bool fcd = false;
	string sourceFileLocation = @"https://projects.sageautomation.com/p/68230/Engineering/Metro%20Engineering%20Sites/Site%2050-%20Wharf%20St%20Drainage%20Cannington";
	CopySafetyFile(sourceFileLocation,"BPS");

}

public void makeIFC(string path)
{

	// PDF the ESR Report

	// server running SharePoint.
	ClientContext context = new ClientContext(new Uri(path));
	context.AuthenticationMode = ClientAuthenticationMode.Default;
	context.Credentials = CredentialCache.DefaultNetworkCredentials;
	// The SharePoint web at the URL.
	Web web = context.Web;

	// Retrieve all lists from the server.
	// For each list, retrieve Title and Id.
	   context.Load(web.Lists,
				 lists => lists.Include(list => list.Title,
										list => list.Id));

context.ExecuteQuery();
	// Enumerate the web.Lists.
	foreach (Microsoft.SharePoint.Client.List list in web.Lists)
	{
		list.Title.Dump();
	}

	// Assume the web has a list named "Announcements".
	//Microsoft.SharePoint.Client.List projectsList = context.Web.Lists.GetByTitle("Projects");
	//CamlQuery query = CamlQuery.CreateAllItemsQuery(100);
	//ListItemCollection items = projectsList.GetItems(query);
	// Retrieve all items in the ListItemCollection from List.GetItems(Query).
	//context.Load(items);

	// Execute query.
	//context.ExecuteQuery();
//
//	foreach (ListItem listItem in items.Take(5))
//	{
//		listItem.Dump();
//	}

	// Copying the site audit from Prelim


	// PDF the Power Calc Report


	// PDF the SID Report and Register



	// PDF the Drawings. This program copies the specified planset drawings and inserts them in the folder


	// Optional FCD PDF if selected

}

public void makeDD(){
	
	
}

public void convertWordToPDF(string path)
{
	
	// Create a new Microsoft Word application object

	var word = new Microsoft.Office.Interop.Word.Application();
	object oMissing = System.Reflection.Missing.Value;

	// Get list of Word files in specified directory
	DirectoryInfo dirInfo = new DirectoryInfo(path);
	FileInfo[] wordFiles = dirInfo.GetFiles("*.doc");
	word.Visible = false;
	word.ScreenUpdating = false;

	foreach (var wordFile in wordFiles)
	{

		// Cast as Object for word Open method
		Object filename = (Object)wordFile.FullName;
		filename.Dump();
	}
}

public void CopyFolder()
{
	string sourceFolder = @"https://projects.sageautomation.com/p/68230/Engineering/Metro%20Engineering%20Sites/Site%2068-%20Kalamunda%20Tank%20Repeater/Drawings/Review/HL%20Tank";
	string dstUrl = "https://projects.sageautomation.com";

	string destFolder = dstUrl + "/p/68230/Engineering/Metro%20Engineering%20Sites/Site%2068-%20Kalamunda%20Tank%20Repeater/Drawings/Review/Test"; ;
	using (ClientContext srcContext = new ClientContext(dstUrl))
	{
		MoveCopyOptions option = new MoveCopyOptions();
		option.KeepBoth = true;
		MoveCopyUtil.CopyFolder(srcContext, sourceFolder, destFolder, option);
		srcContext.ExecuteQuery();
	}
}

public void CopySafetyFile(string sourceFileLocation, string siteType)
{
	string dstUrl = "https://projects.sageautomation.com";

	string destFileLocation = dstUrl + "/p/68230/Engineering/Metro%20Engineering%20Sites/Site%2050-%20Wharf%20St%20Drainage%20Cannington";
	using (ClientContext srcContext = new ClientContext(dstUrl))
	{
		var web = srcContext.Web;
	
		srcContext.Load(web);
		srcContext.Load(web);
		//MoveCopyOptions option = new MoveCopyOptions();
		//option.KeepBoth = true;
		//MoveCopyUtil.CopyFile(srcContext, sourceFileLocation, destFileLocation, false,option);
		srcContext.ExecuteQuery();
			var files = web.GetFolderByServerRelativeUrl(sourceFileLocation).Files.Dump();
	}
}
// Define other methods and classes here