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
		[SerializeField] private GameObject[] _tutorialObjects;
		[SerializeField] private GameObject _endPuzzleButton;
		///  PRIVATE VARIABLES         ///
		private PuzzleManagerMediator _mediator;
		///  PRIVATE METHODS           ///

		///  PUBLIC API                ///

		public void Init(PuzzleManagerMediator mediator)
		{ 
			_mediator = mediator;
			EnableCanvas(false);
			_endPuzzleButton.SetActive(false);
		}

		public void EndPuzzle() 
		{
			_mediator.EndPuzzle();
		}

		public void EnableCanvas(bool enable)
		{ 
			_puzzleCanvas.enabled = enable;	
		}

		public void EnableTutorialInfo(bool enable)
		{
			foreach (GameObject tutorial in _tutorialObjects)
			{ 
				tutorial.SetActive(enable);
			}
		}
	}
}
