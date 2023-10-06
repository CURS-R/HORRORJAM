using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CURSR.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        [HideInInspector] protected bool Initialized;
        
        private static T _instance;

        /*public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>() as T;
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null || _instance == this || _instance == this as T)
            {
                _instance = this as T;
                Init();
                DontDestroyOnLoad(gameObject);
                this.Initialized = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }*/

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if(_instance == null)
                    {
                        _instance = new GameObject().AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if(_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this as T;
            Init();
            DontDestroyOnLoad(this);
            this.Initialized = true;
        }

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        protected abstract void Init();
    }
}