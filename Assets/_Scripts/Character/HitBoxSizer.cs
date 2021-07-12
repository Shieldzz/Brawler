using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSizer : MonoBehaviour
{
    [SerializeField] BoxCollider m_collider;


    // Use this for initialization
    void Start()
    {
        BoxCollider hitbox = GetComponent<BoxCollider>();
        hitbox.center = m_collider.center;
        hitbox.size = m_collider.size;
        hitbox.isTrigger = true;
    }
}
