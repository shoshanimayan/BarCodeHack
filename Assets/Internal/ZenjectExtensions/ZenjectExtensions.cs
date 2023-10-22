using Zenject;
using Core;
namespace Utility
{
    public static class ZenjectExtensions
    {
        public static void BindMediatorView<TMediator, TView>(this DiContainer container)
            where TMediator : MediatorBase<TView>
            where TView : IView
        {
            container.BindInterfacesAndSelfTo<TMediator>().AsSingle();
            container.Bind<TView>().FromComponentsInHierarchy().AsSingle();
        }
    }
}