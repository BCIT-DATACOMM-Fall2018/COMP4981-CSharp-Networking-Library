using NUnit.Framework;
using System;
using NetworkLibrary;
using NetworkLibrary.MessageElements;

namespace NetworkLibraryTest
{
	[TestFixture ()]
	public class HealthElementTest
	{
		[Test ()]
		public void SerializeHealthElement ()
		{
			byte[] bytes = new byte[1024];

			HealthElement element = new HealthElement (10, 18);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			HealthElement element2 = new HealthElement (0, 0);
			element2.ReadFrom (bitstream);

			Assert.AreEqual (element.ActorId, element2.ActorId);
			Assert.AreEqual (element.Health, element2.Health);

		}

		[Test ()]
		public void RequiredBitsTest ()
		{
			int bitsNeeded = MessageElement.RequiredBits (25);

			Assert.AreEqual (bitsNeeded, 5);

		}
	
	}
}

