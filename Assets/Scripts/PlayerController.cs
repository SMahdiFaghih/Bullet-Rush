using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Velocity = 5;

    private Rigidbody Rigidbody;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Rigidbody.velocity = (Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward) * Velocity;
    }
}
