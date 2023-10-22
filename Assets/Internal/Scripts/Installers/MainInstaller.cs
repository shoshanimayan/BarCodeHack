using Barcode;
using UnityEngine;
using Utility;
using Zenject;
using Signal;
using Puzzle;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.BindMediatorView<BarcodeScannerMediator, BarcodeScannerView>();
        Container.BindMediatorView<PuzzleManagerMediator, PuzzleManagerView>();

        Container.DeclareSignal<SendScanValueSignal>();
        Container.DeclareSignal<EnableCameraSignal>();
        Container.DeclareSignal<DisableCameraSignal>();


    }
   
}