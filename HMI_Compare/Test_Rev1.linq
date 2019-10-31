<Query Kind="Program" />

void Main()
{
	Product[] ListofClass1 = { new Product { Name = "apple", Code = 9 },
					   new Product { Name = "orange", Code = 4 },
						new Product { Name = "lemon", Code = 12 } };

	Product[] ListofClass2 = { new Product { Name = "apple", Code = 8 } };

	Product singleClass1 = new Product { Name = "apple", Code = 9 };
	Product singleClass2 = new Product { Name = "apple", Code = 8 };

	Console.WriteLine("The class is Product with unique ID {0}", singleClass1.Name);
	Console.WriteLine("---------------------------------------------------------");
	PublicInstancePropertiesEqual<Product>(singleClass1, singleClass2);

	// Method 1

	//Get all the elements from the first array
	//except for the elements from the second array.

	//IEnumerable<Product> except =
	//    fruits1.Except(fruits2, new ProductComparer());
	//
	//foreach (var product in except)
	//    Console.WriteLine(product.Name + " " + product.Code);

	// Method 2

	//	foreach(var p in fruits1)
	//	{
	//		foreach(var q in fruits2)
	//		{
	//			var result = PublicInstancePropertiesEqual<Product>(p,q);			
	//		}
	//	}
}

// Define other methods and classes here
public class Product
{
	public string Name { get; set; }
	public int Code { get; set; }
}

// Custom comparer for the Product class
class ProductComparer : IEqualityComparer<Product>
{
	// Products are equal if their names and product numbers are equal.
	public bool Equals(Product x, Product y)
	{

		//Check whether the compared objects reference the same data.
		if (Object.ReferenceEquals(x, y)) return true;

		//Check whether any of the compared objects is null.
		if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
			return false;
		//Check whether the products' properties are equal.
		return x.Name == y.Name && x.Code == y.Code;
	}

	// If Equals() returns true for a pair of objects 
	// then GetHashCode() must return the same value for these objects.

	public int GetHashCode(Product product)
	{
		//return 2;
		//Check whether the object is null
		if (Object.ReferenceEquals(product, null)) return 0;

		//Get hash code for the Name field if it is not null.
		int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();

		//Get hash code for the Code field.
		int hashProductCode = product.Code.GetHashCode();

		//Calculate the hash code for the product.
		return hashProductName ^ hashProductCode;
	}
}

public static void PublicInstancePropertiesEqual<T>(T dev, T site) where T : class
{
	if (dev != null && site != null)
	{
		Type type = typeof(T);

		string className = type.Name;

		foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
		{
			object devValue = type.GetProperty(pi.Name).GetValue(dev, null);
			object siteValue = type.GetProperty(pi.Name).GetValue(site, null);

			if (devValue != siteValue && (devValue == null || !devValue.Equals(siteValue)))
			{
				Console.WriteLine("The property {0} value changed from {1} to {2}", pi.Name.ToString(), devValue.ToString(), siteValue.ToString());
			}
		}
	}
}

