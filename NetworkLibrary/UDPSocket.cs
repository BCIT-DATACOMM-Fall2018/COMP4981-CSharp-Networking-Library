using System;

namespace NetworkLibrary
{

	public class UDPSocket
	{
		const int ENOMEM = 12;
		const int EACCES = 13;
		const int EADDRINUSE = 98;
		const int EINVAL = 22;
		const int ENOTSOCK = 88;
		const int EBADF = 9;
		const int EIO = 5;
		const int ECONNRESET = 104;
		const int EMSGSIZE = 90;
		const int ECONNREFUSED = 111;


		private Int32 socket;

		public UDPSocket ()
		{
			socket = CWrapper.createSocket ();
			if (0 == socket) {
				throw new OutOfMemoryException ("Ran out of memory attempting to create socket");
			}
			;

		}

		~UDPSocket ()
		{
			CWrapper.freeSocket (socket);
		}

		public void Bind (ushort port = 0)
		{
			if (0 == CWrapper.bindPort (socket, port)) {
				switch (CWrapper.getSocketError (socket)) {
				case EACCES:
					throw new System.Security.SecurityException ("Insufficient permission to bind port");
				case EADDRINUSE:
					throw new System.IO.IOException ("Port is already in use or out of ephemeral ports");
				case EINVAL:
					throw new InvalidOperationException ("Socket is already bound or the given address was invalid");
				case ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				}
			}
		}

		public void Close ()
		{
			if (0 == CWrapper.closeSocket (socket)) {
				switch (CWrapper.getSocketError (socket)) {
				case EBADF:
					throw new System.IO.IOException ("Socket descriptor invalid");
				case EIO:
					throw new System.IO.IOException ("Error occurred while attempting to close socket");
				}
			}
		}


		public void Send (Packet packet, Destination destination)
		{
			if (0 == CWrapper.sendData (socket, destination, ref packet.Data [0], packet.Length)) {
				switch (CWrapper.getSocketError (socket)) {
				case EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case EMSGSIZE:
					throw new InvalidOperationException ("Message is too large to be sent");
				case ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ENOMEM:
					throw new OutOfMemoryException ("No memory availiable");
				}
			}
		}

		public Packet Receive ()
		{
			Packet packet = new Packet ();
			if (0 == CWrapper.recvData (socket, ref packet.Data [0], (ulong)packet.Data.Length)) {
				switch (CWrapper.getSocketError (socket)) {
				case EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ECONNREFUSED:
					throw new InvalidOperationException ("A remote host refused to allow the network connection\n");
				case ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ENOMEM:
					throw new OutOfMemoryException ("No memory availiable");
				}
			}
			return packet;
		}
	}
}

