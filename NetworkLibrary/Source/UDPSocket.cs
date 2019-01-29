using System;
using NetworkLibrary.CWrapper;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Class: 	UDPSocket - A class to send and recieve data through UDP
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public UDPSocket ()
	/// 
	/// FUNCTIONS:	public void Bind (ushort port = 0)
	/// 			public void Close ()
	/// 			public void Send (Packet packet)
	/// 			public Packet Receive ()
	/// 
	/// DATE: 		January 28th, 2018
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:	A UDP must have Bind called before it can be used to send 
	/// 		or receive data. When done with a socket that socket 
	/// 		should have its Close method called.
	/// ----------------------------------------------
	public class UDPSocket
	{
		private Int32 socket;

		/// ----------------------------------------------
		/// CONSTRUCTOR: UDPSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public TCPSocket ()
		/// 
		/// NOTES: 	Creates a new UDP socket.
		/// ----------------------------------------------
		public UDPSocket ()
		{
			socket = Libsocket.createSocket ();
			if (0 == socket) {
				throw new OutOfMemoryException ("Ran out of memory attempting to create socket");
			}
			;

		}

		/// ----------------------------------------------
		/// DESTRUCTOR: ~UDPSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	~UDPSocket ()
		/// 
		/// NOTES: 	Deallocates the memory used to store the
		/// 		pointer in the underlying libsock library.
		/// ----------------------------------------------
		~UDPSocket ()
		{
			Libsocket.freeSocket (socket);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Bind
		/// 
		/// DATE:		January 28th, 2018
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
			if (0 == Libsocket.bindPort (socket, port)) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EACCES:
					throw new System.Security.SecurityException ("Insufficient permission to bind port");
				case ErrorCodes.EADDRINUSE:
					throw new System.IO.IOException ("Port is already in use or out of ephemeral ports");
				case ErrorCodes.EINVAL:
					throw new InvalidOperationException ("Socket is already bound or the given address was invalid");
				case ErrorCodes.ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				}
			}
		}


		/// ----------------------------------------------
		/// FUNCTION:	Close
		/// 
		/// DATE:		January 28th, 2018
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
			if (0 == Libsocket.closeSocket (socket)) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EBADF:
					throw new System.IO.IOException ("Socket descriptor invalid");
				case ErrorCodes.EIO:
					throw new System.IO.IOException ("Error occurred while attempting to close socket");
				}
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	Send
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public void Send (Packet packet, Destination destination)
		/// 				Packet packet: The packet to send.
		/// 				Destination destination: A destination object containing the IP address
		/// 										 and port to send the data to.
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Sends the specified Packet objects data to the
		/// 			specified Destinations IP address and port. The
		/// 			Destination objects IP address and port should
		/// 			be in network byte order.
		/// ----------------------------------------------
		public void Send (Packet packet, Destination destination)
		{
			if (0 == Libsocket.sendData (socket, destination, ref packet.Data [0], packet.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ErrorCodes.EMSGSIZE:
					throw new InvalidOperationException ("Message is too large to be sent");
				case ErrorCodes.ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ENOMEM:
					throw new OutOfMemoryException ("No memory availiable");
				}
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	Receive
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public Packet Receive ()
		/// 
		/// RETURNS: 	A Packet object containing the recieved data.
		/// 
		/// NOTES:		Recieves a packet that has been sent to the socket.
		/// ----------------------------------------------
		public Packet Receive ()
		{
			Packet packet = new Packet ();
			if (0 == Libsocket.recvData (socket, ref packet.Data [0], (ulong)packet.Data.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ErrorCodes.ECONNREFUSED:
					throw new InvalidOperationException ("A remote host refused to allow the network connection\n");
				case ErrorCodes.ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ENOMEM:
					throw new OutOfMemoryException ("No memory availiable");
				}
			}
			return packet;
		}
	}
}

