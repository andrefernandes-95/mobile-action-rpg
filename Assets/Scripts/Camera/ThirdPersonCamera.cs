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
        [SerializeField] Transform target;
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

        float yaw;

        void Start()
        {
            if (target != null)
            {
                yaw = target.eulerAngles.y;
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
            Vector3 pivot = target.position + Vector3.up * targetHeight;

            float wantedDistance = distance;

            Vector3 desiredPos = pivot - rotation * Vector3.forward * wantedDistance;

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                followSmooth * Time.deltaTime
            );

            transform.rotation = Quaternion.LookRotation(pivot - transform.position, Vector3.up);
        }

        void UpdateYaw()
        {
            float targetYaw = target.eulerAngles.y;
            yaw = Mathf.LerpAngle(yaw, targetYaw, yawFollowSpeed * Time.deltaTime);
        }
    }
}
