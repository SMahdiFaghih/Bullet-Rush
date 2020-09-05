using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 Offset;
    private GameObject Player;
    private Transform CameraRig;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Transform[] childGameObjects = Player.GetComponentsInChildren<Transform>();
        foreach (Transform transform in childGameObjects)
        {
            Debug.Log(transform.gameObject.name);
            if (transform.gameObject.name == "CameraRig")
            {
                CameraRig = transform;
                break;
            }
        }
    }

    void Update()
    {
        transform.position = Player.transform.position + Offset;
        transform.LookAt(CameraRig);
    }
}
