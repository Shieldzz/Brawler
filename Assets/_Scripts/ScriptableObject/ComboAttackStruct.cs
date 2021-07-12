using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BTA
{
    public enum AttackType
    {
        LightAttack = 0,
        HeavyAttack = 1,
        SpecialAttack = 2,
        None
    }

    [CreateAssetMenu()]
    public class ComboAttackStruct : AttackStruct
    {
        public AttackType type = AttackType.LightAttack;

        public float pursueComboTimer = 1.5f;


#if UNITY_EDITOR
        override public void Display()
        {
            GUILayout.Label("Specs", EditorStyles.boldLabel);
            type = (AttackType)EditorGUILayout.EnumPopup("Attack Type", type);

            pursueComboTimer = EditorGUILayout.FloatField("Combo Timer", pursueComboTimer);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            base.Display();
        }
#endif

    }
}
