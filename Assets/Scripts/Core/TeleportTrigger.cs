using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AF
{
    public sealed class TeleportTrigger : MonoBehaviour
    {
        [SerializeField] Transform spawnpointTransform;

        [SerializeField] SceneTag goTo;

        void Awake()
        {
            spawnpointTransform.gameObject.name = ConstructSpawnpointName();

            if (TryGetComponent(out MeshRenderer meshRenderer))
            {
                meshRenderer.enabled = false;
            }
        }

        string ConstructSpawnpointName()
        {
            return goTo.ToString() + "_" + SceneManager.GetActiveScene().name;
        }

        string ConstructNextSpawnpointName()
        {
            return SceneManager.GetActiveScene().name + "_" + goTo.ToString();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Teleport();
            }
        }

        public void Teleport()
        {
            TeleportManager.Instance.Teleport(goTo.ToString(), ConstructNextSpawnpointName());
        }
    }
}
