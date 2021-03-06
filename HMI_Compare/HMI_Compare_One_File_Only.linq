<Query Kind="Program">
  <Connection>
    <ID>e0e1a2f9-3e46-474d-8a95-a03f59555e6e</ID>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <Database>master</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{	
	//RetrieveLatestFileData();
	// Compare Logic between two files
	
//	var difference1 = RetrieveLatestFileData().Except(RetrievePreviousFileData()).ToList();
//	var difference2 = RetrievePreviousFileData().Except(RetrieveLatestFileData()).ToList();
//	Console.WriteLine("Comparing the latest file with the previous file for difference");
//	difference1.Dump();
//	if(difference1 != null)
//	{
//		// Do Something with IRef ID to find the corelation
//		
//	}
//	Console.WriteLine("Comparing the previous file with the latest file for difference");
//	difference2.Dump();
}

public class DisplaySettingsComparer: System.Collections.Generic.IEqualityComparer<DisplaySettings>
{
	public bool Equals(DisplaySettings x, DisplaySettings y)
	{
		//Check whether the compared objects reference the same data.
		if(Object.ReferenceEquals(x,y)) return true;
		
		//Check whether any of the compared objects is null.
		else if(Object.ReferenceEquals(x,null) || Object.ReferenceEquals(y,null))
		return false;

		//Check whether the products' properties are equal.
		return x.Position == y.Position &&

	}

	public int GetHashCode(DisplaySettings product)
	{
		return 2;
//		//Check whether the object is null
//		if (Object.ReferenceEquals(product, null)) return 0;
//	
//		var properties = getPropertyInfo();
//		
//		foreach(var prop in properties)
//		{
//		}
//		//Get hash code for the Name field if it is not null.
//		int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();
//
//		//Get hash code for the Code field.
//		int hashProductCode = product.Code.GetHashCode();
//
//		//Calculate the hash code for the product.
//		return hashProductName ^ hashProductCode;
	}
}

public static PropertyInfo[] getPropertyInfo()
{
	string [] properties;
	// Refer to https://stackoverflow.com/questions/13985261/dynamically-access-class-and-its-property-in-c-sharp
	// get all public static properties of MyClass type
	PropertyInfo[] propertyInfos;
	propertyInfos = typeof(DisplaySettings).GetProperties();
	// sort properties by name
//	Array.Sort(propertyInfos,
//			delegate (PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
//			{ return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });

	// write property names
//	foreach (PropertyInfo propertyInfo in propertyInfos)
//	{
//		Console.WriteLine(propertyInfo.Name);
//	}
	return propertyInfos;
}

public void RetrieveData(string filename)
{
	var retrievedData = new List<String>();
	string xmlFile = FilePathFromDesktop(filename);
	XElement config = XElement.Load(xmlFile);

	var deserializedObject = Deserializer.FromXElement<Gfx>(config);
	
	foreach(var grp in deserializedObject.Group)
	{
		grp.Name.Dump();
	}
	deserializedObject.Dump();
	
//	var result = (
//			from c in config.DescendantNodes().OfType<XElement>()
//			where c.Name == "Program"
//			select c
//				);
//
//	foreach (XElement e in result)
//	{
//		if (e.FirstAttribute.Value == "DutyControl")
//		{
//			var selectedProgram = e;
//			var routines = (
//		from c in selectedProgram.DescendantNodes().OfType<XElement>()
//		where c.Name == "Routine"
//		select c
//			);
//
//			foreach (var routine in routines)
//			{
//				if (routine.FirstAttribute.Value == "PP1500002ABVSD_Duty")
//				{
//					routine.Dump();
//					var deserializedObject = Deserializer.FromXElement<Routine>(routine);
//
//					// Retrieve IREF data
//					foreach (var iref in deserializedObject.FBDContent.Sheet.IRef)
//					{
//						retrievedData.Add(iref.Operand);
//					}
//					// Retrieve Block data
//					foreach (var block in deserializedObject.FBDContent.Sheet.Block)
//					{
//						retrievedData.Add(block.Operand);
//					}
//				}
//			}
//		}
//
//	}
//	return retrievedData;
}

public void RetrieveLatestFileData()
{		
	// Retrieve Latest File Data
	string filename = @"LoadXML\HMI_Compare_Tool\1600_Hydro_PLS_Evaporation_MVR_Fan.xml";
	RetrieveData(filename);
}

//public List<String> RetrievePreviousFileData()
//{	
//	// Retrieve Previous File Data
//	string filename = @"LoadXML\PLC_Compare_Tool\HYDR1A_BHG_20191003.L5X";
//	return RetrieveData(filename);
//}

// Read from desktop a perticular file
private string FilePathFromDesktop(string fileName)
{
	string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	string fullName = System.IO.Path.Combine(desktopPath, fileName);
	return fullName;
}

// https://stackoverflow.com/questions/8373552/serialize-an-object-to-xelement-and-deserialize-it-in-memory 
public static class Deserializer
{
	public static T FromXElement<T>(this XElement xElement)
	{
		var xmlSerializer = new XmlSerializer(typeof(T));
		return (T)xmlSerializer.Deserialize(xElement.CreateReader());
	}
}

// Serialize to XElement - NOT USED --
public static class Serializer
{
	public static XElement ToXElement<T>(this object obj)
	{
		using (var memoryStream = new MemoryStream())
		{
			using (TextWriter streamWriter = new StreamWriter(memoryStream))
			{
				var xmlSerializer = new XmlSerializer(typeof(T));
				xmlSerializer.Serialize(streamWriter, obj);
				return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
			}
		}
	}
}

// DTO Object Parent Object is Routine

	[XmlRoot(ElementName = "displaySettings")]
	public class DisplaySettings
	{
		[XmlAttribute(AttributeName = "displayType")]
		public string DisplayType { get; set; }
		[XmlAttribute(AttributeName = "position")]
		public string Position { get; set; }
		[XmlAttribute(AttributeName = "positionX")]
		public string PositionX { get; set; }
		[XmlAttribute(AttributeName = "positionY")]
		public string PositionY { get; set; }
		[XmlAttribute(AttributeName = "securityCode")]
		public string SecurityCode { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "titleBar")]
		public string TitleBar { get; set; }
		[XmlAttribute(AttributeName = "maximumTagUpdateRate")]
		public string MaximumTagUpdateRate { get; set; }
		[XmlAttribute(AttributeName = "focusHighlightColor")]
		public string FocusHighlightColor { get; set; }
		[XmlAttribute(AttributeName = "disableFocusHighlight")]
		public string DisableFocusHighlight { get; set; }
		[XmlAttribute(AttributeName = "size")]
		public string Size { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "allowMultipleRunningCopies")]
		public string AllowMultipleRunningCopies { get; set; }
		[XmlAttribute(AttributeName = "cacheAfterDisplaying")]
		public string CacheAfterDisplaying { get; set; }
		[XmlAttribute(AttributeName = "sizeToMainWindow")]
		public string SizeToMainWindow { get; set; }
		[XmlAttribute(AttributeName = "showLastAcquiredValue")]
		public string ShowLastAcquiredValue { get; set; }
		[XmlAttribute(AttributeName = "TrackScreenForNavigation")]
		public string TrackScreenForNavigation { get; set; }
		[XmlAttribute(AttributeName = "TrackName")]
		public string TrackName { get; set; }
		[XmlAttribute(AttributeName = "allowResizing")]
		public string AllowResizing { get; set; }
		[XmlAttribute(AttributeName = "whenResized")]
		public string WhenResized { get; set; }
		[XmlAttribute(AttributeName = "beepOnPress")]
		public string BeepOnPress { get; set; }
		[XmlAttribute(AttributeName = "highlightWhenCursorPassesOver")]
		public string HighlightWhenCursorPassesOver { get; set; }
		[XmlAttribute(AttributeName = "interactiveHighlightColor")]
		public string InteractiveHighlightColor { get; set; }
		[XmlAttribute(AttributeName = "displayOnScreenKeyboard")]
		public string DisplayOnScreenKeyboard { get; set; }
		[XmlAttribute(AttributeName = "allowButtonActionOnError")]
		public string AllowButtonActionOnError { get; set; }
		[XmlAttribute(AttributeName = "fieldNotSelectedTextColor")]
		public string FieldNotSelectedTextColor { get; set; }
		[XmlAttribute(AttributeName = "fieldNotSelectedFillColor")]
		public string FieldNotSelectedFillColor { get; set; }
		[XmlAttribute(AttributeName = "fieldSelectedTextColor")]
		public string FieldSelectedTextColor { get; set; }
		[XmlAttribute(AttributeName = "fieldSelectedFillColor")]
		public string FieldSelectedFillColor { get; set; }
		[XmlAttribute(AttributeName = "fieldInErrorNotSelectedTextColor")]
		public string FieldInErrorNotSelectedTextColor { get; set; }
		[XmlAttribute(AttributeName = "fieldInErrorNotSelectedFillColor")]
		public string FieldInErrorNotSelectedFillColor { get; set; }
		[XmlAttribute(AttributeName = "fieldInErrorSelectedTextColor")]
		public string FieldInErrorSelectedTextColor { get; set; }
		[XmlAttribute(AttributeName = "fieldInErrorSelectedFillColor")]
		public string FieldInErrorSelectedFillColor { get; set; }
		[XmlAttribute(AttributeName = "startupCommand")]
		public string StartupCommand { get; set; }
		[XmlAttribute(AttributeName = "shutdownCommand")]
		public string ShutdownCommand { get; set; }
		[XmlAttribute(AttributeName = "useGradientStyle")]
		public string UseGradientStyle { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
	}

	[XmlRoot(ElementName = "line")]
	public class Line
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "wallpaper")]
		public string Wallpaper { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "line")]
		public string _line { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "lineStyle")]
		public string LineStyle { get; set; }
		[XmlAttribute(AttributeName = "lineWidth")]
		public string LineWidth { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
	}

	[XmlRoot(ElementName = "text")]
	public class Text
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "wallpaper")]
		public string Wallpaper { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "wordWrap")]
		public string WordWrap { get; set; }
		[XmlAttribute(AttributeName = "sizeToFit")]
		public string SizeToFit { get; set; }
		[XmlAttribute(AttributeName = "alignment")]
		public string Alignment { get; set; }
		[XmlAttribute(AttributeName = "fontFamily")]
		public string FontFamily { get; set; }
		[XmlAttribute(AttributeName = "fontSize")]
		public string FontSize { get; set; }
		[XmlAttribute(AttributeName = "bold")]
		public string Bold { get; set; }
		[XmlAttribute(AttributeName = "italic")]
		public string Italic { get; set; }
		[XmlAttribute(AttributeName = "underline")]
		public string Underline { get; set; }
		[XmlAttribute(AttributeName = "strikethrough")]
		public string Strikethrough { get; set; }
		[XmlAttribute(AttributeName = "caption")]
		public string Caption { get; set; }
		[XmlElement(ElementName = "animations")]
		public Animations Animations { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
	}

	[XmlRoot(ElementName = "image")]
	public class Image
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
		[XmlAttribute(AttributeName = "imageBackStyle")]
		public string ImageBackStyle { get; set; }
		[XmlAttribute(AttributeName = "imageBackColor")]
		public string ImageBackColor { get; set; }
		[XmlAttribute(AttributeName = "imageColor")]
		public string ImageColor { get; set; }
		[XmlAttribute(AttributeName = "imageBlink")]
		public string ImageBlink { get; set; }
		[XmlAttribute(AttributeName = "description")]
		public string Description { get; set; }
		[XmlAttribute(AttributeName = "imageName")]
		public string ImageName { get; set; }
		[XmlElement(ElementName = "animations")]
		public Animations Animations { get; set; }
	}

	[XmlRoot(ElementName = "caption")]
	public class Caption
	{
		[XmlAttribute(AttributeName = "fontFamily")]
		public string FontFamily { get; set; }
		[XmlAttribute(AttributeName = "fontSize")]
		public string FontSize { get; set; }
		[XmlAttribute(AttributeName = "bold")]
		public string Bold { get; set; }
		[XmlAttribute(AttributeName = "italic")]
		public string Italic { get; set; }
		[XmlAttribute(AttributeName = "underline")]
		public string Underline { get; set; }
		[XmlAttribute(AttributeName = "strikethrough")]
		public string Strikethrough { get; set; }
		[XmlAttribute(AttributeName = "caption")]
		public string _caption { get; set; }
		[XmlAttribute(AttributeName = "color")]
		public string Color { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "alignment")]
		public string Alignment { get; set; }
		[XmlAttribute(AttributeName = "wordWrap")]
		public string WordWrap { get; set; }
		[XmlAttribute(AttributeName = "blink")]
		public string Blink { get; set; }
	}

	[XmlRoot(ElementName = "imageSettings")]
	public class ImageSettings
	{
		[XmlAttribute(AttributeName = "imageName")]
		public string ImageName { get; set; }
		[XmlAttribute(AttributeName = "alignment")]
		public string Alignment { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "color")]
		public string Color { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "scaled")]
		public string Scaled { get; set; }
		[XmlAttribute(AttributeName = "blink")]
		public string Blink { get; set; }
		[XmlAttribute(AttributeName = "imageReference")]
		public string ImageReference { get; set; }
	}

	[XmlRoot(ElementName = "state")]
	public class State
	{
		[XmlElement(ElementName = "caption")]
		public Caption Caption { get; set; }
		[XmlElement(ElementName = "imageSettings")]
		public ImageSettings ImageSettings { get; set; }
		[XmlAttribute(AttributeName = "stateId")]
		public string StateId { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "borderColor")]
		public string BorderColor { get; set; }
		[XmlAttribute(AttributeName = "patternColor")]
		public string PatternColor { get; set; }
		[XmlAttribute(AttributeName = "patternStyle")]
		public string PatternStyle { get; set; }
		[XmlAttribute(AttributeName = "blink")]
		public string Blink { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "states")]
	public class States
	{
		[XmlElement(ElementName = "state")]
		public List<State> State { get; set; }
	}

	[XmlRoot(ElementName = "connection")]
	public class Connection
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "expression")]
		public string Expression { get; set; }
	}

	[XmlRoot(ElementName = "connections")]
	public class Connections
	{
		[XmlElement(ElementName = "connection")]
		public List<Connection> Connection { get; set; }
	}

	[XmlRoot(ElementName = "multistateIndicator")]
	public class MultistateIndicator
	{
		[XmlElement(ElementName = "states")]
		public States States { get; set; }
		[XmlElement(ElementName = "connections")]
		public Connections Connections { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "borderStyle")]
		public string BorderStyle { get; set; }
		[XmlAttribute(AttributeName = "borderUsesBackColor")]
		public string BorderUsesBackColor { get; set; }
		[XmlAttribute(AttributeName = "borderWidth")]
		public string BorderWidth { get; set; }
		[XmlAttribute(AttributeName = "description")]
		public string Description { get; set; }
		[XmlAttribute(AttributeName = "shape")]
		public string Shape { get; set; }
		[XmlAttribute(AttributeName = "triggerType")]
		public string TriggerType { get; set; }
		[XmlAttribute(AttributeName = "currentStateId")]
		public string CurrentStateId { get; set; }
		[XmlAttribute(AttributeName = "captionOnBorder")]
		public string CaptionOnBorder { get; set; }
		[XmlAttribute(AttributeName = "setLastStateId")]
		public string SetLastStateId { get; set; }
	}

	[XmlRoot(ElementName = "command")]
	public class Command
	{
		[XmlAttribute(AttributeName = "pressAction")]
		public string PressAction { get; set; }
		[XmlAttribute(AttributeName = "repeatAction")]
		public string RepeatAction { get; set; }
		[XmlAttribute(AttributeName = "releaseAction")]
		public string ReleaseAction { get; set; }
		[XmlAttribute(AttributeName = "repeatRate")]
		public string RepeatRate { get; set; }
	}

	[XmlRoot(ElementName = "up")]
	public class Up
	{
		[XmlElement(ElementName = "caption")]
		public Caption Caption { get; set; }
		[XmlElement(ElementName = "imageSettings")]
		public ImageSettings ImageSettings { get; set; }
		[XmlAttribute(AttributeName = "patternColor")]
		public string PatternColor { get; set; }
		[XmlAttribute(AttributeName = "patternStyle")]
		public string PatternStyle { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
	}

	[XmlRoot(ElementName = "down")]
	public class Down
	{
		[XmlElement(ElementName = "caption")]
		public Caption Caption { get; set; }
		[XmlElement(ElementName = "imageSettings")]
		public ImageSettings ImageSettings { get; set; }
		[XmlAttribute(AttributeName = "downSameAsUp")]
		public string DownSameAsUp { get; set; }
		[XmlAttribute(AttributeName = "patternColor")]
		public string PatternColor { get; set; }
		[XmlAttribute(AttributeName = "patternStyle")]
		public string PatternStyle { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
	}

	[XmlRoot(ElementName = "ability")]
	public class Ability
	{
		[XmlAttribute(AttributeName = "showDisabledState")]
		public string ShowDisabledState { get; set; }
		[XmlAttribute(AttributeName = "expression")]
		public string Expression { get; set; }
		[XmlAttribute(AttributeName = "enabledWhenExpressionIsTrue")]
		public string EnabledWhenExpressionIsTrue { get; set; }
		[XmlAttribute(AttributeName = "disabledImageType")]
		public string DisabledImageType { get; set; }
	}

	[XmlRoot(ElementName = "confirm")]
	public class Confirm
	{
		[XmlElement(ElementName = "caption")]
		public Caption Caption { get; set; }
		[XmlElement(ElementName = "imageSettings")]
		public ImageSettings ImageSettings { get; set; }
		[XmlAttribute(AttributeName = "confirmAction")]
		public string ConfirmAction { get; set; }
		[XmlAttribute(AttributeName = "buttonSetting")]
		public string ButtonSetting { get; set; }
		[XmlAttribute(AttributeName = "titleBar")]
		public string TitleBar { get; set; }
		[XmlAttribute(AttributeName = "titleBarText")]
		public string TitleBarText { get; set; }
		[XmlAttribute(AttributeName = "windowPosition")]
		public string WindowPosition { get; set; }
	}

	[XmlRoot(ElementName = "button")]
	public class Button
	{
		[XmlElement(ElementName = "command")]
		public Command Command { get; set; }
		[XmlElement(ElementName = "up")]
		public Up Up { get; set; }
		[XmlElement(ElementName = "down")]
		public Down Down { get; set; }
		[XmlElement(ElementName = "ability")]
		public Ability Ability { get; set; }
		[XmlElement(ElementName = "confirm")]
		public Confirm Confirm { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
		[XmlAttribute(AttributeName = "style")]
		public string Style { get; set; }
		[XmlAttribute(AttributeName = "captureCursor")]
		public string CaptureCursor { get; set; }
		[XmlAttribute(AttributeName = "highlightOnFocus")]
		public string HighlightOnFocus { get; set; }
		[XmlAttribute(AttributeName = "tabIndex")]
		public string TabIndex { get; set; }
		[XmlElement(ElementName = "animations")]
		public Animations Animations { get; set; }
	}

	[XmlRoot(ElementName = "stringDisplay")]
	public class StringDisplay
	{
		[XmlElement(ElementName = "connections")]
		public Connections Connections { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "fontFamily")]
		public string FontFamily { get; set; }
		[XmlAttribute(AttributeName = "fontSize")]
		public string FontSize { get; set; }
		[XmlAttribute(AttributeName = "bold")]
		public string Bold { get; set; }
		[XmlAttribute(AttributeName = "italic")]
		public string Italic { get; set; }
		[XmlAttribute(AttributeName = "underline")]
		public string Underline { get; set; }
		[XmlAttribute(AttributeName = "strikethrough")]
		public string Strikethrough { get; set; }
		[XmlAttribute(AttributeName = "justification")]
		public string Justification { get; set; }
		[XmlAttribute(AttributeName = "dimensionsHeight")]
		public string DimensionsHeight { get; set; }
		[XmlAttribute(AttributeName = "dimensionsWidth")]
		public string DimensionsWidth { get; set; }
		[XmlAttribute(AttributeName = "characterOffset")]
		public string CharacterOffset { get; set; }
	}

	[XmlRoot(ElementName = "group")]
	public class Group
	{
		[XmlElement(ElementName = "stringDisplay")]
		public StringDisplay StringDisplay { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
		[XmlElement(ElementName = "rectangle")]
		public List<Rectangle> Rectangle { get; set; }
		[XmlElement(ElementName = "group")]
		public List<Group> Grp { get; set; }
		[XmlElement(ElementName = "image")]
		public List<Image> Image { get; set; }
		[XmlElement(ElementName = "multistateIndicator")]
		public List<MultistateIndicator> MultistateIndicator { get; set; }
		[XmlElement(ElementName = "animations")]
		public Animations Animations { get; set; }
		[XmlElement(ElementName = "parameters")]
		public Parameters Parameters { get; set; }
		[XmlAttribute(AttributeName = "wallpaper")]
		public string Wallpaper { get; set; }
		[XmlElement(ElementName = "ellipse")]
		public Ellipse Ellipse { get; set; }
		[XmlElement(ElementName = "line")]
		public List<Line> Line { get; set; }
		[XmlElement(ElementName = "arc")]
		public List<Arc> Arc { get; set; }
		[XmlElement(ElementName = "button")]
		public Button Button { get; set; }
		[XmlElement(ElementName = "polygon")]
		public Polygon Polygon { get; set; }
		[XmlElement(ElementName = "text")]
		public Text Text { get; set; }
	}

	[XmlRoot(ElementName = "animateVisibility")]
	public class AnimateVisibility
	{
		[XmlAttribute(AttributeName = "expression")]
		public string Expression { get; set; }
		[XmlAttribute(AttributeName = "expressionTrueState")]
		public string ExpressionTrueState { get; set; }
	}

	[XmlRoot(ElementName = "color")]
	public class Color
	{
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
		[XmlAttribute(AttributeName = "foreBehavior")]
		public string ForeBehavior { get; set; }
		[XmlAttribute(AttributeName = "foreColor1")]
		public string ForeColor1 { get; set; }
		[XmlAttribute(AttributeName = "foreColor2")]
		public string ForeColor2 { get; set; }
		[XmlAttribute(AttributeName = "backBehavior")]
		public string BackBehavior { get; set; }
		[XmlAttribute(AttributeName = "backColor1")]
		public string BackColor1 { get; set; }
		[XmlAttribute(AttributeName = "backColor2")]
		public string BackColor2 { get; set; }
		[XmlAttribute(AttributeName = "fillColorMode")]
		public string FillColorMode { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
		[XmlAttribute(AttributeName = "fillEndColor")]
		public string FillEndColor { get; set; }
		[XmlAttribute(AttributeName = "fillGradientStop")]
		public string FillGradientStop { get; set; }
		[XmlAttribute(AttributeName = "fillGradientDirection")]
		public string FillGradientDirection { get; set; }
		[XmlAttribute(AttributeName = "fillGradientShadingStyle")]
		public string FillGradientShadingStyle { get; set; }
	}

	[XmlRoot(ElementName = "animateColor")]
	public class AnimateColor
	{
		[XmlElement(ElementName = "color")]
		public List<Color> Color { get; set; }
		[XmlAttribute(AttributeName = "expression")]
		public string Expression { get; set; }
		[XmlAttribute(AttributeName = "blinkRate")]
		public string BlinkRate { get; set; }
	}

	[XmlRoot(ElementName = "animations")]
	public class Animations
	{
		[XmlElement(ElementName = "animateVisibility")]
		public AnimateVisibility AnimateVisibility { get; set; }
		[XmlElement(ElementName = "animateColor")]
		public AnimateColor AnimateColor { get; set; }
	}

	[XmlRoot(ElementName = "rectangle")]
	public class Rectangle
	{
		[XmlElement(ElementName = "animations")]
		public Animations Animations { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "lineStyle")]
		public string LineStyle { get; set; }
		[XmlAttribute(AttributeName = "lineWidth")]
		public string LineWidth { get; set; }
		[XmlAttribute(AttributeName = "patternStyle")]
		public string PatternStyle { get; set; }
		[XmlAttribute(AttributeName = "patternColor")]
		public string PatternColor { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
		[XmlAttribute(AttributeName = "wallpaper")]
		public string Wallpaper { get; set; }
	}

	[XmlRoot(ElementName = "parameter")]
	public class Parameter
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "description")]
		public string Description { get; set; }
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "parameters")]
	public class Parameters
	{
		[XmlElement(ElementName = "parameter")]
		public List<Parameter> Parameter { get; set; }
	}

	[XmlRoot(ElementName = "polygon")]
	public class Polygon
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "wallpaper")]
		public string Wallpaper { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "lineStyle")]
		public string LineStyle { get; set; }
		[XmlAttribute(AttributeName = "lineWidth")]
		public string LineWidth { get; set; }
		[XmlAttribute(AttributeName = "patternStyle")]
		public string PatternStyle { get; set; }
		[XmlAttribute(AttributeName = "patternColor")]
		public string PatternColor { get; set; }
		[XmlAttribute(AttributeName = "path")]
		public string Path { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
	}

	[XmlRoot(ElementName = "numericDisplay")]
	public class NumericDisplay
	{
		[XmlElement(ElementName = "connections")]
		public Connections Connections { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "fontFamily")]
		public string FontFamily { get; set; }
		[XmlAttribute(AttributeName = "fontSize")]
		public string FontSize { get; set; }
		[XmlAttribute(AttributeName = "bold")]
		public string Bold { get; set; }
		[XmlAttribute(AttributeName = "italic")]
		public string Italic { get; set; }
		[XmlAttribute(AttributeName = "underline")]
		public string Underline { get; set; }
		[XmlAttribute(AttributeName = "strikethrough")]
		public string Strikethrough { get; set; }
		[XmlAttribute(AttributeName = "justification")]
		public string Justification { get; set; }
		[XmlAttribute(AttributeName = "fieldLength")]
		public string FieldLength { get; set; }
		[XmlAttribute(AttributeName = "showDigitGrouping")]
		public string ShowDigitGrouping { get; set; }
		[XmlAttribute(AttributeName = "decimalPlaces")]
		public string DecimalPlaces { get; set; }
		[XmlAttribute(AttributeName = "format")]
		public string Format { get; set; }
		[XmlAttribute(AttributeName = "overflow")]
		public string Overflow { get; set; }
		[XmlAttribute(AttributeName = "leadingCharacter")]
		public string LeadingCharacter { get; set; }
		[XmlAttribute(AttributeName = "decimalPlaceType")]
		public string DecimalPlaceType { get; set; }
	}

	[XmlRoot(ElementName = "ellipse")]
	public class Ellipse
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "lineStyle")]
		public string LineStyle { get; set; }
		[XmlAttribute(AttributeName = "lineWidth")]
		public string LineWidth { get; set; }
		[XmlAttribute(AttributeName = "patternStyle")]
		public string PatternStyle { get; set; }
		[XmlAttribute(AttributeName = "patternColor")]
		public string PatternColor { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
	}

	[XmlRoot(ElementName = "arc")]
	public class Arc
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "left")]
		public string Left { get; set; }
		[XmlAttribute(AttributeName = "top")]
		public string Top { get; set; }
		[XmlAttribute(AttributeName = "visible")]
		public string Visible { get; set; }
		[XmlAttribute(AttributeName = "toolTipText")]
		public string ToolTipText { get; set; }
		[XmlAttribute(AttributeName = "exposeToVba")]
		public string ExposeToVba { get; set; }
		[XmlAttribute(AttributeName = "isReferenceObject")]
		public string IsReferenceObject { get; set; }
		[XmlAttribute(AttributeName = "backStyle")]
		public string BackStyle { get; set; }
		[XmlAttribute(AttributeName = "backColor")]
		public string BackColor { get; set; }
		[XmlAttribute(AttributeName = "foreColor")]
		public string ForeColor { get; set; }
		[XmlAttribute(AttributeName = "lineStyle")]
		public string LineStyle { get; set; }
		[XmlAttribute(AttributeName = "lineWidth")]
		public string LineWidth { get; set; }
		[XmlAttribute(AttributeName = "patternStyle")]
		public string PatternStyle { get; set; }
		[XmlAttribute(AttributeName = "patternColor")]
		public string PatternColor { get; set; }
		[XmlAttribute(AttributeName = "endColor")]
		public string EndColor { get; set; }
		[XmlAttribute(AttributeName = "gradientStop")]
		public string GradientStop { get; set; }
		[XmlAttribute(AttributeName = "gradientDirection")]
		public string GradientDirection { get; set; }
		[XmlAttribute(AttributeName = "gradientShadingStyle")]
		public string GradientShadingStyle { get; set; }
		[XmlAttribute(AttributeName = "startAngle")]
		public string StartAngle { get; set; }
		[XmlAttribute(AttributeName = "endAngle")]
		public string EndAngle { get; set; }
		[XmlElement(ElementName = "transform")]
		public Transform Transform { get; set; }
		[XmlAttribute(AttributeName = "linkSize")]
		public string LinkSize { get; set; }
		[XmlAttribute(AttributeName = "linkConnections")]
		public string LinkConnections { get; set; }
		[XmlAttribute(AttributeName = "linkAnimations")]
		public string LinkAnimations { get; set; }
		[XmlAttribute(AttributeName = "linkBaseObject")]
		public string LinkBaseObject { get; set; }
		[XmlAttribute(AttributeName = "linkToolTipText")]
		public string LinkToolTipText { get; set; }
	}

	[XmlRoot(ElementName = "transform")]
	public class Transform
	{
		[XmlAttribute(AttributeName = "scaleWidth")]
		public string ScaleWidth { get; set; }
		[XmlAttribute(AttributeName = "scaleHeight")]
		public string ScaleHeight { get; set; }
		[XmlAttribute(AttributeName = "shearWidth")]
		public string ShearWidth { get; set; }
		[XmlAttribute(AttributeName = "shearHeight")]
		public string ShearHeight { get; set; }
		[XmlAttribute(AttributeName = "offsetWidth")]
		public string OffsetWidth { get; set; }
		[XmlAttribute(AttributeName = "offsetHeight")]
		public string OffsetHeight { get; set; }
	}

	[XmlRoot(ElementName = "encryptedData")]
	public class EncryptedData
	{
		[XmlAttribute(AttributeName = "hash")]
		public string Hash { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "vbaItem")]
	public class VbaItem
	{
		[XmlElement(ElementName = "encryptedData")]
		public EncryptedData EncryptedData { get; set; }
	}

	[XmlRoot(ElementName = "vbaProject")]
	public class VbaProject
	{
		[XmlElement(ElementName = "vbaItem")]
		public VbaItem VbaItem { get; set; }
	}

	[XmlRoot(ElementName = "gfx")]
	public class Gfx
	{
		[XmlElement(ElementName = "displaySettings")]
		public DisplaySettings DisplaySettings { get; set; }
		[XmlElement(ElementName = "line")]
		public List<Line> Line { get; set; }
		[XmlElement(ElementName = "text")]
		public List<Text> Text { get; set; }
		[XmlElement(ElementName = "group")]
		public List<Group> Group { get; set; }
		[XmlElement(ElementName = "polygon")]
		public List<Polygon> Polygon { get; set; }
		[XmlElement(ElementName = "rectangle")]
		public Rectangle Rectangle { get; set; }
		[XmlElement(ElementName = "vbaProject")]
		public VbaProject VbaProject { get; set; }
		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
		[XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string NoNamespaceSchemaLocation { get; set; }
	}