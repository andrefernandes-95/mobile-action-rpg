using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "AF/Data/Consumable")]
    public class Consumable : Item
    {
        [Header("Effect")]
        public List<Effect> effects = new();
    }
}
