using System;
using System.Runtime.InteropServices;

namespace NetworkLibrary
{
	public class Packet
	{
		public uint Length { get; private set; }

		public byte[] Data { get; private set; }

		public Packet ()
		{
			Data = new byte[1024];
			Length = 0;
		}

		public Packet (int size)
		{
			Data = new byte[size];
			Length = 0;
		}
	}
}

