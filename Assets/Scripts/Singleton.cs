using UnityEngine;


public class Singleton<T> : MonoBehaviour
where T: MonoBehaviour
{
    public static T Instance {get; private set;}

    public virtual void Awake()
    {
     if(Instance != null)
     {
      Destroy(gameObject);
      return;
     } 

     else if(Instance == null)
     {
       Instance = this as T;
       
       DontDestroyOnLoad(gameObject);  
     }   

    }
}