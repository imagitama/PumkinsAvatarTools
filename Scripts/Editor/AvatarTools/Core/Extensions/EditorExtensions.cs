using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.Core.Extensions
{
    public static class EditorExtensions
    {
        public static bool OnInspectorGUINoScriptField(this Editor inspector)
        {
            if(!inspector)
                return false;


            EditorGUI.BeginChangeCheck();
            {
                inspector.serializedObject.Update();

                SerializedProperty iterator = inspector.serializedObject.GetIterator();

                iterator.NextVisible(true);

                while(iterator.NextVisible(false))
                {
                    try //Sometimes throws null even tho it passes null check
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                    catch {}
                }

                inspector.serializedObject.ApplyModifiedProperties();
            }
            return EditorGUI.EndChangeCheck();
        }
    }
}