using System;
using System.Runtime.InteropServices;

namespace NetworkLibrary
{
	public class Packet
	{
		public const int DEFAULT_SIZE = 1024;

		public uint Length { get; private set; }

		public byte[] Data { get; private set; }

		public Packet ()
		{
			Data = new byte[DEFAULT_SIZE];
			Length = 0;
		}

		public Packet (int size)
		{
			Data = new byte[size];
			Length = 0;
		}
	}
}

