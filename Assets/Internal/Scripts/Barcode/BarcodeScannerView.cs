using UnityEngine;
using Core;
using System.Collections;
using System.Collections.Generic;
using BarcodeScanner;
using TMPro;
using UnityEngine.UI;
using BarcodeScanner.Scanner;
using System;

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
        ///  PRIVATE METHODS           ///
        private void Awake()
        {
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }

        private void Start()
        {
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
                ///var rect = Image.GetComponent<RectTransform>();
                ///var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
                ///rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

                _restartTime = Time.realtimeSinceStartup;
            };
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
                if (_currentValue != barCodeValue)
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
        ///  PUBLIC API                ///
        public void CameraStop()
        {
            _barcodeScanner.Stop();
            _isPlaying = false;
            _currentValue = "";
            _textHeader.text = "";
        }

        public void CameraStart()
        {
            _isPlaying = true;
            _restartTime = 0;

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
            Debug.Log(mediator);
        _mediator = mediator;
            Debug.Log(_mediator);
        }

        public void StartPuzzle()
        {
            if (_currentValue != "")
            {
                int val = 1;
                _mediator.StartPuzzle(val);
            }
        }
    }
}
