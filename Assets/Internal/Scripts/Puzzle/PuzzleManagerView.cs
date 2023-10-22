using UnityEngine;
using Core;
using System.Collections;
using System.Collections.Generic;
namespace Puzzle
{
	public class PuzzleManagerView: MonoBehaviour,IView
	{

		///  INSPECTOR VARIABLES       ///

		///  PRIVATE VARIABLES         ///
		private PuzzleManagerMediator _mediator;
		///  PRIVATE METHODS           ///

		///  PUBLIC API                ///

		public void Init(PuzzleManagerMediator mediator)
		{ 
		_mediator = mediator;
		}

		public void EndPuzzle() 
		{
			_mediator.EndPuzzle();
		}
	}
}
