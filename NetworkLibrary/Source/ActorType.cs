using System;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Enum: 	ActorType - An enum to communitcate types of actors
	/// 					between the client and server programs.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// DATE: 		March 8th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:	
	/// ----------------------------------------------
	public enum ActorType : int
	{
		HumanPlayerA = 0,
		HumanPlayerB,
		HumanPlayerC,
		HumanPlayerD,
		HumanPlayerE,
		OrcPlayerA,
		OrcPlayerB,
		OrcPlayerC,
		OrcPlayerD,
		OrcplayerE
	}
}

