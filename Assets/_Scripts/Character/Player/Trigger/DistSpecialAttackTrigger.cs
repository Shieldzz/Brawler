using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class DistSpecialAttackTrigger : SpecialAttackTrigger
    {

        public override void SetRadius(float radius)
        {
            CapsuleCollider collider = m_collider as CapsuleCollider;

            collider.radius = radius;

            base.SetRadius(radius);
        }
    }
}
