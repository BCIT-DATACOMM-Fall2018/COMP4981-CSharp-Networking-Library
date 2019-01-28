using System;

namespace NetworkLibrary
{
	public class ElementIndicatorElement : MessageElement
	{
	
		private static ElementIndicatorElement indicator = new ElementIndicatorElement(ElementId.ElementIndicatorElement);

		public override ElementIndicatorElement GetIndicator(){
			return indicator;
		}


		private static int INDICATOR_BITS = 4;

		public ElementId ElementIndicator { get; private set; }

		public ElementIndicatorElement (int elementIndicator)
		{
			ElementIndicator = (ElementId)elementIndicator;
		}

		public ElementIndicatorElement (ElementId elementIndicator)
		{
			ElementIndicator = elementIndicator;
		}

		public ElementIndicatorElement (BitStream bitStream) : base (bitStream)
		{
		}


		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write ((int)ElementIndicator, 0, INDICATOR_BITS);
		}

		protected override void Deserialize (BitStream bitstream)
		{
			ElementIndicator = (ElementId)bitstream.ReadNext (INDICATOR_BITS);
		}

		public override void UpdateState (IStateMessageBridge bridge)
		{
		}




		protected override void Validate ()
		{
		}
	}
}

