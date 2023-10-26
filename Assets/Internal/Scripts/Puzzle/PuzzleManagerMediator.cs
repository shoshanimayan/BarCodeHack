using UnityEngine;
using Core;
using Zenject;
using UniRx;
using System;
using System.Collections;
using System.Collections.Generic;
using Signal;

namespace Puzzle
{
	public class PuzzleManagerMediator: MediatorBase<PuzzleManagerView>, IInitializable, IDisposable
	{

		///  INSPECTOR VARIABLES       ///

		///  PRIVATE VARIABLES         ///
		private void SetUpPuzzle() {

            _signalBus.Fire(new DisableCameraSignal {  });

        }

		private void OnStartPuzzle()
		{ 
			_view.EnableCanvas(true);
		}


        ///  PRIVATE METHODS           ///



		///  LISTNER METHODS           ///

		///  PUBLIC API                ///
		///  
		public void EndPuzzle()
		{
            _signalBus.Fire(new EnableCameraSignal { });
			_view.EnableCanvas(false);

        }

        ///  IMPLEMENTATION            ///

        [Inject]

		private SignalBus _signalBus;

		readonly CompositeDisposable _disposables = new CompositeDisposable();

		public void Initialize()
		{
			
			_view.Init(this);
            _signalBus.GetStream<SendScanValueSignal>()
                      .Subscribe(x => SetUpPuzzle()).AddTo(_disposables);
            _signalBus.GetStream<DisableCameraSignal>()
                     .Subscribe(x => OnStartPuzzle()).AddTo(_disposables);
        }

		public void Dispose()
		{

			_disposables.Dispose();

		}

	}
}
