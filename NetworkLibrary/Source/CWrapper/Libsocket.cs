using System;
using System.Runtime.InteropServices;

namespace NetworkLibrary.CWrapper
{
	/// ----------------------------------------------
	/// Class: 	Libsocket - A wrapper class for libsocket.so
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// UDP and TCP FUNCTIONS:
	/// 			public static extern Int32 initSocket (ref SocketStruct socket)
	/// 			public static extern Int32 closeSocket (ref SocketStruct socket)
	///
	/// UDP FUNCTIONS:
	/// 			public static extern Int32 initSocket (ref SocketStruct socket)
	/// 			public static extern Int32 sendData (ref SocketStruct socket, Destination dest, ref byte data, UInt64 dataLength)
	/// 			public static extern Int32 recvData (ref SocketStruct socket, ref byte dataBuffer, UInt64 dataBufferLength)
	///
	/// TCP FUNCTIONS:
	/// 			public static extern Int32 initSocketTCP (ref SocketStruct socket)
	/// 			public static extern Int32 connectPort (ref SocketStruct socket, Destination dest)
	/// 			public static extern Int32 acceptClient (ref SocketStruct socket)
	/// 			public static extern Int32 sendDataTCP (ref SocketStruct socket, ref byte data, UInt64 dataBufferSize)
	/// 			public static extern Int32 recvDataTCP (ref SocketStruct socket, ref byte dataBuffer, UInt32 packetSize)
	///
	/// OTHER FUNCTIONS:
	/// 			public static extern Int32 getSocketError (ref SocketStruct socket)
	///
	/// DATE: 		January 28th, 2018
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts, Simon Wu
	///
	/// PROGRAMMER: Cameron Roberts, Simon Wu
	///
	/// NOTES:		This class exists as a wrapper for libsocket so those functions can be used in a C# program.
	/// ----------------------------------------------
	public class Libsocket
	{
        /// ----------------------------------------------
        /// FUNCTION:	initSocket
        /// 
        /// DATE:		January 28th, 2018
        /// 
        /// REVISIONS:	January 31st, 2019
        /// 				-Changed to accept a SocketStruct instead of an Int32
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Cameron Roberts
        /// 
        /// INTERFACE: 	public static extern Int32 initSocket (ref SocketStruct socket)
        /// 				ref SocketStruct socket: A SocketStruct to initialize
        /// 
        /// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
        /// 			should be called with the socket pointer to get the error code.
        /// 
        /// NOTES:		This function is used to initialize the socket as a UDP socket.
        /// ----------------------------------------------
        [DllImport ("libsocket")]
		public static extern Int32 initSocket (ref SocketStruct socket);

		/// ----------------------------------------------
		/// FUNCTION:	sendData
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 sendData (ref SocketStruct socket, Destination dest, ref byte data, UInt64 dataLength)
		/// 				ref SocketStruct socket: A socket struct to send data through.
		/// 				Destination dest: A Destination object containing the address and
		/// 								  port to send data to.
		/// 				ref byte data: The first byte in a byte array containing the data to
		/// 							   be sent.
		/// 				UInt64 dataLength: The length of the data to be sent.
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to send data through a UDP socket.
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 sendData (ref SocketStruct socket, ref Destination dest, ref byte data, UInt64 dataLength);

		/// ----------------------------------------------
		/// FUNCTION:	recvData
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 recvData (ref SocketStruct socket, ref byte data, UInt64 dataLength)
		/// 				ref SocketStruct socket: A SocketStruct to receive data from.
		/// 				ref byte data: The first byte in a byte that data will be written to.
		/// 				UInt64 dataLength: The length of the buffer.
		/// 
		/// RETURNS: 	On success the number of bytes read into the dataBuffer is returned.
		/// 			On error -1 is returned and getSocketError should be called with the 
		/// 			socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to receive data from a UDP socket
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 recvData (ref SocketStruct socket, ref Destination dest, ref byte dataBuffer, UInt64 dataBufferLength);

		/// ----------------------------------------------
		/// FUNCTION:	closeSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 closeSocket (ref SocketStruct socket)
		/// 				ref SocketStruct socket: A SocketStruct to close the socket descriptor of.
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to close a socket. This function should be
		/// 			called once your done with the socket but before calling freeSocket.
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 closeSocket (ref SocketStruct socket);

		/// ----------------------------------------------
		/// FUNCTION:	initSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 initSocket (ref SocketStruct socket)
		/// 				ref SocketStruct socket: A SocketStruct to initialize
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to initialize the socket as a TCP socket.
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 initSocketTCP (ref SocketStruct socket);

		/// ----------------------------------------------
		/// FUNCTION:	bindPort
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Simon Wu, Cameron Roberts
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 bindPort (ref SocketStruct socket, UInt16 port)
		/// 				ref SocketStruct socket: A SocketStruct to bind
		/// 				Uint16 port: A port number in network byte order
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to bind a TCP or UDP socket to a port. This
		/// 			function must be called before attemping to send or receive on the
		/// 			socket.
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 bindPort (ref SocketStruct socket, UInt16 port);

		/// ----------------------------------------------
		/// FUNCTION:	connectPort
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 connectPort (ref SocketStruct socket, Destination dest)
		/// 				ref SocketStruct socket: A SocketStruct to connect to a remote hose
		/// 				Destination dest: A Destination object containing the address
		/// 								  and port to connect to.
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to connect a TCP socket to an accepting TCP socket
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 connectPort (ref SocketStruct socket,ref  Destination dest);

		/// ----------------------------------------------
		/// FUNCTION:	acceptClient
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 connectPort (ref SocketStruct socket)
		/// 				ref SocketStruct socket: A socket pointer obtained by calling createSocket
		/// 
		/// RETURNS: 	On sucess a socket descriptor is returned.
		/// 			On error 0 is returned and getSocketError should be called with
		/// 			the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to accept a new TCP connection on a socket.
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 acceptClient (ref SocketStruct socket);

		/// ----------------------------------------------
		/// FUNCTION:	sendDataTCP
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 sendDataTCP (ref SocketStruct socket, ref byte data, UInt64 dataLength)
		/// 				ref SocketStruct socket: A SocketStruct to send data through.
		/// 				ref byte data: The first byte in a byte array containing the data to
		/// 							   be sent.
		/// 				UInt64 dataBufferSize: The length of the data to be sent.
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to send data through a TCP socket.
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 sendDataTCP (ref SocketStruct socket, ref byte data, UInt64 dataBufferSize);

		/// ----------------------------------------------
		/// FUNCTION:	recvDataTCP
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 recvDataTCP (ref SocketStruct socket, ref byte data, UInt64 dataLength)
		/// 				Int32 socket: A SocketStruct to receive data from
		/// 				ref byte data: The first byte in a byte that data will be written to.
		/// 				UInt64 dataLength: The length of the buffer.
		/// 
		/// RETURNS: 	On success the number of bytes read into the dataBuffer is returned.
		/// 			On error -1 is returned and getSocketError should be called with the 
		/// 			socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to receive data from a UDP socket
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 recvDataTCP (ref SocketStruct socket, ref byte dataBuffer, UInt32 packetSize);

		/// ----------------------------------------------
		/// FUNCTION:	getSocketError
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	January 31st, 2019
		/// 				-Changed to accept a SocketStruct instead of an Int32
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 recvDataTCP (ref SocketStruct socket)
		/// 				ref SocketStruct socket: A SocketStruct to check the error of.
		/// 
		/// RETURNS: 	The last error code associated with the socket
		/// 
		/// NOTES:		This function is used to get the last error associated with a socket.
		/// 			This function should only be called immediatly after a Libsocket
		/// 			function indicates that an error has occurred.
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 getSocketError (ref SocketStruct socket);

		/// ----------------------------------------------
		/// FUNCTION:	attachTimeout
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 attachTimeout (ref SocketStruct socket, Int32 waitDuration)
		/// 				ref SocketStruct socket: A SocketStruct to attach a timeout to.
		/// 				Int32 waitDuration: The timeout to add to the socket in seconds.
		/// 
		/// RETURNS: 	On sucess 1 is returned.
		/// 			On error 0 is returned and getSocketError should be called with
		/// 			the socket pointer to get the error code.
		/// 
		/// NOTES:		This function adds a timeout to a socket. Only the recv, send, and connect functions
		/// 			will timeout.
		/// ----------------------------------------------
		[DllImport ("libsocket")]
		public static extern Int32 attachTimeout (ref SocketStruct socket, Int32 waitDuration);


	}
}

