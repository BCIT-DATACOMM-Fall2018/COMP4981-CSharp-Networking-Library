using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: CollisionElement - A UpdateElement to denote a collision
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public CollisionElement (AbilityType abilityId, int actorHitId, int actorCastId)
	/// 				public CollisionElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			public override void UpdateState (IStateMessageBridge bridge)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		March 8th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class CollisionElement : UpdateElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.CollisionElement);

		private const int ABILITYID_MAX = 127;
		private const int ACTORHITID_MAX = 255;
		private const int ACTORCASTID_MAX = 255;
		private const int COLLISIONID_MAX = 255;

		private static readonly int ABILITYID_BITS = RequiredBits (ABILITYID_MAX);
		private static readonly int ACTORHITID_BITS = RequiredBits (ACTORHITID_MAX);
		private static readonly int ACTORCASTID_BITS = RequiredBits (ACTORCASTID_MAX);
		private static readonly int COLLISIONID_BITS = RequiredBits (COLLISIONID_MAX);


		public AbilityType AbilityId { get; private set; }

		public int ActorHitId { get; private set; }

		public int ActorCastId { get; private set; }

		public int CollisionId { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: CollisionElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public CollisionElement (AbilityType abilityId, int actorHitId, int actorCastId, int collisionId)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public CollisionElement (AbilityType abilityId, int actorHitId, int actorCastId, int collisionId)
		{
			AbilityId = abilityId;
			ActorHitId = actorHitId;
			ActorCastId = actorCastId;
			CollisionId = collisionId;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: CollisionElement
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
		/// 		CollisionElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public CollisionElement (BitStream bitstream) : base (bitstream)
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
		/// RETURNS: 	An ElementIndicatorElement appropriate for a CollisionElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a CollisionElement when 
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
		/// 			CollisionElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a CollisionElement
		/// ----------------------------------------------
		public override int Bits ()
		{
			return ABILITYID_BITS + ACTORHITID_BITS + ACTORCASTID_BITS + COLLISIONID_BITS;
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
		/// 			CollisionElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write ((int)AbilityId, 0, ABILITYID_BITS);
			bitStream.Write	(ActorHitId, 0, ACTORHITID_BITS);
			bitStream.Write	(ActorCastId, 0, ACTORCASTID_BITS);
			bitStream.Write	(CollisionId, 0, COLLISIONID_BITS);
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
		/// 			CollisionElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			AbilityId = (AbilityType)bitstream.ReadNext (ABILITYID_BITS);
			ActorHitId = bitstream.ReadNext (ACTORHITID_BITS);
			ActorCastId = bitstream.ReadNext (ACTORCASTID_BITS);
			CollisionId = bitstream.ReadNext (COLLISIONID_BITS);
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
			bridge.ProcessCollision (AbilityId, ActorHitId, ActorCastId, CollisionId);
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
		/// 			CollisionElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			if ((int)AbilityId > ABILITYID_MAX || ActorHitId > ACTORHITID_MAX || ActorCastId > ACTORCASTID_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

