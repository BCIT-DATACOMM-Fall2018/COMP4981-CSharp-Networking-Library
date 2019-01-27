using System;
using System.Runtime.InteropServices;

namespace NetworkLibrary
{
	public class CWrapper
	{
		
		[DllImport("libsocket.so")]
		public static extern Int32 createSocket();

		[DllImport("libsocket.so")]
		public static extern Int32 initSocket(Int32 socket);

		[DllImport("libsocket.so")]
		public static extern Int32 sendData(Int32 socket, Destination dest, ref byte data, UInt64 dataLength);

		[DllImport("libsocket.so")]
		public static extern Int32 recvData(Int32 socket, ref byte dataBuffer, UInt64 dataBufferLength);

		[DllImport("libsocket.so")]
		public static extern Int32 closeSocket(Int32 socket);

		[DllImport("libsocket.so")]
		public static extern void freeSocket(Int32 socket);

		[DllImport("libsocket.so")]
		public static extern Int32 initSocketTCP(Int32 socket);

		[DllImport("libsocket.so")]
		public static extern Int32 bindPort(Int32 socket, UInt16 port);

		[DllImport("libsocket.so")]
		public static extern Int32 connectPort(Int32 socket, Destination dest);

		[DllImport("libsocket.so")]
		public static extern Int32 acceptClient(Int32 socket);

		[DllImport("libsocket.so")]
		public static extern Int32 sendDataTCP(Int32 socket, ref byte data, UInt64 dataBufferSize);

		[DllImport("libsocket.so")]
		public static extern Int32 recvDataTCP(Int32 socket, ref byte dataBuffer, UInt32 packetSize);

		[DllImport("libsocket.so")]
		public static extern Int32 getSocketError(Int32 socket);

	}
}

