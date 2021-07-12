using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BTA
{
    [Serializable]
    public class SpecialLevel
    {
        public float range;


        public float startUpDuration;
        public float connectDuration;
        public float recoveryDuration;
        public float animationDuration;

        public AttackStruct atkStruct;

#if UNITY_EDITOR
        public void Display()
        {
            range = EditorGUILayout.FloatField("Range", range);

            int tickDamageNb = (atkStruct != null) ? atkStruct.attackDatas.Length : 0;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Timer", EditorStyles.boldLabel);
            startUpDuration = EditorGUILayout.FloatField("Start Up", startUpDuration);
            connectDuration = EditorGUILayout.FloatField("Connect (x" + tickDamageNb + ")", connectDuration);
            recoveryDuration = EditorGUILayout.FloatField("Recovery", recoveryDuration);

            animationDuration = startUpDuration + recoveryDuration + (connectDuration * tickDamageNb);
            EditorGUILayout.LabelField("Connection time = " + connectDuration * tickDamageNb, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Animation time = " + animationDuration, EditorStyles.boldLabel);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            atkStruct = EditorGUILayout.ObjectField("Damage Structure", atkStruct, typeof(AttackStruct), false) as AttackStruct;
            if (atkStruct)
                atkStruct.Display();
        }
#endif
    }

    [CreateAssetMenu()]
    public class SpecialAttack : ScriptableObject
    {
        public List<SpecialLevel> SpecialLevels = new List<SpecialLevel>();
    }
}
