using UnityEngine;

namespace AF
{
    public class Item : ScriptableObject
    {
        [Header("Info")]
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;
        public int price;
    }
}
