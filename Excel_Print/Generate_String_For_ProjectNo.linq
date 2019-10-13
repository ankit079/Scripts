<Query Kind="Program" />

void Main()
{
	int[] arr = Enumerable.Range(1, 126).ToArray();
	string [] arrstring = new string [126];
	//arr.Dump();

	for (int i = 0; i < 126; i++)
	{
		if (arr[i] < 10)
		{
			arrstring[i] = arr[i].ToString();
			arrstring.SetValue(string.Concat("00", arrstring[i]), i);
		}

		else if (arr[i] > 9 && arr[i] < 100)
		{
			arrstring[i] = arr[i].ToString();
			arrstring.SetValue(string.Concat("0", arrstring[i]), i);
		}

		else if (arr[i] > 99 && arr[i] < 127)
		{
			arrstring[i] = arr[i].ToString();
		}
	}
		
	arrstring.Dump();
}

// Define other methods and classes here
