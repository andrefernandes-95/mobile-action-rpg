namespace AF
{

    using System.Collections.Generic;
    using UnityEngine;

    public class Hitbox : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] bool isActive = false;

        List<DamageReceiver> targetsHit = new();

        CharacterManager hitboxOwner;

        [Header("Spawn Refs")]
        public Transform swingSpawnRef;

        void Awake()
        {
            hitboxOwner = GetComponentInParent<CharacterManager>();
        }

        public void OpenHitbox()
        {
            targetsHit.Clear();
            isActive = true;
            hitboxOwner.combatManager.onHitboxOpen?.Invoke(this);
        }

        public void CloseHitbox()
        {
            isActive = false;
            hitboxOwner.combatManager.onHitboxClose?.Invoke(this);
        }

        void OnTriggerStay(Collider other)
        {
            if (!isActive)
            {
                return;
            }

            if (other.CompareTag("DamageReceiver"))
            {
                if (other.TryGetComponent(out DamageReceiver damageReceiver))
                {
                    HandleDamage(other, damageReceiver);
                }
            }
            else if (other.TryGetComponent(out CharacterManager characterManager))
            {
                HandleDamage(other, characterManager.characterDamageReceiver);
            }
        }

        void HandleDamage(Collider other, DamageReceiver damageReceiver)
        {
            if (damageReceiver.transform.root == hitboxOwner.characterDamageReceiver.transform.root)
            {
                return;
            }

            if (!targetsHit.Contains(damageReceiver))
            {
                targetsHit.Add(damageReceiver);
                var hitPosition = other.ClosestPoint(damageReceiver.transform.position);
                hitboxOwner.combatManager.onEnemyHit?.Invoke(hitPosition);
                damageReceiver.ApplyDamage(hitboxOwner);
            }
        }

    }

}
