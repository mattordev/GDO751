using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AstralCandle.Input;
using AstralCore.AI.SteeringSystem;
using AstralCore.Utils;
using mattordev.game.score;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Character{
    public class BirdPlayer : BirdMotor
    {
        public static BirdPlayer Instance;
        [SerializeField] UserInput input;
        [SerializeField, Range(0, 360)] float maxBankAngle = 180f;
        [SerializeField, Range(0, 1)] float diveOrientation = 0.9f;
        [SerializeField] DiveScores[] diveScores;
        float timeDiving = 0;

        Quaternion curRot, angVel;

        Transform _cam;
        Transform Camera => _cam ??= UnityEngine.Camera.main.transform;

        protected override Quaternion GetDesiredRotation(Vector3 lookDir)
        {
            Vector3 camForward = Camera.forward.normalized;
            // Debug.Log($"{camForward} // {lookDir}");
            float pitch = -Mathf.Asin(camForward.y) * Mathf.Rad2Deg;
            float yaw = Mathf.Atan2(camForward.x, camForward.z) * Mathf.Rad2Deg;

            Vector3 flatCamForward = new Vector3(camForward.x, 0, camForward.z).normalized;
            Vector3 flatLook = new Vector3(lookDir.x, 0, lookDir.z).normalized;
            float directionCompare = Vector3.Cross(flatCamForward, flatLook).y;

            float trgtRoll = Mathf.Lerp(0, maxBankAngle, GetPercentSpeed()) * directionCompare;

            Quaternion desiredLook = Quaternion.Euler(pitch, yaw, trgtRoll);

            float turningSharpness = baseTurningSpeed * turningSensitivity.Evaluate(GetPercentSpeed());
            curRot.SmoothDamp(desiredLook, ref angVel, 1f / turningSharpness, Mathf.Infinity, Time.fixedDeltaTime);
            return curRot;
        }


        protected override Vector3[] GetForces()
        {
            Vector3 iv = input.InputVelocity;
            (iv.z, iv.y) = (iv.y, 0);
            iv.x = 0;

            return new Vector3[] {
                SteeringFuncs.Seek(transform.TransformPoint(iv), this)
            };
        }

        protected override void InitOnStart() => CameraController.Instance.Take(this);

        void OnDisable()
        {
            Instance = null;
        }

        protected override void Awake()
        {
            angVel = Quaternion.identity;
            base.Awake();
            Instance = this;
        }

        protected override void ProcessFixedUpdate()
        {
            base.ProcessFixedUpdate();
            bool isDiving = GravityAffector >= diveOrientation;
            if (!isDiving && timeDiving > 0){
                DiveScores result = default;
                bool found = false;
                for (int i = 0; i < diveScores.Length; i++)
                {
                    if (timeDiving < diveScores[i].TimeStamp) { break; }
                    result = diveScores[i];
                    found = true;
                }

                if (found){
                    int score = Mathf.CeilToInt(timeDiving * result.ScoreMultiplier);
                    ScoreManager.Instance.AddScore(score); // Add score for the near miss.
                    ScoreUI.Instance.FlashScoreUI(score, result.name); // Flash the score UI with the added score.
                }
            }
            timeDiving = isDiving ? timeDiving + Time.deltaTime : 0;
        }

        [Serializable]
        struct DiveScores
        {
            public string name;
            public float TimeStamp;
            public float ScoreMultiplier;
        }
    }
}