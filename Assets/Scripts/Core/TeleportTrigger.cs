using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AF
{
    public sealed class TeleportTrigger : MonoBehaviour
    {
        [SerializeField] Transform spawnpointTransform;

        [SerializeField] SceneTag[] goTo;
        SceneTag SelectedGoTo;

        void Awake()
        {
            SelectedGoTo = goTo.Length > 0 ? goTo[UnityEngine.Random.Range(0, goTo.Length)] : null;

            if (TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer.enabled = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (SelectedGoTo == null)
            {
                return;
            }

            if (other.CompareTag("Player"))
            {
                Teleport();
            }
        }

        public void Teleport()
        {
            if (SelectedGoTo == null)
            {
                return;
            }

            TeleportManager.Instance.Teleport(SelectedGoTo.name);
        }
    }
}
