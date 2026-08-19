using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public sealed class NPC : MonoBehaviour
    {
        [SerializeField] string messageToPlayer = "";
        [SerializeField] UIDocument uIDocument;
        [SerializeField] Transform player;

        void Awake()
        {
            if (uIDocument != null)
            {
                uIDocument.rootVisualElement.Q<Label>().text = messageToPlayer;
            }
        }

        void Update()
        {
            if (player != null)
            {
                Vector3 dir = player.transform.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }
}
