namespace AF
{
    using UnityEngine;

    /// <summary>
    /// Third-person camera para mobile: pitch e distancia fixos.
    /// O yaw acompanha a rotacao horizontal do personagem (driven pelo joystick).
    /// Sem free-look por toque/rato.
    /// </summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] CharacterManager target;
        [SerializeField] float targetHeight = 1.5f;

        [Header("Framing (fixo)")]
        [SerializeField] float distance = 5f;
        [SerializeField] float pitch = 25f;


        [Header("Yaw follow")]
        [SerializeField] float yawFollowSpeedMin = 2.5f;
        [SerializeField] float yawFollowSpeedMax = 10f;
        [SerializeField] float lockOnYawMultiplier = 2f;

        [Header("Position smoothing")]
        [SerializeField] float followSmoothMin = 8f;
        [SerializeField] float followSmoothMax = 16f;

        [Header("Collision")]
        [SerializeField] LayerMask wallLayer;
        [SerializeField] float sphereRadius = 0.3f;
        [SerializeField] float wallBuffer = 0.15f;
        [SerializeField] float minDistanceFromTarget = 1.5f;

        float yaw;

        void Start()
        {
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

            float moveAmount = target.MoveAmount; // 0..1

            UpdateYaw(moveAmount);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.transform.position + Vector3.up * targetHeight;

            float wantedDistance = distance;

            Vector3 desiredPos = pivot - rotation * Vector3.forward * wantedDistance;
            desiredPos = ResolveWallOcclusion(desiredPos);

            float follow = Mathf.Lerp(followSmoothMin, followSmoothMax, moveAmount);

            if (target.lockOn != null && target.lockOn.isLockedOn)
            {
                follow *= lockOnYawMultiplier;
            }

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                follow * Time.deltaTime
            );

            transform.rotation = Quaternion.LookRotation(pivot - transform.position, Vector3.up);
        }

        Vector3 ResolveWallOcclusion(Vector3 desiredPos)
        {
            Vector3 castOrigin = target.transform.position + Vector3.up * targetHeight;
            Vector3 direction = desiredPos - castOrigin;
            float maxDistance = direction.magnitude;


            direction /= maxDistance;

            if (Physics.SphereCast(
                castOrigin,
                sphereRadius,
                direction,
                out RaycastHit hit,
                maxDistance,
                wallLayer,
                QueryTriggerInteraction.Ignore
            ))
            {
                float pullDistance = Mathf.Max(
                    hit.distance - wallBuffer,
                    minDistanceFromTarget
                );

                return castOrigin + direction * pullDistance;
            }

            return desiredPos;
        }


        void UpdateYaw(float moveAmount)
        {
            float targetYaw = target.transform.eulerAngles.y;

            float yawSpeed = Mathf.Lerp(yawFollowSpeedMin, yawFollowSpeedMax, moveAmount);

            if (target.lockOn != null && target.lockOn.isLockedOn)
            {
                yawSpeed *= lockOnYawMultiplier;
            }

            yaw = Mathf.LerpAngle(yaw, targetYaw, yawSpeed * Time.deltaTime);
        }
    }
}
