using UnityEngine;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        AudioSource audioSource => GetComponent<AudioSource>();

        public static SoundManager Instance
        {
            get;
            private set;
        }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySound(AudioClip sound)
        {
            this.audioSource.PlayOneShot(sound);
        }

    }
}