using UnityEngine;

namespace Resources.Scripts
{
    public class MonoSingletonGeneric<T> : MonoBehaviour where T : MonoSingletonGeneric<T>
    {
        private static T instance;
        public static T Instance { get { return instance; } }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else
            {
                Debug.Log("Someone Trying to create another instance");
                Destroy(this);
            }
        }

    }
}