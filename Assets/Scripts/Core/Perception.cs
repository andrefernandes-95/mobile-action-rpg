using UnityEngine;

namespace AF
{
    public class Perception : MonoBehaviour
    {
        [Header("Perception")]
        public float sightRange = 10f;
        public float sightAngle = 60f;
        public float eyeHeight = 1.4f;

        [Header("Components")]
        [SerializeField] CharacterManager characterManager;

        public bool CanSeePlayer()
        {
            if (characterManager == null || characterManager.IsPlayer())
            {
                return false;
            }

            CharacterManager player = characterManager.GetPlayer();
            if (player == null || player.health.IsDead)
            {
                return false;
            }

            Vector3 origin = characterManager.transform.position + Vector3.up * eyeHeight;
            Vector3 target = player.transform.position + Vector3.up * eyeHeight;
            Vector3 dir = target - origin;
            float dist = dir.magnitude;

            if (dist > sightRange)
            {
                return false;
            }

            float angle = Vector3.Angle(characterManager.transform.forward, dir);
            if (angle > sightAngle * 0.5f)
            {
                return false;
            }

            if (!Physics.Raycast(
                origin,
                dir.normalized,
                out RaycastHit hit,
                dist,
                ~0,
                QueryTriggerInteraction.Ignore))
            {
                return false;
            }

            CharacterManager hitCharacter = hit.collider.GetComponentInParent<CharacterManager>();
            if (hitCharacter != null && hitCharacter == player)
            {
                return true;
            }

            return false;
        }
    }
}
