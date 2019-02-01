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
	/// FUNCTIONS:	public void Send (Packet packet)
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
	public class UDPSocket : Socket
	{

		public Destination LastReceivedFrom {get { return _lastReceivedFrom; } set {_lastReceivedFrom = value;}}
		private Destination _lastReceivedFrom;

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
		public UDPSocket () : base()
		{
			_lastReceivedFrom = new Destination ();
			if (CWrapper.Libsocket.initSocket (ref socket) == 0) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.EACCES:
					throw new System.IO.IOException ("Permission to create the socket was denied");
				case ErrorCodes.ENOMEM:
					throw new System.OutOfMemoryException ("Out of memory to create socket");
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
			if (0 == Libsocket.sendData (ref socket, ref destination, ref packet.Data [0], packet.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
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
			if (0 == Libsocket.recvData (ref socket, ref _lastReceivedFrom ,ref packet.Data [0], (ulong)packet.Data.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
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

