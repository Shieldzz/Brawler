using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class ExitBrick : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player")
                return;

            LevelManager.Instance.GetInstanceOf<NarrativeManager>().EndingNarration();
        }
    }

}