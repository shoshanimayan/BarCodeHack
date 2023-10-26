using UnityEngine;
using Core;
using System.Collections;
using System.Collections.Generic;
namespace Puzzle
{
	public class PuzzleManagerView: MonoBehaviour,IView
	{

		///  INSPECTOR VARIABLES       ///
		[SerializeField] private Canvas _puzzleCanvas;
		///  PRIVATE VARIABLES         ///
		private PuzzleManagerMediator _mediator;
		///  PRIVATE METHODS           ///

		///  PUBLIC API                ///

		public void Init(PuzzleManagerMediator mediator)
		{ 
			_mediator = mediator;
			EnableCanvas(false);
		}

		public void EndPuzzle() 
		{
			_mediator.EndPuzzle();
		}

		public void EnableCanvas(bool enable)
		{ 
			_puzzleCanvas.enabled = enable;	
		}
	}
}
