using UnityEngine;

namespace AF
{
    public sealed class VFXPooledEffect : MonoBehaviour
    {
        ParticleSystem[] particleSystems;
        float returnTime;
        bool playing;

        public VFXPoolTag Tag
        {
            get;
            set;
        }

        void Awake()
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        }

        public void Play()
        {
            float longest = 0f;
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Clear();
                ps.Play(true);
                var main = ps.main;
                longest = Mathf.Max(longest, main.duration + main.startLifetime.constantMax);
            }
            returnTime = Time.time + longest;
            playing = true;
        }

        void Update()
        {
            if (!playing || Time.time < returnTime)
            {
                return;
            }

            playing = false;
            VFXPool.Instance.Return(this);
        }
    }
}
