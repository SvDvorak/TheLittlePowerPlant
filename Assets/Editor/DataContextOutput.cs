using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEditor;

[CustomEditor(typeof(DataContext))]
public class DataContextOutput : Editor
{
	public override void OnInspectorGUI()
	{
		var dataContext = ((DataContext) target).Data;

		if (dataContext == null)
		{
			EditorGUILayout.LabelField("Empty DataContext");
			return;
		}

		var contextProperties = dataContext.GetType().GetProperties();
		foreach (var property in contextProperties)
		{
			CreateField(property, dataContext);
		}
	}

	private void CreateField(PropertyInfo property, object dataContext)
	{
		object newValue = null;
		if(property.PropertyType == typeof(string))
		{
			newValue = EditorGUILayout.TextField(property.Name, property.GetValue<string>(dataContext));
		}
		else if (property.PropertyType == typeof(int))
		{
			newValue = EditorGUILayout.IntField(property.Name, property.GetValue<int>(dataContext));
		}
		else if (property.PropertyType == typeof(float))
		{
			newValue = EditorGUILayout.FloatField(property.Name, property.GetValue<float>(dataContext));
		}
		else if (property.PropertyType == typeof(Range))
		{
			var range = property.GetValue<Range>(dataContext);
			var newLow = EditorGUILayout.FloatField(property.Name + ".Low", range.Low);
			var newHigh = EditorGUILayout.FloatField(property.Name + ".High", range.High);
			newValue = new Range(newLow, newHigh);
		}

		if (property.CanWrite && newValue != null)
		{
			property.SetValue(dataContext, newValue, null);
		}
	}
}
