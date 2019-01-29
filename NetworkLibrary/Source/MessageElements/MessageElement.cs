using System;

namespace NetworkLibrary.MessageElements
{
	public abstract class MessageElement
	{

		public MessageElement ()
		{
		}

		public abstract ElementIndicatorElement GetIndicator ();

		public MessageElement (BitStream bitstream)
		{
			ReadFrom (bitstream);
		}

		public void WriteTo (BitStream bitStream)
		{
			Serialize (bitStream);
		}

		protected abstract void Serialize (BitStream bitstream);

		public void ReadFrom (BitStream bitstream)
		{
			Deserialize (bitstream);
			Validate ();
		}

		protected abstract void Deserialize (BitStream bitstream);

		protected abstract void Validate ();

		public static int RequiredBits (int v)
		{
			int r = 0;
			while ((v >>= 1) != 0) {
				r++;
			}
			return r + 1;
		}
	}
}

