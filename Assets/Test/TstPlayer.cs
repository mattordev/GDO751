using System.Collections;
using System.Collections.Generic;
using AstralCandle.Input;
using AstralCandle.Utilities;
using UnityEngine;

/// <summary>
/// ©️2025 Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.Game{
    public class TstPlayer : MonoBehaviour{    
        // [SerializeField] UserInput input;
        // Rigidbody _rb;
        // Rigidbody RB => _rb ??= GetComponent<Rigidbody>();
        // [SerializeField] float speed;

        // PlanetManager manager;
        // PlanetManager Manager => manager ??= ISingleton<PlanetManager>.Instance;

        // void Move(Vector3 att){
        //     Vector3 direction = transform.TransformDirection(new Vector3(input.InputVelocity.x, 0, input.InputVelocity.y)) * speed;
        //     RB.AddForce(direction + att, ForceMode.VelocityChange);
        // }

        // void Rotate(Vector3 att){
        //     Quaternion trgtRot = Quaternion.FromToRotation(transform.up, -att.normalized) * transform.rotation;
        //     transform.rotation = trgtRot;
        // }

        // void FixedUpdate(){
        //     Vector3 attraction = Manager.Attract(transform);
        //     Rotate(attraction);
        //     Move(attraction);
        // }
    }
}