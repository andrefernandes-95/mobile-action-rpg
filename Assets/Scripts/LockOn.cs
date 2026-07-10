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
        float lockOnUpdateRate = 0.1f; // every 0.1 seconds
        float lockOnTimer = 0f;

        public void RegisterChasingEnemy(CharacterManager enemy)
        {
            if (!chasingEnemies.Contains(enemy))
            {
                chasingEnemies.Add(enemy);
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
            if (characterManager.IsPlayer())
            {
                lockOnTimer -= Time.deltaTime;
                if (lockOnTimer <= 0f)
                {
                    UpdateLockOn();
                    lockOnTimer = lockOnUpdateRate;
                }
            }
        }

        void UpdateLockOn()
        {
            // Remove dead or far enemies
            for (int i = chasingEnemies.Count - 1; i >= 0; i--)
            {
                var e = chasingEnemies[i];
                if (e == null || e.health.IsDead())
                {
                    chasingEnemies.RemoveAt(i);
                    continue;
                }

                float sqrDist = (e.transform.position - transform.position).sqrMagnitude;
                if (sqrDist > lockOnMaxDistance * lockOnMaxDistance)
                {
                    chasingEnemies.RemoveAt(i);
                }
            }

            if (chasingEnemies.Count == 0)
            {
                ClearLockOn();
                return;
            }

            // Find closest enemy using squared distance
            CharacterManager closest = null;
            float closestSqrDist = float.MaxValue;

            for (int i = 0; i < chasingEnemies.Count; i++)
            {
                var e = chasingEnemies[i];
                float sqrDist = (e.transform.position - transform.position).sqrMagnitude;
                if (sqrDist < closestSqrDist)
                {
                    closestSqrDist = sqrDist;
                    closest = e;
                }
            }

            if (closest != null)
            {
                SetLockOn(closest.transform);
            }
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