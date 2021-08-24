<Query Kind="Program" />

void Main()
{
	
}

// Define other methods and classes here


//public void ConvertWordToPDF(string input, string output, WdSaveFormat format){
//	// Create an instance of Word.exe
//	_Application oWord = new Microsoft.Office.Interop.Word.Application();
//	oWord.Visible = false;
//
//				// Interop requires objects.
//	object oMissing = System.Reflection.Missing.Value;
//	object isVisible = true;
//	object readOnly = true;     
//	object oInput = input;
//	object oOutput = output;
//	object oFormat = format;
//
//	// Load a document into our instance of word.exe
//	_Document oDoc = oWord.Documents.Open(
//		ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing,
//		ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//		ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing
//		);
//
//	// Make this document the active document.
//	oDoc.Activate();
//
//	// Save this document using Word
//	oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing,
//		ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//		ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
//		);
//
//	// Always close Word.exe.
//	oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
//}