using System;
using UnityEngine;

namespace Assets.Code
{
    public static class GameObjectExtensions
    {
        public static object GetDataContext(this GameObject gameObject)
        {
            var dataContext = gameObject.GetComponent<DataContext>();
	        if (dataContext == null)
	        {
		        dataContext = gameObject.GetComponentInParent<DataContext>();
	        }

            VerifyDataContext(dataContext);

	        return dataContext.Data;
        }

	    private static void VerifyDataContext(DataContext dataContext)
	    {
		    if (dataContext == null)
		    {
			    throw new Exception("DataContext behavior not found!");
		    }

		    if (dataContext.Data == null)
		    {
			    throw new Exception("DataContext is empty");
		    }
	    }

	    public static T GetDataContext<T>(this GameObject gameObject) where T : class
		{
			var unknownContext = gameObject.GetDataContext();
			var typedDataContext = unknownContext as T;
			if (typedDataContext == null)
			{
				throw new Exception(string.Format("Expected DataContext of type {0} but found {1}.", typeof(T).Name, unknownContext.GetType().Name));
			}

			return typedDataContext;
		}
    }
}
