using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class AnimationStarter : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float m_startTime = 0f;


        // Use this for initialization
        void Start()
        {
            Animation animation = GetComponent<Animation>();

            animation[animation.clip.name].normalizedTime = m_startTime;
        }
    }
}
