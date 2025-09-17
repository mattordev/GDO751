using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// ©️2024 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.AI{
    /// <summary>
    /// Allows us to add more 'organic' motion to our entities when moving
    /// </summary>
    public interface ISimpleSteering {
        #region VARIABLES
        /// <summary>
        /// The world position of this entity
        /// </summary>
        Vector2 WorldPosition { get; }
        /// <summary>
        /// The max possible speed this agent can reach at this given time
        /// </summary>
        float MaxSpeed { get; }
        /// <summary>
        /// The max possible force that can be inflicted onto this agent at this given time
        /// </summary>
        float MaxForce { get; }

        /// <summary>
        /// The direction * speed this entity is moving in
        /// </summary>
        Vector2 Velocity { get; set; }
        #endregion
        // ---
        #region HELPER FUNCTIONS
        /// <summary>
        /// Calculates the normalised direction to the target position
        /// </summary>
        /// <param name="position">The position we want to move to</param>
        Vector2 GetDirection(Vector2 position) => (position - WorldPosition).normalized;
        /// <summary>
        /// Calculates the distance between the parsed position and the closest point from this entitys collider
        /// </summary>
        /// <param name="position">Target position</param>
        /// <returns>The distance between both positions</returns>
        float GetDistance(Vector2 position) => Vector2.Distance(position, WorldPosition);

        /// <summary>
        /// Iterating upon all the parsed agents, get an average of the requested value e.g. WorldPosition
        /// </summary>
        /// <param name="agents">List of agents</param>
        /// <param name="f">Custom function to get the desired variable from an agent</param>
        /// <returns>An average vector</returns>
        static Vector2 AverageCollector(ISimpleSteering[] agents, Func<ISimpleSteering, Vector2> f) => agents.Aggregate(Vector2.zero, (acc, agent) => acc + f.Invoke(agent)) / agents.Length;
        #endregion
        // ---
        #region MAIN FUNCTIONS
        Vector2 Seek(Vector2 position) {
            Vector2 dir = GetDirection(position);
            return dir * MaxSpeed;
        }
        Vector2 Flee(Vector2 position) => -Seek(position);
        Vector2 Arrive(Vector2 position, float brakeDistance) {
            Vector2 velocity = Seek(position);
            float distance = GetDistance(position);
            float speed = Mathf.Min(velocity.magnitude * (distance / brakeDistance), MaxSpeed);
            return velocity.normalized * speed;
        }
        Vector2 Persuit(ISimpleSteering steering, float? maxPredictionDistance = null) {
            float distance = GetDistance(steering.WorldPosition);
            float prediction = distance / MaxSpeed;
            prediction = Mathf.Min(prediction, maxPredictionDistance ?? prediction);
            Vector2 futurePosition = steering.WorldPosition + steering.Velocity.normalized * prediction;
            return Seek(futurePosition);
        }
        Vector2 Evade(ISimpleSteering steering, float? maxPredictionDistance = null) => -Persuit(steering, maxPredictionDistance);
        Vector2 Wander(Vector2 direction, float maxAngle, float radius, float distance) {
            Vector2 circle = direction * distance;
            float angle = Mathf.Atan2(direction.y, direction.x);
            float rndAngle = UnityEngine.Random.Range(-maxAngle, maxAngle) * Mathf.Rad2Deg;
            float radian = angle + rndAngle;
            Vector2 displacement = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * radius;
            return (circle + displacement).normalized * MaxSpeed;
        }
        #endregion

        Vector2 Avoid(Vector2 direction, Vector2 bottomLeft, Vector2 topRight, float detectionDistance, int directions = 8, float dangerWeight = 0.8f)
        {
            Vector2 result = Vector2.zero;
            for (int i = 0; i < directions; i++)
            {
                Vector2 current = GetCircleDirection(i, directions);
                float interest = Vector2.Dot(current, direction);
                if (interest <= 0) { continue; }

                float xProx = Mathf.Clamp01((WorldPosition.x - bottomLeft.x) / (topRight.x - bottomLeft.x));
                float yProx = Mathf.Clamp01((WorldPosition.y - bottomLeft.y) / (topRight.y - bottomLeft.y));

                // This may need a range cutoff so it has no influence after x distance
                bool xDetect = xProx < detectionDistance || xProx > (1 - detectionDistance);
                bool yDetect = yProx < detectionDistance || yProx > (1 - detectionDistance);
                float dangerX = Mathf.Exp(1 - xProx) - 1;
                float dangerY = Mathf.Exp(1 - yProx) - 1;
                float danger = Mathf.Max(dangerX, dangerY);

                float influence = interest - (danger * dangerWeight);

                result += current * influence;
            }
            return result.normalized * MaxSpeed;
        }


        /// <summary>
        /// Sums up all the forces and clamps it so it accelerates and hits the max speed to the way we have defined this profile
        /// </summary>
        /// <param name="forces">The forces we are wanting to apply to an entity</param>
        /// <returns>The calculated direction and speed to move in</returns>
        Vector2 Result(params Vector2[] forces) {
            Vector2 totalForce = forces.Aggregate(Vector2.zero, (acc, force) => acc + force);
            Vector2 clampedChangeInForce = Vector2.ClampMagnitude(totalForce - Velocity, MaxForce);
            return Velocity = Vector2.ClampMagnitude(Velocity + clampedChangeInForce, MaxSpeed);
        }
        

        Vector2 GetCircleDirection(int index, int resolution){
            float angle = (float)index * 2 * Mathf.PI / resolution;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }
}