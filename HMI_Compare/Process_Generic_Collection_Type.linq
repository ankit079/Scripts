<Query Kind="Program" />

void Main()
{
	// Using GetType to obtain type information:  
	List<int> i = new List<int>();
	i.Add(2);
	i.Add(4);
	System.Type type = i.GetType();
	if(checkType(type))
	{
		Console.WriteLine("List object to be processed");
	}
}

public bool checkType(Type type)
{	
	//System.Console.WriteLine(type);

	if (type.FullName.Contains("System.Collections"))
	{
		return true;
	}
	return false;
}
// Define other methods and classes here