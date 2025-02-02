using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour{
    [SerializeField] Transform planet;
    [SerializeField] float speed = 1;


    void FixedUpdate(){
        Vector3 dirToPlanet = (planet.position - transform.position).normalized;

        Vector3 forwardCross = Vector3.Cross(Vector3.up, dirToPlanet).normalized;
        Quaternion lookDir = Quaternion.LookRotation(forwardCross, Vector3.up);
        transform.rotation = lookDir;


        transform.position += speed * Time.fixedDeltaTime * transform.forward;
    }
}
