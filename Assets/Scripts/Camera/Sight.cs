namespace AF
{
    using UnityEngine;

    public class Sight : MonoBehaviour
    {
        [SerializeField] private float sightRadius = 5f;
        [SerializeField] private LayerMask playerLayer;

        bool isChasingPlayer = false;

        void Update()
        {
            if (isChasingPlayer)
            {
                return;
            }

            SearchPlayer();
        }

        void SearchPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, sightRadius, playerLayer);
            if (hits.Length > 0)
            {
                EngageWithPlayer(hits[0].transform);
            }
        }

        void EngageWithPlayer(Transform playerTransform)
        {
            LockOn playerLockOn = playerTransform.GetComponent<LockOn>();
            if (playerLockOn != null)
            {
                isChasingPlayer = true;
                playerLockOn.SetLockOn(this.transform);
            }
        }
    }
}
