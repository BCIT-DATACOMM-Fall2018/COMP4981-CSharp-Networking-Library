using System;

namespace NetworkLibrary.MessageElements
{
	public abstract class UpdateElement : MessageElement
	{

		public UpdateElement ()
		{
		}

		public UpdateElement (BitStream bitstream) : base (bitstream)
		{
		}

		public abstract void UpdateState (IStateMessageBridge bridge);

	}
}

