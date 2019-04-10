using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Abstract Class: MessageElement - A class that can serialize and deserialize itself into a packet.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public MessageElement ()
	/// 				public MessageElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public abstract ElementIndicatorElement GetIndicator ()
	///				public void WriteTo (BitStream bitStream)
	/// 			protected abstract void Serialize (BitStream bitstream)
	/// 			public void ReadFrom (BitStream bitstream)
	/// 			protected abstract void Deserialize (BitStream bitstream)
	/// 			protected abstract void Validate ()
	/// 			public static int RequiredBits (int v)
	/// 
	/// DATE: 		January 28th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		When reading data from the byte array the data should be
	/// 			read in the same order it was written.
	/// ----------------------------------------------
	public abstract class MessageElement
	{

		/// ----------------------------------------------
		/// CONSTRUCTOR: MessageElement
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public MessageElement ()
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public MessageElement ()
		{
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: MessageElement
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public MessageElement (BitStream bitstream)
		/// 
		/// NOTES:	Constructs a MessageElement by deserializing
		/// 		it from a BitStream object.
		/// ----------------------------------------------
		public MessageElement (BitStream bitstream)
		{
			ReadFrom (bitstream);
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetIndicator
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public abstract ElementIndicatorElement GetIndicator ()
		/// 
		/// RETURNS: 	An ElementIndicatorElement appropriate for the MessageElement
		/// 
		/// NOTES:		A function to be implemented by concrete subclasses. 
		/// 			Returns an ElementIndicatorElement for the subclasses
		/// 			type. Used to reconstruct a MessageElement when 
		/// 			deserializing a Packet.
		/// ----------------------------------------------
		public abstract ElementIndicatorElement GetIndicator ();

		/// ----------------------------------------------
		/// FUNCTION:	Bits
		/// 
		/// DATE:		February 10th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public abstract int Bits ()
		/// 
		/// RETURNS: 	The number of bits needed to store the serialized version
		/// 			of the message element.
		/// 
		/// NOTES:		A function to be implemented by concrete subclasses. 
		/// 			Returns the number of bits needed to store the element
		/// 			when it is serialized.
		/// ----------------------------------------------
		public abstract int Bits();

		/// ----------------------------------------------
		/// FUNCTION:	WriteTo
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public void WriteTo (BitStream bitStream)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Writes the MessageElement to the given
		/// 			BitStream.
		/// ----------------------------------------------
		public void WriteTo (BitStream bitStream)
		{
			Serialize (bitStream);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Serialize
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected abstract void Serialize (BitStream bitstream)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		A function to be implemented by concrete subclasses.
		/// 			Contains logic needed to serialize a MessageElement 
		/// 			to a BitStream.
		/// ----------------------------------------------
		protected abstract void Serialize (BitStream bitstream);

		/// ----------------------------------------------
		/// FUNCTION:	ReadFrom
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public void ReadFrom (BitStream bitStream)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Reads values from the given BitStream and
		/// 			uses them to set the MessageElements
		/// 			properties. After setting properties will
		/// 			validate its current values throwing an
		/// 			exception if they are invalid.
		/// ----------------------------------------------
		public void ReadFrom (BitStream bitstream)
		{
			Deserialize (bitstream);
			Validate ();
		}

		/// ----------------------------------------------
		/// FUNCTION:	Deserialize
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected abstract void Deserialize (BitStream bitstream)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		A function to be implemented by concrete subclasses.
		/// 			Contains logic needed to deserialze a MessageElement 
		/// 			from a BitStream.
		/// ----------------------------------------------
		protected abstract void Deserialize (BitStream bitstream);

		/// ----------------------------------------------
		/// FUNCTION:	Validate
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected abstract void Validate ()
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		A function to be implemented by concrete subclasses.
		/// 			Contains logic needed to validate that a MessageElement
		/// 			is valid.
		/// ----------------------------------------------
		protected abstract void Validate ();

		/// ----------------------------------------------
		/// FUNCTION:	RequiredBits
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static int RequiredBits (int v)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		A helper function that returns the number 
		/// 			of bits needed to store the specified number.
		/// 			Code found here: http://graphics.stanford.edu/~seander/bithacks.html#IntegerLogObvious
		/// ----------------------------------------------
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

