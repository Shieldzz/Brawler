using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public enum EnemyType
    {
        None,
        CacEnemy,
        SpeCacEnemy,
        DistEnemy,
        //SpeDistEnemy
    }

    public class EnemySpawnData : MonoBehaviour
    {

        public EnemyType type = EnemyType.None;
    }
}
