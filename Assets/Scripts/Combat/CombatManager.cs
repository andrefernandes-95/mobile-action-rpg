using UnityEngine;
using UnityEngine.Events;

namespace AF
{
    public class CombatManager : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] Ability specialAbility;
        public Ability SpecialAbility => specialAbility;

        [Header("Components")]
        [SerializeField] CharacterManager characterManager;
        [SerializeField] EquipmentManager equipmentManager;

        [Header("Debug")]
        [SerializeField] Ability currentAbility;

        [HideInInspector] public UnityEvent<Hitbox> onHitboxOpen;
        [HideInInspector] public UnityEvent<Hitbox> onHitboxClose;
        [HideInInspector] public UnityEvent<Vector3> onEnemyHit;

        float abilityCooldownEndTime;

        public void ResetStates()
        {

        }

        public void Attack()
        {
            if (characterManager.isBusy)
            {
                return;
            }

            if (Time.time < abilityCooldownEndTime)
            {
                return;
            }

            if (equipmentManager == null || equipmentManager.weaponInstance == null)
            {
                return;
            }

            WeaponAbility weaponAbility = equipmentManager.weaponInstance.ability;
            if (weaponAbility == null)
            {
                return;
            }

            SetCurrentAbility(weaponAbility, true);
        }

        public void OnSpecialAbility()
        {
            if (specialAbility == null)
            {
                return;
            }

            SetCurrentAbility(specialAbility, false);
        }

        public void SetCurrentAbility(Ability next, bool updateCooldown)
        {
            if (currentAbility != null)
            {
                currentAbility.OnEnd(characterManager);
            }

            currentAbility = next;
            currentAbility?.OnStart(characterManager);

            if (updateCooldown && currentAbility != null)
            {
                abilityCooldownEndTime = Time.time + currentAbility.cooldown;
            }
        }

        public bool TryBuildDamagePacket(Vector3 hitPoint, out DamagePacket packet)
        {
            if (currentAbility == null)
            {
                packet = default;
                return false;
            }

            return currentAbility.TryBuildDamagePacket(characterManager, hitPoint, out packet);
        }

        public float GetEngageRadius()
        {
            if (TryGetCurrentWeaponAbility(out WeaponAbility weaponAbility))
            {
                return weaponAbility.engageRadius;
            }

            return characterManager.agent.stoppingDistance;
        }

        bool TryGetCurrentWeaponAbility(out WeaponAbility weaponAbility)
        {
            weaponAbility = null;

            if (equipmentManager != null && equipmentManager.weaponInstance != null && equipmentManager.weaponInstance.ability != null)
            {
                weaponAbility = equipmentManager.weaponInstance.ability;
                return true;
            }

            return false;
        }
    }
}
