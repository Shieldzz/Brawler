using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class Notifier : MonoBehaviour
    {

        void Start()
        {

        }

        void Update()
        {

        }

        private void OnEnable()
        {
            GameplayManager gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();
            if (gameplayManager.ShopVisited)
                gameObject.SetActive(false);
        }
    }

}