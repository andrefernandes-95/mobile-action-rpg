namespace AF
{
    using UnityEngine;

    public class Health : MonoBehaviour
    {
        [Header("Health")]
        public int health = 100;

        [Header("Character")]
        [SerializeField] CharacterManager characterManager;

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health < 0)
            {
                health = 0;
                Die();
            }
        }

        void Die()
        {
            characterManager.animator.Play("Death");
            characterManager.agent.ResetPath();
            characterManager.agent.enabled = false;
            characterManager.isBusy = true;

            var player = characterManager.GetPlayer();
            if (player != null)
            {
                player.lockOn.UnregisterChasingEnemy(characterManager);
            }
        }

        public bool IsDead() => health <= 0;

    }
}
