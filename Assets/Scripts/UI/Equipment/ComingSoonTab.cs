using UnityEngine.UIElements;

namespace AF
{
    public sealed class ComingSoonTab : ITabPage
    {
        public string TabButtonName { get; }

        readonly string message;

        public ComingSoonTab(string tabButtonName, string message)
        {
            TabButtonName = tabButtonName;
            this.message = message;
        }

        public void Enter(VisualElement host)
        {
            host.Clear();
            var label = new Label(message);
            label.AddToClassList("label");
            host.Add(label);
        }

        public void Leave() { }
    }
}