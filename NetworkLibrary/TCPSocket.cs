using System;

namespace NetworkLibrary
{
	public class TCPSocket
	{
		const int EPERM = 1;
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
		const int EADDRNOTAVAIL = 99;
		const int EISCONN = 106;
		const int ENETUNREACH = 101;
		const int EHOSTUNREACH = 113;
		const int ENETDOWN = 100;
		const int EOPNOTSUPP = 95;
		const int ENOTCONN = 107;


		private Int32 socket;

		public TCPSocket ()
		{
			socket = CWrapper.createSocket ();
			if (0 == socket) {
				throw new OutOfMemoryException ("Ran out of memory attempting to create socket");
			}
			;
		}

		protected TCPSocket (Int32 socket)
		{
			this.socket = socket;
		}

		~TCPSocket ()
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

		public void Connect (Destination destination)
		{
			if (0 == CWrapper.connectPort (socket, destination)) {
				switch (CWrapper.getSocketError (socket)) {
				case EADDRNOTAVAIL:
					throw new System.IO.IOException ("The specified address is not availiable from the local machine");
				case EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ECONNREFUSED:
					throw new InvalidOperationException ("The target address was not listening for connections or refused the connection request");
				case EISCONN:
					throw new InvalidOperationException ("The specified socket is already connected");
				case ENETUNREACH:
					throw new System.IO.IOException ("Network unreachable");
				case ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ECONNRESET:
					throw new System.IO.IOException ("Remote host reset the connection request");
				case EHOSTUNREACH:
					throw new System.IO.IOException ("Destination host could not be reached");
				case ENETDOWN:
					throw new System.IO.IOException ("The local network interface used to reach the destination is down");
				case EOPNOTSUPP:
					throw new InvalidOperationException ("The socket is listening and cannot be connected");
				case EADDRINUSE:
					throw new System.IO.IOException ("Address is already in use");
				case EINVAL:
					throw new InvalidOperationException ("Address is invalid");
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



		public void Send (Packet packet)
		{
			if (0 == CWrapper.sendDataTCP (socket, ref packet.Data [0], packet.Length)) {
				switch (CWrapper.getSocketError (socket)) {
				case EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ENOTCONN:
					throw new InvalidOperationException ("The socket is not connected");
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
					throw new InvalidOperationException ("A remote host refused to allow the network connection");
				case ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case ENOMEM:
					throw new OutOfMemoryException ("No memory availiable");
				case ENOTCONN:
					throw new InvalidOperationException ("The socket has not been connected");
				}
			}
			return packet;
		}

		public TCPSocket Accept ()
		{
			Int32 acceptSocket = CWrapper.acceptClient (socket);
			if (0 == acceptSocket) {
				throw new OutOfMemoryException ("No memory availiable to create socket");
			}
			if (-1 == acceptSocket) {
				switch (CWrapper.getSocketError (socket)) {
				case EBADF:
					throw new System.IO.IOException ("An invalid socket descriptor was specified");
				case EINVAL:
					throw new InvalidOperationException ("Socket is not listening for connections");
				case ENOTSOCK:
					throw new InvalidOperationException ("Socket operation attempted on non socket");
				case EPERM:
					throw new System.IO.IOException ("Firewall rules forbid connection");
				}
			}
			return new TCPSocket (acceptSocket);
		}
	}
}

