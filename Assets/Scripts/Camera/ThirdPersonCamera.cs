namespace AF
{
    using UnityEngine;

    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] CharacterManager target;
        [SerializeField] float targetHeight = 1.5f;

        [Header("Framing (fixo)")]
        [SerializeField] float distance = 5f;
        [SerializeField] float pitch = 25f;

        [Header("Yaw follow")]
        [SerializeField] float yawSmoothTimeMin = 0.45f;
        [SerializeField] float yawSmoothTimeMax = 0.18f;

        [Header("Position smoothing")]
        [SerializeField] float followSmoothTime = 0.12f;

        [Header("Collision")]
        [SerializeField] LayerMask wallLayer = ~0;
        [SerializeField] float wallBuffer = 0.15f;
        [SerializeField] float minDistanceFromTarget = 0.8f;
        [SerializeField] float zoomOutSmoothTime = 0.2f;

        readonly RaycastHit[] hitBuffer = new RaycastHit[16];

        float yaw;
        float yawVelocity;
        float smoothedMoveAmount;
        float moveAmountVelocity;
        float currentDistance;
        float distanceVelocity;

        void Start()
        {
            currentDistance = distance;

            if (target != null)
            {
                yaw = target.transform.eulerAngles.y;
            }
            else
            {
                yaw = transform.eulerAngles.y;
            }
        }

        void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            bool lockedOn = target.lockOn != null
                && target.lockOn.isLockedOn
                && target.lockOn.lockOnTarget != null;

            float rawMove = Mathf.Clamp01(target.MoveAmount);
            float cameraBlend;

            if (lockedOn)
            {
                smoothedMoveAmount = rawMove;
                moveAmountVelocity = 0f;
                cameraBlend = 1f;
            }
            else
            {
                smoothedMoveAmount = Mathf.SmoothDamp(
                    smoothedMoveAmount,
                    rawMove,
                    ref moveAmountVelocity,
                    0.15f
                );
                cameraBlend = smoothedMoveAmount * smoothedMoveAmount;
            }

            UpdateYaw(cameraBlend, lockedOn);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.transform.position + Vector3.up * targetHeight;
            Vector3 toCamera = -(rotation * Vector3.forward);
            float occludedDistance = ResolveCameraDistance(pivot, toCamera);

            bool blocked = occludedDistance < distance - 0.01f;

            if (blocked || lockedOn)
            {
                currentDistance = occludedDistance;
                distanceVelocity = 0f;
            }
            else
            {
                currentDistance = Mathf.SmoothDamp(
                    currentDistance,
                    occludedDistance,
                    ref distanceVelocity,
                    zoomOutSmoothTime
                );
            }

            Vector3 desiredPos = pivot + toCamera * currentDistance;

            if (blocked || lockedOn)
            {
                transform.position = desiredPos;
            }
            else
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    desiredPos,
                    1f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.01f, followSmoothTime))
                );
            }

            Vector3 look = pivot - transform.position;
            if (look.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(look, Vector3.up);
            }
        }

        void UpdateYaw(float cameraBlend, bool lockedOn)
        {
            float targetYaw = target.transform.eulerAngles.y;

            if (lockedOn)
            {
                yaw = targetYaw;
                yawVelocity = 0f;
                return;
            }

            float smoothTime = Mathf.Lerp(yawSmoothTimeMin, yawSmoothTimeMax, cameraBlend);

            yaw = Mathf.SmoothDampAngle(
                yaw,
                targetYaw,
                ref yawVelocity,
                Mathf.Max(0.01f, smoothTime)
            );
        }

        float ResolveCameraDistance(Vector3 pivot, Vector3 toCamera)
        {
            float preferred = distance;
            if (toCamera.sqrMagnitude < 0.0001f)
            {
                return preferred;
            }

            toCamera.Normalize();

            int hitCount = Physics.RaycastNonAlloc(
                pivot,
                toCamera,
                hitBuffer,
                preferred,
                wallLayer,
                QueryTriggerInteraction.Ignore
            );

            float nearest = preferred;
            bool found = false;

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = hitBuffer[i];

                if (hit.collider == null)
                {
                    continue;
                }

                if (IsHitOnTarget(hit))
                {
                    continue;
                }

                if (hit.distance < nearest)
                {
                    nearest = hit.distance;
                    found = true;
                }
            }

            if (!found)
            {
                return preferred;
            }

            float pulled = nearest - wallBuffer;
            return Mathf.Clamp(pulled, minDistanceFromTarget, preferred);
        }

        bool IsHitOnTarget(RaycastHit hit)
        {
            if (hit.collider == null || target == null)
            {
                return false;
            }

            Transform hitRoot = hit.collider.transform;
            if (hitRoot == target.transform || hitRoot.IsChildOf(target.transform))
            {
                return true;
            }

            CharacterManager hitCharacter = hit.collider.GetComponentInParent<CharacterManager>();
            if (hitCharacter != null && hitCharacter == target)
            {
                return true;
            }

            return false;
        }
    }
}
