using System;
using UnityEngine;

namespace Assets.Code
{
    public static class GameObjectExtensions
    {
        public static object GetDataContext(this GameObject gameObject)
        {
            var dataContext = (DataContext)gameObject.GetComponentInParent(typeof(DataContext));
            if (dataContext == null)
            {
                throw new Exception("Datacontext not found!");
            }

            return dataContext.Data;
        }
    }
}
