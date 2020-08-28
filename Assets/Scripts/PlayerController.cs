using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Velocity = 5;
    public int BulletVelocity = 20;

    public GameObject BulletPrefab;

    private Rigidbody Rigidbody;
    private GameObject LeftGunFireTransform;
    private GameObject RightGunFireTransform;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        LeftGunFireTransform = GameObject.FindGameObjectWithTag("LeftGunFireTransform");
        RightGunFireTransform = GameObject.FindGameObjectWithTag("RightGunFireTransform");

        StartCoroutine(Fire());
    }

    void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Rigidbody.velocity = (Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward) * Velocity;
    }

    private void Rotate()
    {
        Plane plane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            transform.LookAt(hitPoint);
        }
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            if (Input.GetAxis("Fire1") != 0)
            {
                Rigidbody bullet = Instantiate(BulletPrefab, LeftGunFireTransform.transform.position, LeftGunFireTransform.transform.rotation).GetComponent<Rigidbody>();

                bullet.velocity = transform.forward * BulletVelocity;
            }

            if (Input.GetAxis("Fire2") != 0)
            {
                Rigidbody bullet = Instantiate(BulletPrefab, RightGunFireTransform.transform.position, RightGunFireTransform.transform.rotation).GetComponent<Rigidbody>();

                bullet.velocity = transform.forward * BulletVelocity;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
