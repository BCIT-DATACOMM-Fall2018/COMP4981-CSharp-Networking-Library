
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
	/// 			public static extern Int32 createSocket ()
	/// 			public static extern Int32 initSocket (Int32 socket)
	/// 			public static extern Int32 closeSocket (Int32 socket)
	/// 			public static extern void freeSocket (Int32 socket)
	///
	/// UDP FUNCTIONS:
	/// 			public static extern Int32 initSocket (Int32 socket)
	/// 			public static extern Int32 sendData (Int32 socket, Destination dest, ref byte data, UInt64 dataLength)
	/// 			public static extern Int32 recvData (Int32 socket, ref byte dataBuffer, UInt64 dataBufferLength)
	///
	/// TCP FUNCTIONS:
	/// 			public static extern Int32 initSocketTCP (Int32 socket)
	/// 			public static extern Int32 connectPort (Int32 socket, Destination dest)
	/// 			public static extern Int32 acceptClient (Int32 socket)
	/// 			public static extern Int32 sendDataTCP (Int32 socket, ref byte data, UInt64 dataBufferSize)
	/// 			public static extern Int32 recvDataTCP (Int32 socket, ref byte dataBuffer, UInt32 packetSize)
	///
	/// OTHER FUNCTIONS:
	/// 			public static extern Int32 getSocketError (Int32 socket)
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
		/// FUNCTION:	createSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 createSocket (void)
		/// 
		/// RETURNS: 	A socket pointer to be passed into other Libsocket functions
		/// 
		/// NOTES:		This function is used to allocate memory for the underlying
		/// 			struct used to store the socket.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 createSocket ();

		/// ----------------------------------------------
		/// FUNCTION:	initSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 initSocket (Int32 socket)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to initialize the socket as a UDP socket.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 initSocket (Int32 socket);

		/// ----------------------------------------------
		/// FUNCTION:	sendData
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 sendData (Int32 socket, Destination dest, ref byte data, UInt64 dataLength)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket.
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
		[DllImport ("libsocket.so")]
		public static extern Int32 sendData (Int32 socket, Destination dest, ref byte data, UInt64 dataLength);

		/// ----------------------------------------------
		/// FUNCTION:	recvData
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 recvData (Int32 socket, ref byte data, UInt64 dataLength)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket.
		/// 				ref byte data: The first byte in a byte that data will be written to.
		/// 				UInt64 dataLength: The length of the buffer.
		/// 
		/// RETURNS: 	On success the number of bytes read into the dataBuffer is returned.
		/// 			On error -1 is returned and getSocketError should be called with the 
		/// 			socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to receive data from a UDP socket
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 recvData (Int32 socket, ref byte dataBuffer, UInt64 dataBufferLength);

		/// ----------------------------------------------
		/// FUNCTION:	closeSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 closeSocket (Int32 socket)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket.
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to close a socket. This function should be
		/// 			called once your done with the socket but before calling freeSocket.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 closeSocket (Int32 socket);

		/// ----------------------------------------------
		/// FUNCTION:	freeSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern void sendData (Int32 socket)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket.
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		This function is used to free the underlying struct used to store
		/// 			a socket.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern void freeSocket (Int32 socket);

		/// ----------------------------------------------
		/// FUNCTION:	initSocket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 initSocket (Int32 socket)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to initialize the socket as a TCP socket.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 initSocketTCP (Int32 socket);

		/// ----------------------------------------------
		/// FUNCTION:	bindPort
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Simon Wu, Cameron Roberts
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 bindPort (Int32 socket, UInt16 port)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket
		/// 				Uint16 port: A port number in network byte order
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to bind a TCP or UDP socket to a port. This
		/// 			function must be called before attemping to send or receive on the
		/// 			socket.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 bindPort (Int32 socket, UInt16 port);

		/// ----------------------------------------------
		/// FUNCTION:	connectPort
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 connectPort (Int32 socket, Destination dest)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket
		/// 				Destination dest: A Destination object containing the address
		/// 								  and port to connect to.
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to connect a TCP socket to an accepting TCP socket
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 connectPort (Int32 socket, Destination dest);

		/// ----------------------------------------------
		/// FUNCTION:	acceptClient
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 connectPort (Int32 socket)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket
		/// 
		/// RETURNS: 	On sucess a pointer to a newly created socketStruct is returned.
		/// 			On error 0 is returned and getSocketError should be called with
		/// 			the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to accept a new TCP connection on a socket.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 acceptClient (Int32 socket);

		/// ----------------------------------------------
		/// FUNCTION:	sendDataTCP
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 sendDataTCP (Int32 socket, ref byte data, UInt64 dataLength)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket.
		/// 				ref byte data: The first byte in a byte array containing the data to
		/// 							   be sent.
		/// 				UInt64 dataBufferSize: The length of the data to be sent.
		/// 
		/// RETURNS: 	On success 1 is returned. On error 0 is returned and getSocketError
		/// 			should be called with the socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to send data through a TCP socket.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 sendDataTCP (Int32 socket, ref byte data, UInt64 dataBufferSize);

		/// ----------------------------------------------
		/// FUNCTION:	recvDataTCP
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Simon Wu
		/// 
		/// PROGRAMMER:	Simon Wu, Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 recvDataTCP (Int32 socket, ref byte data, UInt64 dataLength)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket.
		/// 				ref byte data: The first byte in a byte that data will be written to.
		/// 				UInt64 dataLength: The length of the buffer.
		/// 
		/// RETURNS: 	On success the number of bytes read into the dataBuffer is returned.
		/// 			On error -1 is returned and getSocketError should be called with the 
		/// 			socket pointer to get the error code.
		/// 
		/// NOTES:		This function is used to receive data from a UDP socket
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 recvDataTCP (Int32 socket, ref byte dataBuffer, UInt32 packetSize);

		/// ----------------------------------------------
		/// FUNCTION:	getSocketError
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static extern Int32 recvDataTCP (Int32 socket)
		/// 				Int32 socket: A socket pointer obtained by calling createSocket.
		/// 
		/// RETURNS: 	The last error code associated with the socket
		/// 
		/// NOTES:		This function is used to get the last error associated with a socket.
		/// 			This function should only be called immediatly after a Libsocket
		/// 			function indicates that an error has occurred.
		/// ----------------------------------------------
		[DllImport ("libsocket.so")]
		public static extern Int32 getSocketError (Int32 socket);

	}
}

