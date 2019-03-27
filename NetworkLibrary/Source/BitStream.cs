using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Class: 	BitStream - A class to serialize/deserialize values into/from a byte array.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public BitStream (byte[] bytes)
	/// 
	/// FUNCTIONS:	public void Write (int data, int offset, int bits)
	///				public int Read (int offset, int bits)
	/// 			public int ReadNext (int bits)
	/// 			private static int GetBits (int data, int offset, int bits)
	/// 
	/// DATE: 		January 28th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		When reading data from the byte array the data should be
	/// 			read in the same order it was written.
	/// ----------------------------------------------
	public class BitStream
	{
		public const int BYTE_SIZE = 8;
		private byte[] buffer;
		private uint wordCount;
		private int spaceInByte;
		private int readIndex;

		/// ----------------------------------------------
		/// CONSTRUCTOR: Bitstream
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public BitStream (byte[] bytes)
		/// 				byte[] bytes: A byte array to read/write data from/to.			
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public BitStream (byte[] bytes)
		{
			buffer = bytes;
			spaceInByte = BYTE_SIZE;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Write
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public void Write (int data, int offset, int bits)
		/// 				int data: The data to be written to the byte array.
		/// 				int offset: An offset on the data to be written.
		/// 				int bits: The number of bits to write
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		The offset and number of bits are calculated from the
		/// 			least significant bit.
		/// 			Example: data = 89 (0000 0000 0000 0000 0000 0000 0101 1001)
		/// 					 offset = 2
		/// 					 bits = 5
		/// 			Would result in 10110 being writted to the byte array
		/// ----------------------------------------------
		public void Write (int data, int offset, int bits)
		{
			//TODO Add check to make sure bits isnt larger than the size of data
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

		/// ----------------------------------------------
		/// FUNCTION:	Write
		/// 
		/// DATE:		March 10th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public void Write (byte data, int offset, int bits)
		/// 				byte data: The data to be written to the byte array.
		/// 				int offset: An offset on the data to be written.
		/// 				int bits: The number of bits to write
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:	Functions identically to the function Write(int data, int offset, int bits)7
		/// 		with the exception that this function writes a byte rather than an int.
		/// ----------------------------------------------
		public void Write (byte data, int offset, int bits)
		{
			//TODO Add check to make sure bits isnt larger than the size of data
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

		/// ----------------------------------------------
		/// FUNCTION:	Read
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public int Read (int offset, int bits)
		/// 				int offset: An offset on the data to be read.
		/// 				int bits: The number of bits to be read
		/// 
		/// RETURNS: 	An integer representation of the bits read from the
		/// 			byte array.
		/// 
		/// NOTES:		This function will read the specified number of bits
		/// 			from the byte array starting at the offset. It is
		/// 			recommended to use ReadNext instead of this function
		/// 			when deserializing data.
		/// ----------------------------------------------
		public int Read (int offset, int bits)
		{
			//TODO Add check to make sure bits isnt larger than could be returned in an integer
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

		/// ----------------------------------------------
		/// FUNCTION:	ReadByte
		/// 
		/// DATE:		March 10th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public int ReadByte (int offset, int bits)
		/// 				int offset: An offset on the data to be read.
		/// 				int bits: The number of bits to be read
		/// 
		/// RETURNS: 	An integer representation of the bits read from the
		/// 			byte array.
		/// 
		/// NOTES:		Functions the same as Read but returns a byte.
		/// ----------------------------------------------
		public byte ReadByte (int offset, int bits)
		{
			//TODO Add check to make sure bits isnt larger than could be returned in an integer
			//TODO Add check to make sure you cant read past the end of the data buffer
			int byteIndex = offset / BYTE_SIZE;
			int bitIndex = offset % BYTE_SIZE;
			int bitsRemaining = bits;
			byte output = 0;
			int nextBits = Math.Min (BYTE_SIZE - bitIndex, bitsRemaining);

			while (bitsRemaining > 0) {
				if (bitIndex == BYTE_SIZE) {
					bitIndex = 0;
					byteIndex++;
				}
				output <<= nextBits;
				output |= (byte)GetBits (buffer [byteIndex], BYTE_SIZE - bitIndex - nextBits, nextBits);

				bitIndex += nextBits;
				bitsRemaining -= nextBits;
				nextBits = Math.Min (8 - bitIndex, bitsRemaining);
			}
			return output;
		}

		/// ----------------------------------------------
		/// FUNCTION:	ReadNext
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public int ReadNext (int bits)
		/// 				int bits: The number of bits to be read
		/// 
		/// RETURNS: 	An integer representation of the bits read from the
		/// 			byte array.
		/// 
		/// NOTES:		This function will read the specified number of bits
		/// 			from the byte array and advance the readIndex. The next
		/// 			call to this function will read will read bits starting
		/// 			from that point.
		/// ----------------------------------------------
		public int ReadNext (int bits)
		{
			//TODO Add check to make sure you cant read past the end of the data buffer
			int result = Read (readIndex, bits);
			readIndex += bits;
			return result;
		}

		/// ----------------------------------------------
		/// FUNCTION:	ReadNextByte
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public int ReadNextByte (int bits)
		/// 				int bits: The number of bits to be read
		/// 
		/// RETURNS: 	An integer representation of the bits read from the
		/// 			byte array.
		/// 
		/// NOTES:		Functions as ReadNext but returns a byte instead of an int.
		/// ----------------------------------------------
		public byte ReadNextByte (int bits)
		{
			//TODO Add check to make sure you cant read past the end of the data buffer
			byte result = ReadByte (readIndex, bits);
			readIndex += bits;
			return result;
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetBits
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	private static int GetBits (int data, int offset, int bits)
		/// 				int data: The data to be get bits from.
		/// 				int offset: An offset on the data to be gotten.
		/// 				int bits: The number of bits to get.
		/// 
		/// RETURNS: 	An integer with the only the requested bits brought down
		/// 			to the least significant bit.
		/// 
		/// NOTES:		The offset and number of bits are calculated from the
		/// 			least significant bit.
		/// 			Example: data = 189 (0000 0000 0000 0000 0000 0000 1011 1101)
		/// 					 offset = 2
		/// 					 bits = 4
		/// 			Would result in the integer 15 (0000 0000 0000 0000 0000 0000 0000 1111)
		/// 			being returned.
		/// ----------------------------------------------
		private static int GetBits (int data, int offset, int bits)
		{
			data >>= offset;
			int temp = ((int)Math.Pow (2, bits) - 1);
			return data & temp;
		}
	}
}

