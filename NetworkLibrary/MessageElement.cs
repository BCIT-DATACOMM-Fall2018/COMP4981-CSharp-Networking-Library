using System;

namespace NetworkLibrary
{
	public abstract class MessageElement
	{
		public int ID { get; private set; }

		public virtual int SIZE { get;}

		public MessageElement ()
		{

		}

		public void WriteTo (BitStream bitStream){
			Serialize(bitStream);
		}

		protected abstract void Serialize(BitStream bitstream);

		public void ReadFrom (BitStream bitstream){
			Deserialize(bitstream);
			Validate ();
		}

		protected abstract void Deserialize(BitStream bitstream);

		protected abstract void Validate ();

		public abstract void UpdateState(IStateMessageBridge bridge);


		protected static int RequiredBits (int v)
		{
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v++;
			return v;
		}
	}
}

