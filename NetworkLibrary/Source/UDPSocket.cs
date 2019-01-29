using System;
using NetworkLibrary.CWrapper;

namespace NetworkLibrary
{

	public class UDPSocket
	{
		private Int32 socket;

		public UDPSocket ()
		{
			socket = Libsocket.createSocket ();
			if (0 == socket) {
				throw new OutOfMemoryException ("Ran out of memory attempting to create socket");
			}
			;

		}

		~UDPSocket ()
		{
			Libsocket.freeSocket (socket);
		}

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

