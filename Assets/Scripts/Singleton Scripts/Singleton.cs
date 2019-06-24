using UnityEngine;

/// <summary>
/// Makes a MonoBehaviour derived class unique.
/// </summary>
/// <typeparam name="T">
/// Class type.
/// </typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;



    /// <summary>
    /// Returns instance reference.
    /// </summary>
    public static T Instance
    {
        get
        {
            return instance;
        }
    }



    /// <summary>
    /// Initializes static instance reference.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            Initialize();
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    /// <summary>
    /// Use this method instead of Awake().
    /// </summary>
    protected virtual void Initialize() { }
}
