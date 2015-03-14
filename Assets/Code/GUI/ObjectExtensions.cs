using System;
using System.Collections.Generic;
using System.Reflection;

public static class ObjectExtensions
{
    public static PropertyInfo GetProperty(this object affectedObject, string propertyName)
    {
        var type = affectedObject.GetType();
        var propertyInfo = type.GetProperty(propertyName);
        if (propertyInfo == null)
        {
            throw new Exception(string.Format("Property {0} does not exist in {1}", propertyName, type.Name));
        }
        return propertyInfo;
    }

	public static MethodInfo GetMethod(this object affectedObject, string methodName)
    {
        var type = affectedObject.GetType();
        var methodInfo = type.GetMethod(methodName);
        if (methodInfo == null)
        {
            throw new Exception(string.Format("Method {0} does not exist in {1}", methodName, type.Name));
        }
        return methodInfo;
    }

	public static List<T> AsList<T>(this T element)
	{
		return new List<T> { element };
	}
}

public static class PropertyInfoExtensions
{
	public static T GetValue<T>(this PropertyInfo property, object affectedObject)
	{
		var value = property.GetValue(affectedObject, null);
		if (!(value is T))
		{
			throw new Exception(string.Format("Property {0} is not of type {1} but of type {2}.", property.Name, typeof(T), value.GetType()));
		}
		return (T)value;
	}
}