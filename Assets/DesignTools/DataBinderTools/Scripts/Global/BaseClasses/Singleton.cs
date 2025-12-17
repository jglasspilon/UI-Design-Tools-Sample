using UnityEngine;

/// <summary>
/// Enforces a single instance of the reference type exists in the scene. Lazily instantiates an instance if none exist.
/// </summary>
/// <typeparam name="T">Reference type to set as singleton</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public Singleton()
    {
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    Debug.LogWarning($"Instance of type {typeof(T)} does not exist in the scene. Generating a default instance, results may not be as expected");
                    GameObject go = new GameObject(typeof(T).ToString());
                    _instance = go.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = gameObject.GetComponent<T>();

        else if (_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            Debug.LogWarning(string.Format("Instance of {0} already exists, removing {1}", GetType().FullName, ToString()));
        }
    }
}