using AstralCandle.AI;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UIAI{
    public abstract class Turtle : MonoBehaviour, ISimpleSteering{
        [SerializeField] float maxSpeed;
        [SerializeField] float maxForce;
        [SerializeField] RectTransform canvas;
        

        public Vector2 WorldPosition => Transform.anchoredPosition;
        public float MaxSpeed => maxSpeed;
        public float MaxForce => maxForce;
        public Vector2 Velocity { get; set; }

        RectTransform _t;
        protected RectTransform Transform => _t ??= transform.GetComponent<RectTransform>();
        ISimpleSteering _s;
        protected ISimpleSteering Steering => _s ??= this;

        /// <summary>
        /// Converts screen positions to usable position in UI space
        /// </summary>
        /// <param name="screenPosition">The position in screen space</param>
        /// <returns>UI space position</returns>
        protected Vector2 ScreenToUISpace(Vector3 screenPosition){
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas,
                screenPosition,
                null,
                out Vector2 localPos
            );
            return localPos;
        }


        /// <summary>
        /// Called in Update, should be used to calculate all the steering forces
        /// </summary>
        protected abstract Vector2[] Move();
        void Update(){
            Transform.anchoredPosition += Steering.Result(Move()) * Time.deltaTime;
            transform.right = (Vector3)Velocity.normalized;
        }
    }
}