using System;
using System.Runtime.InteropServices;


namespace NetworkLibrary.CWrapper
{
	/// ----------------------------------------------
	/// Struct: SocketStruct - A struct to socketDescriptors.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	///	CONSTRUCTORS:	public SocketStruct(Int32 socketDescriptor = 0)
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
	[StructLayout (LayoutKind.Sequential)]
	public struct SocketStruct
	{
		private Int32 socketDescriptor;
		private Int32 lastError;

		/// ----------------------------------------------
		/// CONSTRUCTOR: SocketStruct
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public SocketStruct(Int32 socketDescriptor = 0)
		/// 				Int32 socketDescriptor: A socket descriptor
		/// 
		/// NOTES:	
		/// ----------------------------------------------
		public SocketStruct(Int32 socketDescriptor = 0){
			this.socketDescriptor = socketDescriptor;
			lastError = 0;
		}
	}
}

