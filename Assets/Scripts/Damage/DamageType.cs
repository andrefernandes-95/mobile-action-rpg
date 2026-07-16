using UnityEngine;

namespace AF
{

    [CreateAssetMenu(menuName = "AF/Damage/Damage Type")]
    public sealed class DamageType : ScriptableObject
    {
        public string displayName;
        public Sprite icon;
        public Color uiColor = Color.white;
    }
}
