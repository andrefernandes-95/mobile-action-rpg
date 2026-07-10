using UnityEngine;

namespace AF
{
    public class DestroyAfter : MonoBehaviour
    {
        [SerializeField] float duration;

        void OnEnable()
        {
            Destroy(this.gameObject, duration);
        }
    }
}
