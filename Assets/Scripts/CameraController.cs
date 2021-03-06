﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public static bool IsLevelCompleted = false;

    public Vector3 Offset;
    public Vector3 LevelCompletedOffset;
    private GameObject Player;
    private Transform CameraRig;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Transform[] childGameObjects = Player.GetComponentsInChildren<Transform>();
        foreach (Transform transform in childGameObjects)
        {
            if (transform.gameObject.name == "CameraRig")
            {
                CameraRig = transform;
                break;
            }
        }
        IsLevelCompleted = false;
    }


    void Update()
    {
        if (IsLevelCompleted)
        {
            transform.position = Player.transform.position + LevelCompletedOffset;
            transform.LookAt(Player.transform);
        }
        else
        {
            transform.position = Player.transform.position + Offset;
            transform.LookAt(CameraRig);
        }
    }
}
