using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstralCandle.Input;
using AstralCandle.Utilities;

public class Rotate : MonoBehaviour{
    [SerializeField] Transform planet;
    [SerializeField] float speed = 1;

    [SerializeField] UserInput input;
    CameraController _cam;
    CameraController Camera => _cam ??= ISingleton<CameraController>.Instance;
    // NOTE: FREE MOVEMENT CLOSER TO PLANET BUT ARCH IT MORE WHEN ALTITUDE EXCEEDS AN AMOUNT

    void FixedUpdate(){        
        Vector3 dirToPlanet = (transform.position - planet.position).normalized;
        
        transform.rotation = Camera.transform.rotation;
        
        Vector3 tangentForward = Vector3.ProjectOnPlane(transform.forward, dirToPlanet).normalized;
        Quaternion moveDir = Quaternion.LookRotation(tangentForward, dirToPlanet);


        transform.position += speed * Time.fixedDeltaTime * tangentForward;

        // POTENTIAL FIX: USE PROJECT ON PLANE LINE AS A GUIDELINE TO ROTATE AROUND PLANET. MAY NEED TO CHANGE IT FROM TRANSFORM.FORWARD TO SOMETHING ELSE THO
        // THE 'ALTITUDE' POSITION CAN CHANGE DEPENDING ON THE ANGLE OF THE CAMERA,
    }
}
