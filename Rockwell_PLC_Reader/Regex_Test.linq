<Query Kind="Program" />

void Main()
{
	string input = "Sts_Available is used nop() in this sequence for Valves and Val_NotifyAll used for drives XIC(AM1300001SQE_Step[1].X)XIC(AM1300001SQE_Step[1].AlarmHigh)OTE(AM1300001SQE_StepInAlarm[1]);";

	var matches = Regex.Matches(input, @"\(.*?\)", RegexOptions.None);
	
	var match = matches.Cast<Match>();
	foreach (var data in match)
	{
		data.Value.Dump();
	}
//	foreach (var data in matches)
//	{
//		Console.WriteLine(dat);
//	}
}

// Define other methods and classes here
