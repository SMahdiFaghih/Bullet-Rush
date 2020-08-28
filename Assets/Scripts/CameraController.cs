using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 Offset;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + Offset;
        transform.LookAt(Player.transform);
    }
}
