using System;
using System.Runtime.InteropServices;


namespace NetworkLibrary
{
	[StructLayout (LayoutKind.Sequential)]
	public struct Destination
	{
		private UInt32 address;
		private UInt16 port;

		public Destination (UInt32 address, UInt16 port)
		{
			this.address = address;
			this.port = port;
		}
	}
}

