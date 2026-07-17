using UnityEngine;

namespace AF
{
    public class Perception : MonoBehaviour
    {

        [Header("Perception")]
        public float sightRange = 10f;
        public float sightAngle = 60f;

        [Header("Components")]
        [SerializeField] CharacterManager characterManager;

        public bool CanSeePlayer()
        {
            if (characterManager == null || characterManager.IsPlayer() || characterManager.GetPlayer().health.IsDead)
            {
                return false;
            }

            Vector3 dir = characterManager.GetPlayer().transform.position - characterManager.transform.position;
            float dist = dir.magnitude;

            if (dist > sightRange) return false;

            float angle = Vector3.Angle(characterManager.transform.forward, dir);
            if (angle > sightAngle * 0.5f) return false;

            if (Physics.Raycast(
                characterManager.transform.position,
                dir.normalized,
                dist,
                ~0,
                QueryTriggerInteraction.Ignore))
            {
                return true;
            }

            return false;
        }
    }
}
