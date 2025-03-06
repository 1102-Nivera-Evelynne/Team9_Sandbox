using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeyondLimitsStudios
{
    public class IgnoreCollision : MonoBehaviour
    {
        [SerializeField]
        private List<Collider> colList1 = new List<Collider>();
        [SerializeField]
        private List<Collider> colList2 = new List<Collider>();

        private void Awake()
        {
            IgnoreCollisions();
        }

        private void IgnoreCollisions()
        {
            foreach (var col1 in colList1)
                foreach (var col2 in colList2)
                    Physics.IgnoreCollision(col1, col2);
        }
    }
}