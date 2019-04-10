using NUnit.Framework;
using System;
using NetworkLibrary;

namespace NetworkLibraryTest
{
	[TestFixture ()]
	public class BitStreamTest
	{
		[Test ()]
		public void WriteLessThanByteTest ()
		{
			byte[] bytes = new byte[1024];
			int testData = 14;
			int bits = 4;
			BitStream bitstream = new BitStream (bytes);
			bitstream.Write (testData, 0, bits);
			int retrievedData = bitstream.Read (0, bits);
			Assert.AreEqual (testData, retrievedData);
		}

		[Test ()]
		public void WriteFullByteTest ()
		{
			byte[] bytes = new byte[1024];

			int testData = 225;
			int bits = 8;
			BitStream bitstream = new BitStream (bytes);
			bitstream.Write (testData, 0, bits);
			int retrievedData = bitstream.Read (0, bits);
			Assert.AreEqual (testData, retrievedData);
		}

		[Test ()]
		public void WriteMoreThanAByteTest ()
		{
			byte[] bytes = new byte[1024];

			int testData = 1057;
			int bits = 20;
			BitStream bitstream = new BitStream (bytes);
			bitstream.Write (testData, 0, bits);
			int retrievedData = bitstream.Read (0, bits);
			Assert.AreEqual (testData, retrievedData);
		}

		[Test ()]
		public void WriteOffsetTest ()
		{
			byte[] bytes = new byte[1024];

			int testData = 1057;
			int bits = 20;
			int offset = 10;
			BitStream bitstream = new BitStream (bytes);
			bitstream.Write (testData, offset, bits);
			int retrievedData = bitstream.Read (0, bits);
			Assert.AreEqual (testData >> offset, retrievedData);
		}

		[Test ()]
		public void WriteMultipleTest ()
		{
			byte[] bytes = new byte[1024];

			int testData = 1057;
			int testData2 = 15;
			int testData3 = 4;
		
			int bits = 20;
			int bits2 = 4;
			int bits3 = 3;

			BitStream bitstream = new BitStream (bytes);

			bitstream.Write (testData, 0, bits);
			bitstream.Write (testData2, 0, bits2);
			bitstream.Write (testData3, 0, bits3);

			int retrievedData = bitstream.Read (0, bits);
			int retrievedData2 = bitstream.Read (bits, bits2);
			int retrievedData3 = bitstream.Read (bits + bits2, bits3);

			Assert.AreEqual (testData, retrievedData);
			Assert.AreEqual (testData2, retrievedData2);
			Assert.AreEqual (testData3, retrievedData3);
		}

		[Test ()]
		public void ReadNextTest ()
		{
			byte[] bytes = new byte[1024];

			int testData = 1057;
			int testData2 = 15;
			int testData3 = 4;

			int bits = 20;
			int bits2 = 4;
			int bits3 = 3;

			BitStream bitstream = new BitStream (bytes);

			bitstream.Write (testData, 0, bits);
			bitstream.Write (testData2, 0, bits2);
			bitstream.Write (testData3, 0, bits3);

			int retrievedData = bitstream.ReadNext (bits);
			int retrievedData2 = bitstream.ReadNext (bits2);
			int retrievedData3 = bitstream.ReadNext (bits3);

			Assert.AreEqual (testData, retrievedData);
			Assert.AreEqual (testData2, retrievedData2);
			Assert.AreEqual (testData3, retrievedData3);
		}

		[Test ()]
		public void SerializeByte ()
		{
			byte[] bytes = new byte[1024];
			byte testData = 14;
			int bits = 4;
			BitStream bitstream = new BitStream (bytes);
			bitstream.Write (testData, 0, bits);
			byte retrievedData = bitstream.ReadByte (0, bits);
			Assert.AreEqual (testData, retrievedData);
		}
	}
}

