using System;
using System.Linq;
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
        static Vector3 Average(ISteer[] agents, Func<ISteer, Vector3> callback) => agents.Aggregate(Vector3.zero, (acc, agent) => acc + callback.Invoke(agent)) / agents.Length;
        public static Vector3 Alignment(ISteer[] agents) => Average(agents, (a) => a.Velocity);
        public static Vector3 Cohesion(ISteer[] agents) => Average(agents, (a) => a.Profile.Position);
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

        public static Vector3 Resolve(ISteer agent, params Vector3[] forces)
        {
            Vector3 totalForce = forces.Aggregate(Vector3.zero, (acc, f) => acc + f);
            Vector3 clampedChange = Vector3.ClampMagnitude(totalForce - agent.Velocity, agent.Profile.maxForce);
            Vector3 desiredVelocity = Vector3.ClampMagnitude(agent.Velocity + clampedChange, agent.Profile.maxSpeed);

            // Need to potentially limit this function so that it limits the angle at which the agent can turn. Because if agent suddenly goes in reverse, the character would immediately rotate
            
            return agent.Velocity = desiredVelocity;
        }
    }
}