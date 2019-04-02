using System;

namespace NetworkLibrary.MessageElements
{
    /// ----------------------------------------------
    /// Class: AbilityAssignmentElement - A UpdateElement to give players new abilities
    /// 
    /// PROGRAM: NetworkLibrary
    ///
    /// CONSTRUCTORS:	public AbilityAssignmentElement (int actorId, int abilityId)
    /// 				public AbilityAssignmentElement (BitStream bitstream)
    /// 
    /// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
    ///				protected override void Serialize (BitStream bitStream)
    /// 			protected override void Deserialize (BitStream bitstream)
    /// 			public override void UpdateState (IStateMessageBridge bridge)
    /// 			protected override void Validate ()
    /// 
    /// DATE: 		April 1st, 2019
    ///
    /// REVISIONS: 
    ///
    /// DESIGNER: 	Cameron Roberts
    ///
    /// PROGRAMMER: Kieran Lee
    ///
    /// NOTES:		
    /// ----------------------------------------------
    public class AbilityAssignmentElement : UpdateElement
    {
        private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement(ElementId.AbilityAssignmentElement);

        private const int ABILITY_MAX = 32;
        private const int ACTORID_MAX = 127;
        private static readonly int HEALTH_BITS = RequiredBits(ABILITY_MAX);
        private static readonly int ACTORID_BITS = RequiredBits(ACTORID_MAX);

        public int ActorId { get; private set; }

        public int AbilityId { get; private set; }

        /// ----------------------------------------------
        /// CONSTRUCTOR: AbilityAssignmentElement
        /// 
        /// DATE:		April 1st, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public AbilityAssignmentElement (int actorId, int abilityId)
        /// 
        /// NOTES:		
        /// ----------------------------------------------
        public AbilityAssignmentElement(int actorId, int health)
        {
            ActorId = actorId;
            AbilityId = health;
        }

        /// ----------------------------------------------
        /// CONSTRUCTOR: AbilityAssignmentElement
        /// 
        /// DATE:		April 1st, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public AbilityAssignmentElement (BitStream bitstream)
        /// 
        /// NOTES:	Calls the parent class constructor to create a 
        /// 		AbilityAssignmentElement by deserializing it 
        /// 		from a BitStream object.
        /// ----------------------------------------------
        public AbilityAssignmentElement(BitStream bitstream) : base(bitstream)
        {
        }

        /// ----------------------------------------------
        /// FUNCTION:	GetIndicator
        /// 
        /// DATE:		April 1st, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public override ElementIndicatorElement GetIndicator ()
        /// 
        /// RETURNS: 	An ElementIndicatorElement appropriate for a AbilityAssignmentElement
        /// 
        /// NOTES:		Returns an ElementIndicatorElement to be used 
        /// 			to reconstruct a AbilityAssignmentElement when 
        /// 			deserializing a Packet.
        /// ----------------------------------------------
        public override ElementIndicatorElement GetIndicator()
        {
            return INDICATOR;
        }

        /// ----------------------------------------------
        /// FUNCTION:	Bits
        /// 
        /// DATE:		February 10th, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public int Bits ()
        /// 
        /// RETURNS: 	The number of bits needed to store a
        /// 			AbilityAssignmentElement
        /// 
        /// NOTES:		Returns the number of bits needed to store
        /// 			a AbilityAssignmentElement
        /// ----------------------------------------------
        public override int Bits()
        {
            return HEALTH_BITS + ACTORID_BITS;
        }

        /// ----------------------------------------------
        /// FUNCTION:	Serialize
        /// 
        /// DATE:		April 1st, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	protected override void Serialize (BitStream bitStream)
        /// 
        /// RETURNS: 	void.
        /// 
        /// NOTES:		Contains logic needed to serialize a 
        /// 			AbilityAssignmentElement to a BitStream.
        /// ----------------------------------------------
        protected override void Serialize(BitStream bitStream)
        {
            bitStream.Write(ActorId, 0, ACTORID_BITS);
            bitStream.Write(AbilityId, 0, HEALTH_BITS);
        }

        /// ----------------------------------------------
        /// FUNCTION:	Deserialize
        /// 
        /// DATE:		April 1st, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	protected override void Deserialize (BitStream bitstream)
        /// 
        /// RETURNS: 	void.
        /// 
        /// NOTES:		Contains logic needed to deserialze a 
        /// 			AbilityAssignmentElement from a BitStream.
        /// ----------------------------------------------
        protected override void Deserialize(BitStream bitstream)
        {
            ActorId = bitstream.ReadNext(ACTORID_BITS);
            AbilityId = bitstream.ReadNext(HEALTH_BITS);

        }

        /// ----------------------------------------------
        /// FUNCTION:	UpdateState
        /// 
        /// DATE:		April 1st, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public override void UpdateState (IStateMessageBridge bridge)
        /// 
        /// RETURNS: 	void.
        /// 
        /// NOTES:		Contains logic needed to update the game state through
        /// 			the use of a IStateMessageBridge.
        /// ----------------------------------------------
        public override void UpdateState(IStateMessageBridge bridge)
        {
            bridge.UpdateAbilityAssignment(ActorId, AbilityId);
        }
        
        /// ----------------------------------------------
        /// FUNCTION:	Validate
        /// 
        /// DATE:		April 1st, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Cameron Roberts
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	protected override void Validate ()
        /// 
        /// RETURNS: 	void.
        /// 
        /// NOTES:		Contains logic needed to validate a 
        /// 			AbilityAssignmentElement.
        /// ----------------------------------------------
        protected override void Validate()
        {
            if (ActorId > ACTORID_MAX || AbilityId > ABILITY_MAX)
            {
                throw new System.Runtime.Serialization.SerializationException("Attempt to deserialize invalid packet data");
            }
        }
    }
}

