using System;
using System.Runtime.InteropServices;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Class: 	Packet - A class to store data to be sent using a TCP or UDP socket
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public Packet ()
	/// 				public Packet (int size)
	/// 
	/// FUNCTIONS:	None
	/// 
	/// DATE: 		January 28th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class Packet
	{
		public const int DEFAULT_SIZE = 1024;

		public byte[] Data { get;  set; }

		public ulong Length {get {return  (ulong)Data.Length;}}


		/// ----------------------------------------------
		/// CONSTRUCTOR: Packet
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public Packet ()
		/// 
		/// NOTES: Creates a packet of the default size.
		/// ----------------------------------------------
		public Packet ()
		{
			Data = new byte[DEFAULT_SIZE];
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: Packet
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public Packet ()
		/// 
		/// NOTES: Creates a packet of the specified size.
		/// ----------------------------------------------
		public Packet (int size)
		{
			Data = new byte[size];
		}
			
	}
}

