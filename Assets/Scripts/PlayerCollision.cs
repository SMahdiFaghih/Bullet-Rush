using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerController PlayerController;
    private bool LevelCompleted = false;

    void Start()
    {
        PlayerController = GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!LevelCompleted)
        {
            if (collision.collider.tag == "Enemy")
            {
                GameManager.Instance.Restart();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!LevelCompleted)
        {
            if (collider.tag == "Helicopter Landing Pad")
            {
                LevelCompleted = true;
                GameManager.Instance.StartLevelCompletedProcesses();
            }
            else if (collider.tag == "Speed Boost")
            {
                Destroy(collider.gameObject);
                StartCoroutine(PlayerController.ActivateSpeedBoost());
            }
        }
    }
}
