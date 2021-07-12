using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BTA
{
    //[Serializable]
    public class PlayerDistFighting : PlayerFighting
    {
        private GameObject m_smallBulletPrefab;
        private GameObject m_heavyBulletPrefab;

        [SerializeField] private float m_smallAttackBulletTimeToLive = 1.0f;
        [SerializeField] private float m_heavyAttackBulletTimeToLive = 1.0f;

        [SerializeField] private Transform m_bulletOrigin;
        [SerializeField] private float m_force = 400.0f;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            m_smallBulletPrefab = Resources.Load("SmallBullet") as GameObject;
            m_heavyBulletPrefab = Resources.Load("HeavyBullet") as GameObject;
        }


        protected override void AttackConnect(AttackType type, AttackData data)
        {
            switch (type)
            {
                case AttackType.LightAttack:
                    {
                        InstanceBullet(m_smallBulletPrefab, m_smallAttackBulletTimeToLive, data);
                        m_sounds.LaunchLightAttackSound();
                        break;
                    }
                case AttackType.HeavyAttack:
                    {
                        InstanceBullet(m_heavyBulletPrefab, m_heavyAttackBulletTimeToLive, data);
                        m_sounds.LaunchHeavyAttackSound();
                        break;
                    }
                case AttackType.SpecialAttack:
                    {
                        m_specialAttackTrigger.Enable(data);
                        m_sounds.LaunchSpecialAttackSound();
                        break;
                    }
                default:
                    break;
            }
        }

        protected override void AttackRecovery(AttackType type)
        {
            if (type == AttackType.SpecialAttack)
                m_specialAttackTrigger.Disable();
        }


        private void InstanceBullet(GameObject bulletPrefab, float timeToLive, AttackData data)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab);

            //Debug.Log("Bullet " + bullet.name);

            DistWeaponTrigger trigger = bullet.GetComponent<DistWeaponTrigger>();
            trigger.OnHitEvent.AddListener(OnSuccessfulHit);

            trigger.Enable(data);
            trigger.Owner = m_player;

            bullet.transform.position = m_bulletOrigin.position;
            //TODO : Use curr Weapon Rotation , so that player always shoot in the right direction 
            // Then need to set the rotation 
            bullet.transform.rotation = gameObject.transform.rotation;
            //TODO : Maybe place those 2 things in the Bullet -> parameter per bullet instead of per character
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * m_force);
            Destroy(bullet, timeToLive);
        }
    }
}