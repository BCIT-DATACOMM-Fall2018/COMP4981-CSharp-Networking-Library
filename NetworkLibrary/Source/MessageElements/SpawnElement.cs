﻿using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: SpawnElement - A UpdateElement to cause an actor to be spawned
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public SpawnElement (ActorType actorType, int actorId, int team, int x, int z)
	/// 				public SpawnElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			public override void UpdateState (IStateMessageBridge bridge)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		March 8th, 2019
	///
	/// REVISIONS:  March 17th, 2019
	/// 				Modified class to support teams
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class SpawnElement : UpdateElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.SpawnElement);

		private const int ACTORTYPE_MAX = 31;
		private const int ACTORID_MAX = 255;
		private const int ACTORTEAM_MAX = 7;
		private const float X_MAX = 500;
		private const float Z_MAX = 500;

		private static readonly int ACTORTYPE_BITS = RequiredBits (ACTORTYPE_MAX);
		private static readonly int ACTORID_BITS = RequiredBits (ACTORID_MAX);
		private static readonly int ACTORTEAM_BITS = RequiredBits (ACTORTEAM_MAX);
		private static readonly int X_BITS = sizeof (float)*BitStream.BYTE_SIZE;
		private static readonly int Z_BITS = sizeof (float)*BitStream.BYTE_SIZE;

		public ActorType ActorType { get; private set; }

		public int ActorId { get; private set; }

		public int ActorTeam { get; private set; }

		public float X { get; private set; }

		public float Z { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: SpawnElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public SpawnElement (ActorType actorType, int actorId, int actorTeam, float x, float z)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public SpawnElement (ActorType actorType, int actorId, int actorTeam, float x, float z)
		{
			ActorType = actorType;
			ActorId = actorId;
			ActorTeam = actorTeam;
			X = x;
			Z = z;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: SpawnElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public HealthElement (BitStream bitstream)
		/// 
		/// NOTES:	Calls the parent class constructor to create a 
		/// 		SpawnElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public SpawnElement (BitStream bitstream) : base (bitstream)
		{
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetIndicator
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public override ElementIndicatorElement GetIndicator ()
		/// 
		/// RETURNS: 	An ElementIndicatorElement appropriate for a SpawnElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a SpawnElement when 
		/// 			deserializing a Packet.
		/// ----------------------------------------------
		public override ElementIndicatorElement GetIndicator ()
		{
			return INDICATOR;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Bits
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public int Bits ()
		/// 
		/// RETURNS: 	The number of bits needed to store a
		/// 			SpawnElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a SpawnElement
		/// ----------------------------------------------
		public override int Bits(){
			return ACTORTYPE_BITS + ACTORID_BITS + ACTORTEAM_BITS + X_BITS + Z_BITS;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Serialize
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected override void Serialize (BitStream bitStream)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Contains logic needed to serialize a 
		/// 			SpawnElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write ((int)ActorType, 0, ACTORTYPE_BITS);
			bitStream.Write (ActorId, 0, ACTORID_BITS);
			bitStream.Write (ActorTeam, 0, ACTORTEAM_BITS);

			byte[] bytes = BitConverter.GetBytes (X);
			foreach (var item in bytes) {
				bitStream.Write (item, 0, BitStream.BYTE_SIZE);
			}

			bytes = BitConverter.GetBytes (Z);
			foreach (var item in bytes) {
				bitStream.Write (item, 0, BitStream.BYTE_SIZE);
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	Deserialize
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected override void Deserialize (BitStream bitstream)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Contains logic needed to deserialze a 
		/// 			SpawnElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			ActorType = (ActorType)bitstream.ReadNext (ACTORTYPE_BITS);
			ActorId = bitstream.ReadNext (ACTORID_BITS);
			ActorTeam = bitstream.ReadNext (ACTORTEAM_BITS);

			byte[] bytes = new byte[sizeof(float)];
			for (int i = 0; i < sizeof(float); i++) {
				bytes [i] = bitstream.ReadNextByte (BitStream.BYTE_SIZE);
			}
			X = BitConverter.ToSingle (bytes, 0);
			bytes = new byte[sizeof(float)];
			for (int i = 0; i < sizeof(float); i++) {
				bytes [i] = bitstream.ReadNextByte (BitStream.BYTE_SIZE);
			}
			Z = BitConverter.ToSingle (bytes, 0);
		}

		/// ----------------------------------------------
		/// FUNCTION:	UpdateState
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public override void UpdateState (IStateMessageBridge bridge)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Contains logic needed to update the game state through
		/// 			the use of a IStateMessageBridge.
		/// ----------------------------------------------
		public override void UpdateState (IStateMessageBridge bridge)
		{
			bridge.SpawnActor (ActorType, ActorId, ActorTeam, X, Z);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Validate
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected override void Validate ()
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Contains logic needed to validate a 
		/// 			SpawnElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			if ((int)ActorType > ACTORTYPE_MAX || ActorId > ACTORID_MAX || ActorTeam > ACTORTEAM_MAX || X > X_MAX || Z > Z_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

