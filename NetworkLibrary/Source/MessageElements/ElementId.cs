using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Enum: 	ElementId - An enum to store MessageElement IDs
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// DATE: 		January 28th, 2018
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:	
	/// ----------------------------------------------
	public enum ElementId : int
	{
		MessageElement = 0,
		ElementIndicatorElement,
		PacketHeaderElement,
		HealthElement
	}
}

