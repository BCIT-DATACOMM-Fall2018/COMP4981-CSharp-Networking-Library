using NUnit.Framework;
using System;
using NetworkLibrary;
using System.Collections.Generic;

namespace NetworkLibraryTest
{
	[TestFixture ()]
	public class ReliableUDPConnectionTest
	{
		
		[Test ()]
		public void AddReliableMessagesToBuffer ()
		{
			List<MessageElement> unreliableElements = new List<MessageElement> ();
			List<MessageElement> reliableElements = new List<MessageElement> ();
			unreliableElements.Add(new HealthElement(10,10));
			reliableElements.Add(new HealthElement(10,10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();

			conn.CreatePacket (new UnpackedPacket(unreliableElements, reliableElements));
			conn.CreatePacket (new UnpackedPacket(unreliableElements, reliableElements));

			Assert.AreEqual (conn.messageIndex, 2);
		}

		[Test ()]
		public void UnpackingUnreliableElements ()
		{

			List<MessageElement> unreliableElements = new List<MessageElement> ();
			List<MessageElement> reliableElements = new List<MessageElement> ();
			unreliableElements.Add(new HealthElement(10,10));
			reliableElements.Add(new HealthElement(10,10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			UnpackedPacket unpacked1 = new UnpackedPacket (unreliableElements, reliableElements);
			Packet packet = conn.CreatePacket (unpacked1);
			UnpackedPacket unpacked2 = conn.ProcessPacket (packet);

			Assert.AreEqual (unpacked1.UnreliableElements.Count, unpacked2.UnreliableElements.Count);

			for (int i = 0; i < unpacked1.UnreliableElements.Count; i++) {
				Assert.AreEqual (unpacked1.UnreliableElements[i], unpacked2.UnreliableElements[i]);

			}
		}

		[Test ()]
		public void UnpackingReliableElements ()
		{

			List<MessageElement> unreliableElements = new List<MessageElement> ();
			List<MessageElement> reliableElements = new List<MessageElement> ();
			unreliableElements.Add(new HealthElement(10,10));
			reliableElements.Add(new HealthElement(10,10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			UnpackedPacket unpacked1 = new UnpackedPacket (unreliableElements, reliableElements);
			Packet packet = conn.CreatePacket (unpacked1);
			UnpackedPacket unpacked2 = conn.ProcessPacket (packet);

			Assert.AreEqual (unpacked1.ReliableElements.Count, unpacked2.ReliableElements.Count);

			for (int i = 0; i < unpacked1.ReliableElements.Count; i++) {
				Assert.AreEqual (unpacked1.ReliableElements[i], unpacked2.ReliableElements[i]);

			}
		}
	}
}

