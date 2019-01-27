using System;
using System.Collections.Generic;

namespace NetworkLibrary
{
	public class ReliableUDPConnection
	{

		private static int BUFFER_SIZE = 1024;

		private MessageElement[] messageBuffer;
		private int messageIndex;
		private List<int>[] packetBuffer;
		private int packetIndex;

		private int highestPacketRecieved;
		private ulong recievedAckBits;

		private ulong ackBitsToSend;

		public ReliableUDPConnection ()
		{
			messageBuffer = new MessageElement[BUFFER_SIZE];
			packetBuffer = new List<int>[BUFFER_SIZE];
		}

		private Packet CreatePacket (MessageElement[] unreliableElements, MessageElement[] reliableElements)
		{
			int seqNumber = ++packetIndex;
			int ack = highestPacketRecieved;
			uint ackBits;

			//put packet header information into packet

			foreach (var element in reliableElements) {
				messageBuffer [messageIndex % BUFFER_SIZE] = element;
				messageIndex++;
			}

			// pack unreliable elements into packet

			int lastMessageIndex = 0;
			uint prevMessageIncluded = 0;

			// pack reliable packet header into packet

			for (int i = messageIndex - 32; i < messageIndex; i++) {
				prevMessageIncluded <<= 1;

				if (messageBuffer [messageIndex % BUFFER_SIZE] != null) {
					lastMessageIndex = i;
					// pack reliable element into packet
					packetBuffer [packetIndex % 1024].Add (messageIndex % BUFFER_SIZE);
				}

				if (lastMessageIndex == i - 1) {
					// if last message was included
					prevMessageIncluded += 1;
				}
			}

			// return packet
			recievedAckBits <<= 1;
			packetIndex++;
			return null;
		}

		private void ProcessPacket (Packet packet)
		{
			//read packet header info
			int seqNumber = 0;
			int ackNumber = 0;
			ulong ackBits = 0;
			int packetDifference = messageIndex - seqNumber;
			ackBits = ackBits << packetDifference;

			// remove recieved messages from buffer
			ulong temp = ackBits ^ recievedAckBits;
			for (int i = 64; i >= 0; i--) {
				if (temp << 1 >= temp) {
					foreach (int index in packetBuffer [(packetIndex - i) % BUFFER_SIZE]) {
						messageBuffer [index] = null;
					}
				}
			}

			// extract elements from packet

			// do stuff with them?

		}
	}
}

