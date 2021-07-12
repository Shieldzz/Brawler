using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class CacSpecialAttackTrigger : SpecialAttackTrigger
    {

        public override void SetRadius(float radius)
        {
            SphereCollider collider = m_collider as SphereCollider;

            collider.radius = radius;

            base.SetRadius(radius);
        }
    }
}
