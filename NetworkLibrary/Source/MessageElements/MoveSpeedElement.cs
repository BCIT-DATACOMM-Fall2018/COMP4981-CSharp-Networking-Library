using System;

namespace NetworkLibrary.MessageElements
{
    /// ----------------------------------------------
    /// Class: MoveSpeedElement - A UpdateElement to update actor health
    /// 
    /// PROGRAM: NetworkLibrary
    ///
    /// CONSTRUCTORS:	public MoveSpeedElement (int actorId, int health)
    /// 				public MoveSpeedElement (BitStream bitstream)
    /// 
    /// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
    ///				protected override void Serialize (BitStream bitStream)
    /// 			protected override void Deserialize (BitStream bitstream)
    /// 			public override void UpdateState (IStateMessageBridge bridge)
    /// 			protected override void Validate ()
    /// 
    /// DATE: 		March 29th, 2019
    ///
    /// REVISIONS: 
    ///
    /// DESIGNER: 	Kieran Lee
    ///
    /// PROGRAMMER: Kieran Lee
    ///
    /// NOTES:		
    /// ----------------------------------------------
    public class MoveSpeedElement : UpdateElement
    {
        private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement(ElementId.MoveSpeedElement);

        private const int SPEED_MAX = 999;  //speed is represented as a float on the server side and client side. 
                                            //It is smaller to send as an int though
                                            //please treat SPEED max as a float defined by   =>  SPEED_MAX * (10^-2)
        private const int ACTORID_MAX = 127;
        private static readonly int SPEED_BITS = RequiredBits(SPEED_MAX);
        private static readonly int ACTORID_BITS = RequiredBits(ACTORID_MAX);

        public int ActorId { get; private set; }

        public int Speed { get; private set; }

        /// ----------------------------------------------
        /// CONSTRUCTOR: MoveSpeedElement
        /// 
        /// DATE:		March 29th, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Kieran Lee
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public MoveSpeedElement (int actorId, int health)
        /// 
        /// NOTES:		
        /// ----------------------------------------------
        public MoveSpeedElement(int actorId, int speed)
        {
            ActorId = actorId;
            Speed = speed;
        }

        /// ----------------------------------------------
        /// CONSTRUCTOR: MoveSpeedElement
        /// 
        /// DATE:		March 29th, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Kieran Lee
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public MoveSpeedElement (BitStream bitstream)
        /// 
        /// NOTES:	Calls the parent class constructor to create a 
        /// 		MoveSpeedElement by deserializing it 
        /// 		from a BitStream object.
        /// ----------------------------------------------
        public MoveSpeedElement(BitStream bitstream) : base(bitstream)
        {
        }

        /// ----------------------------------------------
        /// FUNCTION:	GetIndicator
        /// 
        /// DATE:		March 29th, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Kieran Lee
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public override ElementIndicatorElement GetIndicator ()
        /// 
        /// RETURNS: 	An ElementIndicatorElement appropriate for a MoveSpeedElement
        /// 
        /// NOTES:		Returns an ElementIndicatorElement to be used 
        /// 			to reconstruct a MoveSpeedElement when 
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
        /// DESIGNER:	Kieran Lee
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	public int Bits ()
        /// 
        /// RETURNS: 	The number of bits needed to store a
        /// 			MoveSpeedElement
        /// 
        /// NOTES:		Returns the number of bits needed to store
        /// 			a MoveSpeedElement
        /// ----------------------------------------------
        public override int Bits()
        {
            return SPEED_BITS + ACTORID_BITS;
        }

        /// ----------------------------------------------
        /// FUNCTION:	Serialize
        /// 
        /// DATE:		March 29th, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Kieran Lee
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	protected override void Serialize (BitStream bitStream)
        /// 
        /// RETURNS: 	void.
        /// 
        /// NOTES:		Contains logic needed to serialize a 
        /// 			MoveSpeedElement to a BitStream.
        /// ----------------------------------------------
        protected override void Serialize(BitStream bitStream)
        {
            bitStream.Write(ActorId, 0, ACTORID_BITS);
            bitStream.Write(Speed, 0, SPEED_BITS);
        }

        /// ----------------------------------------------
        /// FUNCTION:	Deserialize
        /// 
        /// DATE:		March 29th, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Kieran Lee
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	protected override void Deserialize (BitStream bitstream)
        /// 
        /// RETURNS: 	void.
        /// 
        /// NOTES:		Contains logic needed to deserialze a 
        /// 			MoveSpeedElement from a BitStream.
        /// ----------------------------------------------
        protected override void Deserialize(BitStream bitstream)
        {
            ActorId = bitstream.ReadNext(ACTORID_BITS);
            Speed = bitstream.ReadNext(SPEED_BITS);

        }

        /// ----------------------------------------------
        /// FUNCTION:	UpdateState
        /// 
        /// DATE:		March 29th, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Kieran Lee
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
            bridge.UpdateActorSpeed(ActorId, Speed);
        }

        /// ----------------------------------------------
        /// FUNCTION:	Validate
        /// 
        /// DATE:		March 29th, 2019
        /// 
        /// REVISIONS:	
        /// 
        /// DESIGNER:	Kieran Lee
        /// 
        /// PROGRAMMER:	Kieran Lee
        /// 
        /// INTERFACE: 	protected override void Validate ()
        /// 
        /// RETURNS: 	void.
        /// 
        /// NOTES:		Contains logic needed to validate a 
        /// 			MoveSpeedElement.
        /// ----------------------------------------------
        protected override void Validate()
        {
            if (ActorId > ACTORID_MAX || Speed > SPEED_MAX)
            {
                throw new System.Runtime.Serialization.SerializationException("Attempt to deserialize invalid packet data");
            }
        }
    }
}

