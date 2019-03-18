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
			reliableElements.Add (new SpawnElement (ActorType.Player, 1,3,  0, 0));
			ReliableUDPConnection conn = new ReliableUDPConnection (1);

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
			unreliableElements.Add (new PositionElement (1,10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			Packet packet = conn.CreatePacket (unreliableElements);
			UnpackedPacket unpacked = conn.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement, ElementId.PositionElement });

			Assert.AreEqual (unreliableElements.Count, unpacked.UnreliableElements.Count);

		}

		[Test ()]
		public void UnpackingReliableElements ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new SpawnElement (ActorType.Player, 1,3, 0, 0));
			reliableElements.Add (new ReadyElement (true, 0, 1));

			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			ReliableUDPConnection conn2 = new ReliableUDPConnection (2);

			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			UnpackedPacket unpacked = conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });

			Assert.AreEqual (reliableElements.Count, unpacked.ReliableElements.Count);

		}


		[Test ()]
		public void ConnectionAckNumberIncreases ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			ReliableUDPConnection conn2 = new ReliableUDPConnection (1);

			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			Packet packet2 = conn.CreatePacket (unreliableElements, reliableElements);
			conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (conn2.CurrentAck, 1);
			conn2.ProcessPacket (packet2, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (conn2.CurrentAck, 2);
		}

		[Test ()]
		public void DuplicatePacketDoesNotIncreaseCurrentAck ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			ReliableUDPConnection conn2 = new ReliableUDPConnection (1);

			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (conn2.CurrentAck, 1);
			conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreNotEqual (conn2.CurrentAck, 2);
		}

		[Test ()]
		public void ReliableElementsWillBeResent ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			reliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			ReliableUDPConnection conn2 = new ReliableUDPConnection (1);

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

			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			ReliableUDPConnection conn2 = new ReliableUDPConnection (1);

			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			UnpackedPacket unpacked2 = conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (unpacked2.ReliableElements.Count, 4);
		}

		[Test ()]
		public void NoReliableElements ()
		{
			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			ReliableUDPConnection conn2 = new ReliableUDPConnection (1);

			Packet packet = conn.CreatePacket (unreliableElements);
			UnpackedPacket unpacked2 = conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			Assert.AreEqual (unpacked2.UnreliableElements.Count, 1);
		}

		[Test ()]
		public void VariablePacketSize ()
		{
			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection (1);

			Packet packet = conn.CreatePacket (unreliableElements);
			Assert.AreEqual (packet.Length, 7);
		}

		[Test ()]
		public void GetPlayerID ()
		{
			int playerID = 5;
			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));

			ReliableUDPConnection conn = new ReliableUDPConnection (playerID);

			Packet packet = conn.CreatePacket (unreliableElements);
			int extractedPlayerID = ReliableUDPConnection.GetPlayerID(packet);
			Assert.AreEqual (playerID, extractedPlayerID);
		}

		[Test ()]
		public void OverfillMessageBuffer ()
		{
			int playerID = 5;
			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			reliableElements.Add (new HealthElement (10, 10));


			ReliableUDPConnection conn = new ReliableUDPConnection (playerID);

			Assert.Throws<InsufficientMemoryException> (() => {
				for (int i = 0; i < 4000; i++) {
					Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
				}
			});
		}

		[Test ()]
		public void ReliableElementCatchUp ()
		{
			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			reliableElements.Add (new HealthElement (10, 10));
			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			ReliableUDPConnection conn2 = new ReliableUDPConnection (2);

			UnpackedPacket unpacked;
			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			for (int i = 0; i < 5; i++) {
				packet = conn.CreatePacket (unreliableElements, reliableElements);
				unpacked = conn2.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement });
			}

			Assert.AreEqual (conn2.CurrentAck, 6);

		}

		[Test ()]
		public void UnpackingLobbyStatusElement ()
		{

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			unreliableElements.Add (new HealthElement (10, 10));
			unreliableElements.Add (new PositionElement (1,10, 10));
			List<LobbyStatusElement.PlayerInfo> playerInfo = new List<LobbyStatusElement.PlayerInfo> ();
			playerInfo.Add (new LobbyStatusElement.PlayerInfo (0, "Alice", 1, true));
			playerInfo.Add (new LobbyStatusElement.PlayerInfo (1, "Bob", 2, false));
			playerInfo.Add (new LobbyStatusElement.PlayerInfo (2, "Cedric", 2, true));

			reliableElements.Add(new LobbyStatusElement (playerInfo));


			ReliableUDPConnection conn = new ReliableUDPConnection (1);
			Packet packet = conn.CreatePacket (unreliableElements, reliableElements);
			UnpackedPacket unpacked = conn.ProcessPacket (packet, new ElementId[] { ElementId.HealthElement, ElementId.PositionElement });

			Assert.AreEqual (unreliableElements.Count, unpacked.UnreliableElements.Count);

		}
	}
}

