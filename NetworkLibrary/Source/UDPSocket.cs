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

		public Destination LastReceivedFrom { get { return _lastReceivedFrom; } set { _lastReceivedFrom = value; } }

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
		/// INTERFACE: 	public TCPSocket (int timeout = DEFAULT_TIMEOUT)
		/// 				int timeout: A timeout to place on socket send and receive
		/// 							 operations. Defaults to DEFAULT_TIMEOUT. A 
		/// 							 timeout of 0 means no timeout.
		/// 
		/// NOTES: 	Creates a new UDP socket.
		/// ----------------------------------------------
		public UDPSocket (int timeout = DEFAULT_TIMEOUT) : base ()
		{
			_lastReceivedFrom = new Destination ();
			if (CWrapper.Libsocket.initSocket (ref socket) == 0) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_PERMISSION:
					throw new System.IO.IOException ("Permission to create the socket was denied");
				case ErrorCodes.ERR_NOMEMORY:
					throw new System.OutOfMemoryException ("Out of memory to create socket");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("Unknown exception occured trying to initialize UDP socket");
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
				case ErrorCodes.ERR_ILLEGALOP:
					throw new InvalidOperationException ("Illegal operation attempted");
				case ErrorCodes.ERR_BADSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ERR_NOMEMORY:
					throw new OutOfMemoryException ("No memory availiable");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("Unknown exception occured trying to send UDP data");
				case ErrorCodes.ERR_TIMEOUT:
					throw new TimeoutException ("Timeout while attempting to send data");
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
			if (-1 == Libsocket.recvData (ref socket, ref _lastReceivedFrom, ref packet.Data [0], (ulong)packet.Data.Length)) {
				switch ((ErrorCodes)Libsocket.getSocketError (ref socket)) {
				case ErrorCodes.ERR_CONREFUSED:
					throw new InvalidOperationException ("A remote host refused to allow the network connection\n");
				case ErrorCodes.ERR_BADSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ErrorCodes.ERR_NOMEMORY:
					throw new OutOfMemoryException ("No memory availiable");
				case ErrorCodes.ERR_UNKNOWN:
					throw new Exception ("Unknown exception occured trying to receive UDP data");
				case ErrorCodes.ERR_TIMEOUT:
					throw new TimeoutException ("Timeout while attempting to receive data");
				}
			}
			return packet;
		}
	}
}

