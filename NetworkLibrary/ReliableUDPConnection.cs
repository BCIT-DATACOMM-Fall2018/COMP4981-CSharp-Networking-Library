using System;

namespace NetworkLibrary
{
	public class ReliableUDPConnection
	{
		private UDPSocket socket;

		public ReliableUDPConnection (UDPSocket socket)
		{
			this.socket = socket;
		}
	}
}

