using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BTA
{
    [CreateAssetMenu()]
    public class MultiplierLadder : ScriptableObject
    {
        public int[] requiredIntegers = new int[1];
        public float[] associatedMultiplier = new float[1];


        public float GetMultiplierFromRequirement(int currValue)
        {
            int size = requiredIntegers.Length;
            int multiplierID = 0;

            for (int i = 0; i < size; i++)
            {
                if (currValue >= requiredIntegers[i])
                    multiplierID = i;
                
                else
                    break;
            }
            return associatedMultiplier[multiplierID];
        }

#if UNITY_EDITOR
        public void Display()
        {
            System.Array.Resize<int>(ref requiredIntegers, EditorGUILayout.IntSlider("Number of level", requiredIntegers.Length, 1, 10));
            System.Array.Resize<float>(ref associatedMultiplier, requiredIntegers.Length);

            int size = requiredIntegers.Length;

            for (int i = 0; i < size; i++)
            {
                GUILayout.BeginVertical("box");
                {
                    //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    requiredIntegers[i] = EditorGUILayout.IntField("Value ", requiredIntegers[i]);
                    associatedMultiplier[i] = EditorGUILayout.FloatField("Multiplier ", associatedMultiplier[i]);
                }
                GUILayout.EndVertical();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Ladder", EditorStyles.boldLabel);

            for (int i = 0; i < size; i++)
                EditorGUILayout.LabelField("From " + requiredIntegers[i] + " => x" + associatedMultiplier[i]);
        }
#endif
    }
}

