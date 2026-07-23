using UnityEngine.UIElements;

namespace AF
{
    public static class ItemRowView
    {
        public static VisualElement Build(VisualTreeAsset template, ItemRowModel model)
        {
            VisualElement row = template.Instantiate();

            var name = row.Q<Label>("ItemName");
            if (name != null)
            {
                name.text = model.Name;
            }

            var icon = row.Q("ItemIcon");
            if (icon != null && model.Icon != null)
            {
                icon.style.backgroundImage = new StyleBackground(model.Icon);
            }

            var description = row.Q<Label>("Description");
            if (description != null)
            {
                description.text = model.Description;
            }

            var status = row.Q<Label>("Status");
            if (status != null)
            {
                status.text = model.Status;
            }

            var indicator = row.Q("Indicator");
            if (indicator != null)
            {
                indicator.style.display = model.ShowIndicator
                    ? DisplayStyle.Flex : DisplayStyle.None;
                indicator.style.backgroundColor = model.IndicatorColor;
            }

            var actions = row.Q("Actions");
            if (actions != null)
            {
                actions.Clear();
                foreach (var action in model.Actions)
                {
                    // In older versions of C#, foreach had a closure bug where every lambda captured the same loop variable. This meant all buttons would invoke the last action.
                    var captured = action;
                    var btn = new Button(() => captured.OnClick?.Invoke())
                    {
                        text = captured.Label
                    };
                    btn.AddToClassList("button");
                    btn.SetEnabled(captured.Enabled);
                    actions.Add(btn);
                }
            }

            return row;
        }
    }
}