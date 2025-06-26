using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI.SteeringSystem{
    public abstract class SteeringMono : MonoBehaviour, ISteer{
        [SerializeField, Tooltip("Dictates how this agent reacts to the world")] SteeringProfile profile;
        public SteeringProfile Profile => profile;
        public Vector3 Velocity { get; set; }
        public abstract Vector3 GetClosestTo(Vector3 position);

        void Start() => InitOnStart();
        void FixedUpdate(){
            SteeringFuncs.Resolve(this, GetForces());
            ProcessFixedUpdate();
        }


        /// <summary>
        /// Should return a list of forces to be processed -- E.g. SteeringFuncs.Seek(...) * weight
        /// </summary>
        protected abstract Vector3[] GetForces();

        /// <summary>
        /// Runs once, on start
        /// </summary>
        protected abstract void InitOnStart();
        /// <summary>
        /// Runs every physics frame
        /// </summary>
        protected abstract void ProcessFixedUpdate();
    }
}