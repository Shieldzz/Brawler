using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BTA
{
    //[Serializable]
    public class EnemyDistFighting : EnemyFighting
    {
        private GameObject m_smallBulletPrefab;
        //TODO : Move Time to Live & Force to the bullet prefab instead of here

        [SerializeField] private float m_smallAttackBulletTimeToLive = 1.0f;

        [SerializeField] private Transform m_bulletOrigin;
        [SerializeField] private float m_force = 400.0f;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            m_smallBulletPrefab = Resources.Load("EnemyBullet") as GameObject;
            //m_specialBulletGameObjectPreload = Resources.Load("HeavyBullet") as GameObject;
        }

        protected override void AttackConnect(AttackType type, AttackData data)
        {
            switch (type)
            {
                case AttackType.LightAttack:
                    InstanceBullet(m_smallBulletPrefab, m_smallAttackBulletTimeToLive, data);
                    break;
                default:
                    break;
            }
        }

        protected override void AttackRecovery(AttackType type)
        {
        }


        private void InstanceBullet(GameObject bulletPrefab, float timeToLive, AttackData data)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab);

            m_sounds.LaunchAttackSound(); 

            DistWeaponTrigger trigger = bullet.GetComponent<DistWeaponTrigger>();
            trigger.Enable(data);
            

            bullet.transform.position = m_bulletOrigin.position;
            //TODO : Use curr Weapon Rotation , so that player always shoot in the right direction 
            // Then need to set the rotation 
            bullet.transform.rotation = gameObject.transform.rotation;
            //TODO : Maybe place those 2 things in the Bullet -> parameter per bullet instead of per character
            Vector3 direction = m_enemy.GetTargetPlayer.transform.position - m_enemy.transform.position;
            direction.Normalize();
            direction.z = 0;
            bullet.GetComponent<Rigidbody>().AddForce(direction * m_force);
            Destroy(bullet, timeToLive);
        }
    }
}