using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstralCandle.Input;

public class TstInput : MonoBehaviour{
    [SerializeField] UserInput input;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(input.InputVelocity);
    }
}
