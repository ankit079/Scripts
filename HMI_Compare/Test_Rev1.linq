<Query Kind="Program" />

void Main()
{
	Product[] fruits1 = { new Product { Name = "apple", Code = 9 }, 
                       new Product { Name = "orange", Code = 4 },
                        new Product { Name = "lemon", Code = 12 } };

Product[] fruits2 = { new Product { Name = "apple", Code = 9 } };

//Get all the elements from the first array
//except for the elements from the second array.

IEnumerable<Product> except =
    fruits1.Except(fruits2, new ProductComparer());

foreach (var product in except)
    Console.WriteLine(product.Name + " " + product.Code);
	

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
	
		//Check whether the products' properties are equal.
		//return x.Code == y.Code && x.Name == y.Name;
	}

	
	
	// If Equals() returns true for a pair of objects 
	// then GetHashCode() must return the same value for these objects.

	public int GetHashCode(Product product)
	{
		return 2;
//		//Check whether the object is null
//		if (Object.ReferenceEquals(product, null)) return 0;
//
//		//Get hash code for the Name field if it is not null.
//		int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();
//
//		//Get hash code for the Code field.
//		int hashProductCode = product.Code.GetHashCode();
//
//		//Calculate the hash code for the product.
//		return hashProductName ^ hashProductCode;
	}

	public static bool PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
	{
		if (self != null && to != null)
		{
			Type type = typeof(T);
			List<string> ignoreList = new List<string>(ignore);
			foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
			{
				if (!ignoreList.Contains(pi.Name))
				{
					object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
					object toValue = type.GetProperty(pi.Name).GetValue(to, null);

					if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
					{
						return false;
					}
				}
			}
			return true;
		}
		return self == to;
	}

}