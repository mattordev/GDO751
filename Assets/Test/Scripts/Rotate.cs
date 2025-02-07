using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstralCandle.Input;

public class Rotate : MonoBehaviour{
    [SerializeField] Transform planet;
    [SerializeField] float speed = 1;

    [SerializeField] UserInput input;

    void FixedUpdate(){        
        Vector3 dirToPlanet = (transform.position - planet.position).normalized;

        Vector3 tangentForward = Vector3.ProjectOnPlane(transform.forward, dirToPlanet).normalized;

        Vector3 inputDir = ((transform.forward * input.InputVelocity.y) + (transform.right * input.InputVelocity.x)).normalized;
        Vector3 final = Vector3.ProjectOnPlane(inputDir, dirToPlanet).normalized;


        Quaternion moveDir = Quaternion.LookRotation(final, dirToPlanet);
        transform.rotation = moveDir;

        transform.position += speed * Time.fixedDeltaTime * final;

        
    }
}
