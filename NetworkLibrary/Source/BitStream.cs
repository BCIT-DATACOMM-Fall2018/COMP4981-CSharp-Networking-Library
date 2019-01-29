using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;

namespace NetworkLibrary
{
	public class BitStream
	{
		private const int BYTE_SIZE = 8;
		private byte[] buffer;
		private uint wordCount;
		private int spaceInByte;
		private int readIndex;

		public BitStream (byte[] bytes)
		{
			buffer = bytes;
			spaceInByte = BYTE_SIZE;
		}

		public void Write (int data, int offset, int bits)
		{
			//TODO Add check to make sure you cant attempt to write past the end of the buffer
			data >>= offset;
			int bitsRemaining = bits;
			int nextBits;
			while (bitsRemaining > 0) {
				if (spaceInByte == 0) {
					spaceInByte = BYTE_SIZE;
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
			//TODO Add check to make sure you cant read past the end of the data buffer
			int byteIndex = offset / BYTE_SIZE;
			int bitIndex = offset % BYTE_SIZE;
			int bitsRemaining = bits;
			int output = 0;
			int nextBits = Math.Min (BYTE_SIZE - bitIndex, bitsRemaining);

			while (bitsRemaining > 0) {
				if (bitIndex == BYTE_SIZE) {
					bitIndex = 0;
					byteIndex++;
				}
				output <<= nextBits;
				output |= GetBits (buffer [byteIndex], BYTE_SIZE - bitIndex - nextBits, nextBits);

				bitIndex += nextBits;
				bitsRemaining -= nextBits;
				nextBits = Math.Min (8 - bitIndex, bitsRemaining);
			}
			return output;
		}

		public int ReadNext (int bits)
		{
			//TODO Add check to make sure you cant read past the end of the data buffer
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

