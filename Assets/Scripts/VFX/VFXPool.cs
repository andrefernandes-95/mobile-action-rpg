using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public sealed class VFXPool : MonoBehaviour
    {
        public static VFXPool Instance
        {
            get;
            private set;
        }

        [System.Serializable]
        public class Entry
        {
            public VFXPoolTag tag;
            public GameObject prefab;
            public int prewarm = 5;
            [HideInInspector] public Queue<VFXPooledEffect> pool = new();
        }

        [SerializeField] Entry[] effects;

        void Awake()
        {
            Instance = this;

            foreach (Entry e in effects)
            {
                for (int i = 0; i < e.prewarm; i++)
                {
                    VFXPooledEffect fx = Create(e.prefab, e.tag);
                    fx.gameObject.SetActive(false);
                    e.pool.Enqueue(fx);
                }
            }
        }

        public void Play(VFXPoolTag tag, Vector3 position, Quaternion rotation)
        {
            Entry entry = Find(tag);
            if (entry == null)
            {
                return;
            }

            VFXPooledEffect fx = entry.pool.Dequeue();

            fx.transform.SetPositionAndRotation(position, rotation);
            fx.gameObject.SetActive(true);
            fx.Play();
        }

        public void Return(VFXPooledEffect effect)
        {
            effect.gameObject.SetActive(false);

            Entry entry = Find(effect.Tag);
            if (entry != null)
            {
                entry.pool.Enqueue(effect);
            }
        }

        Entry Find(VFXPoolTag tag)
        {
            foreach (Entry e in effects)
            {
                if (e.tag == tag)
                {
                    return e;
                }
            }

            return null;
        }

        VFXPooledEffect Create(GameObject prefab, VFXPoolTag tag)
        {
            GameObject go = Instantiate(prefab, transform);

            VFXPooledEffect fx = go.GetComponent<VFXPooledEffect>();
            if (fx == null)
            {
                fx = go.AddComponent<VFXPooledEffect>();
            }

            fx.Tag = tag;
            return fx;
        }
    }
}
