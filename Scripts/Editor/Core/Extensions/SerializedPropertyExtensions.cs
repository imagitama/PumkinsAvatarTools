#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.Core.Extensions
{
	public static class SerializedPropertyExtensions
	{
		/// <summary>
		/// Gets value from SerializedProperty - even if value is nested
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public static object GetValue(this SerializedProperty property)
		{
			object obj = property.serializedObject.targetObject;

			FieldInfo field = null;
			foreach(var path in property.propertyPath.Split('.'))
			{
				var type = obj.GetType();
				field = type.GetField(path);
				obj = field.GetValue(obj);
			}
			return obj;
		}

		/// <summary>
		/// Sets value from SerializedProperty - even if value is nested
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		public static void SetValue(this SerializedProperty property, object value)
		{
			object obj = property.serializedObject.targetObject;

			List<KeyValuePair<FieldInfo, object>> list = new List<KeyValuePair<FieldInfo, object>>();

			FieldInfo field = null;
			foreach(var path in property.propertyPath.Split('.'))
			{
				var type = obj.GetType();
				field = type.GetField(path);
				list.Add(new KeyValuePair<FieldInfo, object>(field, obj));
				obj = field.GetValue(obj);
			}

			// Now set values of all objects, from child to parent
			for(int i = list.Count - 1; i >= 0; --i)
			{
				list[i].Key.SetValue(list[i].Value, value);
				// New 'val' object will be parent of current 'val' object
				value = list[i].Value;
			}
		}

		/// <summary>
		/// Removes all null elements from array and resizes it to fit elements
		/// </summary>
		/// <param name="property"></param>
		public static void RemoveNullReferencesFromArray(this SerializedProperty property)
		{
			var arr = property.FindPropertyRelative("Array");
			if(!property.isArray)
				throw new ArgumentException($"{property} is not an array.");

			var newList = new List<UnityEngine.Object>();

			for(int i = 0; i < arr.arraySize; i++)
			{
				var d = arr.GetArrayElementAtIndex(i);
				if(d?.objectReferenceValue)
					newList.Add(d.objectReferenceValue);
			}

			if(newList.Count == arr.arraySize)
				return;

			arr.ClearArray();
			for(int i = 0; i < newList.Count; i++)
			{
				arr.InsertArrayElementAtIndex(i);
				arr.GetArrayElementAtIndex(i).objectReferenceValue = newList[i];
			}
		}
	}
}
#endif