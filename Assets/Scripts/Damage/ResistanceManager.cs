using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public class ResistanceManager : MonoBehaviour
    {
        public List<DamageResistance> resistances = new();

        public int ApplyReduction(DamageType damageType, int amount)
        {
            float reduction = GetReduction(damageType);
            return Mathf.RoundToInt(amount * (1f - reduction));
        }

        float GetReduction(DamageType damageType)
        {
            for (int i = 0; i < resistances.Count; i++)
            {
                if (resistances[i].damageType == damageType)
                {
                    return resistances[i].reduction;
                }
            }

            return 0f;
        }
    }
}
