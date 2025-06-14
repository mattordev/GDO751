using System;
using System.Linq;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2024 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.EntitySystem{
    /// <summary>
    /// Allows us to add more 'organic' motion to our entities when moving
    /// </summary>
    public interface ISteering: ILook, ICollider{
        #region VARIABLES
        /// <summary>
        /// The world position of this entity
        /// </summary>
        Vector3 WorldPosition{ get; }  
        /// <summary>
        /// The max possible speed this agent can reach at this given time
        /// </summary>
        float MaxSpeed{ get; }
        /// <summary>
        /// The max possible force that can be inflicted onto this agent at this given time
        /// </summary>
        float MaxForce{ get; }
        /// <summary>
        /// Dictates how speed chances the more we move backwards from facing direction(0-1)
        /// </summary>
        float ReverseFactor{ get; }
        /// <summary>
        /// The direction * speed this entity is moving in
        /// </summary>
        Vector3 Velocity{ get; set; }
        #endregion
        // ---
        #region HELPER FUNCTIONS
        /// <summary>
        /// Calculates the normalised direction to the target position
        /// </summary>
        /// <param name="position">The position we want to move to</param>
        Vector3 GetDirection(Vector3 position) => (position - WorldPosition).normalized;
        /// <summary>
        /// Calculates the distance between the parsed position and the closest point from this entitys collider
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The distance between both positions</returns>
        float GetDistance(Vector3 position) => Vector3.Distance(position, Collider.ClosestPoint(position));
        /// <summary>
        /// Calculates the speed based on the direction we are moving in compared to the direction we are looking towards
        /// </summary>
        /// <param name="direction">The direction we are wanting to move in</param>
        float GetSpeed(Vector3 direction) => UFunc.Remap(
            Vector3.Dot(LookDirection, direction),
            -1, 1,
            MaxSpeed * ReverseFactor, 
            MaxSpeed
        );
        /// <summary>
        /// Iterating upon all the parsed agents, get an average of the requested value e.g. WorldPosition
        /// </summary>
        /// <param name="agents">List of agents</param>
        /// <param name="f">Custom function to get the desired variable from an agent</param>
        /// <returns>An average vector</returns>
        static Vector3 AverageCollector(ISteering[] agents, Func<ISteering, Vector3> f) => agents.Aggregate(Vector3.zero, (acc, agent) => acc + f.Invoke(agent)) / agents.Length;
        #endregion
        // ---
        #region MAIN FUNCTIONS
        Vector3 Seek(Vector3 position){
            Vector3 dir = GetDirection(position);
            return dir * GetSpeed(dir);
        }
        Vector3 Flee(Vector3 position) => -Seek(position);        
        Vector3 Arrive(Vector3 position, float brakeDistance){
            Vector3 velocity = Seek(position);
            float distance = GetDistance(position);
            float speed = Mathf.Min(velocity.magnitude * (distance / brakeDistance), MaxSpeed);
            return velocity.normalized * speed;
        }
        Vector3 Persuit(ISteering steering, float? maxPredictionDistance = null){
            float distance = GetDistance(steering.WorldPosition);
            float prediction = distance / MaxSpeed;
            prediction = Mathf.Min(prediction, maxPredictionDistance ?? prediction);
            Vector3 futurePosition = steering.WorldPosition + steering.Velocity.normalized * prediction;
            return Seek(futurePosition);
        }
        Vector3 Evade(ISteering steering, float? maxPredictionDistance = null) => -Persuit(steering, maxPredictionDistance);
        Vector3 Wander(Vector3 direction, float maxAngle, float radius, float distance){
            Vector3 circle = direction * distance;
            float angle = Mathf.Atan2(direction.y, direction.x);
            float rndAngle = UnityEngine.Random.Range(-maxAngle, maxAngle) * Mathf.Rad2Deg;
            float radian = angle + rndAngle;
            Vector3 displacement = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)) * radius;
            return (circle + displacement).normalized * GetSpeed(direction);
        }
        #endregion
        // ---
        #region GROUP FUNCTIONS
        /// <summary>
        /// Calculates the mean velocity from all parsed agents
        /// </summary>
        /// <param name="agents">List of agents</param>
        /// <returns>The average velocity from the parsed agents</returns>
        static Vector3 Alignment(ISteering[] agents) => AverageCollector(agents, (a) => a.Velocity);
        /// <summary>
        /// Calculates the center point from all parsed agents
        /// </summary>
        /// <param name="agents">List of agents</param>
        /// <returns>The average world position of the parsed agents | Requires a 'MAIN' (SEEK/PERSUIT/etc...) function to act upon it</returns>
        static Vector3 Cohesion(ISteering[] agents) => AverageCollector(agents, (a) => a.WorldPosition);
        /// <summary>
        /// Calculates the average direction away from the group of enemies (Path of least resistance)
        /// </summary>
        /// <param name="agents">List of agents</param>
        /// <param name="separationDistance">The distance from where we start moving away</param>
        /// <returns></returns>
        Vector3 Separation(ISteering[] agents, float separationDistance){
            Vector3 direction = AverageCollector(agents, (a) =>{
                if(a == this){ return default; } // Stops self influence
                Vector3 direction = WorldPosition - a.WorldPosition;
                float distance = GetDistance(a.WorldPosition);
                return distance < separationDistance ? direction / distance : Vector3.zero;
            });
            return direction.normalized * GetSpeed(direction);
        }
        #endregion

        /// <summary>
        /// Attempts to avoid obstacles
        /// </summary>
        /// <param name="direction">The direction we are wanting to move towards</param>
        /// <param name="scanRadius">How far out from the character should we scan?</param>
        /// <param name="obstacles">What layers are deemed as obstacles?</param>
        /// <param name="directions">The number of directions around the character we are enquiring about</param>
        /// <param name="dangerWeight">The pushback force from obstacles</param>
        /// <param name="showDebug">Shows lines showing the decisions this algorithm is making</param>
        /// <returns>A direction avoiding any obstacles in the way</returns>
        Vector3 Avoid(GameObject agent, Vector3 direction, float scanRadius, LayerMask obstacles, int directions = 8, float dangerWeight = 0.8f, bool showDebug = false){
            Vector3 result = default;
            int ogLayer = agent.layer;
            agent.layer = Physics2D.IgnoreRaycastLayer;

            for(int i = 0; i < directions; i++){
                Vector3 current = UFunc.GetCircleDirection(i, directions);
                float interest = Vector3.Dot(current, direction);
                if(interest <= 0){ continue; } // Stops negative pushback force from where we are trying to avoid

                RaycastHit2D hit = Physics2D.Raycast(WorldPosition, current, scanRadius, obstacles);
                float danger = hit? Mathf.Exp(1 - (hit.distance / scanRadius)) - 1: 0; // EXP has huge influence the closer we are to obstacle
                float influence = interest - (danger * dangerWeight);

                result += current * influence;

                if(showDebug){ Debug.DrawRay(WorldPosition, current * influence, Color.Lerp(Color.white, Color.red, danger)); }
            }

            agent.layer = ogLayer;            
            result.Normalize();
            return result * GetSpeed(result);
        }
        /// <summary>
        /// Sums up all the forces and clamps it so it accelerates and hits the max speed to the way we have defined this profile
        /// </summary>
        /// <param name="forces">The forces we are wanting to apply to an entity</param>
        /// <returns>The calculated direction and speed to move in</returns>
        Vector3 Result(params Vector3[] forces){
            Vector3 totalForce = forces.Aggregate(Vector3.zero, (acc, force) => acc + force);
            Vector3 clampedChangeInForce = Vector3.ClampMagnitude(totalForce - Velocity, MaxForce);
            return Velocity = Vector3.ClampMagnitude(Velocity + clampedChangeInForce, MaxSpeed);
        }
    }
}