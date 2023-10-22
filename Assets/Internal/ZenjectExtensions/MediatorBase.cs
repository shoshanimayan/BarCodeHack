using Zenject;

namespace Core
{
    public abstract class MediatorBase<TView> where TView : IView
    {
        [Inject]
        protected TView _view;
    }
}