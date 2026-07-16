using UnityEngine;

namespace AF
{
    public class AutoAttackManager : MonoBehaviour
    {
        [SerializeField] CharacterManager characterManager;
        [SerializeField] float engageRange = 2.5f;
        [SerializeField] float attackInterval = 1.0f;

        float attackTimer;

        void OnEnable()
        {
            attackTimer = 0f;
        }

        public void SetAttackInterval(float interval)
        {
            attackInterval = Mathf.Max(0.05f, interval);
        }

        void Update()
        {
            if (characterManager == null)
            {
                return;
            }

            if (characterManager.health.IsDead)
            {
                return;
            }

            if (characterManager.isBusy)
            {
                return;
            }

            if (attackTimer > 0f)
            {
                attackTimer -= Time.deltaTime;
            }

            if (!HasTargetInRange())
            {
                return;
            }

            if (attackTimer > 0f)
            {
                return;
            }

            characterManager.combatManager.Attack();
            attackTimer = attackInterval;
        }

        bool HasTargetInRange()
        {
            LockOn lockOn = characterManager.lockOn;
            if (lockOn == null || !lockOn.isLockedOn || lockOn.lockOnTarget == null)
            {
                return false;
            }

            float sqr = (lockOn.lockOnTarget.position - transform.position).sqrMagnitude;
            return sqr <= engageRange * engageRange;
        }
    }
}
