<Query Kind="Program">
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
<<<<<<< HEAD
	// Test Setup
	var testGroup1 = new Group();
	testGroup1.Name = "TestGroup1";
	testGroup1.IntegerCollection = new List<int>();
	testGroup1.IntegerCollection.Add(2);
	testGroup1.IntegerCollection.Add(4);

	var testGroup2 = new Group();
	testGroup2.Name = "TestGroup2";
	testGroup2.IntegerCollection = new List<int>();
	testGroup2.IntegerCollection.Add(3);
	testGroup2.IntegerCollection.Add(4);
	
	// 
	PublicInstancePropertiesEqual<Group>(testGroup1,testGroup2,"Test");
}

public bool checkType(Type i)
{
	//System.Type type = i.GetType();
=======
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
>>>>>>> 9590a281b7a03a9b700e3bc46dbe5effadcb96aa
	//System.Console.WriteLine(type);

	if (i.FullName.Contains("System.Collections"))
	{
		return true;
	}
	return false;
}
<<<<<<< HEAD
// Define other methods and classes here

public void PublicInstancePropertiesEqual<T>(T dev, T site, string objectName) where T : class
{
	if (dev != null && site != null)
	{
		Type type = typeof(T);

		foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
		{
			if (!checkType(pi.PropertyType))
			{
				object devValue = type.GetProperty(pi.Name).GetValue(dev, null);
				object siteValue = type.GetProperty(pi.Name).GetValue(site, null);

				if (devValue != siteValue && (devValue == null || !devValue.Equals(siteValue)))
				{
					Console.WriteLine(objectName.ToString());
					Console.WriteLine("The property {0} value changed from {1} to {2}", pi.Name.ToString(), devValue.ToString(), siteValue.ToString());
				}
			}
			else if (checkType(pi.PropertyType))
			{
				// Process List<T> System.Collections Object
				
				Console.WriteLine("This has been hit");
			}
		}
	}
}

[XmlRoot(ElementName = "group")]
public class Group
{
	[XmlAttribute(AttributeName = "name")]
	public string Name { get; set; }
	public List<int> IntegerCollection { get; set; }
}
=======
// Define other methods and classes here
>>>>>>> 9590a281b7a03a9b700e3bc46dbe5effadcb96aa
