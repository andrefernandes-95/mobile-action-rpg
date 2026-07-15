namespace AF
{
    using UnityEngine;

    public class TopDownCameraFollow : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] Transform target;

        [SerializeField] Vector3 offset = new(0f, 10f, 0f);
        [SerializeField] float followSpeed = 10f;

        [Header("Wall Occlusion")]
        [SerializeField] bool enableWallOcclusion = true;
        [SerializeField] LayerMask wallLayer;
        [SerializeField] float targetHeightOffset = 1f;
        [SerializeField] float sphereRadius = 0.3f;
        [SerializeField] float wallBuffer = 0.15f;
        [SerializeField] float minDistanceFromTarget = 1.5f;


        void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPos = target.position + offset;

            if (enableWallOcclusion)
            {
                desiredPos = ResolveWallOcclusion(desiredPos);
            }

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                followSpeed * Time.deltaTime
            );
        }

        Vector3 ResolveWallOcclusion(Vector3 desiredPos)
        {
            Vector3 castOrigin = target.position + Vector3.up * targetHeightOffset;
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
    }
}
