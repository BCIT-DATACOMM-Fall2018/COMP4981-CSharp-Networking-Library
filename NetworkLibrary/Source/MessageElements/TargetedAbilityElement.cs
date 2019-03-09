using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: TargetedAbilityElement - A UpdateElement to denote an ability use
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public TargetedAbilityElement (int actorId, AbilityType abilityId, int targetId)
	/// 				public TargetedAbilityElement (BitStream bitstream)
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
	public class TargetedAbilityElement : UpdateElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.TargetedAbilityElement);

		private const int ACTORID_MAX = 255;
		private const int ABILITYID_MAX = 127;
		private const int TARGETID_MAX = 255;

		private static readonly int ACTORID_BITS = RequiredBits (ACTORID_MAX);
		private static readonly int ABILITYID_BITS = RequiredBits (ABILITYID_MAX);
		private static readonly int TARGETID_BITS = RequiredBits (TARGETID_MAX);

		public int ActorId { get; private set; }

		public AbilityType AbilityId { get; private set; }

		public int TargetId { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: TargetedAbilityElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public TargetedAbilityElement (int actorId, AbilityType abilityId, int targetId)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public TargetedAbilityElement (int actorId, AbilityType abilityId, int targetId)
		{
			ActorId = actorId;
			AbilityId = abilityId;
			TargetId = targetId;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: TargetedAbilityElement
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
		/// 		TargetedAbilityElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public TargetedAbilityElement (BitStream bitstream) : base (bitstream)
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
		/// RETURNS: 	An ElementIndicatorElement appropriate for a TargetedAbilityElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a TargetedAbilityElement when 
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
		/// 			TargetedAbilityElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a TargetedAbilityElement
		/// ----------------------------------------------
		public override int Bits ()
		{
			return ACTORID_BITS + ABILITYID_BITS + TARGETID_BITS;
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
		/// 			TargetedAbilityElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write	(ActorId, 0, ACTORID_BITS);
			bitStream.Write ((int)AbilityId, 0, ABILITYID_BITS);
			bitStream.Write	(TargetId, 0, TARGETID_BITS);
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
		/// 			TargetedAbilityElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			ActorId = bitstream.ReadNext (ACTORID_BITS);
			AbilityId = (AbilityType)bitstream.ReadNext (ABILITYID_BITS);
			TargetId = bitstream.ReadNext (TARGETID_BITS);

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
			bridge.UseTargetedAbility (ActorId, AbilityId, TargetId);
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
		/// 			TargetedAbilityElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			if ( ActorId > ACTORID_MAX || (int)AbilityId > ABILITYID_MAX || TargetId > TARGETID_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

