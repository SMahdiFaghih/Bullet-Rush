using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public static bool IsLevelCompleted = false;

    public int Velocity = 5;
    public int BulletVelocity = 20;

    public TextMesh BoostRemainingTimeText;
    private int SpeedBoost = 1;
    private float SpeedBoostDuration = 6.0f;
    private float BoostRemainingTime;

    public GameObject BulletPrefab;

    public AudioSource LeftGunShotSound;
    public AudioSource RightGunShotSound;
    public AudioSource RemainingTimeLessThan4Sound;

    private Rigidbody Rigidbody;
    private GameObject LeftGunFireTransform;
    private GameObject RightGunFireTransform;

    private Coroutine FireCoroutine = null;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        LeftGunFireTransform = GameObject.FindGameObjectWithTag("LeftGunFireTransform");
        RightGunFireTransform = GameObject.FindGameObjectWithTag("RightGunFireTransform");

        FireCoroutine = StartCoroutine(Fire());
        BoostRemainingTimeText.gameObject.SetActive(false);
        IsLevelCompleted = false;
    }

    void Update()
    {
        if (!IsLevelCompleted)
        {
            Move();
            Rotate();
        }
        else
        {
            StopCoroutine(FireCoroutine);
            RightGunShotSound.Stop();
            LeftGunShotSound.Stop();
        }
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

                if (!LeftGunShotSound.isPlaying)
                {
                    LeftGunShotSound.Play();
                }
            }
            else
            {
                LeftGunShotSound.Stop();
            }

            yield return new WaitForSeconds(0.05f);

            if (Input.GetAxis("Fire2") != 0)
            {
                Rigidbody bullet = Instantiate(BulletPrefab, RightGunFireTransform.transform.position, RightGunFireTransform.transform.rotation).GetComponent<Rigidbody>();
                bullet.velocity = transform.forward * BulletVelocity;

                if (!RightGunShotSound.isPlaying)
                {
                    RightGunShotSound.Play();
                }
            }
            else
            {
                RightGunShotSound.Stop();
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
                if (BoostRemainingTime < 4f && !RemainingTimeLessThan4Sound.isPlaying)
                {
                    RemainingTimeLessThan4Sound.Play();
                }
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
            RemainingTimeLessThan4Sound.Stop();
        }
    }
}
