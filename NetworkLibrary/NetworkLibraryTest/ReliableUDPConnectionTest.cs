using NUnit.Framework;
using System;
using NetworkLibrary;
using System.Collections.Generic;
using NetworkLibrary.MessageElements;

namespace NetworkLibraryTest
{
	[TestFixture ()]
	public class ReliableUDPConnectionTest
	{
		
		[Test ()]
		public void AddReliableMessagesToBuffer ()
		{
			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();

			conn.CreatePacket (unreliableElements, reliableElements);
			conn.CreatePacket (unreliableElements, reliableElements);

			Assert.AreEqual (conn.MessageIndex, 2);
		}

		[Test ()]
		public void UnpackingUnreliableElements ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			UnpackedPacket unpacked = conn.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });

			Assert.AreEqual (unreliableElements.Count, unpacked.UnreliableElements.Count);

		}

		[Test ()]
		public void UnpackingReliableElements ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			UnpackedPacket unpacked = conn.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });

			Assert.AreEqual (reliableElements.Count, unpacked.ReliableElements.Count);

		}


		[Test ()]
		public void ConnectionAckNumberIncreases ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			ReliableUDPConnection conn2 = new ReliableUDPConnection ();

			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			Packet packet2 = conn.CreatePacket (unreliableElements, reliableElements);
			conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (conn2.CurrentAck, 2);
			conn2.ProcessPacket (packet2, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (conn2.CurrentAck, 3);
		}

		[Test ()]
		public void DuplicatePacketDoesNotIncreaseCurrentAck ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			ReliableUDPConnection conn2 = new ReliableUDPConnection ();

			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (conn2.CurrentAck, 2);
			conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreNotEqual (conn2.CurrentAck, 3);
		}

		[Test ()]
		public void ReliableElementsWillBeResent ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			ReliableUDPConnection conn2 = new ReliableUDPConnection ();

			conn.CreatePacket (unreliableElements, reliableElements);
			conn.CreatePacket (unreliableElements, reliableElements);
			conn.CreatePacket (unreliableElements, reliableElements);
			conn.CreatePacket (unreliableElements, reliableElements);
			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			UnpackedPacket unpacked2 = conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (unpacked2.ReliableElements.Count, 5);
		}

		[Test ()]
		public void MultipleReliableElementsInOnePacket ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (15, 18));
			reliableElements.Add (new HealthElement (9, 1));
			reliableElements.Add (new HealthElement (2, 3));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			ReliableUDPConnection conn2 = new ReliableUDPConnection ();

			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			UnpackedPacket unpacked2 = conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (unpacked2.ReliableElements.Count, 4);
		}

		[Test ()]
		public void NoReliableElements ()
		{
			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection ();
			ReliableUDPConnection conn2 = new ReliableUDPConnection ();

			Packet packet = conn.CreatePacket (unreliableElements);
			UnpackedPacket unpacked2 = conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (unpacked2.UnreliableElements.Count, 1);
		}
	}
}

