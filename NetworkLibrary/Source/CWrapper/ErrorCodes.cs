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
		EPERM = 1,
		EIO = 5,
		EBADF = 9,
		ENOMEM = 12,
		EACCES = 13,
		EINVAL = 22,
		ENOTSOCK = 88,
		EMSGSIZE = 90,
		EOPNOTSUPP = 95,
		EADDRINUSE = 98,
		EADDRNOTAVAIL = 99,
		ENETDOWN = 100,
		ENETUNREACH = 101,
		ECONNRESET = 104,
		EISCONN = 106,
		ENOTCONN = 107,
		ECONNREFUSED = 111,
		EHOSTUNREACH = 113,

	}
}

