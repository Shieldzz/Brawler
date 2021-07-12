using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class PoisonBarrel : MonoBehaviour
    {
        [SerializeField] GameObject PoisonZone;

        public void SpawnPoison()
        {
            if (!PoisonZone)
                return;

            GameObject poison = GameObject.Instantiate(PoisonZone);
            poison.transform.position = transform.position;
        }
    }
}
