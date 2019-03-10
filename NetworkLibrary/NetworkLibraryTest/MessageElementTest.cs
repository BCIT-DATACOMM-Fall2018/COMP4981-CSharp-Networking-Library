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
		public void SerializeSpawnElement ()
		{
			byte[] bytes = new byte[1024];

			SpawnElement element = new SpawnElement (ActorType.Player, 1,0,0);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			SpawnElement element2 = new SpawnElement (bitstream);

			Assert.AreEqual (element.ActorId, element2.ActorId);
			Assert.AreEqual (element.ActorType, element2.ActorType);
			Assert.AreEqual (element.X, element2.X);
			Assert.AreEqual (element.Y, element2.Y);

		}

		[Test ()]
		public void SerializePositionElement ()
		{
			byte[] bytes = new byte[1024];

			PositionElement element = new PositionElement (1,0,0);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			PositionElement element2 = new PositionElement (bitstream);

			Assert.AreEqual (element.ActorId, element2.ActorId);
			Assert.AreEqual (element.X, element2.X);
			Assert.AreEqual (element.Y, element2.Y);

		}

		[Test ()]
		public void RequiredBitsTest ()
		{
			int bitsNeeded = MessageElement.RequiredBits (25);

			Assert.AreEqual (bitsNeeded, 5);

		}
	
	}
}

