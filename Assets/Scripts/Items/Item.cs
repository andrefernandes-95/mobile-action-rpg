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

        [Header("Item Prefab")]
        public GameObject prefab;

        public virtual string GetDescription()
        {
            return description;
        }

        public virtual int Difference(Item item)
        {
            return 0;
        }

        public virtual void SpawnInWorld(Vector3 pos)
        {
            Instantiate(prefab, pos, Quaternion.identity);

            ItemPreview itemPreview = prefab.GetComponentInChildren<ItemPreview>(true);
            if (itemPreview != null)
            {
                itemPreview.Spawn();
            }
        }
    }
}
