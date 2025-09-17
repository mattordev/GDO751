using System.Collections;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.SceneManagement{
    /// <summary>
    /// Allows for scene transitions to be built off of
    /// </summary>
    public abstract class SceneTransition: MonoBehaviour{
        [SerializeField] protected AnimEaser easing;

        void Awake() => easing.SetReverse(true);

        protected abstract void Update();

        /// <summary>
        /// Handy for progress bars
        /// </summary>
        public virtual void WhileLoading(float progress){ return; }

        /// <summary>
        /// Easing into view
        /// </summary>
        public virtual IEnumerator In(){
            easing.SetReverse(false);
            yield return new WaitForSeconds(easing.Duration);
        }
        /// <summary>
        /// Easing out of view
        /// </summary>
        public virtual IEnumerator Out(){
            easing.SetReverse(true);
            yield return new WaitForSeconds(easing.Duration);
        }
    }
}