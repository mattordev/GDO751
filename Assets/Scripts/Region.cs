using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class Region : MonoBehaviour
{
    public string regionName;
    public BoxCollider regionCollider; // region collider

    // Start is called before the first frame update
    void Start()
    {
        if (regionCollider == null)
            regionCollider = GetComponent<BoxCollider>();
    }
}
