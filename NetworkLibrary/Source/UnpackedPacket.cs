using System;
using System.Collections.Generic;
using NetworkLibrary.MessageElements;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Class: 	UnpackedPacket - A class to UpdateElements that have been extracted from
	/// 						 a Packet object
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public UnpackedPacket (List<UpdateElement> unreliableElements, List<UpdateElement> reliableElements )
	/// 
	/// FUNCTIONS:	None
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
	public class UnpackedPacket
	{
		public List<UpdateElement> UnreliableElements { get;}
		public List<UpdateElement> ReliableElements { get;}

		/// ----------------------------------------------
		/// CONSTRUCTOR: UnpackedPacket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public Packet ()
		/// 				List<UpdateElement> unreliableElements: A list of unreliable UpdateElements
		/// 				List<UpdateElement> reliableElements: A list of reliable UpdateElements
		/// 
		/// NOTES: 
		/// ----------------------------------------------
		public UnpackedPacket (List<UpdateElement> unreliableElements, List<UpdateElement> reliableElements )
		{
			this.UnreliableElements = unreliableElements;
			this.ReliableElements = reliableElements;
		}
	}
}

