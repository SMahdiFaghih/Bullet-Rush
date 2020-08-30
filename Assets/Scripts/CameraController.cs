using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 Offset;
    private GameObject Player;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = Player.transform.position + Offset;
        transform.LookAt(Player.transform);
    }
}
