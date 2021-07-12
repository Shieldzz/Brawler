using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class DistWeaponTrigger : AttackTrigger
    {
        [SerializeField] float DurationOutOfScreen = 0.05f;

        override protected void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            //Debug.Log("Dist Weapon Trigger ? ");

            if (other.gameObject.GetComponent<Damageable>())

                Destroy(gameObject);      

            else if (other.gameObject.layer == LayerMask.NameToLayer("PlayerRestricted"))

                Destroy(gameObject, DurationOutOfScreen);

        }
    }
}
