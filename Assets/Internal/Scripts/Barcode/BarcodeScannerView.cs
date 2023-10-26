using UnityEngine;
using Core;
using System.Collections;
using System.Collections.Generic;
using BarcodeScanner;
using TMPro;
using UnityEngine.UI;
using BarcodeScanner.Scanner;

namespace Barcode
{
    public class BarcodeScannerView : MonoBehaviour, IView
    {

        ///  INSPECTOR VARIABLES       ///
        [SerializeField] private TextMeshProUGUI _textHeader;
        [SerializeField] private RawImage _image;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private Button _puzzleButton;
        [SerializeField] private Canvas _scannerCanvas;
        ///  PRIVATE VARIABLES         ///
        private IScanner _barcodeScanner;
        private float _restartTime;
        private string _currentValue = "";
        private bool _isPlaying = true;
        private BarcodeScannerMediator _mediator;

        private HashSet<string> _barcodeValues;
        ///  PRIVATE METHODS           ///
        private void Awake()
        {
            _barcodeValues = new HashSet<string>();
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;

            _puzzleButton.gameObject.SetActive(false);
            // Create a basic scanner
            _barcodeScanner = new Scanner();
            _barcodeScanner.Camera.Play();

            // Display the camera texture through a RawImage
            _barcodeScanner.OnReady += (sender, arg) => {
                // Set Orientation & Texture
                _image.transform.localEulerAngles = _barcodeScanner.Camera.GetEulerAngles();
                _image.transform.localScale = _barcodeScanner.Camera.GetScale();
                _image.texture = _barcodeScanner.Camera.Texture;

                // Keep Image Aspect Ratio
                var rect = _image.GetComponent<RectTransform>();
                var newHeight = rect.sizeDelta.x * _barcodeScanner.Camera.Height / _barcodeScanner.Camera.Width;
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

                _restartTime = Time.realtimeSinceStartup;
            };
        }

        private void Start()
        {
          
        }

        /// <summary>
        /// Start a scan and wait for the callback (wait 1s after a scan success to avoid scanning multiple time the same element)
        /// </summary>
        private void StartScanner()
        {

            _barcodeScanner.Scan((barCodeType, barCodeValue) => {
                _barcodeScanner.Stop();
                if (_textHeader.text.Length > 250)
                {
                    _textHeader.text = "";
                }
                if (_currentValue != barCodeValue )
                {
                    if (_barcodeValues.Contains(barCodeValue))
                    {
                        _currentValue = barCodeValue;

                        _textHeader.text = "barcode already scanned";
                        _restartTime = 1f;
                        _audio.Play();

#if UNITY_ANDROID || UNITY_IOS
                        Handheld.Vibrate();
#endif

                    }
                    else
                    {

                        _puzzleButton.gameObject.SetActive(true);

                        _currentValue = barCodeValue;

                        _textHeader.text = "Detected: " + barCodeValue;
                        _restartTime = 1f;

                        // Feedback

                        _audio.Play();

#if UNITY_ANDROID || UNITY_IOS
                        Handheld.Vibrate();
#endif
                    }
                }
            });
        }

        /// <summary>
        /// The Update method from unity need to be propagated
        /// </summary>
        private void Update()
        {
            if (_isPlaying)
            {
                if (_barcodeScanner != null)
                {
                    _barcodeScanner.Update();
                }

                // Check if the Scanner need to be started or restarted
                if (_restartTime != 0 && _restartTime < Time.realtimeSinceStartup)
                {
                    StartScanner();
                    _restartTime = 0;
                }
            }
        }

        private int GetDifficultyValue()
        {
            int result = 0;

            char randomChar = _currentValue[Random.Range(0, _currentValue.Length)];

            if (char.IsDigit(randomChar))
            {
                result = int.Parse(randomChar.ToString());
                if (result < 2)
                {
                    result = 2;
                }
                if (result > 5)
                {
                    result %= 5;
                }
            }
            else 
            {
                result = (int)randomChar;
                result %= 5;

                if (result < 2)
                {
                    result = 2;
                }
            }

            return result;
        }

        ///  PUBLIC API                ///
        public void CameraStop()
        {
            _barcodeScanner.Stop();
            _isPlaying = false;
            _currentValue = "";
            _textHeader.text = "";
            _puzzleButton.gameObject.SetActive(false);

        }

        public void CameraStart()
        {
            _isPlaying = true;
            _restartTime = Time.realtimeSinceStartup;

        }

        public void ShowCamera(bool active)
        { 
            if (active)
            {
                CameraStart();
            }
            else
            { 
                CameraStop();
            }
            _scannerCanvas.gameObject.SetActive(active);

        }

        public void Init(BarcodeScannerMediator mediator)
        {
             _mediator = mediator;
        }

        public void StartPuzzle()
        {
            if (_currentValue != "")
            {
                _barcodeValues.Add(_currentValue);

                int val = GetDifficultyValue();
                _mediator.StartPuzzle(val);
            }
        }
    }
}
