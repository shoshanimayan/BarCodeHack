using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ContinuousDemo : MonoBehaviour {

	private IScanner BarcodeScanner;
	public TextMeshProUGUI TextHeader;
	public RawImage Image;
	public AudioSource Audio;
	private float RestartTime;
	private string _currentValue="";
	private bool _isPlaying = true;
	[SerializeField] private Button _puzzleButton;

	// Disable Screen Rotation on that screen
	private void Awake()
	{
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
	}

	private void Start () {
		_puzzleButton.gameObject.SetActive(false);
		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			// Set Orientation & Texture
			Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
			Image.transform.localScale = BarcodeScanner.Camera.GetScale();
			Image.texture = BarcodeScanner.Camera.Texture;

			// Keep Image Aspect Ratio
			///var rect = Image.GetComponent<RectTransform>();
			///var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
			///rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

			RestartTime = Time.realtimeSinceStartup;
		};
	}

	/// <summary>
	/// Start a scan and wait for the callback (wait 1s after a scan success to avoid scanning multiple time the same element)
	/// </summary>
	private void StartScanner()
	{
		
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			BarcodeScanner.Stop();
			if (TextHeader.text.Length > 250)
			{
				TextHeader.text = "";
			}
			if (_currentValue != barCodeValue)
			{
                _puzzleButton.gameObject.SetActive(true);

                _currentValue = barCodeValue;

                TextHeader.text = barCodeValue;
				RestartTime = 1f;

				// Feedback

				Audio.Play();

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
			if (BarcodeScanner != null)
			{
				BarcodeScanner.Update();
			}

			// Check if the Scanner need to be started or restarted
			if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
			{
				StartScanner();
				RestartTime = 0;
			}
		}
	}

	#region UI Buttons

	

	/// <summary>
	/// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
	/// Trying to stop the camera in OnDestroy provoke random crash on Android
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>

	public void CameraStop()
	{ 
		BarcodeScanner.Stop();
		_isPlaying = false;
	}

	public void CameraStart()
	{ 
		_isPlaying = true;
		RestartTime = 0;

	}

	#endregion
}
