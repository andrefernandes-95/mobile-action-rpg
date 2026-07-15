namespace AF
{
    using UnityEngine;
    using System.Collections.Generic;

    public class CameraOcclusionHide : MonoBehaviour
    {
        public Transform player;
        public LayerMask occluderLayer;

        public Material fadeMaterial;

        [Range(0f, 1f)] public float fadeAlpha = 0.25f;

        private readonly Dictionary<Collider, Renderer> rendererCache = new();
        private readonly RaycastHit[] hitBuffer = new RaycastHit[32];
        private readonly HashSet<Renderer> currentHits = new();
        private readonly List<Renderer> toRestore = new();

        private readonly Dictionary<Renderer, OccludeRenderer> fadedRenderers = new();

        private class OccludeRenderer
        {
            public Material[] originals;
            public Material[] fades;
        }

        void Awake()
        {
            if (fadeMaterial == null)
            {
                Shader shader = Shader.Find("AF/OcclusionFade");
                if (shader != null)
                {
                    fadeMaterial = new Material(shader);
                }
            }
        }


        void LateUpdate()
        {
            if (player == null || fadeMaterial == null)
            {
                return;
            }

            Vector3 dir = player.position + player.transform.up - transform.position;
            float dist = dir.magnitude;
            if (dist < Mathf.Epsilon)
            {
                return;
            }

            currentHits.Clear();
            toRestore.Clear();

            int hitCount = Physics.RaycastNonAlloc(
                transform.position,
                dir / dist,
                hitBuffer,
                dist,
                occluderLayer
            );


            for (int i = 0; i < hitCount; i++)
            {
                Collider collider = hitBuffer[i].collider;
                if (collider == null)
                {
                    continue;
                }

                if (!rendererCache.TryGetValue(collider, out Renderer rend))
                {
                    rend = collider.GetComponent<Renderer>();
                    rendererCache[collider] = rend;
                }

                if (rend == null)
                {
                    continue;
                }

                currentHits.Add(rend);
                ApplyFade(rend);
            }

            foreach (var kvp in fadedRenderers)
            {
                if (kvp.Key == null)
                {
                    // Object was destroyed
                    DestroyFadeMaterials(kvp.Value);
                    continue;
                }

                if (!currentHits.Contains(kvp.Key))
                {
                    toRestore.Add(kvp.Key);
                }
            }

            foreach (Renderer rendererToRestore in toRestore)
            {
                RestoreRenderer(rendererToRestore);
            }
        }

        void OnDisable()
        {
            RestoreAll();
        }

        void OnDestroy()
        {
            RestoreAll();
        }

        void ApplyFade(Renderer renderer)
        {
            if (fadedRenderers.ContainsKey(renderer))
            {
                return; // Already faded
            }

            Material[] originals = renderer.sharedMaterials;
            Material[] fades = new Material[originals.Length];

            for (int i = 0; i < originals.Length; i++)
            {
                fades[i] = CreateFadeMaterial(originals[i]);
            }

            fadedRenderers[renderer] = new OccludeRenderer
            {
                originals = originals,
                fades = fades
            };

            renderer.materials = fades;
        }

        void RestoreRenderer(Renderer renderer)
        {
            if (!fadedRenderers.TryGetValue(renderer, out OccludeRenderer state))
            {
                return;
            }

            renderer.sharedMaterials = state.originals;
            DestroyFadeMaterials(state);
            fadedRenderers.Remove(renderer);
        }

        void RestoreAll()
        {
            foreach (var kvp in fadedRenderers)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.sharedMaterials = kvp.Value.originals;
                }

                DestroyFadeMaterials(kvp.Value);
            }

            fadedRenderers.Clear();
        }

        Material CreateFadeMaterial(Material original)
        {
            Material fade = new Material(fadeMaterial);
            CopyVisuals(original, fade);

            Color color = GetSourceColor(original);
            color.a = fadeAlpha;
            fade.SetColor("_BaseColor", color);

            return fade;
        }

        static void CopyVisuals(Material source, Material target)
        {
            if (source == null)
            {
                return;
            }

            if (source.HasProperty("_BaseMap"))
            {
                target.SetTexture("_BaseMap", source.GetTexture("_BaseMap"));
                target.SetTextureScale("_BaseMap", source.GetTextureScale("_BaseMap"));
                target.SetTextureOffset("_BaseMap", source.GetTextureOffset("_BaseMap"));
            }
            else if (source.HasProperty("_MainTex"))
            {
                target.SetTexture("_BaseMap", source.GetTexture("_MainTex"));
                target.SetTextureScale("_BaseMap", source.GetTextureScale("_MainTex"));
                target.SetTextureOffset("_BaseMap", source.GetTextureOffset("_MainTex"));
            }
            else if (source.HasProperty("_Lowpoly_DiffuseMap"))
            {
                target.SetTexture("_BaseMap", source.GetTexture("_Lowpoly_DiffuseMap"));
            }
        }

        static Color GetSourceColor(Material source)
        {
            if (source.HasProperty("_BaseColor"))
            {
                return source.GetColor("_BaseColor");
            }

            if (source.HasProperty("_Color"))
            {
                return source.GetColor("_Color");
            }

            if (source.HasProperty("_Lowpoly_DiffuseColor"))
            {
                return source.GetColor("_Lowpoly_DiffuseColor");
            }

            return Color.white;
        }

        static void DestroyFadeMaterials(OccludeRenderer state)
        {
            if (state.fades == null)
            {
                return;
            }

            foreach (Material mat in state.fades)
            {
                if (mat != null)
                {
                    Destroy(mat);
                }
            }
        }
    }
}
