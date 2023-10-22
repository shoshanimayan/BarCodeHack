using UnityEngine;
using Core;
using Zenject;
using UniRx;
using System;
using System.Collections;
using System.Collections.Generic;
using Signal;

namespace Barcode
{
    public class BarcodeScannerMediator : MediatorBase<BarcodeScannerView>, IInitializable, IDisposable
	{

		///  INSPECTOR VARIABLES       ///

		///  PRIVATE VARIABLES         ///

		///  PRIVATE METHODS           ///
		private void EnableCamera() 
		{
			_view.ShowCamera(true);
		}

		private void DisableCamera()
		{
			_view.ShowCamera(false); ;
		}
		///  LISTNER METHODS           ///

		///  PUBLIC API                ///
		public void StartPuzzle(int value)
		{
            _signalBus.Fire(new SendScanValueSignal { Difficulty = value });
        }
		///  IMPLEMENTATION            ///

		[Inject]

		private SignalBus _signalBus;

		readonly CompositeDisposable _disposables = new CompositeDisposable();

		public void Initialize()
		{
			_view.Init(this);
            _signalBus.GetStream<EnableCameraSignal>()
                       .Subscribe(x => EnableCamera()).AddTo(_disposables);
            _signalBus.GetStream<DisableCameraSignal>()
                      .Subscribe(x => DisableCamera()).AddTo(_disposables);

        }

        public void Dispose()
		{

			_disposables.Dispose();

		}

	}
}
