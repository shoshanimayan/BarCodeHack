using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Puzzle
{

    public class PuzzlePieceInteractable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        ///  INSPECTOR VARIABLES       ///
        [SerializeField] private Image _indicator;
        [SerializeField] private float _speed = 100;

        ///  PRIVATE VARIABLES         ///
        private bool _frozen = false;
        private bool _isInteractable;
        private bool _isRotating;

        private enum DraggedDirection
        {
            Up,
            Down,
            Right,
            Left
        }


        private Camera _myCam;
        private Vector3 _screenPos;
        private Vector3 _mousePos;
        private float _angleOffset;
        private Collider2D _col;

        private PointerEventData _pointerEventData;

        ///  PRIVATE METHODS           ///
        private void Update()
        {
            if (!_frozen && _isInteractable)
            {
                if (_isRotating)
                {
                    // Debug.Log("Press position + " + _pointerEventData.pressPosition);
                    // Debug.Log("End position + " + _pointerEventData.position);
                    Vector3 dragVectorDirection = (_pointerEventData.position - _pointerEventData.pressPosition).normalized;
                    //Debug.Log("norm + " + dragVectorDirection);
                    var direction = GetDragDirection(dragVectorDirection);
                    Debug.Log(direction.ToString());
                    if (_mousePos != Input.mousePosition)
                    {
                        switch (direction)
                        {
                            case DraggedDirection.Up:
                                transform.Rotate(Vector3.forward, _speed * Time.deltaTime);

                                break;
                            case DraggedDirection.Down:
                                transform.Rotate(Vector3.back, _speed * Time.deltaTime);

                                break;
                            case DraggedDirection.Left:
                                transform.Rotate(Vector3.back, _speed * Time.deltaTime);
                                break;
                            case DraggedDirection.Right:
                                transform.Rotate(Vector3.forward, _speed * Time.deltaTime);
                                break;
                        }
                    }

                }
                /*
                // Debug.Log(_isRotating);
                if (_isRotating)
                {
                    Vector3 vec3 = Input.mousePosition - _screenPos;
                    Debug.Log(vec3);
                    float angle = (Mathf.Atan2(vec3.y, vec3.x) * Mathf.Rad2Deg);
                    Debug.Log(angle);
                    Debug.Log((angle + _angleOffset + (Time.deltaTime * _speed)));


                    transform.eulerAngles = new Vector3(0, 0, (angle + _angleOffset+(Time.deltaTime*_speed)));
                    
                }
                */
            }
            _mousePos = Input.mousePosition;


        }

        private DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            float positiveX = Mathf.Abs(dragVector.x);
            float positiveY = Mathf.Abs(dragVector.y);
            DraggedDirection draggedDir;
            if (positiveX > positiveY)
            {
                draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
            }
            else
            {
                draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
            }
            return draggedDir;

        }

        private void Start()
        {
            _myCam = Camera.main;
            _col = GetComponent<Collider2D>();
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

        public void OnDrag(PointerEventData eventData)
        {


            _pointerEventData = eventData;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _pointerEventData = eventData;
            /*
            _mousePos = _myCam.ScreenToWorldPoint(Input.mousePosition);
            _screenPos = _myCam.WorldToScreenPoint(transform.position);
            Vector3 vec3 = Input.mousePosition - _screenPos;
            _angleOffset =( (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg);
            */
            _isRotating = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            _isRotating = false;
        }
    }
}
