using System;
using NetworkLibrary.CWrapper;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Abstract Class: Socket - An abstract socket class
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public Socket ()
	/// 
	/// FUNCTIONS:	public void Bind (ushort port = 0)
	/// 			public void Close ()
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
	public abstract class Socket
	{
		protected SocketStruct socket;
		protected const int DEFAULT_TIMEOUT = 5;

		/// ----------------------------------------------
		/// CONSTRUCTOR: Socket
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public Socket ()
		/// 
		/// NOTES: 	Creates a new socket.
		/// ----------------------------------------------
		public Socket ()
		{
		}


		/// ----------------------------------------------
		/// FUNCTION:	Bind
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public void Bind (ushort port = 0)
		/// 				ushort port 0: The port to bind to. 0 if unspecified.
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Will bind the socket to the specified port or an ephemeral
		/// 			port if no port is specified.
		/// ----------------------------------------------
		public void Bind (ushort port = 0)
		{
			if (0 == Libsocket.bindPort (ref socket, port)) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_PERMISSION:
					throw new System.Security.SecurityException ("Insufficient permission to bind port");
				case ErrorCodes.ERR_ADDRINUSE:
					throw new System.IO.IOException ("Port is already in use or out of ephemeral ports");
				case ErrorCodes.ERR_ILLEGALOP:
					throw new InvalidOperationException ("Socket is already bound or the given address was invalid");
				case ErrorCodes.ERR_BADSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("An unknown error occured while binding the socket to a port");
				}
			}
		}


		/// ----------------------------------------------
		/// FUNCTION:	Close
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public void Close ()
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		A port should be closed once it will no longer be used.
		/// ----------------------------------------------
		public void Close ()
		{
			if (0 == Libsocket.closeSocket (ref socket)) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_BADSOCK:
					throw new System.IO.IOException ("Socket descriptor invalid");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("Error occurred while attempting to close socket");
				}
			}
		}
			
	}
}

