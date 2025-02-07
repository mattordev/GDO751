using UnityEngine;

/// <summary>
/// ©️2024-2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Utilities{
    /// <summary>
    /// Interface method of creating singletons
    /// </summary>
    /// <typeparam name="T">The type to become a singleton</typeparam>
    public interface ISingleton<T> where T: MonoBehaviour{
        static T Instance{ get; private set; }

        public bool CreateSingleton(T obj){
            if(Instance){ return false; }
            Instance = obj;
            return true;
        }
    }
    
    /// <summary>
    /// Class method of creating singletons
    /// </summary>
    /// <typeparam name="T">The type to become a singleton</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour{
        public static T Instance{ get; private set; }

        public bool CreateSingleton(T obj){
            if(Instance){ 
                Destroy(gameObject);
                return false; 
            }
            Instance = obj;
            return true;
        }
    }
}