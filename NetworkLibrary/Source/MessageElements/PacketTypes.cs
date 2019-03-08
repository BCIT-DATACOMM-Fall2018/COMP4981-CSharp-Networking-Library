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
	public enum PacketType : int
	{
		GameplayPacket = 0,
		RequestPacket,
		ConfirmationPacket
	}
}

