namespace AF
{
    using UnityEngine;

    /// <summary>
    /// Third-person camera mobile: pitch/distância fixos.
    /// Colisão: RaycastAll do pivot até à câmara, ignora o player, pull-in imediato.
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
        [Tooltip("Suavização só quando NÃO há parede. Com parede a posição aplica-se logo.")]
        [SerializeField] float followSmoothTime = 0.12f;

        [Header("Collision")]
        [SerializeField] LayerMask wallLayer = ~0;
        [SerializeField] float wallBuffer = 0.15f;
        [SerializeField] float minDistanceFromTarget = 0.8f;
        [Tooltip("Tempo para a câmara VOLTAR a afastar-se (entrar na parede é imediato).")]
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

            // Direção do pivot → câmara desejada
            Vector3 toCamera = -(rotation * Vector3.forward);
            float occludedDistance = ResolveCameraDistance(pivot, toCamera);

            bool blocked = occludedDistance < distance - 0.01f;

            if (blocked)
            {
                // Entrar (puxar para o player) = imediato — senão ficas atrás da parede
                currentDistance = occludedDistance;
                distanceVelocity = 0f;
            }
            else
            {
                // Sair = suave
                currentDistance = Mathf.SmoothDamp(
                    currentDistance,
                    occludedDistance,
                    ref distanceVelocity,
                    zoomOutSmoothTime
                );
            }

            Vector3 desiredPos = pivot + toCamera * currentDistance;

            if (blocked)
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

        float ResolveCameraDistance(Vector3 pivot, Vector3 toCamera)
        {
            float preferred = distance;
            if (toCamera.sqrMagnitude < 0.0001f)
            {
                return preferred;
            }

            toCamera.Normalize();

            // Raycast (não SphereCast): MeshColliders não-convexos das dungeons
            // não respondem bem a SphereCast.
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
