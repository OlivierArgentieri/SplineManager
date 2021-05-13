using UnityEngine;

namespace Unity_Framework.Scripts.Import.ManagerTemplate
{
    public class ManagerTemplate<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region f/p
        private static T instance = null;
        public static T Instance => instance;

        [SerializeField, Header("Keep ?")] private bool keep = false;
        #endregion

        #region unity methods

        protected virtual void Awake()
        {
            InitSingleton();
        }

        #endregion

        #region custom methods

        private void InitSingleton()
        {
            if (instance == null)
                instance = this as T;

            if (instance != this)
                Destroy(gameObject);
        
            if(keep) DontDestroyOnLoad(gameObject);

            name += "[MNG]";
        }

        #endregion
    }
}
