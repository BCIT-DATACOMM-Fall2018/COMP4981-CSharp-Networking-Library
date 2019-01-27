using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;

namespace NetworkLibrary
{
	public class BitStream
	{
		private byte[] buffer;
		private uint wordCount;
		private int spaceInByte;
		private int readIndex;

		public BitStream ()
		{
			buffer = new byte[20];
			spaceInByte = 8;
		}

		public void Write (int data, int offset, int bits)
		{
			data >>= offset;
			int bitsRemaining = bits;
			int nextBits;
			while (bitsRemaining > 0) {
				if (spaceInByte == 0) {
					spaceInByte = 8;
					wordCount++;
				}
				nextBits = Math.Min (spaceInByte, bitsRemaining);
				buffer [wordCount] |= (byte)(GetBits (data, bitsRemaining - nextBits, nextBits) << (spaceInByte - nextBits));
				spaceInByte -= nextBits;
				bitsRemaining -= nextBits;
			}
		}

		public int Read (int offset, int bits)
		{
			int byteIndex = offset / 8;
			int bitIndex = offset % 8;
			int bitsRemaining = bits;
			int nextBits;
			int output = 0;
			nextBits = Math.Min (8 - bitIndex, bitsRemaining);

			while (bitsRemaining > 0) {
				if (bitIndex == 8) {
					bitIndex = 0;
					byteIndex++;
				}
				output <<= nextBits;
				output |= GetBits (buffer [byteIndex], 8 - bitIndex - nextBits, nextBits);

				bitIndex += nextBits;
				bitsRemaining -= nextBits;
				nextBits = Math.Min (8 - bitIndex, bitsRemaining);
			}
			return output;
		}

		public int ReadNext (int bits)
		{
			int result = Read (readIndex, bits);
			readIndex += bits;
			return result;
		}

		private static int GetBits (int data, int offset, int bits)
		{
			data >>= offset;
			int temp = ((int)Math.Pow (2, bits) - 1);
			return data & temp;
		}
	}
}

