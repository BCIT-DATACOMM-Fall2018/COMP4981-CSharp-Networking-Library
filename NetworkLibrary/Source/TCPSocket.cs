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
	/// DATE: 		January 28th, 2019
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
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public TCPSocket (int timeout = DEFAULT_TIMEOUT)
		/// 				int timeout: A timeout to place on socket send, receive, and connect
		/// 							 operations. Defaults to DEFAULT_TIMEOUT. A timeout of 0
		/// 							 means no timeout.
		/// 
		/// NOTES: 	Creates a new TCP socket.
		/// ----------------------------------------------
		public TCPSocket (int timeout = DEFAULT_TIMEOUT) : base()
		{
			if (CWrapper.Libsocket.initSocketTCP (ref socket) == 0) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_PERMISSION:
					throw new System.IO.IOException ("Permission to create the socket was denied");
				case ErrorCodes.ERR_NOMEMORY:
					throw new System.OutOfMemoryException ("Out of memory to create socket");
				case ErrorCodes.ERR_UNKNOWN:
					throw new System.Exception ("Unkown error occured while trying to initialize socket");
				}
			}
			if (timeout != 0) {
				if (Libsocket.attachTimeout (ref socket, timeout) == 0) {
					switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
					case ErrorCodes.ERR_BADSOCK:
						throw new InvalidOperationException ("Socket operation attempted on non socket");
					case ErrorCodes.ERR_ILLEGALOP:
						throw new InvalidOperationException ("Illegal operation attempted");
					case ErrorCodes.ERR_UNKNOWN:
						throw new System.Exception ("Unkown error occured while trying to set socket timeout");
					}
				}
			}
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: TCPSocket
		/// 
		/// DATE:		January 28th, 2019
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
		protected TCPSocket (SocketStruct socket)
		{
			this.socket = socket;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Connect
		/// 
		/// DATE:		January 28th, 2019
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
			if (0 == Libsocket.connectPort (ref socket, ref destination)) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_ADDRNOTAVAIL:
					throw new System.IO.IOException ("The specified address is not availiable from the local machine");
				case ErrorCodes.ERR_BADSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ERR_CONREFUSED:
					throw new InvalidOperationException ("The target address was not listening for connections or refused the connection request");
				case ErrorCodes.ERR_DESTUNREACH:
					throw new System.IO.IOException ("Destination could not be reached");
				case ErrorCodes.ERR_CONRESET:
					throw new System.IO.IOException ("Remote host reset the connection request");
				case ErrorCodes.ERR_ILLEGALOP:
					throw new InvalidOperationException ("Illegal socket operation attempted");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("Unknown exception occured trying create TCP connection");
				case ErrorCodes.ERR_TIMEOUT:
					throw new TimeoutException ("Timeout while attempting to connect");
				}
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	Send
		/// 
		/// DATE:		January 28th, 2019
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
			if (0 == Libsocket.sendDataTCP (ref socket, ref packet.Data [0], packet.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_BADSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ERR_ILLEGALOP:
					throw new InvalidOperationException ("The socket is not connected");
				case ErrorCodes.ERR_NOMEMORY:
					throw new OutOfMemoryException ("No memory availiable");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("Unknown exception occured trying send TCP data");
				case ErrorCodes.ERR_TIMEOUT:
					throw new TimeoutException ("Timeout while attempting to send data");
				}
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	Receive
		/// 
		/// DATE:		January 28th, 2019
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
			if (-1 == Libsocket.recvDataTCP (ref socket,ref packet.Data [0], (uint)packet.Data.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_CONREFUSED:
					throw new InvalidOperationException ("A remote host refused to allow the network connection");
				case ErrorCodes.ERR_BADSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ERR_NOMEMORY:
					throw new OutOfMemoryException ("No memory availiable");
				case ErrorCodes.ERR_ILLEGALOP:
					throw new InvalidOperationException ("The socket has not been connected");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("Unknown exception occured trying receive TCP data");
				case ErrorCodes.ERR_TIMEOUT:
					throw new TimeoutException ("Timeout while attempting to receive data");
				}
			}
			return packet;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Accept
		/// 
		/// DATE:		January 28th, 2019
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
			Int32 acceptSocket = Libsocket.acceptClient (ref socket);
			if (0 == acceptSocket) {
				throw new OutOfMemoryException ("No memory availiable to create socket");
			}
			if (-1 == acceptSocket) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_ILLEGALOP:
					throw new InvalidOperationException ("Socket is not listening for connections");
				case ErrorCodes.ERR_BADSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ERR_PERMISSION:
					throw new System.IO.IOException ("Firewall rules forbid connection");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("Unknown exception occured trying accept TCP connection");
				}
			}
			return new TCPSocket (new SocketStruct(acceptSocket));
		}
	}
}

