using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Puzzle
{
	public class PuzzlePieceInteractable: MonoBehaviour
	{
        ///  INSPECTOR VARIABLES       ///
        [SerializeField] Image _indicator;
        ///  PRIVATE VARIABLES         ///
        private bool _frozen = false;
        private bool _isInteractable;
        ///  PRIVATE METHODS           ///
        private void Update()
        {
            if (!_frozen && _isInteractable)
            {

            }
        }

        ///  PUBLIC API                ///


        public void FreezePiece(bool freeze)
        {
            if (freeze)
            {
                _frozen = true;
                _indicator.color = Color.red;
            }
            else
            {
                _frozen = false;
                _indicator.color = Color.white;
            }
        }

        public void SetInteractable(bool interactable) { _isInteractable = interactable; }

    }
}
