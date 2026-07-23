using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AF
{
    public class TabView
    {
        readonly VisualElement buttonsRoot;
        readonly VisualElement contentHost;
        readonly List<ITabPage> pages = new();

        ITabPage current;

        public TabView(VisualElement buttonsRoot, VisualElement contentHost)
        {
            this.buttonsRoot = buttonsRoot;
            this.contentHost = contentHost;
        }

        public void AddPage(ITabPage page)
        {
            pages.Add(page);

            var btn = buttonsRoot.Q<Button>(page.TabButtonName);
            if (btn != null)
            {
                btn.clicked += () => Select(page);
            }
        }

        public void Select(ITabPage page)
        {
            if (current == page)
            {
                page.Enter(contentHost);
                return;
            }

            current?.Leave();
            current = page;
            contentHost.Clear();
            current.Enter(contentHost);
        }

        public void SelectFirst()
        {
            if (pages.Count > 0)
            {
                Select(pages[0]);
            }
        }
    }
}