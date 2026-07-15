namespace AF
{
    using UnityEngine;
    using System.Collections;

    public class CameraShake : MonoBehaviour
    {
        public float fovPunchAmount = 2f;     // how much to zoom in
        public float punchDuration = 0.12f;   // total time

        private Camera cam;
        private float originalFov;
        private Coroutine punchRoutine;

        void Awake()
        {
            cam = GetComponent<Camera>();
            originalFov = cam.fieldOfView;
        }

        public void Punch()
        {
            if (punchRoutine != null)
                StopCoroutine(punchRoutine);

            punchRoutine = StartCoroutine(PunchRoutine());
        }

        IEnumerator PunchRoutine()
        {
            float half = punchDuration * 0.5f;
            float t = 0f;

            // Zoom In
            while (t < half)
            {
                cam.fieldOfView = Mathf.Lerp(
                    originalFov,
                    originalFov - fovPunchAmount,
                    t / half
                );

                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;

            // Zoom Out
            while (t < half)
            {
                cam.fieldOfView = Mathf.Lerp(
                    originalFov - fovPunchAmount,
                    originalFov,
                    t / half
                );

                t += Time.deltaTime;
                yield return null;
            }

            cam.fieldOfView = originalFov;
            punchRoutine = null;
        }
    }
}
