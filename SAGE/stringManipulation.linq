<Query Kind="Program" />

void Main()
{
	string testString = "12414-45-9";
	int firstPosition = testString.IndexOf("-");
	testString.Substring(0,firstPosition).Dump();
	
	string newString = 	testString.Substring(firstPosition+1);
	int lastPosition = newString.LastIndexOf("-");
	newString.Substring(0,lastPosition).Dump();
	newString.Substring(lastPosition+1).Dump();
}

// Define other methods and classes here
