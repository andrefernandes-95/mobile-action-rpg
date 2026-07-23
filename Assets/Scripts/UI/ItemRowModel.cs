using System;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public struct ItemAction
    {
        public string Label;
        public Action OnClick;
        public bool Enabled;

        public ItemAction(string label, Action onClick, bool enabled = true)
        {
            this.Label = label;
            this.OnClick = onClick;
            this.Enabled = enabled;
        }
    }

    public class ItemRowModel
    {
        public string Name;
        public Sprite Icon;
        public string Description; // ex. Attack Damage: 110
        public string Status; // ex. Current Durability: 100/200
        public bool ShowIndicator;
        public Color IndicatorColor = Color.white;
        public readonly List<ItemAction> Actions = new();
    }
}