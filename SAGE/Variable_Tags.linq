<Query Kind="Program" />

void Main()
{
	List<string> result = new List<string>();
	string[] lines = System.IO.File.ReadAllLines(@"C:\Users\ankit.shah\Desktop\tags.txt");
	foreach (var item in lines)
	{
		if (item != null)
		{
			if (item.Contains(":") && item.Contains("/") && !item.Contains("."))
			{
				result.Add(Method2(item));
			}

			else if (item.Contains(":") && !item.Contains("/") && !item.Contains("."))
			{
				result.Add(Method3(item));
			}

			else if (!item.Contains(":") && item.Contains("/") && !item.Contains("."))
			{
				result.Add(Method1(item));
			}
			else if (item.Contains(":") && item.Contains(".") && !item.Contains("/"))
			{
				result.Add(Method4(item));
			}
		}
		else{
			Console.WriteLine("No method exists to convert this Tag {0}",item);
		}
	}
	result.Dump();
}

// Define other methods and classes here
// Pattern B12/1672 --->  B12{104}/8 

public string Method1(string text)
{
	string[] txt = text.Split('/');
	int lastChr = Convert.ToInt32(txt[1]) % 16;
	decimal middleChr = Math.Floor(Convert.ToDecimal(txt[1]) / 16);
	string newString = string.Concat(txt[0], "{", middleChr, "}", "/", lastChr);
	return newString;
}

// Pattern O:030/12 --->  O{30}/10 (Inputs and Outputs)

public string Method2(string text)
{
	char[] splitConditions = { ':', '/' };
	string[] txt = text.Split(splitConditions);
	int numMiddle = Convert.ToInt32(txt[1]);
	int numEnd = Convert.ToInt32(txt[2]);
	if (numEnd > 7)
	{
		numEnd = numEnd - 2;
	}
	string newString = string.Concat(txt[0], "{", numMiddle, "}", "/", numEnd);
	return newString;
}

// Define other methods and classes here
// Pattern N24:243 --->  N24{243}
// Integer Array of N24

public string Method3(string text)
{

	string[] txt = text.Split(':');
	string newString = string.Concat(txt[0], "{", txt[1], "}");
	return newString;
}

// Define other methods and classes here
// Pattern T4:0.PRE --->  T4{0}.PRE
// Timers

public string Method4(string text)
{
	char[] splitConditions = { ':', '.' };
	string[] txt = text.Split(splitConditions);
	string newString = string.Concat(txt[0], "{",txt[1], "}",".",txt[2]);
	return newString;
}

