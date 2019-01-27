using NUnit.Framework;
using System;
using NetworkLibrary;

namespace NetworkLibraryTest
{
	[TestFixture ()]
	public class HealthElementTest
	{
		[Test ()]
		public void SerializeHealthElement ()
		{
			HealthElement element = new HealthElement (10, 18);
			BitStream bitstream = new BitStream ();
			element.WriteTo (bitstream);

			HealthElement element2 = new HealthElement (0, 0);
			element2.ReadFrom (bitstream);

			Assert.AreEqual (element.ActorId, element2.ActorId);
			Assert.AreEqual (element.Health, element2.Health);

		}
	}
}

