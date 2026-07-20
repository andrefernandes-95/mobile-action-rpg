using UnityEngine;

namespace AF
{
    [System.Serializable]
    public class Checkpoint
    {
        public Vector3 worldPos;
        public string scene;
        public bool ShouldRespawn;
    }
}
