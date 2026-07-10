namespace AF
{
    using UnityEngine;

    public class TopDownCameraFollow : MonoBehaviour
    {
        public Transform target;

        public Vector3 offset = new(0f, 10f, 0f);
        public float followSpeed = 10f;

        void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPos = target.position + offset;
            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                followSpeed * Time.deltaTime
            );
        }
    }
}
