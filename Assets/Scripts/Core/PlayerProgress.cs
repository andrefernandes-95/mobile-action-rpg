using UnityEngine;
using UnityEngine.SceneManagement;

namespace AF
{
    public class PlayerProgress : MonoBehaviour
    {
        public static PlayerProgress Instance
        {
            get;
            private set;
        }

        [Header("Wallet")]
        public Wallet wallet = new();

        [Header("Checkpoint")]
        [SerializeField] Checkpoint lastCheckpoint;
        public Checkpoint LastCheckpoint => lastCheckpoint;

        [Header("Bloodstain")]
        [SerializeField] Bloodstain currentBloodstain;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetCheckpoint(Checkpoint checkpoint)
        {
            lastCheckpoint = checkpoint;
        }

        public void DropBloodstain(Vector3 worldPos)
        {
            Bloodstain bloodstain = new()
            {
                drop = wallet.DrainAll(),
                worldPos = worldPos,
                scene = SceneManager.GetActiveScene().name
            };

            this.currentBloodstain = bloodstain;
        }

        public void ClearBloodstain(Bloodstain stain)
        {
            if (currentBloodstain == stain)
            {
                currentBloodstain = null;
            }
        }
    }
}
