using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class CacWeaponTrigger : AttackTrigger
    {
        override public void Enable(AttackData data)
        {
            base.Enable(data);
            m_firstHit = true;
        }
    }
}