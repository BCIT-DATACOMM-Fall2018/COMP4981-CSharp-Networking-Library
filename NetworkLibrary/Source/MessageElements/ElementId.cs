using System;

namespace NetworkLibrary.MessageElements
{
	public enum ElementId : int
	{
		MessageElement = 0,
		ElementIndicatorElement,
		PacketHeaderElement,
		HealthElement
	}
}

