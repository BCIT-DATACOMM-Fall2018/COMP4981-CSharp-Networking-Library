using System;

namespace NetworkLibrary.MessageElements
{
	public class HealthElement : UpdateElement
	{
		private static readonly ElementIndicatorElement indicator = new ElementIndicatorElement (ElementId.HealthElement);

		public override ElementIndicatorElement GetIndicator ()
		{
			return indicator;
		}

		private const int HEALTH_MAX = 20;
		private const int ACTORID_MAX = 32;
		private static readonly int HEALTH_BITS = RequiredBits (HEALTH_MAX);
		private static readonly int ACTORID_BITS = RequiredBits (ACTORID_MAX);

		public int ActorId { get; private set; }

		public int Health { get; private set; }


		public HealthElement (int actorId, int health)
		{
			ActorId = actorId;
			Health = health;
		}

		public HealthElement (BitStream bitstream) : base (bitstream)
		{
		}


		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write (ActorId, 0, ACTORID_BITS);
			bitStream.Write	(Health, 0, HEALTH_BITS);
		}

		protected override void Deserialize (BitStream bitstream)
		{
			ActorId = bitstream.ReadNext (ACTORID_BITS);
			Health = bitstream.ReadNext (HEALTH_BITS);

		}

		public override void UpdateState (IStateMessageBridge bridge)
		{
			bridge.UpdateActorHealth (ActorId, Health);
		}


		protected override void Validate ()
		{
			if (ActorId > ACTORID_MAX || Health > HEALTH_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

