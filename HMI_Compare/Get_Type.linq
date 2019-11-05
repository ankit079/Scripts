<Query Kind="Program" />

void Main()
{
	// Using GetType to obtain type information:  
	List<int> i = new List<int>();
	i.Add(2);
	i.Add(4);
	System.Type type = i.GetType();
	System.Console.WriteLine(type);
	
	if(type.FullName.Contains("System.Collections"))
	{
		Console.WriteLine("This type is a List type");
	}
}

// Define other methods and classes here
