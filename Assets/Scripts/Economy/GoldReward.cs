using UnityEngine;

namespace AF
{
    /// <summary>
    /// Place it on AI, gives gold to player upon death
    /// </summary>
    public class GoldReward : MonoBehaviour
    {
        [SerializeField] Health health;
        [SerializeField] int goldAmount = 30;
        [SerializeField] int goldVariance = 5; // +/-

        bool paid;

        void OnEnable()
        {
            if (health != null)
            {
                health.OnDied += HandleDied;
            }
        }

        void OnDisable()
        {

            if (health != null)
            {
                health.OnDied -= HandleDied;
            }
        }

        void HandleDied()
        {
            if (paid)
            {
                return;
            }

            paid = true;
            if (PlayerProgress.Instance == null)
            {
                return;
            }

            int lo = Mathf.Max(0, goldAmount - goldVariance);
            int hi = goldAmount + goldVariance;
            int amount = Random.Range(lo, hi + 1);
            PlayerProgress.Instance.wallet.Add(amount);
        }
    }
}
