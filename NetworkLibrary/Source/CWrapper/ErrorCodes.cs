using System;

namespace NetworkLibrary.CWrapper
{
	/// ----------------------------------------------
	/// Enum: 	ErrorCodes - An enum to store Linux error codes
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// DATE: 		January 28th, 2018
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		This enum exists to store relevant Linux error codes.
	/// ----------------------------------------------
	public enum ErrorCodes: int
	{
		ERR_UNKNOWN = 0,
		ERR_NOMEMORY,
		ERR_ILLEGALOP,
		ERR_CONREFUSED,
		ERR_DESTUNREACH,
		ERR_ADDRINUSE,
		ERR_BADSOCK,
		ERR_CONRESET,
		ERR_PERMISSION,
		ERR_ADDRNOTAVAIL,
		ERR_TIMEOUT
	}
}

