using System;
using System.Linq;
using AstralCore.Utils;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCore.AI.SteeringSystem{
    public interface ISteer{
        /// <summary>
        /// Dictates how this agent reacts to the world
        /// </summary>
        SteeringProfile Profile { get; }
        
        /// <summary>
        /// Displays how fast this agent is moving
        /// </summary>
        Vector3 Velocity{ get; set; }


        /// <summary>
        /// Using a collider, should return the point closest to the parsed position
        /// </summary>
        /// <param name="position">The position we want to find the closest point to</param>
        /// <returns>The position in the collider closest to the parse position</returns>
        Vector3 GetClosestTo(Vector3 position);

        // ---
        Vector3 DirectionTo(Vector3 position) => (position - GetClosestTo(position)).normalized;
        float DistanceTo(Vector3 position) => Vector3.Distance(position, GetClosestTo(position));
        float DistanceTo(ISteer agent) => DistanceTo(agent.GetClosestTo(Profile.Position));
    }

    public static class SteeringFuncs
    {
        public static Vector3 Seek(Vector3 target, ISteer agent) => agent.DirectionTo(target) * agent.Profile.maxSpeed;
        public static Vector3 Seek(ISteer target, ISteer agent) => Seek(target.Profile.Position, agent);
        public static Vector3 Flee(Vector3 target, ISteer agent) => -Seek(target, agent);
        public static Vector3 Flee(ISteer target, ISteer agent) => -Seek(target, agent);

        public static Vector3 Arrive(Vector3 target, float brakeDistance, ISteer agent)
        {
            Vector3 velocity = Seek(target, agent);
            float distance = agent.DistanceTo(target);

            float speed = Mathf.Min(velocity.magnitude * (distance / brakeDistance), agent.Profile.maxSpeed);
            return velocity.normalized * speed;
        }
        public static Vector3 Arrive(ISteer target, float brakeDistance, ISteer agent) => Arrive(target.Profile.Position, brakeDistance, agent);

        public static Vector3 Persuit(ISteer target, ISteer agent, float? maxPredictionDistance = null)
        {
            float distance = agent.DistanceTo(target);
            float prediction = distance / agent.Profile.maxSpeed;
            prediction = Mathf.Min(prediction, maxPredictionDistance ?? prediction);
            Vector3 future = target.Profile.Position + target.Velocity * prediction;
            return Seek(future, agent);
        }
        public static Vector3 Evade(ISteer target, ISteer agent, float? maxPredictionDistance = null) => -Persuit(target, agent, maxPredictionDistance);
        public static Vector3 Wander(float maxAngle, float radius, float distance, ISteer agent)
        {
            Vector3 direction = agent.Velocity.normalized;
            Vector3 circle = direction * distance;

            float angle = Mathf.Atan2(direction.y, direction.x);
            float rndAngle = UnityEngine.Random.Range(-maxAngle, maxAngle) * Mathf.Rad2Deg;

            float radian = angle + rndAngle;
            Vector3 displacement = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)) * radius;
            return (circle + displacement).normalized * agent.Profile.maxSpeed;
        }

        // ---
        static Vector3 Average(ISteer[] agents, Func<ISteer, Vector3> callback) => agents.Aggregate(Vector3.zero, (acc, agent) => {
            try{
                return acc += callback.Invoke(agent) / agents.Length;
            }
            catch (Exception){
                return acc;
            }
        });
        public static Vector3 Alignment(ISteer[] agents) => Average(agents, (a) => a.Velocity);
        public static Vector3 Cohesion(ISteer[] agents, ISteer agent) => (Average(agents, (a) => a.Profile.Position) - agent.Profile.Position).normalized;
        
        public static Vector3 Separation(ISteer[] agents, float separationDistance, ISteer agent)
        {
            Vector3 direction = Average(agents, (a) =>
            {
                if (a == agent) { return default; }
                Vector3 d = agent.Profile.Position - a.Profile.Position;
                float distance = agent.DistanceTo(a.GetClosestTo(agent.Profile.Position));
                return distance < separationDistance && distance >= Mathf.Epsilon ? d / distance : Vector3.zero;
            });
            return direction.normalized * agent.Profile.maxSpeed;
        }
        
        public static Vector3 Avoid(ISteer agent, Vector3 direction, float scanRadius, LayerMask obstacles, Directions directions, float dangerWeight = 0.8f, bool showDebug = false){
            Vector3 result = Vector3.zero;

            GameObject agtObj = (agent as MonoBehaviour).gameObject;
            int originalLayer = agtObj.layer;
            agtObj.layer = Physics.IgnoreRaycastLayer;

            int count = 0;

            foreach (Vector3 selectedDirection in directions){
                float interest = Mathf.Max(0, Vector3.Dot(selectedDirection, direction));

                float danger = 0f;
                if (Physics.Raycast(agent.Profile.Position, selectedDirection, out RaycastHit hit, scanRadius, obstacles)){
                    danger = Mathf.Exp(1f - (hit.distance / scanRadius)) - 1f;
                }

                float influence = interest - (danger * dangerWeight);
                result += selectedDirection * influence;
                count++;

                if (showDebug){
                    Color debugColor = Color.Lerp(Color.white, Color.red, danger);
                    Debug.DrawRay(agent.Profile.Position, (selectedDirection * scanRadius) * influence, debugColor);
                }
            }

            agtObj.layer = originalLayer;

            if (count == 0 || result == Vector3.zero) return Vector3.zero;

            result /= count;
            return result.normalized * agent.Profile.maxSpeed;
        }

        public static Vector3 Resolve(ISteer agent, params Vector3[] forces)
        {
            Vector3 totalForce = forces.Aggregate(Vector3.zero, (acc, f) => acc + f);
            Vector3 desiredDir = totalForce.normalized;
            Vector3 currentDir = agent.Velocity.normalized;
            float angle = Vector3.Angle(agent.Velocity.normalized, desiredDir);

            if(angle > agent.Profile.maxTurnAngle){
                desiredDir = Vector3.Slerp(currentDir, desiredDir, agent.Profile.maxTurnAngle / angle);
            }

            Vector3 desiredVelocity = desiredDir * agent.Profile.maxSpeed;
            Vector3 clampedChange = Vector3.ClampMagnitude(desiredVelocity - agent.Velocity, agent.Profile.maxForce);

            return agent.Velocity = Vector3.ClampMagnitude(agent.Velocity + clampedChange, agent.Profile.maxSpeed);
        }
    }
}