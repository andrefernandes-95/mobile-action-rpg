using UnityEngine.UIElements;

namespace AF
{
    public interface ITabPage
    {
        string TabButtonName { get; }
        void Enter(VisualElement host);
        void Leave();
    }
}
