namespace AF
{
    using UnityEngine;

    /// <summary>
    /// Third-person camera mobile: pitch/distância fixos.
    /// Colisão: SphereCast a partir de fora do player, ignora o próprio alvo, distância suavizada.
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
        [SerializeField] float yawSmoothTimeMin = 0.45f;
        [SerializeField] float yawSmoothTimeMax = 0.18f;

        [Header("Position smoothing")]
        [SerializeField] float followSmoothTimeMin = 0.2f;
        [SerializeField] float followSmoothTimeMax = 0.1f;

        [Header("Collision")]
        [SerializeField] LayerMask wallLayer = ~0;
        [SerializeField] float sphereRadius = 0.25f;
        [SerializeField] float wallBuffer = 0.2f;
        [SerializeField] float minDistanceFromTarget = 1.2f;
        [SerializeField] float collisionSmoothTime = 0.08f;

        float yaw;
        float yawVelocity;
        float smoothedMoveAmount;
        float moveAmountVelocity;
        Vector3 positionVelocity;
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

            float rawMove = Mathf.Clamp01(target.MoveAmount);
            smoothedMoveAmount = Mathf.SmoothDamp(
                smoothedMoveAmount,
                rawMove,
                ref moveAmountVelocity,
                0.15f
            );

            float cameraBlend = smoothedMoveAmount * smoothedMoveAmount;

            UpdateYaw(cameraBlend);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.transform.position + Vector3.up * targetHeight;
            Vector3 back = rotation * Vector3.forward;

            float targetDistance = ResolveCameraDistance(pivot, -back);
            currentDistance = Mathf.SmoothDamp(
                currentDistance,
                targetDistance,
                ref distanceVelocity,
                collisionSmoothTime
            );

            Vector3 desiredPos = pivot - back * currentDistance;

            float followTime = Mathf.Lerp(followSmoothTimeMin, followSmoothTimeMax, cameraBlend);
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPos,
                ref positionVelocity,
                Mathf.Max(0.01f, followTime)
            );

            Vector3 look = pivot - transform.position;
            if (look.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(look, Vector3.up);
            }
        }

        void UpdateYaw(float cameraBlend)
        {
            float targetYaw = target.transform.eulerAngles.y;
            float smoothTime = Mathf.Lerp(yawSmoothTimeMin, yawSmoothTimeMax, cameraBlend);

            yaw = Mathf.SmoothDampAngle(
                yaw,
                targetYaw,
                ref yawVelocity,
                Mathf.Max(0.01f, smoothTime)
            );
        }

        float ResolveCameraDistance(Vector3 pivot, Vector3 castDir)
        {
            float preferred = distance;

            // Começa FORA do collider do player — senão o cast "bate" nele e desiste
            float skin = sphereRadius + 0.05f;
            Vector3 origin = pivot + castDir * skin;
            float castLength = Mathf.Max(0f, preferred - skin);

            if (castLength <= 0.001f)
            {
                return minDistanceFromTarget;
            }

            if (!Physics.SphereCast(
                origin,
                sphereRadius,
                castDir,
                out RaycastHit hit,
                castLength,
                wallLayer,
                QueryTriggerInteraction.Ignore))
            {
                return preferred;
            }

            // Ignora hits no próprio player (ou filhos)
            if (IsHitOnTarget(hit))
            {
                return preferred;
            }

            float pulled = hit.distance + skin - wallBuffer;
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
