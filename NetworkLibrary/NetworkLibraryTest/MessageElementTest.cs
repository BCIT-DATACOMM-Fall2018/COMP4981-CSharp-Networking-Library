﻿using NUnit.Framework;
using System;
using NetworkLibrary;
using NetworkLibrary.MessageElements;
using System.Collections.Generic;

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

			SpawnElement element = new SpawnElement (ActorType.HumanPlayerA, 1, 6,8,100);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			SpawnElement element2 = new SpawnElement (bitstream);

			Assert.AreEqual (element.ActorId, element2.ActorId);
			Assert.AreEqual (element.ActorType, element2.ActorType);
			Assert.AreEqual (element.ActorTeam, element2.ActorTeam);
			Assert.AreEqual (element.X, element2.X);
			Assert.AreEqual (element.Z, element2.Z);

		}

		[Test ()]
		public void SerializePositionElement ()
		{
			byte[] bytes = new byte[1024];

			PositionElement element = new PositionElement (1,18.298F,-10.4F);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			PositionElement element2 = new PositionElement (bitstream);

			Assert.AreEqual (element.ActorId, element2.ActorId);
			Assert.AreEqual (element.X, element2.X);
			Assert.AreEqual (element.Z, element2.Z);

		}

		[Test ()]
		public void SerializeCollisionElement ()
		{
			byte[] bytes = new byte[1024];

			CollisionElement element = new CollisionElement (0,7,7, 100);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			CollisionElement element2 = new CollisionElement (bitstream);

			Assert.AreEqual (element.AbilityId, element2.AbilityId);
			Assert.AreEqual (element.ActorCastId, element2.ActorCastId);
			Assert.AreEqual (element.ActorHitId, element2.ActorHitId);
			Assert.AreEqual (element.CollisionId, element2.CollisionId);

		}

		[Test ()]
		public void SerializeAreaAbilityElement ()
		{
			byte[] bytes = new byte[1024];

			AreaAbilityElement element = new AreaAbilityElement (0 ,AbilityType.TestProjectile,7, 0, 10);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			AreaAbilityElement element2 = new AreaAbilityElement (bitstream);

			Assert.AreEqual (element.AbilityId, element2.AbilityId);
			Assert.AreEqual (element.ActorId, element2.ActorId);
			Assert.AreEqual (element.CollisionId, element2.CollisionId);
			Assert.AreEqual (element.X, element2.X);
			Assert.AreEqual (element.Z, element2.Z);

		}

		[Test ()]
		public void SerializeLobbyStatusElement ()
		{
			byte[] bytes = new byte[1024];
			List<LobbyStatusElement.PlayerInfo> playerInfo = new List<LobbyStatusElement.PlayerInfo> ();
			playerInfo.Add (new LobbyStatusElement.PlayerInfo (0, "Alice", 1, true));
			playerInfo.Add (new LobbyStatusElement.PlayerInfo (1, "Bob", 2, false));
			playerInfo.Add (new LobbyStatusElement.PlayerInfo (2, "Cedric", 2, true));

			LobbyStatusElement element = new LobbyStatusElement (playerInfo);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			LobbyStatusElement element2 = new LobbyStatusElement (bitstream);
			int bits = element.Bits ();

			for (int i = 0; i < element.PlayerInformation.Count; i++) {
				Assert.AreEqual (element.PlayerInformation[i].Id, element.PlayerInformation[i].Id);
				Assert.AreEqual (element.PlayerInformation[i].Team, element.PlayerInformation[i].Team);
				Assert.AreEqual (element.PlayerInformation[i].ReadyStatus, element.PlayerInformation[i].ReadyStatus);
				Assert.IsTrue (element.PlayerInformation [i].Name.CompareTo (element.PlayerInformation [i].Name) == 0);
			}
		}

		[Test ()]
		public void SerializeReadyElement ()
		{
			byte[] bytes = new byte[1024];

			ReadyElement element = new ReadyElement (true,0,1);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			ReadyElement element2 = new ReadyElement (bitstream);
			Assert.AreEqual (element.Ready, element2.Ready);
			Assert.AreEqual (element.ClientId, element2.ClientId);
			Assert.AreEqual (element.Team, element2.Team);
		}

		[Test ()]
		public void SerializeGameEndElement ()
		{
			byte[] bytes = new byte[1024];

			GameEndElement element = new GameEndElement (3);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			GameEndElement element2 = new GameEndElement (bitstream);
			Assert.AreEqual (element.WinningTeam, element2.WinningTeam);
		}

		[Test ()]
		public void SerializeExperienceElement ()
		{
			byte[] bytes = new byte[1024];

			ExperienceElement element = new ExperienceElement (3, 3001);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			ExperienceElement element2 = new ExperienceElement (bitstream);
			Assert.AreEqual (element.Experience, element2.Experience);
			Assert.AreEqual (element.ActorId, element2.ActorId);

		}

		[Test ()]
		public void SerializeAbilityAssignmentElement ()
		{
			byte[] bytes = new byte[1024];

			AbilityAssignmentElement element = new AbilityAssignmentElement (3, 3);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			AbilityAssignmentElement element2 = new AbilityAssignmentElement (bitstream);
			Assert.AreEqual (element.ActorId, element2.ActorId);
			Assert.AreEqual (element.AbilityId, element2.AbilityId);

		}

		[Test ()]
		public void SerializePacketHeaderElement ()
		{
			byte[] bytes = new byte[1024];

			PacketHeaderElement element = new PacketHeaderElement (PacketType.RequestPacket, 1, 1, 1);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			PacketHeaderElement element2 = new PacketHeaderElement (bitstream);
			Assert.AreEqual (element.ReliableElements, element2.ReliableElements);
			Assert.AreEqual (element.AckNumber, element2.AckNumber);
			Assert.AreEqual (element.SeqNumber, element2.SeqNumber);
			Assert.AreEqual (element.Type, element2.Type);
		}

		[Test ()]
		public void SerializeRemainingLivesElement ()
		{
			byte[] bytes = new byte[1024];

			var livesInfo = new List<RemainingLivesElement.LivesInfo> ();
			livesInfo.Add(new RemainingLivesElement.LivesInfo(1, 5));
			livesInfo.Add(new RemainingLivesElement.LivesInfo(2, 50));
			RemainingLivesElement element = new RemainingLivesElement (livesInfo);
			BitStream bitstream = new BitStream (bytes);
			element.WriteTo (bitstream);

			RemainingLivesElement element2 = new RemainingLivesElement (bitstream);
			Assert.AreEqual (element.TeamLivesInfo.Count, element2.TeamLivesInfo.Count);
			for (int i = 0; i < element.TeamLivesInfo.Count; i++) {
				Assert.AreEqual (element.TeamLivesInfo [i].TeamNumber, element2.TeamLivesInfo [i].TeamNumber);
				Assert.AreEqual (element.TeamLivesInfo [i].Lives, element2.TeamLivesInfo [i].Lives);
			}
		}

		[Test ()]
		public void RequiredBitsTest ()
		{
			int bitsNeeded = MessageElement.RequiredBits (25);

			Assert.AreEqual (bitsNeeded, 5);

		}


	
	}
}

