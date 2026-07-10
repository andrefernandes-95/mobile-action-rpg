using UnityEngine;

namespace AF
{
    public class VFXHelper : MonoBehaviour
    {
        CameraShake _cameraShake;

        [SerializeField] bool shouldShakeCamera = false;

        CameraShake GetCameraShake()
        {
            if (_cameraShake == null && Camera.main != null)
            {
                _cameraShake = Camera.main.GetComponent<CameraShake>();
            }
            return _cameraShake;
        }

        void OnEnable()
        {
            // Play particle system
            if (TryGetComponent(out ParticleSystem ps))
            {
                ps.Play();
            }

            // Play audio
            if (TryGetComponent(out AudioSource audioSource))
            {
                audioSource.Play();
            }

            // Camera shake
            if (shouldShakeCamera)
            {
                GetCameraShake()?.Punch();
            }
        }
    }
}
