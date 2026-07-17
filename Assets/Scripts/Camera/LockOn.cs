using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public class LockOn : MonoBehaviour
    {
        [Header("Lock On")]
        public Transform lockOnTarget;
        public bool isLockedOn;
        public float lockOnRotationSpeed = 15f;


        [Header("Threat Tracking")]
        public List<CharacterManager> chasingEnemies = new();
        public float lockOnMaxDistance = 12f;

        [SerializeField] CharacterManager characterManager;

        public void RegisterChasingEnemy(CharacterManager enemy)
        {
            if (!chasingEnemies.Contains(enemy))
            {
                chasingEnemies.Add(enemy);
                SetLockOn(enemy.transform);
            }
        }

        public void UnregisterChasingEnemy(CharacterManager enemy)
        {
            if (chasingEnemies.Contains(enemy))
            {
                chasingEnemies.Remove(enemy);
            }
        }

        void Update()
        {
            if (!characterManager.IsPlayer())
            {
                return;
            }

            PruneThreats();

            if (isLockedOn && !IsValidTarget(lockOnTarget))
            {
                ClearLockOn();
            }
        }

        void PruneThreats()
        {
            float maxSqr = lockOnMaxDistance * lockOnMaxDistance;
            for (int i = chasingEnemies.Count - 1; i >= 0; i--)
            {
                var e = chasingEnemies[i];
                if (e == null || e.health.IsDead)
                {
                    chasingEnemies.RemoveAt(i);
                    continue;
                }
                float sqr = (e.transform.position - transform.position).sqrMagnitude;
                if (sqr > maxSqr)
                {
                    chasingEnemies.RemoveAt(i);
                }
            }
        }

        bool IsValidTarget(Transform t)
        {
            if (t == null)
            {
                return false;
            }

            var cm = t.GetComponent<CharacterManager>();
            if (cm == null || cm.health.IsDead)
            {
                return false;
            }

            float sqr = (t.position - transform.position).sqrMagnitude;
            return sqr <= lockOnMaxDistance * lockOnMaxDistance;
        }


        void SetLockOn(Transform target)
        {
            lockOnTarget = target;
            isLockedOn = target != null;
        }

        void ClearLockOn()
        {
            lockOnTarget = null;
            isLockedOn = false;
        }


    }
}