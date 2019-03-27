using System;
using System.Runtime.InteropServices;


namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Struct: Destination - A struct to store IP address and port information.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	///	CONSTRUCTORS:	public Destination (UInt32 address, UInt16 port)
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
	/// NOTES:		IP addresses and ports should be store in network byte order.
	/// ----------------------------------------------
	[StructLayout (LayoutKind.Sequential)]
	public struct Destination
	{
		private UInt32 address;
		private UInt16 port;

		/// ----------------------------------------------
		/// CONSTRUCTOR: Destination
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public Destination (UInt32 address, UInt16 port)
		/// 				UInt32 address: A network byte order IP address
		/// 				UInt16 port: A newwork byte order port
		/// 
		/// NOTES:	IP addresses and ports should be stored in network byte order
		/// ----------------------------------------------
		public Destination (UInt32 address, UInt16 port)
		{
			this.address = address;
			this.port = port;
		}
	}
}

