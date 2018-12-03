using UnityEngine;
using System.Collections;

public class Singleton<T>: MonoBehaviour where T : MonoBehaviour
{
    #region Singleton Setup
    private static T instance;

    public Singleton() { }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<T>();
                    singleton.name = "(singleton) " + typeof(T).ToString();

                    DontDestroyOnLoad(singleton);
                }
            }

            return instance;
        }
    }
    #endregion
}
