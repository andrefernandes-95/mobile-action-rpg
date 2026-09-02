namespace AF
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ModelRandomizer : MonoBehaviour
    {
        [SerializeField] List<SkinnedMeshRenderer> heads;
        [SerializeField] List<SkinnedMeshRenderer> hairs;
        [SerializeField] List<SkinnedMeshRenderer> eyebrows;
        [SerializeField] List<SkinnedMeshRenderer> beards;
        [SerializeField] Material[] materials;
        Material selectedMaterial;

        void Start()
        {
            if (this.isActiveAndEnabled)
            {
                if (materials.Length > 0)
                {
                    selectedMaterial = materials[Random.Range(0, materials.Length)];
                }

                Randomize();
            }
        }

        void Randomize()
        {
            ChoosePiece(heads, Random.Range(0, heads.Count));
            ChoosePiece(hairs, Random.Range(0, hairs.Count));
            ChoosePiece(eyebrows, Random.Range(0, eyebrows.Count));
            ChoosePiece(beards, Random.Range(0, beards.Count));
        }

        void ChoosePiece(List<SkinnedMeshRenderer> pieces, int pieceToActivate)
        {
            if (pieces.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < pieces.Count; i++)
            {
                bool isActive = pieceToActivate == i;
                pieces[i].gameObject.SetActive(isActive);

                if (isActive)
                {
                    pieces[i].SetMaterials(new() { selectedMaterial });
                }
            }
        }

    }
}
