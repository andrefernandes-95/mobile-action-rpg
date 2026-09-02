using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public class LockOn : MonoBehaviour
    {
        [Header("Lock On")]
        [HideInInspector] public Transform lockOnTarget;
        [HideInInspector] public bool isLockedOn;
        public float lockOnRotationSpeed = 15f;

        void Update()
        {
            if (isLockedOn)
            {
                if (lockOnTarget == null || lockOnTarget.GetComponent<Health>().IsDeadAndNotReviving())
                {
                    ClearLockOn();
                }
            }
        }

        public void SetLockOn(Transform target)
        {
            lockOnTarget = target;
            isLockedOn = target != null;
        }

        void ClearLockOn()
        {
            lockOnTarget = null;
            isLockedOn = false;
        }
    }
}
