using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Pumkin.Extensions
{
	public static class SerializedPropertyExtensions
	{
#if UNITY_EDITOR

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
#endif // UNITY_EDITOR
	}

}
