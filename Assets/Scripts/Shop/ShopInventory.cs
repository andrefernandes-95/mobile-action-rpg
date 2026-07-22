using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    [CreateAssetMenu(menuName = "AF/Data/Shop Inventory")]
    public sealed class ShopInventory : ScriptableObject
    {
        [Header("Weapons for sale")]
        public List<Weapon> weapons = new();
    }
}