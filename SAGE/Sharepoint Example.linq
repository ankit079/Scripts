<Query Kind="Program">
  <NuGetReference>Microsoft.SharePointOnline.CSOM</NuGetReference>
  <Namespace>Microsoft.SharePoint.Client</Namespace>
  <Namespace>Microsoft.SharePoint.Client.Application</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Security</Namespace>
</Query>

void Main()
{
	//https://projects.sageautomation.com/_api/web
	
	ClientContext context = new ClientContext("https://projects.sageautomation.com/p/68230/Engineering/Forms/ByEngineeringPhase");
	//List list = context.Web.Lists.GetByTitle("Engineering");
	Web web = context.Web;
	context.Load(web);
	context.ExecuteQuery();
	web.Title.Dump();
}

// Define other methods and classes here
