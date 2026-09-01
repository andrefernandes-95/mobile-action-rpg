namespace AF
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class Footstep : MonoBehaviour
    {
        AudioSource audioSource => GetComponent<AudioSource>();
        [SerializeField] AudioClip[] footsteps;
        [SerializeField] private float interval = 0.12f;
        private float nextFootstep;

        public void OnFootstep()
        {
            if (Time.time < nextFootstep || footsteps.Length == 0)
            {
                return;
            }

            audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
            nextFootstep = Time.time + interval;
        }
    }
}
