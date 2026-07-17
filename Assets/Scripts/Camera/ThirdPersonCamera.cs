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
        [Tooltip("Quao depressa a camara acompanha a viragem do personagem.")]
        [SerializeField] float yawFollowSpeed = 8f;
        [Tooltip("So acompanha o yaw enquanto o personagem se mexe (evita rodar em idle).")]

        [Header("Position smoothing")]
        [SerializeField] float followSmooth = 15f;

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

            UpdateYaw();

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.transform.position + Vector3.up * targetHeight;

            float wantedDistance = distance;

            Vector3 desiredPos = pivot - rotation * Vector3.forward * wantedDistance;
            desiredPos = ResolveWallOcclusion(desiredPos);
            float finalFollowSmooth = followSmooth * (target.lockOn.isLockedOn ? 2 : 1f);

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                finalFollowSmooth * Time.deltaTime
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

        void UpdateYaw()
        {
            float targetYaw = target.transform.eulerAngles.y;

            float finalYawFollowSpeed = yawFollowSpeed * (target.lockOn.isLockedOn ? 2 : 1f);
            yaw = Mathf.LerpAngle(yaw, targetYaw, finalYawFollowSpeed * Time.deltaTime);
        }
    }
}
