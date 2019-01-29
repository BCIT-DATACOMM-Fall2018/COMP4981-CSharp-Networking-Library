using System;
using NetworkLibrary.CWrapper;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Class: 	TCPSocket - A class to send and recieve data through TCP
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public TCPSocket ()
	/// 				protected TCPSocket (Int32 socket)
	/// 
	/// FUNCTIONS:	public void Connect (Destination destination)
	/// 			public void Send (Packet packet)
	/// 			public Packet Receive ()
	/// 			public TCPSocket Accept ()
	/// 
	/// DATE: 		January 28th, 2018
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:	A TCPSocket must have Bind or Connect called before it
	/// 		can be used. A TCPSocket opened with Bind should not
	/// 		be used to send data, only to accept incoming connections.
	/// 		When done with a socket that socket should have its Close
	/// 		method called.
	/// ----------------------------------------------
	public class TCPSocket : Socket
	{

		/// ----------------------------------------------
		/// CONSTRUCTOR: TCPSocket
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
		/// NOTES: 	Creates a new TCP socket.
		/// ----------------------------------------------
		public TCPSocket () : base()
		{
			if (CWrapper.Libsocket.initSocketTCP (socket) == 0) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EACCES:
					throw new System.IO.IOException ("Permission to create the socket was denied");
				case ErrorCodes.ENOMEM:
					throw new System.OutOfMemoryException ("Out of memory to create socket");
				}
			}
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: TCPSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected TCPSocket (Int32 socket)
		/// 
		/// NOTES: 	Creates a new TCP socket from the given socket
		/// 	   	pointer. Only intended to be used by the Accept
		/// 	   	function
		/// ----------------------------------------------
		protected TCPSocket (Int32 socket)
		{
			this.socket = socket;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Connect
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public void Connect (Destination destination)
		/// 				Destination destination: A Destination object containing the IP address
		/// 										 and port to connect to.
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		The Destination objects IP address and port should
		/// 			be in network byte order.
		/// ----------------------------------------------
		public void Connect (Destination destination)
		{
			if (0 == Libsocket.connectPort (socket, destination)) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EADDRNOTAVAIL:
					throw new System.IO.IOException ("The specified address is not availiable from the local machine");
				case ErrorCodes.EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ErrorCodes.ECONNREFUSED:
					throw new InvalidOperationException ("The target address was not listening for connections or refused the connection request");
				case ErrorCodes.EISCONN:
					throw new InvalidOperationException ("The specified socket is already connected");
				case ErrorCodes.ENETUNREACH:
					throw new System.IO.IOException ("Network unreachable");
				case ErrorCodes.ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ECONNRESET:
					throw new System.IO.IOException ("Remote host reset the connection request");
				case ErrorCodes.EHOSTUNREACH:
					throw new System.IO.IOException ("Destination host could not be reached");
				case ErrorCodes.ENETDOWN:
					throw new System.IO.IOException ("The local network interface used to reach the destination is down");
				case ErrorCodes.EOPNOTSUPP:
					throw new InvalidOperationException ("The socket is listening and cannot be connected");
				case ErrorCodes.EADDRINUSE:
					throw new System.IO.IOException ("Address is already in use");
				case ErrorCodes.EINVAL:
					throw new InvalidOperationException ("Address is invalid");
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
		/// INTERFACE: 	public void Send (Packet packet)
		/// 				Packet packet: The packet to send.
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Sends the specified Packet objects data to the
		/// 			connected socket.
		/// ----------------------------------------------
		public void Send (Packet packet)
		{
			if (0 == Libsocket.sendDataTCP (socket, ref packet.Data [0], packet.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ErrorCodes.ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ENOTCONN:
					throw new InvalidOperationException ("The socket is not connected");
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
		/// NOTES:		Recieves a packet from the connected socket.
		/// ----------------------------------------------
		public Packet Receive ()
		{
			Packet packet = new Packet ();
			if (0 == Libsocket.recvData (socket, ref packet.Data [0], (ulong)packet.Data.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ErrorCodes.ECONNREFUSED:
					throw new InvalidOperationException ("A remote host refused to allow the network connection");
				case ErrorCodes.ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ENOMEM:
					throw new OutOfMemoryException ("No memory availiable");
				case ErrorCodes.ENOTCONN:
					throw new InvalidOperationException ("The socket has not been connected");
				}
			}
			return packet;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Accept
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public TCPSocket Accept ()
		/// 
		/// RETURNS: 	A TCPSocket object for the new connection.
		/// 
		/// NOTES:		Used to accept an incoming connection.
		/// ----------------------------------------------
		public TCPSocket Accept ()
		{
			Int32 acceptSocket = Libsocket.acceptClient (socket);
			if (0 == acceptSocket) {
				throw new OutOfMemoryException ("No memory availiable to create socket");
			}
			if (-1 == acceptSocket) {
				switch ((ErrorCodes)Libsocket.getSocketError (socket)) {
				case ErrorCodes.EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ErrorCodes.EINVAL:
					throw new InvalidOperationException ("Socket is not listening for connections");
				case ErrorCodes.ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.EPERM:
					throw new System.IO.IOException ("Firewall rules forbid connection");
				}
			}
			return new TCPSocket (acceptSocket);
		}
	}
}

