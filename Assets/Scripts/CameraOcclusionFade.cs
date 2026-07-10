namespace AF
{
    using UnityEngine;
    using System.Collections.Generic;

    public class CameraOcclusionHide : MonoBehaviour
    {
        public Transform player;
        public LayerMask occluderLayer;

        private HashSet<Renderer> hiddenRenderers = new HashSet<Renderer>();

        void LateUpdate()
        {
            if (player == null) return;

            Vector3 dir = player.position - transform.position;
            float dist = dir.magnitude;

            Ray ray = new Ray(transform.position, dir);
            RaycastHit[] hits = Physics.RaycastAll(ray, dist, occluderLayer);

            HashSet<Renderer> currentHits = new HashSet<Renderer>();

            foreach (RaycastHit hit in hits)
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend == null) continue;

                currentHits.Add(rend);

                if (rend.enabled)
                    rend.enabled = false;

                hiddenRenderers.Add(rend);
            }

            // Restore objects no longer blocking
            hiddenRenderers.RemoveWhere(rend =>
            {
                if (!currentHits.Contains(rend))
                {
                    rend.enabled = true;
                    return true;
                }
                return false;
            });
        }
    }
}
