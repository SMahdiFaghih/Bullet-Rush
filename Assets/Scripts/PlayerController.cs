using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Velocity = 5;
    public int BulletVelocity = 20;

    public TextMesh BoostRemainingTimeText;
    private int SpeedBoost = 1;
    private float SpeedBoostDuration = 5.0f;
    private float BoostRemainingTime;

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
        BoostRemainingTimeText.gameObject.SetActive(false);
    }

    void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Rigidbody.velocity = (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward) * Velocity * SpeedBoost;
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

        BoostRemainingTimeText.transform.rotation = Quaternion.identity;
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

            yield return new WaitForSeconds(0.05f);

            if (Input.GetAxis("Fire2") != 0)
            {
                Rigidbody bullet = Instantiate(BulletPrefab, RightGunFireTransform.transform.position, RightGunFireTransform.transform.rotation).GetComponent<Rigidbody>();

                bullet.velocity = transform.forward * BulletVelocity;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator ActivateSpeedBoost()
    {
        if (SpeedBoost == 1)
        {
            BoostRemainingTime = SpeedBoostDuration;

            SpeedBoost = 2;
            BoostRemainingTimeText.gameObject.SetActive(true);
            while (BoostRemainingTime > 0f)
            {
                BoostRemainingTime -= Time.deltaTime;
                BoostRemainingTimeText.text = BoostRemainingTime.ToString("0.0") + "s";
                yield return null;
            }
            SpeedBoost = 1;
            BoostRemainingTimeText.gameObject.SetActive(false);
        }
        else
        {
            BoostRemainingTime += SpeedBoostDuration;
        }
    }
}
