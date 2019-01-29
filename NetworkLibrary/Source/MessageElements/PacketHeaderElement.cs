using System;

namespace NetworkLibrary.MessageElements
{
	public class PacketHeaderElement : MessageElement
	{
		private static readonly ElementIndicatorElement indicator = new ElementIndicatorElement (ElementId.PacketHeaderElement);

		public override ElementIndicatorElement GetIndicator ()
		{
			return indicator;
		}

		private const int SEQ_BITS = 10;
		private const int ACK_BITS = 10;
		private const int RELIABLE_BITS = 10;

		public int SeqNumber { get; private set; }

		public int AckNumber { get; private set; }

		public int ReliableElements { get; private set; }


		public PacketHeaderElement (int seqNumber, int ackNumber, int reliableElements)
		{
			SeqNumber = seqNumber;
			AckNumber = ackNumber;
			ReliableElements = reliableElements;
		}

		public PacketHeaderElement (BitStream bitStream) : base (bitStream)
		{
		}


		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write (SeqNumber, 0, SEQ_BITS);
			bitStream.Write	(AckNumber, 0, ACK_BITS);
			bitStream.Write	(ReliableElements, 0, RELIABLE_BITS);
		}

		protected override void Deserialize (BitStream bitstream)
		{
			SeqNumber = bitstream.ReadNext (SEQ_BITS);
			AckNumber = bitstream.ReadNext (ACK_BITS);
			ReliableElements = bitstream.ReadNext (RELIABLE_BITS);
		}

		protected override void Validate ()
		{
		}
	}
}

