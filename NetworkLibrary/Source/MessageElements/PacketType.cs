using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Enum: 	PacketType - An enum to differentiate types of packets
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// DATE: 		March 8th, 2019
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

