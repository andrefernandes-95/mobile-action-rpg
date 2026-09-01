namespace AF
{

    using System.Collections.Generic;
    using UnityEngine;

    public class Hitbox : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] bool isActive = false;

        readonly List<IDamageable> targetsHit = new();
        CharacterManager hitboxOwner;

        public Transform swingSpawnRef;

        [SerializeField] TrailRenderer trailRenderer;

        AudioSource swingAudioSource;
        AudioSource hitAudioSource;

        void Awake()
        {
            hitboxOwner = GetComponentInParent<CharacterManager>();
            if (trailRenderer)
            {
                trailRenderer.emitting = false;
            }

            swingAudioSource = this.gameObject.AddComponent<AudioSource>();
            hitAudioSource = this.gameObject.AddComponent<AudioSource>();
            NormalizeAudioSource(swingAudioSource);
            NormalizeAudioSource(hitAudioSource);
        }

        void NormalizeAudioSource(AudioSource audioSource)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1;
        }

        public void OpenHitbox()
        {
            targetsHit.Clear();
            isActive = true;
            if (trailRenderer)
            {
                trailRenderer.emitting = true;
            }
            hitboxOwner.combatManager.onHitboxOpen?.Invoke(this);

            PlaySwing();
        }

        public void CloseHitbox()
        {
            isActive = false;
            if (trailRenderer)
            {
                trailRenderer.emitting = false;
            }
            hitboxOwner.combatManager.onHitboxClose?.Invoke(this);
        }

        void OnTriggerStay(Collider other)
        {
            if (!isActive)
            {
                return;
            }

            IDamageable damageable = null;
            if (other.CompareTag("DamageReceiver") && other.TryGetComponent(out DamageReceiver receiver))
            {
                damageable = receiver;
            }
            else if (other.TryGetComponent(out CharacterManager characterManager) && characterManager.characterDamageReceiver != null)
            {
                damageable = characterManager.characterDamageReceiver;
            }

            if (damageable == null)
            {
                return;
            }

            HandleDamage(other, damageable);
        }

        void HandleDamage(Collider other, IDamageable damageable)
        {
            // Avoid self-hit
            if (damageable is Component c && c.transform.root == hitboxOwner.transform.root)
            {
                return;
            }

            if (targetsHit.Contains(damageable))
            {
                return;
            }

            targetsHit.Add(damageable);

            Vector3 hitPosition = other.ClosestPoint(other.transform.position);
            hitboxOwner.combatManager.onEnemyHit?.Invoke(hitPosition);

            if (!hitboxOwner.combatManager.TryBuildDamagePacket(hitPosition, out DamagePacket packet))
            {
                return;
            }

            damageable.Receive(packet);
            PlayHit();
        }

        Weapon GetWeapon()
        {
            if (hitboxOwner.TryGetComponent<EquipmentManager>(out var equipmentManager))
            {
                if (equipmentManager.weaponInstance != null)
                {
                    return equipmentManager.weaponInstance.weaponData;
                }
            }
            return null;
        }

        void PlaySwing()
        {
            Weapon wp = GetWeapon();

            if (wp != null)
            {
                swingAudioSource.PlayOneShot(wp.swing);
            }
        }

        void PlayHit()
        {
            Weapon wp = GetWeapon();

            if (wp != null)
            {
                swingAudioSource.PlayOneShot(wp.hit);
            }
        }
    }
}
