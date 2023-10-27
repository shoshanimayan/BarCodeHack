using UnityEngine;
using Core;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle
{
	public class PuzzleManagerView: MonoBehaviour,IView
	{

		///  INSPECTOR VARIABLES       ///
		[SerializeField] private Canvas _puzzleCanvas;
		[SerializeField] private GameObject[] _tutorialObjects;
		[SerializeField] private PuzzlePieceInteractable[] _puzzlePieces;
		[SerializeField] private GameObject _endPuzzleButton;
		[SerializeField]private GameObject _puzzleContainer;
		///  PRIVATE VARIABLES         ///
		private PuzzleManagerMediator _mediator;
		private PuzzlePieceInteractable[] _activePieces;
		private PuzzlePieceInteractable _frozenPiece;
		private bool _completedTutorial;
        ///  PRIVATE METHODS           ///

        private void Update()
        {
			if (_activePieces.Length > 0 && RotationCheck(_activePieces))
			{
                _endPuzzleButton.SetActive(true);
                foreach (PuzzlePieceInteractable piece in _activePieces)
                {
					piece.SetInteractable(false);
                }

            }
        }

        private void Awake()
        {
            _puzzleCanvas.enabled = false;
            _activePieces = new PuzzlePieceInteractable[] { };
			_puzzleContainer.SetActive(false);

        }

        private bool RotationCheck(PuzzlePieceInteractable[] pieces)
		{
			bool match = true;

			float Z = _frozenPiece.transform.rotation.eulerAngles.z;


            foreach (PuzzlePieceInteractable piece in pieces) 
			{
				if (Mathf.Abs( piece.transform.rotation.eulerAngles.z-Z)>1f)
				{
					match = false; 
				}
			}

			return match;
		}

        ///  PUBLIC API                ///

        public void Init(PuzzleManagerMediator mediator)
		{ 
			_mediator = mediator;
			EnableCanvas(false);
			_endPuzzleButton.SetActive(false);
		}

		public void EndPuzzle() 
		{
			_activePieces = new PuzzlePieceInteractable[] { };
            foreach (PuzzlePieceInteractable piece in _puzzlePieces)
            {
				piece.transform.rotation= Quaternion.Euler(0,0,0);
				piece.gameObject.SetActive(false);
            }
			_frozenPiece.FreezePiece(false);
			_frozenPiece = null;
            _endPuzzleButton.SetActive(false);
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
			if (!enable && !_completedTutorial)
			{
				_completedTutorial = true;
                _puzzleContainer.SetActive(true);

            }
        }

		public void SetUpPuzzle(int difficulty)
		{
            var sliced = new PuzzlePieceInteractable[difficulty];
            Array.Copy(_puzzlePieces, 0, sliced, 0, difficulty);
			_activePieces=sliced;

			foreach (PuzzlePieceInteractable piece in _puzzlePieces)
			{
				if (!_activePieces.Contains(piece))
				{
					piece.gameObject.SetActive(false);
				}
				else
				{
                    piece.gameObject.SetActive(true);
					piece.transform.rotation= Quaternion.Euler(0,0,UnityEngine.Random.Range(0,360));

                }
            }

			_frozenPiece = _activePieces[UnityEngine.Random.Range(0, _activePieces.Length )];
			_frozenPiece.FreezePiece(true);

            foreach (PuzzlePieceInteractable piece in _activePieces)
            {
                piece.SetInteractable(true);
            }

            _mediator.DisableCamera();
        }
	}
}
